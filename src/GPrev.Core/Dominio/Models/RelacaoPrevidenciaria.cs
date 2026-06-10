using GPrev.Core.Dominio.Models.ValueObjects;

namespace GPrev.Core.Dominio.Models;

public class RelacaoPrevidenciaria(
    Origem origem,
    TipoContribuinte tipo,
    string dataInicio,
    string dataFim,
    string ultimaRemuneracao,
    List<RegistroRemuneracao> registrosRemuneracao = null!) : IEquatable<RelacaoPrevidenciaria>
{
    public Origem Origem { get; init; } = origem;
    public TipoContribuinte Tipo { get; init; } = tipo;
    public string DataInicio { get; init; } = dataInicio;
    public string DataFim { get; init; } = dataFim;
    public string UltimaRemuneracao { get; init; } = ultimaRemuneracao;
    public IReadOnlyList<RegistroRemuneracao> RegistrosRemuneracao { get; init; } = 
        registrosRemuneracao?.AsReadOnly() ?? new List<RegistroRemuneracao>().AsReadOnly();

    public bool Equals(RelacaoPrevidenciaria? outra)
    {
        if (outra is null) return false;
        if (ReferenceEquals(this, outra)) return true;
        
        return DataInicio == outra.DataInicio &&
               DataFim == outra.DataFim &&
               Origem.Codigo == outra.Origem.Codigo;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((RelacaoPrevidenciaria)obj);
    }

    public override int GetHashCode() => HashCode.Combine(DataInicio, DataFim, Origem.Codigo);
    
    public static bool operator ==(RelacaoPrevidenciaria? left, RelacaoPrevidenciaria? right) => Equals(left, right);
    
    public static bool operator !=(RelacaoPrevidenciaria? left, RelacaoPrevidenciaria? right) => !Equals(left, right);
    
}