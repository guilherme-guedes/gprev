using GPrev.Core.Dominio.Infra;
using GPrev.Core.Dominio.Models;
using GPrev.Core.Dominio.Models.Enums;
using GPrev.Core.Dominio.Models.ValueObjects;
using GPrev.Core.Dominio.Services.CalculadorasInss;
using GPrev.Core.Exceptions;
using GPrev.InssData;
using System.Text.RegularExpressions;
using GPrev.Core.Utils;

namespace GPrev.Core.Dominio.Services;

public class CnisServico
{
    private const string CabecalhoVinculo =
        "Código Emp.Origem do VínculoTipo Filiado noVínculoData InícioData FimÚlt. Remun.Seq.NITMatrícula doTrabalhador";

    private static readonly Regex RegexDadosPessoais = new(
        @"NIT:([\d.\-]+)CPF:([\d.\-]+)Nome:(.+?)Data de nascimento:",
        RegexOptions.Compiled);

    private static readonly Regex RegexTipo = new(
        @"Empregado ou Agente\s?P[uú]blico|Contribuinte Individual|Empregado Dom[eé]stico|Trabalhador Avulso|Contribuinte Facultativo|Segurado Especial",
        RegexOptions.Compiled);

    private static readonly Regex RegexIndexCodigo = new(
        @"^(\d+?)(\d{2}\.\d{3}\.\d{3}(?:/\d{4}-\d{2})?)?(.*?)$",
        RegexOptions.Compiled | RegexOptions.Singleline);

    private static readonly Regex RegexDatasNit = new(
        @"(\d{2}/\d{2}/\d{4})(\d{2}/\d{2}/\d{4})?(\d{2}/\d{4})?(\d{3}\.\d{5}\.\d{2}-\d)(\d*)",
        RegexOptions.Compiled);

    private const string PatternIndicador = @"([A-Z][A-Z0-9,\- ]*?(?=\d{2}/\d{4}|[\r\n]|$))?";

    private static readonly Regex RegexContribuicaoStandard = new(
        @$"(\d{{2}}/\d{{4}})(\d{{1,3}}(?:\.\d{{3}})*,\d{{2}}){PatternIndicador}",
        RegexOptions.Compiled);

    private static readonly Regex RegexContribuicaoCooperativa = new(
        @$"(\d{{2}}/\d{{4}})\d{{2}}\.\d{{3}}\.\d{{3}}(\d{{2}}\.\d{{3}}\.\d{{3}}/\d{{4}}-\d{{2}})([^0-9,]*)(\d{{1,3}}(?:\.\d{{3}})*,\d{{2}}){PatternIndicador}",
        RegexOptions.Compiled);

    private static readonly Regex RegexCnpj = new(
        @"\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}",
        RegexOptions.Compiled);

    private readonly LeitorCnisAbstrato _pdfLeitor;
    private readonly Dictionary<ETipoContribuinte, ICalculadoraContribuicao> _calculadoras;

    public CnisServico(LeitorCnisAbstrato pdfLeitor, InssDataService inssDataService)
    {
        _pdfLeitor = pdfLeitor;

        var clt = new CalculadoraInssCLT(inssDataService);
        var individual = new CalculadoraInssIndividual(inssDataService);
        var especial = new CalculadoraInssSeguradoEspecial();

        _calculadoras = new Dictionary<ETipoContribuinte, ICalculadoraContribuicao>
        {
            [ETipoContribuinte.Empregado_AgentePublico] = clt,
            [ETipoContribuinte.Empregado_Domestico] = clt,
            [ETipoContribuinte.Trabalhador_Avulso] = clt,
            [ETipoContribuinte.Contribuinte_Individual] = individual,
            [ETipoContribuinte.Contribuinte_Facultativo] = individual,
            [ETipoContribuinte.Segurado_Especial] = especial,
        };
    }

    public Task<Contribuinte> ExtrairDadosContribuidorAsync(string caminhoArquivoPdf) =>
        Task.Run(() => ProcessarTexto(_pdfLeitor.LerTextoCompleto(caminhoArquivoPdf)));

    public Task<Contribuinte> ExtrairDadosContribuidorAsync(Stream streamPdf) =>
        Task.Run(() => ProcessarTexto(_pdfLeitor.LerTextoCompleto(streamPdf)));

    private Contribuinte ProcessarTexto(ReadOnlyMemory<char> textoOriginal)
    {
        var contribuidor = ExtrairEPreencherDadosPessoais(textoOriginal.ToString());

        var textoLimpo = LeitorCnisAbstrato.LimparTextoCompleto(textoOriginal).ToString();
        var blocos = textoLimpo.Split(CabecalhoVinculo, StringSplitOptions.RemoveEmptyEntries);

        foreach (var bloco in blocos)
        {
            var vinculo = ExtrairRelacaoPrevidenciaria(bloco.Trim());
            if (vinculo != null)
                contribuidor.AdicionarRelacaoPrevidenciaria(PreencherContribuicoesInss(vinculo));
        }

        return contribuidor;
    }

    private RelacaoPrevidenciaria PreencherContribuicoesInss(RelacaoPrevidenciaria vinculo)
    {
        var registrosAtualizados = vinculo.RegistrosRemuneracao
            .Select(registro =>
            {
                return registro with { ContribuicaoINSS = CalcularInss(registro.Remuneracao, registro.Competencia.Ano, vinculo.Tipo, registro.FormaPrestacao) };
            })
            .ToList();

        return new RelacaoPrevidenciaria(
            vinculo.Origem, vinculo.Tipo, vinculo.DataInicio, vinculo.DataFim,
            vinculo.UltimaRemuneracao, registrosAtualizados);
    }

    private decimal CalcularInss(decimal remuneracao, int ano, TipoContribuinte tipoContribuinte, string formaPrestacao = "")
    {
        if (remuneracao <= 0) return 0m;
        return _calculadoras.TryGetValue(tipoContribuinte, out var calc)
            ? calc.Calcular(remuneracao, ano, formaPrestacao)
            : 0m;
    }

    private Contribuinte ExtrairEPreencherDadosPessoais(string texto)
    {
        var match = RegexDadosPessoais.Match(texto);
        if (!match.Success) throw new ContribuinteNaoEncontradoException();

        return new Contribuinte(nit: match.Groups[1].Value.Trim(), cpf: match.Groups[2].Value.Trim(), nome: match.Groups[3].Value.Trim());
    }

    private RelacaoPrevidenciaria? ExtrairRelacaoPrevidenciaria(string blocoVinculoPrevidenciario)
    {
        if (string.IsNullOrWhiteSpace(blocoVinculoPrevidenciario)) return null;

        var matchTipo = RegexTipo.Match(blocoVinculoPrevidenciario);
        if (!matchTipo.Success) return null;

        var textoAntes = blocoVinculoPrevidenciario[..matchTipo.Index].Trim();
        var textoAposTipo = blocoVinculoPrevidenciario[(matchTipo.Index + matchTipo.Length)..];

        var idxRemuneracoes = textoAposTipo.IndexOf("Remunerações", StringComparison.Ordinal);
        string textoHeader, textoContribuicoes;

        if (idxRemuneracoes >= 0)
        {
            textoHeader = textoAposTipo[..idxRemuneracoes];
            textoContribuicoes = textoAposTipo[(idxRemuneracoes + "Remunerações".Length)..].Trim();
        }
        else
        {
            textoHeader = textoAposTipo;
            textoContribuicoes = string.Empty;
        }

        var matchIdx = RegexIndexCodigo.Match(textoAntes);
        if (!matchIdx.Success) return null;

        _ = int.TryParse(matchIdx.Groups[1].Value, out var indice);
        var codigo = matchIdx.Groups[2].Value.Trim();
        var nome = matchIdx.Groups[3].Value.Trim();

        var matchHeader = RegexDatasNit.Match(textoHeader.Trim());

        var origem = new Origem(
                indice: indice,
                Codigo: codigo,
                NomeEmpresa: nome,
                NIT: matchHeader.Success ? matchHeader.Groups[4].Value : string.Empty,
                Matricula: matchHeader.Success ? matchHeader.Groups[5].Value : string.Empty
        );

        var remuneracoes = ExtrairRegistrosRemuneracao(textoContribuicoes);

        var vinculo = new RelacaoPrevidenciaria(
            origem: origem,
            tipo: TipoContribuinte.Converter(matchTipo.Value),
            dataInicio: matchHeader.Groups[1].Success ? matchHeader.Groups[1].Value : string.Empty,
            dataFim: matchHeader.Groups[2].Success ? matchHeader.Groups[2].Value : string.Empty,
            ultimaRemuneracao: matchHeader.Groups[3].Success ? matchHeader.Groups[3].Value : string.Empty,
            registrosRemuneracao: remuneracoes
        );

        return vinculo;
    }

    private List<RegistroRemuneracao> ExtrairRegistrosRemuneracao(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto)) return [];

        if (RegexCnpj.IsMatch(texto))
            return PreencherRemuneracoesContribuinteIndividualEAutonomo(texto);
        else
            return PreencherRemuneracoesEmpregadoEGeral(texto);
    }

    private List<RegistroRemuneracao> PreencherRemuneracoesEmpregadoEGeral(string texto)
    {
        List<RegistroRemuneracao> registrosRemuneracao = new List<RegistroRemuneracao>();

        foreach (Match match in RegexContribuicaoStandard.Matches(texto))
        {
            if (!DecimalUtils.TentarConverterValor(match.Groups[2].Value, out var remuneracao))
                continue;

            if (!Competencia.TentarConverter(match.Groups[1].Value, out var competencia))
                continue;

            registrosRemuneracao.Add(new RegistroRemuneracao(            
                Competencia: competencia,
                Remuneracao: remuneracao,
                Indicadores: match.Groups[3].Value.Trim()
            ));
        }

        return registrosRemuneracao;
    }

    private List<RegistroRemuneracao> PreencherRemuneracoesContribuinteIndividualEAutonomo(string texto)
    {
        List<RegistroRemuneracao> registrosRemuneracao = new List<RegistroRemuneracao>();

        foreach (Match match in RegexContribuicaoCooperativa.Matches(texto))
        {
            if (!DecimalUtils.TentarConverterValor(match.Groups[4].Value, out var remuneracao))
                continue;

            if (!Competencia.TentarConverter(match.Groups[1].Value, out var competencia))
                continue;

            registrosRemuneracao.Add(new RegistroRemuneracao
            (
                Competencia: competencia,
                Estabelecimento: match.Groups[2].Value,
                FormaPrestacao: match.Groups[3].Value.Trim(),
                Remuneracao: remuneracao,
                Indicadores: match.Groups[5].Value.Trim()
            ));
        }

        return registrosRemuneracao;
    }
}
