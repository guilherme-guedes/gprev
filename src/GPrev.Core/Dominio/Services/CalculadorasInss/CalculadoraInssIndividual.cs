using GPrev.InssData;

namespace GPrev.Core.Dominio.Services.CalculadorasInss;

public sealed class CalculadoraInssIndividual : ICalculadoraContribuicao
{
    private readonly InssDataService _inssDataService;

    public CalculadoraInssIndividual(InssDataService inssDataService)
    {
        _inssDataService = inssDataService;
    }

    public decimal Calcular(decimal remuneracao, int ano, string formaPrestacao)
    {
        var dados = _inssDataService.ObterContribuinteIndividual(ano);
        var aliquota = formaPrestacao.Contains("Não Cooperado", StringComparison.OrdinalIgnoreCase)
            ? 0.11m
            : dados.PercentualPlanoNormal / 100m;
        var valor = Math.Min(remuneracao, dados.BaseMaximaTeto) * aliquota;
        return Math.Min(Math.Round(valor, 2), dados.ValorMaximoContribuicaoMensalTeto);
    }
}
