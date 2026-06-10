using System.Text;
using System.Text.RegularExpressions;

namespace GPrev.Core.Dominio.Infra;

public abstract class LeitorCnisAbstrato
{
    protected const int CapacidadeEstimadaPorPagina = 2048;

    protected static readonly string TextoRodapeInss =
        "O INSS poderá rever a qualquer tempo as informações constantes deste extrato, observados os arts.19 ao 19-F do RPS aprovado pelo Decreto 3.048/99.O segurado somente terá reconhecida como tempo de contribuição ao RGPS a competência cujo valor consolidado seja igual ou superior ao salário mínimo, sendo assegurados os ajustes de complementação, utilizaçãoou agrupamento, conforme o caso, de acordo com o § 14 do art.195 da CF/1988 e art.29 da EC 103/2019.";

    protected static readonly string[] TextosRemover =
    [
        "Relações Previdenciárias",
        "CompetênciaRemuneraçãoIndicadoresCompetênciaRemuneraçãoIndicadoresCompetênciaRemuneraçãoIndicadores",
        "CompetênciaContrat.EstabelecimentoTomadorForma Prestação ServiçoRemuneraçãoIndicadores",
        "Identificação do Filiado",
    ];

    protected static readonly Regex RegexCabecalhoPagina = new(
        @"INSS.+?Identificação do Filiado",
        RegexOptions.Compiled);

    public abstract ReadOnlyMemory<char> LerTextoCompleto(string caminhoArquivo);

    public abstract ReadOnlyMemory<char> LerTextoCompleto(Stream stream);

    internal static StringBuilder LimparTextoCompleto(ReadOnlyMemory<char> texto)
    {
        var str = texto.Trim().ToString();

        var idxValores = str.IndexOf("Valores Consolidados por Ano Civil", StringComparison.Ordinal);
        if (idxValores > 0)
            str = str[..idxValores];

        var textoLimpo = new StringBuilder(RegexCabecalhoPagina.Replace(str, ""));

        textoLimpo.Replace(TextoRodapeInss, "");

        foreach (var remover in TextosRemover)
            textoLimpo.Replace(remover, "");

        return textoLimpo;
    }
}