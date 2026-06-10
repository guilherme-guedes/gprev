namespace GPrev.Core.Dominio;

public static class IndicadoresCnis
{
    private static readonly HashSet<string> _indicadoresBloqueadosRestituicao = new(StringComparer.OrdinalIgnoreCase)
    {
        "IREM-ACD"
    };

    public static bool BloqueiaRestituicao(string indicadores) =>
        !string.IsNullOrWhiteSpace(indicadores) &&
        _indicadoresBloqueadosRestituicao.Any(i => indicadores.Contains(i, StringComparison.OrdinalIgnoreCase));
}
