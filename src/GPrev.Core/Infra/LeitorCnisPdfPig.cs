using GPrev.Core.Dominio.Infra;
using System.Text;
using UglyToad.PdfPig;

namespace GPrev.Core.Infra;

public class LeitorCnisPdfPig : LeitorCnisAbstrato
{

    public override ReadOnlyMemory<char> LerTextoCompleto(string caminhoArquivo)
    {
        using var document = AbrirDocumento(caminhoArquivo);
        return ExtrairTextoSemLimpeza(document);
    }

    public override ReadOnlyMemory<char> LerTextoCompleto(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        using var document = PdfDocument.Open(stream);
        return ExtrairTextoSemLimpeza(document);
    }

    private static PdfDocument AbrirDocumento(string caminhoArquivo)
    {
        if (string.IsNullOrEmpty(caminhoArquivo))
            throw new ArgumentException("Caminho do arquivo não pode ser nulo ou vazio.", nameof(caminhoArquivo));

        if (!File.Exists(caminhoArquivo))
            throw new FileNotFoundException($"Arquivo não encontrado: {caminhoArquivo}");

        return PdfDocument.Open(caminhoArquivo);
    }

    private static ReadOnlyMemory<char> ExtrairTextoSemLimpeza(PdfDocument document)
    {
        var capacidade = Math.Max(document.NumberOfPages * CapacidadeEstimadaPorPagina, CapacidadeEstimadaPorPagina);
        var sb = new StringBuilder(capacidade);

        foreach (var pagina in document.GetPages())
        {
            sb.Append(pagina.Text);
            sb.Append('\n');
        }

        return sb.ToString().AsMemory();
    }
}
