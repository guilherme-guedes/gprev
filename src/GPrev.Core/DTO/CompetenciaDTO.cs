namespace GPrev.Core.Dtos;

public sealed class CompetenciaDTO
{
    public string Empregador { get; init; } = string.Empty;
    public DateTime Data { get; init; }
    public decimal Remuneracao { get; init; }
    public decimal Contribuicao { get; init; }
    public string TipoContribuicao { get; init; } = string.Empty;
    public decimal ValorCorrigido { get; init; }
    public decimal ExcedenteTeto { get; init; }
    public string Indicadores { get; init; } = string.Empty;
}
