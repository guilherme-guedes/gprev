using System.Text.Json;
using System.Text.Json.Serialization;

namespace GPrev.InssData;

public class InssDataService
{
    private const int AnoReferenciaPadrao = 2026;

    private readonly Dictionary<int, TabelaInssAnual> _tabelasPorAno;
    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    public InssDataService()
    {
        var jsonPath = Path.Combine(AppContext.BaseDirectory, "inss-dados.json");
        var jsonContent = File.ReadAllText(jsonPath);
        var dados = JsonSerializer.Deserialize<InssDadosRaiz>(jsonContent, _options)
            ?? throw new InvalidOperationException("Erro ao carregar dados do INSS.");

        _tabelasPorAno = dados.TabelasHistoricasInss
            .Where(kvp => kvp.Key.StartsWith("ano_") && int.TryParse(kvp.Key["ano_".Length..], out _))
            .ToDictionary(
                kvp => int.Parse(kvp.Key["ano_".Length..]),
                kvp => kvp.Value);
    }

    public TabelaInssAnual ObterTabela(int ano) =>
        _tabelasPorAno.GetValueOrDefault(ano) ?? _tabelasPorAno[AnoReferenciaPadrao];

    public decimal ObterTeto(int ano) => ObterTabela(ano).RegimeClt.TetoMaximo;

    public IReadOnlyList<FaixaAliquota> ObterFaixas(int ano) =>
        ObterTabela(ano).RegimeClt.FaixasAliquotasPercentual;

    public ContribuinteIndividualAutonomo ObterContribuinteIndividual(int ano) =>
        ObterTabela(ano).ContribuinteIndividualAutonomo;
}

public record TabelaInssAnual
{
    [JsonPropertyName("portaria_referencia")]
    public string PortariaReferencia { get; init; } = string.Empty;

    [JsonPropertyName("regime_clt")]
    public RegimeClt RegimeClt { get; init; } = new();

    [JsonPropertyName("contribuinte_individual_autonomo")]
    public ContribuinteIndividualAutonomo ContribuinteIndividualAutonomo { get; init; } = new();
}

public record RegimeClt
{
    [JsonPropertyName("salario_minimo_piso")]
    public decimal SalarioMinimoPiso { get; init; }

    [JsonPropertyName("teto_maximo")]
    public decimal TetoMaximo { get; init; }

    [JsonPropertyName("faixas_aliquotas_percentual")]
    public List<FaixaAliquota> FaixasAliquotasPercentual { get; init; } = [];

    [JsonPropertyName("valor_maximo_contribuicao_mensal_teto")]
    public decimal ValorMaximoContribuicaoMensalTeto { get; init; }
}

public record ContribuinteIndividualAutonomo
{
    [JsonPropertyName("base_minima")]
    public decimal BaseMinima { get; init; }

    [JsonPropertyName("base_maxima_teto")]
    public decimal BaseMaximaTeto { get; init; }

    [JsonPropertyName("percentual_plano_normal")]
    public decimal PercentualPlanoNormal { get; init; }

    [JsonPropertyName("valor_maximo_contribuicao_mensal_teto")]
    public decimal ValorMaximoContribuicaoMensalTeto { get; init; }
}

public record FaixaAliquota
{
    [JsonPropertyName("de")]
    public decimal De { get; init; }

    [JsonPropertyName("ate")]
    public decimal Ate { get; init; }

    [JsonPropertyName("percentual")]
    public decimal Percentual { get; init; }
}

internal class InssDadosRaiz
{
    [JsonPropertyName("tabelas_historicas_inss")]
    public Dictionary<string, TabelaInssAnual> TabelasHistoricasInss { get; set; } = new();
}
