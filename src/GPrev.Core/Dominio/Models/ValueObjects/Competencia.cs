using System.Globalization;

namespace GPrev.Core.Dominio.Models.ValueObjects;

public readonly record struct Competencia
{
    private readonly DateTime _data;

    private Competencia(DateTime data) => _data = data;

    public int Ano => _data.Year;
    public DateTime Data => _data;

    public static bool TentarConverter(string texto, out Competencia competencia)
    {
        competencia = default;
        if (string.IsNullOrWhiteSpace(texto))
            return false;

        if (!DateTime.TryParseExact(
                texto.Trim(),
                "MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var data))
            return false;

        competencia = new Competencia(data);
        return true;
    }

    public override string ToString() => _data.ToString("MM/yyyy");
}
