namespace GPrev.Core.Dominio.Services.CalculadorasInss;

public interface ICalculadoraContribuicao
{
    decimal Calcular(decimal remuneracao, int ano, string formaPrestacao);
}
