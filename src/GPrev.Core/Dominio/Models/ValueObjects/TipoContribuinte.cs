using GPrev.Core.Dominio.Models.Enums;

namespace GPrev.Core.Dominio.Models.ValueObjects;

public readonly record struct TipoContribuinte
{
    private readonly ETipoContribuinte valor;

    private TipoContribuinte(ETipoContribuinte valor)
    {
        this.valor = valor;
    }

    public static readonly TipoContribuinte EmpregadoAgentePublico = new(ETipoContribuinte.Empregado_AgentePublico);
    public static readonly TipoContribuinte ContribuinteIndividual = new(ETipoContribuinte.Contribuinte_Individual);
    public static readonly TipoContribuinte EmpregadoDomestico = new(ETipoContribuinte.Empregado_Domestico);
    public static readonly TipoContribuinte TrabalhadorAvulso = new(ETipoContribuinte.Trabalhador_Avulso);
    public static readonly TipoContribuinte ContribuinteFacultativo = new(ETipoContribuinte.Contribuinte_Facultativo);
    public static readonly TipoContribuinte SeguradoEspecial = new(ETipoContribuinte.Segurado_Especial);

    public bool EhVinculoCLT() =>
        valor is ETipoContribuinte.Empregado_AgentePublico
            or ETipoContribuinte.Empregado_Domestico
            or ETipoContribuinte.Trabalhador_Avulso;

    public bool EhContribuinteIndividual() =>
        valor is ETipoContribuinte.Contribuinte_Individual
            or ETipoContribuinte.Contribuinte_Facultativo;

    public string ObterDescricao() => valor switch
    {
        ETipoContribuinte.Empregado_AgentePublico => "Empregado / Agente público",
        ETipoContribuinte.Contribuinte_Individual => "Contribuinte individual",
        ETipoContribuinte.Empregado_Domestico => "Empregado doméstico",
        ETipoContribuinte.Trabalhador_Avulso => "Trabalhador avulso",
        ETipoContribuinte.Contribuinte_Facultativo => "Contribuinte facultativo",
        ETipoContribuinte.Segurado_Especial => "Segurado especial",
        _ => valor.ToString()
    };

    public static TipoContribuinte Converter(string texto) =>
        TentarConverterInterno(texto) ?? throw new ArgumentException($"Tipo de contribuinte desconhecido: '{texto}'", nameof(texto));

    private static TipoContribuinte? TentarConverterInterno(string texto) => texto?.Trim() switch
    {
        var t when t?.StartsWith("Empregado ou Agente") == true => EmpregadoAgentePublico,
        "Contribuinte Individual" => ContribuinteIndividual,
        var t when t?.StartsWith("Empregado Dom") == true => EmpregadoDomestico,
        "Trabalhador Avulso" => TrabalhadorAvulso,
        "Contribuinte Facultativo" => ContribuinteFacultativo,
        "Segurado Especial" => SeguradoEspecial,
        _ => null
    };

    public static implicit operator ETipoContribuinte(TipoContribuinte tipo) => tipo.valor;

    public override string ToString() => ObterDescricao();
}