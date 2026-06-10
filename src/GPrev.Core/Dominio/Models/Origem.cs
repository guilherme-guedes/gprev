namespace GPrev.Core.Dominio.Models;

public record Origem(string NIT, string Codigo, string NomeEmpresa, string Matricula, int? indice = null)
{
    public int Indice { get; set; } = indice.HasValue ? indice.Value : 0;

    public string Titulo() => $"NIT {this.NIT} | {this.NomeEmpresa}";
}