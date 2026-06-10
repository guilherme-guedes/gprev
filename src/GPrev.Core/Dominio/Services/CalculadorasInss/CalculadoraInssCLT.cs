using GPrev.InssData;

namespace GPrev.Core.Dominio.Services.CalculadorasInss;

public sealed class CalculadoraInssCLT : ICalculadoraContribuicao
{
    private readonly InssDataService _inssDataService;

    public CalculadoraInssCLT(InssDataService inssDataService)
    {
        _inssDataService = inssDataService;
    }

    public decimal Calcular(decimal remuneracao, int ano, string formaPrestacao)
    {
        var tabela = _inssDataService.ObterTabela(ano);
        var valor = 0m;

        foreach (var faixa in tabela.RegimeClt.FaixasAliquotasPercentual)
        {
            if (remuneracao <= faixa.De) break;
            var parcela = Math.Min(remuneracao, faixa.Ate) - faixa.De;
            valor += parcela * (faixa.Percentual / 100m);
        }

        return Math.Min(Math.Round(valor, 2), tabela.RegimeClt.ValorMaximoContribuicaoMensalTeto);
    }
}
