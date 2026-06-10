namespace GPrev.Core.Dominio.Services.CalculadorasInss;

public sealed class CalculadoraInssSeguradoEspecial : ICalculadoraContribuicao
{
    public decimal Calcular(decimal remuneracao, int ano, string formaPrestacao) =>
        Math.Round(remuneracao * 0.013m, 2);
}
