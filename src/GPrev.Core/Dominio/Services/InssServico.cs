using GPrev.Core.Dominio.Models;
using GPrev.Core.Dominio.Models.ValueObjects;
using GPrev.Core.Dtos;
using GPrev.InssData;
using GPrev.SelicData;

namespace GPrev.Core.Dominio.Services;

public class InssServico
{
    private readonly InssDataService _dadosInss;
    private readonly SelicDataService _dadosSelic;

    public InssServico(InssDataService dadosInss, SelicDataService dadosSelic)
    {
        _dadosInss = dadosInss;
        _dadosSelic = dadosSelic;
    }

    public IReadOnlyList<CompetenciaDTO> CalcularExcedentesContribuicoes(Contribuinte contribuidor)
    {
        var lancamentos = ListarLancamentos(contribuidor);
        var resultado = new List<CompetenciaDTO>();

        if (!contribuidor.TemRelacoesPrevidenciaria())
            return resultado;

        foreach (var grupo in lancamentos.GroupBy(l => l.RegistroCNIS.Competencia))
            resultado.AddRange(CalcularExcedentesCompetencia(grupo.ToList()));

        return resultado;
    }

    public bool AptoParaRestituicao(CompetenciaDTO contribuicao)
    {
        if (IndicadoresCnis.DesconsiderarContribuicao(contribuicao.Indicadores))
            return false;

        var competenciaInicio = ObterCompetenciaLimiteInicioRestituicao();
        var competenciaContribuicao = new DateTime(contribuicao.Data.Year, contribuicao.Data.Month, 1);

        return competenciaContribuicao >= competenciaInicio && competenciaContribuicao < DateTime.Now.Date.AddMonths(1);
    }

    public bool TemExcedente(CompetenciaDTO contribuicao) => contribuicao.ExcedenteTeto > 0;

    public IReadOnlyList<CompetenciaDTO> FiltrarComExcedente(IReadOnlyList<CompetenciaDTO> contribuicoes) =>
        Filtrar(contribuicoes, TemExcedente);

    public IReadOnlyList<CompetenciaDTO> FiltrarAptosRestituicao(IReadOnlyList<CompetenciaDTO> contribuicoes) =>
        Filtrar(contribuicoes, c => AptoParaRestituicao(c));

    public IReadOnlyList<CompetenciaDTO> FiltrarSemPendencia(IReadOnlyList<CompetenciaDTO> contribuicoes) =>
        Filtrar(contribuicoes, c => !IndicadoresCnis.ContribuicaoComPendencia(c.Indicadores));

    public IReadOnlyList<CompetenciaDTO> FiltrarPorPeriodo(
        IReadOnlyList<CompetenciaDTO> contribuicoes,
        DateTime dataInicial,
        DateTime dataFinal)
    {
        var dataFinalAjustada = dataFinal.Date.AddDays(1).AddTicks(-1);

        return Filtrar(contribuicoes, c => c.Data >= dataInicial.Date && c.Data <= dataFinalAjustada);
    }

    private static Dictionary<RegistroRemuneracao, decimal> AlocarExcedentePrimarioAtingiuTeto(
        IReadOnlyList<LancamentoCompetencia> ordenados,
        RelacaoPrevidenciaria vinculoPrimario,
        decimal tetoContribuicao)
    {
        var excedentes = new Dictionary<RegistroRemuneracao, decimal>();
        var restanteValido = tetoContribuicao;

        foreach (var lancamento in ordenados)
        {
            if (lancamento.Relacao == vinculoPrimario)
            {
                var valido = Math.Min(lancamento.RegistroCNIS.ContribuicaoINSS, restanteValido);
                excedentes[lancamento.RegistroCNIS] = lancamento.RegistroCNIS.ContribuicaoINSS - valido;
                restanteValido -= valido;
            }
            else
            {
                excedentes[lancamento.RegistroCNIS] = lancamento.RegistroCNIS.ContribuicaoINSS;
            }
        }

        return excedentes;
    }

    private static Dictionary<RegistroRemuneracao, decimal> AlocarExcedentePorAbatimento(
        IReadOnlyList<LancamentoCompetencia> ordenados,
        decimal tetoContribuicao)
    {
        var excedentes = new Dictionary<RegistroRemuneracao, decimal>();
        var restanteValido = tetoContribuicao;

        foreach (var lancamento in ordenados)
        {
            var valido = Math.Min(lancamento.RegistroCNIS.ContribuicaoINSS, restanteValido);
            excedentes[lancamento.RegistroCNIS] = lancamento.RegistroCNIS.ContribuicaoINSS - valido;
            restanteValido -= valido;
        }

        return excedentes;
    }

    private static List<LancamentoCompetencia> ListarLancamentos(Contribuinte contribuidor)
    {
        var lancamentos = new List<LancamentoCompetencia>();

        foreach (var relacao in contribuidor.RelacoesPrevidenciarias)
        {
            var empregador = relacao.Origem.Titulo();
            var tipo = relacao.Tipo.ObterDescricao();

            foreach (var contribuicao in relacao.RegistrosRemuneracao)
            {
                lancamentos.Add(new LancamentoCompetencia(
                    relacao,
                    contribuicao,
                    empregador,
                    tipo,
                    relacao.Origem.Indice));
            }
        }

        return lancamentos;
    }


    private static DateTime ObterCompetenciaLimiteInicioRestituicao() =>
        DateTime.Now.Date.AddMonths(1).AddYears(-5);

    private static IReadOnlyList<CompetenciaDTO> Filtrar(
        IReadOnlyList<CompetenciaDTO> contribuicoes,
        Func<CompetenciaDTO, bool> predicate) =>
        contribuicoes.Where(predicate).ToList();

    private IReadOnlyList<CompetenciaDTO> CalcularExcedentesCompetencia(IReadOnlyList<LancamentoCompetencia> lancamentos)
    {
        if (lancamentos.Count == 0)
            return [];

        var competencia = lancamentos[0].RegistroCNIS.Competencia;
        var ano = competencia.Ano;
        var tetoRemuneracao = _dadosInss.ObterTeto(ano);
        var tetoContribuicao = ObterTetoContribuicaoCompetencia(lancamentos, ano);

        var ordenados = lancamentos
            .OrderBy(l => PrioridadeTipo(l.Relacao.Tipo))
            .ThenBy(l => l.IndiceVinculo)
            .ToList();

        var vinculoPrimario = ordenados
            .GroupBy(l => l.Relacao)
            .OrderBy(g => PrioridadeTipo(g.Key.Tipo))
            .ThenBy(g => g.Key.Origem.Indice)
            .First()
            .Key;

        var remunPrimario = ordenados
            .Where(l => l.Relacao == vinculoPrimario)
            .Sum(l => l.RegistroCNIS.Remuneracao);

        Dictionary<RegistroRemuneracao, decimal> excedentes;

        if (remunPrimario >= tetoRemuneracao)
        {
            excedentes = AlocarExcedentePrimarioAtingiuTeto(ordenados, vinculoPrimario, tetoContribuicao);
        }
        else
        {
            var somaRemuneracoes = ordenados.Sum(l => l.RegistroCNIS.Remuneracao);

            excedentes = somaRemuneracoes > tetoRemuneracao
                ? AlocarExcedentePorAbatimento(ordenados, tetoContribuicao)
                : ordenados.ToDictionary(l => l.RegistroCNIS, _ => 0m);
        }

        var fatorSelic = ObterFatorCorrecaoMonetaria(competencia.Data);

        return ordenados.Select(l =>
        {
            var excedente = excedentes.GetValueOrDefault(l.RegistroCNIS, 0m);
            return new CompetenciaDTO
            {
                Data = competencia.Data,
                Remuneracao = l.RegistroCNIS.Remuneracao,
                Contribuicao = l.RegistroCNIS.ContribuicaoINSS,
                Empregador = l.Empregador,
                TipoContribuicao = l.Tipo,
                ExcedenteTeto = excedente,
                ValorCorrigido = excedente * fatorSelic,
                Indicadores = l.RegistroCNIS.Indicadores
            };
        }).ToList();
    }

    private decimal ObterTetoContribuicaoCompetencia(IReadOnlyList<LancamentoCompetencia> lancamentos, int ano)
    {
        if (lancamentos.Any(l => l.Relacao.Tipo.EhVinculoCLT()))
        {
            return _dadosInss.ObterTabela(ano).RegimeClt.ValorMaximoContribuicaoMensalTeto;
        }

        var tipoPrimario = lancamentos
            .OrderBy(l => l.IndiceVinculo)
            .First()
            .Relacao.Tipo;

        return ObterTetoContribuicaoPorTipo(tipoPrimario, ano);
    }

    private decimal ObterTetoContribuicaoPorTipo(TipoContribuinte tipo, int ano)
    {
        if (tipo.EhVinculoCLT())
            return _dadosInss.ObterTabela(ano).RegimeClt.ValorMaximoContribuicaoMensalTeto;

        if (tipo.EhContribuinteIndividual())
            return _dadosInss.ObterContribuinteIndividual(ano).ValorMaximoContribuicaoMensalTeto;

        return _dadosInss.ObterTabela(ano).RegimeClt.ValorMaximoContribuicaoMensalTeto;
    }

    private decimal ObterFatorCorrecaoMonetaria(DateTime data)
    {
        var selic = _dadosSelic.ObterSelic(data.Month, data.Year);
        return selic is null ? 1m : 1m + selic.Value / 100m;
    }

    private static int PrioridadeTipo(TipoContribuinte tipo) => tipo.EhVinculoCLT() ? 0 : 1;

    private sealed record LancamentoCompetencia(
        RelacaoPrevidenciaria Relacao,
        RegistroRemuneracao RegistroCNIS,
        string Empregador,
        string Tipo,
        int IndiceVinculo);
}