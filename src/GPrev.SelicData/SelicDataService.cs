using GPrev.SelicData.Model;
using System.Globalization;
using System.Text.Json;

namespace GPrev.SelicData;

public class SelicDataService
{
    private readonly Dictionary<string, decimal> _selicData;
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

    public SelicDataService()
    {
        var jsonPath = Path.Combine(AppContext.BaseDirectory, "selic-historica.json");
        var jsonContent = File.ReadAllText(jsonPath);
        var dados = JsonSerializer.Deserialize<SelicData[]>(jsonContent, _options)
            ?? throw new InvalidOperationException("Erro ao carregar dados da Selic.");

        _selicData = dados.ToDictionary(
            x => x.Data,
            x => decimal.Parse(x.Valor, CultureInfo.InvariantCulture));
    }

    public decimal? ObterSelic(int mes, int ano)
    {
        var chave = $"{mes:D2}/{ano}";
        return _selicData.TryGetValue(chave, out var selic) ? selic : null;
    }

    public decimal ObterSelic(Competencia competencia)
    {
        var selic = ObterSelic(competencia.Mes, competencia.Ano);
        if (selic is null)
            throw new InvalidOperationException("Dados não encontrados para a competência informada.");
        return selic.Value;
    }
}

internal class SelicData
{
    public string Data { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
}
