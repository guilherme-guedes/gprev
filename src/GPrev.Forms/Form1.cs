using GPrev.Core.Dominio.Models;
using GPrev.Core.Dominio.Services;
using GPrev.Core.Dtos;

namespace GPrev.Forms;

public partial class Form1 : Form
{
    private readonly CnisServico _cnisServico;
    private readonly InssServico _inssServico;

    private Contribuinte? _contribuidorAtual;
    private List<CompetenciaDTO> _todasContribuicoes = new();
    private List<CompetenciaDTO> _contribuicoesFiltradas = new();
    private int _paginaAtual = 1;
    private const int ITENS_POR_PAGINA = 15;

    public Form1(CnisServico cnisServico, InssServico inssServico)
    {
        InitializeComponent();

        _cnisServico = cnisServico;
        _inssServico = inssServico;

        ConfigurarGrid();
        ConfigurarGridAgrupado();
        InicializarFiltros();
        AtualizarEstadoControles();
    }

    private void ConfigurarGrid()
    {
        dgvContribuicoes.Columns.Clear();
        dgvContribuicoes.Columns.Add("Indice", "#");
        dgvContribuicoes.Columns.Add("Data", "Data");
        dgvContribuicoes.Columns.Add("Empregador", "Empregador");
        dgvContribuicoes.Columns.Add("Remuneracao", "Remuneração");
        dgvContribuicoes.Columns.Add("Contribuicao", "Contribuição");
        dgvContribuicoes.Columns.Add("TipoContribuicao", "Tipo");

        dgvContribuicoes.Columns["Indice"]!.Width = 30;
        dgvContribuicoes.Columns["Data"]!.Width = 100;
        dgvContribuicoes.Columns["Empregador"]!.Width = 350;
        dgvContribuicoes.Columns["Remuneracao"]!.Width = 120;
        dgvContribuicoes.Columns["Contribuicao"]!.Width = 120;
        dgvContribuicoes.Columns["TipoContribuicao"]!.Width = 150;

        dgvContribuicoes.Columns["Remuneracao"]!.DefaultCellStyle.Format = "C2";
        dgvContribuicoes.Columns["Contribuicao"]!.DefaultCellStyle.Format = "C2";
        dgvContribuicoes.Columns["Data"]!.DefaultCellStyle.Format = "MM/yyyy";

        dgvContribuicoes.Columns["Indice"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvContribuicoes.Columns["Data"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvContribuicoes.Columns["TipoContribuicao"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
    }

    private void ConfigurarGridAgrupado()
    {
        dgvAgrupado.Columns.Clear();
        dgvAgrupado.Columns.Add("Competencia", "Competência");
        dgvAgrupado.Columns.Add("Remuneracao", "Remuneração");
        dgvAgrupado.Columns.Add("Contribuicao", "Total Contribuição");
        dgvAgrupado.Columns.Add("Excedente", "Excedente");
        dgvAgrupado.Columns.Add("Corrigido", "Excedente Corrigido");

        dgvAgrupado.Columns["Remuneracao"]!.DefaultCellStyle.Format = "C2";
        dgvAgrupado.Columns["Contribuicao"]!.DefaultCellStyle.Format = "C2";
        dgvAgrupado.Columns["Excedente"]!.DefaultCellStyle.Format = "C2";
        dgvAgrupado.Columns["Corrigido"]!.DefaultCellStyle.Format = "C2";
    }

    private void InicializarFiltros()
    {
        dtpDataInicial.Value = DateTime.Now.AddYears(-30);
        dtpDataFinal.Value = DateTime.Now;
    }

    private bool TemFiltrosAtivos()
    {
        var intervaloAtual = dtpDataFinal.Value.Subtract(dtpDataInicial.Value).Days;
        var intervaloMaximo = TimeSpan.FromDays(365 * 25).Days;
        return intervaloAtual < intervaloMaximo;
    }

    private async void btnImportarPdf_Click(object sender, EventArgs e)
    {
        using var openFileDialog = new OpenFileDialog
        {
            Title = "Selecionar arquivo PDF do CNIS",
            Filter = "Arquivos PDF (*.pdf)|*.pdf",
            FilterIndex = 1
        };

        if (openFileDialog.ShowDialog() != DialogResult.OK)
            return;

        await CarregarArquivoPdf(openFileDialog.FileName);
    }

    private async Task CarregarArquivoPdf(string caminhoArquivo)
    {
        try
        {
            MostrarProgresso("Carregando arquivo PDF...");

            var contribuidor = await _cnisServico.ExtrairDadosContribuidorAsync(caminhoArquivo);
                        
            _contribuidorAtual = contribuidor;

            var contribuicoesDTO = _inssServico.CalcularExcedentesContribuicoes(contribuidor);
            _todasContribuicoes = contribuicoesDTO.ToList();

            lblArquivo.Text = Path.GetFileName(caminhoArquivo);

            if (TemFiltrosAtivos())
                AplicarFiltros();
            else
            {
                _contribuicoesFiltradas = OrdenarPorDataDesc(_todasContribuicoes);
                _paginaAtual = 1;
                AtualizarVisualizacao();
            }

            var mesesUnicos = _todasContribuicoes.Select(c => (c.Data.Year, c.Data.Month)).Distinct().Count();

            toolStripStatusLabel.Text =
                $"Arquivo carregado. {_todasContribuicoes.Count} lançamentos, {mesesUnicos} meses distintos.";
            
        }
        catch (Exception ex)
        {
            _contribuidorAtual = null;
            _todasContribuicoes.Clear();
            _contribuicoesFiltradas.Clear();
            AtualizarVisualizacao();

            MessageBox.Show(ex.Message, "Erro ao importar PDF", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            EsconderProgresso();
        }
    }

    private void btnFiltrar_Click(object sender, EventArgs e)
    {
        AplicarFiltros();
    }

    private void btnLimparFiltros_Click(object sender, EventArgs e)
    {
        LimparFiltros();
    }

    private void chkAptosRestituicao_CheckedChanged(object sender, EventArgs e)
    {
        AplicarFiltros();
    }

    private void chkExcedentesAptos_CheckedChanged(object sender, EventArgs e)
    {
        AplicarFiltros();
    }

    private void chkComExcedente_CheckedChanged(object sender, EventArgs e)
    {
        AplicarFiltros();
    }


    private void btnPaginaAnterior_Click(object sender, EventArgs e)
    {
        if (_paginaAtual > 1)
        {
            _paginaAtual--;
            AtualizarVisualizacao();
        }
    }

    private void btnProximaPagina_Click(object sender, EventArgs e)
    {
        var totalPaginas = CalcularTotalPaginas();
        if (_paginaAtual < totalPaginas)
        {
            _paginaAtual++;
            AtualizarVisualizacao();
        }
    }

    private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
    {
        var naAbaLancamentos = tabControl.SelectedIndex == 0;
        
        if (naAbaLancamentos)
        {
            chkAptosRestituicao.Checked = false;
            chkExcedentesAptos.Checked = false;
            chkComExcedente.Checked = false;
            AplicarFiltros();
        }
        
        AtualizarEstadoControles();
    }

    private void AplicarFiltros()
    {
        if (_contribuidorAtual == null)
            return;

        _contribuicoesFiltradas = _inssServico.FiltrarPorPeriodo(
            _todasContribuicoes, 
            dtpDataInicial.Value, 
            dtpDataFinal.Value).ToList();

        if (chkAptosRestituicao.Checked)
        {
            _contribuicoesFiltradas = _inssServico.FiltrarAptosRestituicao(_contribuicoesFiltradas).ToList();
        }

        if (chkExcedentesAptos.Checked)
        {
            _contribuicoesFiltradas = _inssServico.FiltrarExcedentesAptos(_contribuicoesFiltradas).ToList();
        }

        if (chkComExcedente.Checked)
        {
            _contribuicoesFiltradas = _inssServico.FiltrarComExcedente(_contribuicoesFiltradas).ToList();
        }

        _contribuicoesFiltradas = OrdenarPorDataDesc(_contribuicoesFiltradas);

        _paginaAtual = 1;
        AtualizarVisualizacao();

        var filtros = new List<string>();
        
        if (chkAptosRestituicao.Checked)
            filtros.Add($"aptos para restituição");
                
        if (chkExcedentesAptos.Checked)
            filtros.Add($"excedentes aptos com valor pago");
                
        if (chkComExcedente.Checked)
            filtros.Add("competências com excedente");
                
        var filtroTexto = filtros.Count > 0 ? $" ({string.Join(" + ", filtros)})" : "";
        toolStripStatusLabel.Text = $"Filtro aplicado. {_contribuicoesFiltradas.Count} lançamentos no período{filtroTexto}.";
    }

    private void LimparFiltros()
    {
        dtpDataInicial.Value = DateTime.Now.AddYears(-30);
        dtpDataFinal.Value = DateTime.Now;
        chkAptosRestituicao.Checked = false;
        chkExcedentesAptos.Checked = false;
        chkComExcedente.Checked = false;

        if (_contribuidorAtual == null)
        {
            _contribuicoesFiltradas.Clear();
        }
        else
        {
            _contribuicoesFiltradas = OrdenarPorDataDesc(_todasContribuicoes);
        }

        _paginaAtual = 1;
        AtualizarVisualizacao();
    }

    private void AtualizarVisualizacao()
    {
        AtualizarGrid();
        AtualizarGridAgrupado();
        AtualizarTotais();
        AtualizarPaginacao();
        AtualizarEstadoControles();
    }

    private void AtualizarGrid()
    {
        dgvContribuicoes.Rows.Clear();

        var contribuicoesPagina = _contribuicoesFiltradas
            .Skip((_paginaAtual - 1) * ITENS_POR_PAGINA)
            .Take(ITENS_POR_PAGINA)
            .ToList();

        var indiceInicial = (_paginaAtual - 1) * ITENS_POR_PAGINA + 1;

        for (var i = 0; i < contribuicoesPagina.Count; i++)
        {
            var contribuicao = contribuicoesPagina[i];
            dgvContribuicoes.Rows.Add(
                indiceInicial + i,
                contribuicao.Data,
                contribuicao.Empregador,
                contribuicao.Remuneracao,
                contribuicao.Contribuicao,
                contribuicao.TipoContribuicao);
        }
    }

    private void AtualizarGridAgrupado()
    {
        dgvAgrupado.Rows.Clear();

        var agrupados = _contribuicoesFiltradas
            .GroupBy(c => new { c.Data.Year, c.Data.Month })
            .OrderByDescending(g => g.Key.Year).ThenByDescending(g => g.Key.Month)
            .Select(g => new
            {
                Competencia = new DateTime(g.Key.Year, g.Key.Month, 1),
                Remuneracao = g.Sum(c => c.Remuneracao),
                Contribuicao = g.Sum(c => c.Contribuicao),
                Excedente = g.Sum(c => c.ExcedenteTeto),
                Corrigido = g.Sum(c => c.ValorCorrigido)
            });

        foreach (var linha in agrupados)
        {
            dgvAgrupado.Rows.Add(
                linha.Competencia.ToString("MM/yyyy"),
                linha.Remuneracao,
                linha.Contribuicao,
                linha.Excedente,
                linha.Corrigido);
        }
    }

    private void AtualizarTotais()
    {
        if (_contribuidorAtual == null)
        {
            lblContribuidor.Text = "Nenhum dado carregado";
            ResetarTotaisVazios();
            return;
        }

        var nome = string.IsNullOrWhiteSpace(_contribuidorAtual.Nome)
            ? "Nome não identificado"
            : _contribuidorAtual.Nome;

        var extras = new List<string>(2);
        if (!string.IsNullOrWhiteSpace(_contribuidorAtual.CPF))
            extras.Add($"CPF {_contribuidorAtual.CPF}");
        if (!string.IsNullOrWhiteSpace(_contribuidorAtual.NIT))
            extras.Add($"NIT {_contribuidorAtual.NIT}");

        lblContribuidor.Text = extras.Count > 0 ? $"{nome}  |  {string.Join("  ·  ", extras)}" : nome;

        if (_contribuicoesFiltradas.Count > 0)
        {
            var totalFiltrado = _contribuicoesFiltradas.Sum(c => c.Remuneracao);
            var totalExcedente = _contribuicoesFiltradas.Sum(c => c.ExcedenteTeto);
            var totalCorrigidoExcedente = _contribuicoesFiltradas
                .Where(c => c.ExcedenteTeto > 0)
                .Sum(c => c.ValorCorrigido);
            var primeiraContribuicao = _contribuicoesFiltradas.Min(c => c.Data);
            var ultimaContribuicao = _contribuicoesFiltradas.Max(c => c.Data);
            var mesesUnicos = _contribuicoesFiltradas
                .Select(c => (c.Data.Year, c.Data.Month))
                .Distinct()
                .Count();

            lblTotalMeses.Text = $"Meses: {mesesUnicos}";
            lblTotalContribuicoes.Text = $"Total: {totalFiltrado:C2}";
            lblTotalCorrigido.Text = $"Excedente Corrigido: {totalCorrigidoExcedente:C2}";
            lblTotalExcedente.Text = $"Total Excedente: {totalExcedente:C2}";
            lblRangeDatas.Text = $"Período: {primeiraContribuicao:MM/yyyy} - {ultimaContribuicao:MM/yyyy}";
            lblTotalContribuicoesCount.Text = $"Lançamentos: {_contribuicoesFiltradas.Count}";
        }
        else
        {
            ResetarTotaisVazios();
        }
    }

    private static List<CompetenciaDTO> OrdenarPorDataDesc(IEnumerable<CompetenciaDTO> contribuicoes) =>
        contribuicoes.OrderByDescending(c => c.Data).ToList();

    private void ResetarTotaisVazios()
    {
        lblTotalMeses.Text = "Meses: 0";
        lblTotalContribuicoes.Text = "Total: R$ 0,00";
        lblTotalCorrigido.Text = "Excedente Corrigido: R$ 0,00";
        lblTotalExcedente.Text = "Total Excedente: R$ 0,00";
        lblRangeDatas.Text = "Período: N/A - N/A";
        lblTotalContribuicoesCount.Text = "Lançamentos: 0";
    }

    private void AtualizarPaginacao()
    {
        var totalPaginas = CalcularTotalPaginas();
        lblPaginaInfo.Text = totalPaginas == 0
            ? "Sem lançamentos"
            : $"Página {_paginaAtual}/{totalPaginas}";

        btnPaginaAnterior.Enabled = _paginaAtual > 1;
        btnProximaPagina.Enabled = _paginaAtual < totalPaginas && totalPaginas > 0;
    }

    private void AtualizarEstadoControles()
    {
        var temDados = _contribuidorAtual != null;
        var naAbaLancamentos = tabControl.SelectedIndex == 0;
        var naAbaCompetencia = tabControl.SelectedIndex == 1;

        groupBoxFiltros.Enabled = temDados;
        groupBoxPaginacao.Enabled = temDados && naAbaLancamentos;
        dgvContribuicoes.Enabled = temDados;
        dgvAgrupado.Enabled = temDados;
        
        chkAptosRestituicao.Enabled = temDados && naAbaCompetencia;
        chkExcedentesAptos.Enabled = temDados && naAbaCompetencia;
        chkComExcedente.Enabled = temDados && naAbaCompetencia;
    }

    private int CalcularTotalPaginas()
    {
        if (_contribuicoesFiltradas.Count == 0)
            return 0;

        return (int)Math.Ceiling((double)_contribuicoesFiltradas.Count / ITENS_POR_PAGINA);
    }

    private void MostrarProgresso(string mensagem)
    {
        progressBar.Visible = true;
        toolStripStatusLabel.Text = mensagem;
        btnImportarPdf.Enabled = false;
        progressBar.Refresh();
        statusStrip.Refresh();
    }

    private void EsconderProgresso()
    {
        progressBar.Visible = false;
        btnImportarPdf.Enabled = true;
    }
}
