namespace GPrev.Forms;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private Button btnImportarPdf;
    private Label lblArquivo;
    private GroupBox groupBoxFiltros;
    private DateTimePicker dtpDataFinal;
    private DateTimePicker dtpDataInicial;
    private Label lblDataFinal;
    private Label lblDataInicial;
    private Button btnFiltrar;
    private Button btnLimparFiltros;
    private TabControl tabControl;
    private TabPage tabPageLancamentos;
    private TabPage tabPageAgrupado;
    private DataGridView dgvContribuicoes;
    private DataGridView dgvAgrupado;
    private GroupBox groupBoxPaginacao;
    private Label lblPaginaInfo;
    private Button btnProximaPagina;
    private Button btnPaginaAnterior;
    private GroupBox groupBoxTotais;
    private Label lblTotalCorrigido;
    private Label lblTotalContribuicoes;
    private Label lblTotalExcedente;
    private Label lblTotalMeses;
    private Label lblContribuidor;
    private Label lblRangeDatas;
    private Label lblTotalContribuicoesCount;
    private CheckBox chkAptosRestituicao;
    private CheckBox chkExcedentesAptos;
    private CheckBox chkComExcedente;
    private ProgressBar progressBar;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel toolStripStatusLabel;

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        btnImportarPdf = new Button();
        lblArquivo = new Label();
        groupBoxFiltros = new GroupBox();
        dtpDataFinal = new DateTimePicker();
        dtpDataInicial = new DateTimePicker();
        lblDataFinal = new Label();
        lblDataInicial = new Label();
        btnFiltrar = new Button();
        btnLimparFiltros = new Button();
        chkAptosRestituicao = new CheckBox();
        chkExcedentesAptos = new CheckBox();
        chkComExcedente = new CheckBox();
        tabControl = new TabControl();
        tabPageLancamentos = new TabPage();
        dgvContribuicoes = new DataGridView();
        tabPageAgrupado = new TabPage();
        dgvAgrupado = new DataGridView();
        groupBoxPaginacao = new GroupBox();
        lblPaginaInfo = new Label();
        btnProximaPagina = new Button();
        btnPaginaAnterior = new Button();
        groupBoxTotais = new GroupBox();
        lblTotalCorrigido = new Label();
        lblTotalContribuicoes = new Label();
        lblTotalExcedente = new Label();
        lblTotalMeses = new Label();
        lblContribuidor = new Label();
        lblRangeDatas = new Label();
        lblTotalContribuicoesCount = new Label();
        progressBar = new ProgressBar();
        statusStrip = new StatusStrip();
        toolStripStatusLabel = new ToolStripStatusLabel();
        groupBoxFiltros.SuspendLayout();
        tabControl.SuspendLayout();
        tabPageLancamentos.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvContribuicoes).BeginInit();
        tabPageAgrupado.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvAgrupado).BeginInit();
        groupBoxPaginacao.SuspendLayout();
        groupBoxTotais.SuspendLayout();
        statusStrip.SuspendLayout();
        SuspendLayout();
        // 
        // btnImportarPdf
        // 
        btnImportarPdf.Location = new Point(14, 16);
        btnImportarPdf.Margin = new Padding(3, 4, 3, 4);
        btnImportarPdf.Name = "btnImportarPdf";
        btnImportarPdf.Size = new Size(137, 47);
        btnImportarPdf.TabIndex = 0;
        btnImportarPdf.Text = "Importar PDF";
        btnImportarPdf.UseVisualStyleBackColor = true;
        btnImportarPdf.Click += btnImportarPdf_Click;
        // 
        // lblArquivo
        // 
        lblArquivo.AutoSize = true;
        lblArquivo.ForeColor = SystemColors.GrayText;
        lblArquivo.Location = new Point(169, 29);
        lblArquivo.Name = "lblArquivo";
        lblArquivo.Size = new Size(203, 20);
        lblArquivo.TabIndex = 1;
        lblArquivo.Text = "Nenhum arquivo selecionado";
        // 
        // groupBoxFiltros
        // 
        groupBoxFiltros.Controls.Add(dtpDataFinal);
        groupBoxFiltros.Controls.Add(dtpDataInicial);
        groupBoxFiltros.Controls.Add(lblDataFinal);
        groupBoxFiltros.Controls.Add(lblDataInicial);
        groupBoxFiltros.Controls.Add(btnFiltrar);
        groupBoxFiltros.Controls.Add(btnLimparFiltros);
        groupBoxFiltros.Controls.Add(chkAptosRestituicao);
        groupBoxFiltros.Controls.Add(chkExcedentesAptos);
        groupBoxFiltros.Controls.Add(chkComExcedente);
        groupBoxFiltros.Location = new Point(14, 87);
        groupBoxFiltros.Margin = new Padding(3, 4, 3, 4);
        groupBoxFiltros.Name = "groupBoxFiltros";
        groupBoxFiltros.Padding = new Padding(3, 4, 3, 4);
        groupBoxFiltros.Size = new Size(1353, 107);
        groupBoxFiltros.TabIndex = 2;
        groupBoxFiltros.TabStop = false;
        groupBoxFiltros.Text = "Filtros";
        // 
        // dtpDataFinal
        // 
        dtpDataFinal.Format = DateTimePickerFormat.Short;
        dtpDataFinal.Location = new Point(155, 57);
        dtpDataFinal.Margin = new Padding(3, 4, 3, 4);
        dtpDataFinal.Name = "dtpDataFinal";
        dtpDataFinal.Size = new Size(114, 27);
        dtpDataFinal.TabIndex = 5;
        // 
        // dtpDataInicial
        // 
        dtpDataInicial.Format = DateTimePickerFormat.Short;
        dtpDataInicial.Location = new Point(7, 57);
        dtpDataInicial.Margin = new Padding(3, 4, 3, 4);
        dtpDataInicial.Name = "dtpDataInicial";
        dtpDataInicial.Size = new Size(114, 27);
        dtpDataInicial.TabIndex = 4;
        // 
        // lblDataFinal
        // 
        lblDataFinal.AutoSize = true;
        lblDataFinal.Location = new Point(155, 30);
        lblDataFinal.Name = "lblDataFinal";
        lblDataFinal.Size = new Size(79, 20);
        lblDataFinal.TabIndex = 3;
        lblDataFinal.Text = "Data Final:";
        // 
        // lblDataInicial
        // 
        lblDataInicial.AutoSize = true;
        lblDataInicial.Location = new Point(7, 30);
        lblDataInicial.Name = "lblDataInicial";
        lblDataInicial.Size = new Size(87, 20);
        lblDataInicial.TabIndex = 2;
        lblDataInicial.Text = "Data Inicial:";
        // 
        // btnFiltrar
        // 
        btnFiltrar.Location = new Point(304, 53);
        btnFiltrar.Margin = new Padding(3, 4, 3, 4);
        btnFiltrar.Name = "btnFiltrar";
        btnFiltrar.Size = new Size(86, 31);
        btnFiltrar.TabIndex = 1;
        btnFiltrar.Text = "Filtrar";
        btnFiltrar.UseVisualStyleBackColor = true;
        btnFiltrar.Click += btnFiltrar_Click;
        // 
        // btnLimparFiltros
        // 
        btnLimparFiltros.Location = new Point(407, 53);
        btnLimparFiltros.Margin = new Padding(3, 4, 3, 4);
        btnLimparFiltros.Name = "btnLimparFiltros";
        btnLimparFiltros.Size = new Size(114, 31);
        btnLimparFiltros.TabIndex = 0;
        btnLimparFiltros.Text = "Limpar Filtros";
        btnLimparFiltros.UseVisualStyleBackColor = true;
        btnLimparFiltros.Click += btnLimparFiltros_Click;
        // 
        // chkAptosRestituicao
        // 
        chkAptosRestituicao.AutoSize = true;
        chkAptosRestituicao.Location = new Point(540, 57);
        chkAptosRestituicao.Name = "chkAptosRestituicao";
        chkAptosRestituicao.Size = new Size(181, 24);
        chkAptosRestituicao.TabIndex = 6;
        chkAptosRestituicao.Text = "Aptos para Restituição";
        chkAptosRestituicao.UseVisualStyleBackColor = true;
        chkAptosRestituicao.CheckedChanged += chkAptosRestituicao_CheckedChanged;
        // 
        // chkExcedentesAptos
        // 
        chkExcedentesAptos.AutoSize = true;
        chkExcedentesAptos.Location = new Point(750, 57);
        chkExcedentesAptos.Name = "chkExcedentesAptos";
        chkExcedentesAptos.Size = new Size(201, 24);
        chkExcedentesAptos.TabIndex = 7;
        chkExcedentesAptos.Text = "Apenas Excedentes Aptos";
        chkExcedentesAptos.UseVisualStyleBackColor = true;
        chkExcedentesAptos.CheckedChanged += chkExcedentesAptos_CheckedChanged;
        // 
        // chkComExcedente
        // 
        chkComExcedente.AutoSize = true;
        chkComExcedente.Location = new Point(957, 57);
        chkComExcedente.Name = "chkComExcedente";
        chkComExcedente.Size = new Size(230, 24);
        chkComExcedente.TabIndex = 8;
        chkComExcedente.Text = "Competências com Excedente";
        chkComExcedente.UseVisualStyleBackColor = true;
        chkComExcedente.CheckedChanged += chkComExcedente_CheckedChanged;
        // 
        // tabControl
        // 
        tabControl.Controls.Add(tabPageLancamentos);
        tabControl.Controls.Add(tabPageAgrupado);
        tabControl.Location = new Point(14, 213);
        tabControl.Margin = new Padding(3, 4, 3, 4);
        tabControl.Name = "tabControl";
        tabControl.SelectedIndex = 0;
        tabControl.Size = new Size(1463, 450);
        tabControl.TabIndex = 3;
        tabControl.SelectedIndexChanged += tabControl_SelectedIndexChanged;
        // 
        // tabPageLancamentos
        // 
        tabPageLancamentos.Controls.Add(dgvContribuicoes);
        tabPageLancamentos.Location = new Point(4, 29);
        tabPageLancamentos.Name = "tabPageLancamentos";
        tabPageLancamentos.Padding = new Padding(3);
        tabPageLancamentos.Size = new Size(1455, 417);
        tabPageLancamentos.TabIndex = 0;
        tabPageLancamentos.Text = "Lançamentos";
        tabPageLancamentos.UseVisualStyleBackColor = true;
        // 
        // dgvContribuicoes
        // 
        dgvContribuicoes.AllowUserToAddRows = false;
        dgvContribuicoes.AllowUserToDeleteRows = false;
        dgvContribuicoes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvContribuicoes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvContribuicoes.Dock = DockStyle.Fill;
        dgvContribuicoes.Location = new Point(3, 3);
        dgvContribuicoes.Margin = new Padding(3, 4, 3, 4);
        dgvContribuicoes.MultiSelect = false;
        dgvContribuicoes.Name = "dgvContribuicoes";
        dgvContribuicoes.ReadOnly = true;
        dgvContribuicoes.RowHeadersWidth = 51;
        dgvContribuicoes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvContribuicoes.Size = new Size(1449, 411);
        dgvContribuicoes.TabIndex = 0;
        // 
        // tabPageAgrupado
        // 
        tabPageAgrupado.Controls.Add(dgvAgrupado);
        tabPageAgrupado.Location = new Point(4, 29);
        tabPageAgrupado.Name = "tabPageAgrupado";
        tabPageAgrupado.Padding = new Padding(3);
        tabPageAgrupado.Size = new Size(1455, 417);
        tabPageAgrupado.TabIndex = 1;
        tabPageAgrupado.Text = "Por Competência";
        tabPageAgrupado.UseVisualStyleBackColor = true;
        // 
        // dgvAgrupado
        // 
        dgvAgrupado.AllowUserToAddRows = false;
        dgvAgrupado.AllowUserToDeleteRows = false;
        dgvAgrupado.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvAgrupado.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvAgrupado.Dock = DockStyle.Fill;
        dgvAgrupado.Location = new Point(3, 3);
        dgvAgrupado.Margin = new Padding(3, 4, 3, 4);
        dgvAgrupado.MultiSelect = false;
        dgvAgrupado.Name = "dgvAgrupado";
        dgvAgrupado.ReadOnly = true;
        dgvAgrupado.RowHeadersWidth = 51;
        dgvAgrupado.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvAgrupado.Size = new Size(1449, 411);
        dgvAgrupado.TabIndex = 0;
        // 
        // groupBoxPaginacao
        // 
        groupBoxPaginacao.Controls.Add(lblPaginaInfo);
        groupBoxPaginacao.Controls.Add(btnProximaPagina);
        groupBoxPaginacao.Controls.Add(btnPaginaAnterior);
        groupBoxPaginacao.Location = new Point(14, 671);
        groupBoxPaginacao.Margin = new Padding(3, 4, 3, 4);
        groupBoxPaginacao.Name = "groupBoxPaginacao";
        groupBoxPaginacao.Padding = new Padding(3, 4, 3, 4);
        groupBoxPaginacao.Size = new Size(623, 153);
        groupBoxPaginacao.TabIndex = 4;
        groupBoxPaginacao.TabStop = false;
        groupBoxPaginacao.Text = "Paginação";
        // 
        // lblPaginaInfo
        // 
        lblPaginaInfo.AutoSize = true;
        lblPaginaInfo.Location = new Point(123, 33);
        lblPaginaInfo.Name = "lblPaginaInfo";
        lblPaginaInfo.Size = new Size(79, 20);
        lblPaginaInfo.TabIndex = 2;
        lblPaginaInfo.Text = "Página 0/0";
        // 
        // btnProximaPagina
        // 
        btnProximaPagina.Location = new Point(400, 27);
        btnProximaPagina.Margin = new Padding(3, 4, 3, 4);
        btnProximaPagina.Name = "btnProximaPagina";
        btnProximaPagina.Size = new Size(86, 33);
        btnProximaPagina.TabIndex = 1;
        btnProximaPagina.Text = "Próxima";
        btnProximaPagina.UseVisualStyleBackColor = true;
        btnProximaPagina.Click += btnProximaPagina_Click;
        // 
        // btnPaginaAnterior
        // 
        btnPaginaAnterior.Location = new Point(4, 27);
        btnPaginaAnterior.Margin = new Padding(3, 4, 3, 4);
        btnPaginaAnterior.Name = "btnPaginaAnterior";
        btnPaginaAnterior.Size = new Size(86, 33);
        btnPaginaAnterior.TabIndex = 0;
        btnPaginaAnterior.Text = "Anterior";
        btnPaginaAnterior.UseVisualStyleBackColor = true;
        btnPaginaAnterior.Click += btnPaginaAnterior_Click;
        // 
        // groupBoxTotais
        // 
        groupBoxTotais.Controls.Add(lblTotalCorrigido);
        groupBoxTotais.Controls.Add(lblTotalContribuicoes);
        groupBoxTotais.Controls.Add(lblTotalExcedente);
        groupBoxTotais.Controls.Add(lblTotalMeses);
        groupBoxTotais.Controls.Add(lblContribuidor);
        groupBoxTotais.Controls.Add(lblRangeDatas);
        groupBoxTotais.Controls.Add(lblTotalContribuicoesCount);
        groupBoxTotais.Location = new Point(643, 671);
        groupBoxTotais.Margin = new Padding(3, 4, 3, 4);
        groupBoxTotais.Name = "groupBoxTotais";
        groupBoxTotais.Padding = new Padding(3, 4, 3, 4);
        groupBoxTotais.Size = new Size(834, 153);
        groupBoxTotais.TabIndex = 5;
        groupBoxTotais.TabStop = false;
        groupBoxTotais.Text = "Resumo";
        // 
        // lblTotalCorrigido
        // 
        lblTotalCorrigido.AutoSize = true;
        lblTotalCorrigido.Location = new Point(506, 109);
        lblTotalCorrigido.Margin = new Padding(10, 0, 3, 0);
        lblTotalCorrigido.Name = "lblTotalCorrigido";
        lblTotalCorrigido.Size = new Size(164, 20);
        lblTotalCorrigido.TabIndex = 3;
        lblTotalCorrigido.Text = "Total Corrigido: R$ 0,00";
        // 
        // lblTotalContribuicoes
        // 
        lblTotalContribuicoes.AutoSize = true;
        lblTotalContribuicoes.Location = new Point(506, 53);
        lblTotalContribuicoes.Margin = new Padding(10, 0, 3, 0);
        lblTotalContribuicoes.Name = "lblTotalContribuicoes";
        lblTotalContribuicoes.Size = new Size(192, 20);
        lblTotalContribuicoes.TabIndex = 2;
        lblTotalContribuicoes.Text = "Total Contribuições: R$ 0,00";
        // 
        // lblTotalExcedente
        // 
        lblTotalExcedente.AutoSize = true;
        lblTotalExcedente.Location = new Point(506, 80);
        lblTotalExcedente.Margin = new Padding(10, 0, 3, 0);
        lblTotalExcedente.Name = "lblTotalExcedente";
        lblTotalExcedente.Size = new Size(169, 20);
        lblTotalExcedente.TabIndex = 6;
        lblTotalExcedente.Text = "Total Excedente: R$ 0,00";
        // 
        // lblTotalMeses
        // 
        lblTotalMeses.AutoSize = true;
        lblTotalMeses.Location = new Point(17, 109);
        lblTotalMeses.Margin = new Padding(10, 0, 3, 0);
        lblTotalMeses.Name = "lblTotalMeses";
        lblTotalMeses.Size = new Size(65, 20);
        lblTotalMeses.TabIndex = 1;
        lblTotalMeses.Text = "Meses: 0";
        // 
        // lblContribuidor
        // 
        lblContribuidor.AutoSize = true;
        lblContribuidor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblContribuidor.Location = new Point(17, 27);
        lblContribuidor.Name = "lblContribuidor";
        lblContribuidor.Size = new Size(183, 20);
        lblContribuidor.TabIndex = 0;
        lblContribuidor.Text = "Nenhum dado carregado";
        // 
        // lblRangeDatas
        // 
        lblRangeDatas.AutoSize = true;
        lblRangeDatas.Location = new Point(17, 53);
        lblRangeDatas.Name = "lblRangeDatas";
        lblRangeDatas.Size = new Size(135, 20);
        lblRangeDatas.TabIndex = 4;
        lblRangeDatas.Text = "Período: N/A - N/A";
        // 
        // lblTotalContribuicoesCount
        // 
        lblTotalContribuicoesCount.AutoSize = true;
        lblTotalContribuicoesCount.Location = new Point(17, 80);
        lblTotalContribuicoesCount.Name = "lblTotalContribuicoesCount";
        lblTotalContribuicoesCount.Size = new Size(115, 20);
        lblTotalContribuicoesCount.TabIndex = 5;
        lblTotalContribuicoesCount.Text = "Contribuições: 0";
        // 
        // progressBar
        // 
        progressBar.Location = new Point(14, 852);
        progressBar.Margin = new Padding(3, 4, 3, 4);
        progressBar.Name = "progressBar";
        progressBar.Size = new Size(1463, 10);
        progressBar.Style = ProgressBarStyle.Marquee;
        progressBar.TabIndex = 6;
        progressBar.Visible = false;
        // 
        // statusStrip
        // 
        statusStrip.ImageScalingSize = new Size(20, 20);
        statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel });
        statusStrip.Location = new Point(0, 874);
        statusStrip.Name = "statusStrip";
        statusStrip.Padding = new Padding(1, 0, 16, 0);
        statusStrip.Size = new Size(1489, 26);
        statusStrip.TabIndex = 7;
        statusStrip.Text = "statusStrip1";
        // 
        // toolStripStatusLabel
        // 
        toolStripStatusLabel.Name = "toolStripStatusLabel";
        toolStripStatusLabel.Size = new Size(53, 20);
        toolStripStatusLabel.Text = "Pronto";
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1489, 900);
        Controls.Add(statusStrip);
        Controls.Add(progressBar);
        Controls.Add(groupBoxTotais);
        Controls.Add(groupBoxPaginacao);
        Controls.Add(tabControl);
        Controls.Add(groupBoxFiltros);
        Controls.Add(lblArquivo);
        Controls.Add(btnImportarPdf);
        Margin = new Padding(3, 4, 3, 4);
        Name = "Form1";
        Text = "GPrev Suite - Análise CNIS";
        groupBoxFiltros.ResumeLayout(false);
        groupBoxFiltros.PerformLayout();
        tabControl.ResumeLayout(false);
        tabPageLancamentos.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvContribuicoes).EndInit();
        tabPageAgrupado.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvAgrupado).EndInit();
        groupBoxPaginacao.ResumeLayout(false);
        groupBoxPaginacao.PerformLayout();
        groupBoxTotais.ResumeLayout(false);
        groupBoxTotais.PerformLayout();
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
