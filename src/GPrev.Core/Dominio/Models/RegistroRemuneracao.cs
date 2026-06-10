using GPrev.Core.Dominio.Models.ValueObjects;

namespace GPrev.Core.Dominio.Models;

public record RegistroRemuneracao(
    Competencia Competencia, 
    decimal Remuneracao, 
    string Indicadores, 
    string FormaPrestacao = "", 
    string Estabelecimento = "")
{
    public decimal ContribuicaoINSS { get; init; }
}