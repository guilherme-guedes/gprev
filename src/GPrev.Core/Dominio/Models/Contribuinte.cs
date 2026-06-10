namespace GPrev.Core.Dominio.Models;

public class Contribuinte(string nit, string cpf, string nome)
{
    public string CPF { get; } = cpf;
    public string Nome { get; } = nome;
    public string NIT { get; } = nit;

    private List<RelacaoPrevidenciaria> _relacoesPrevidenciarias = new();
    public IReadOnlyList<RelacaoPrevidenciaria> RelacoesPrevidenciarias => _relacoesPrevidenciarias.AsReadOnly();

    public bool TemRelacoesPrevidenciaria() => _relacoesPrevidenciarias?.Count > 0;

    public bool AdicionarRelacaoPrevidenciaria(RelacaoPrevidenciaria relacaoPrevidenciaria)
    {
        ArgumentNullException.ThrowIfNull(relacaoPrevidenciaria);

        if (_relacoesPrevidenciarias.Contains(relacaoPrevidenciaria))
            return false;

        _relacoesPrevidenciarias.Add(relacaoPrevidenciaria);
        return true;
    }
}