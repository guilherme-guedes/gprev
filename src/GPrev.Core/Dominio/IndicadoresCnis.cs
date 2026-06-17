namespace GPrev.Core.Dominio;

public static class IndicadoresCnis
{
    private static readonly HashSet<string> _indicadoresDesconsiderarContribuicao = new(StringComparer.OrdinalIgnoreCase)
    {
        "IREM-ACD"
    };

    private static readonly HashSet<string> _indicadoresContribuicaoComPendencia = new(StringComparer.OrdinalIgnoreCase)
    {
        "PREM-EXT",
        "PEXT",
        "PEND-REM",
        "PEND-VINC",
        "PEND-RECOLH",
        "IECO",
        "PREM-BLOQ"
    };

    public static bool DesconsiderarContribuicao(string indicadores) =>
        !string.IsNullOrWhiteSpace(indicadores) &&
        _indicadoresDesconsiderarContribuicao.Any(i => indicadores.Contains(i, StringComparison.OrdinalIgnoreCase));

    public static bool ContribuicaoComPendencia(string indicadores) =>
        !string.IsNullOrWhiteSpace(indicadores) &&
        _indicadoresContribuicaoComPendencia.Any(i => indicadores.Contains(i, StringComparison.OrdinalIgnoreCase));
}
