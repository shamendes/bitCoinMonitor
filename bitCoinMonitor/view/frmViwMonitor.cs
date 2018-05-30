using bitCoinMonitor.api.objetos_tapi;
using bitCoinMonitor.control;
using bitCoinMonitor.tools;
using bitCoinMonitor.view;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace bitCoinMonitor
{
    public partial class frmViwMonitor : Form
    {
        private clsCtrMonitor _ObjMonitor;
        private clsCtrOperacao _ObjMovimento;
        private int _DiasLimite = 1;
        private bool _Bloqueado;
        private DateTime _DataAnoResultado = DateTime.Now;
        private DateTime _DataMesResultado = DateTime.Now;
        private enum enumTipoResultado { Mensal, Anual, Ambos };
        private Point? _PrevPosition = null;
        private ToolTip _ToolTip = new ToolTip();
        private DataPoint _PontoAnterior;


        public frmViwMonitor()
        {
            InitializeComponent();
        }

        private void frmViwMonitor_Load(object sender, EventArgs e)
        {
            this.cmbPeriodoHistorico.SelectedIndex = 0;
            this.tbpLogErro.Parent = null;

            this._ObjMovimento = new clsCtrOperacao();

            this._ObjMonitor = new clsCtrMonitor();
            this._ObjMonitor.pIntervalo = 30000;
            this._ObjMonitor.pDataInicio = DateTime.Now.AddDays(this._DiasLimite * -1);
            this._ObjMonitor.pDataFim = DateTime.Now.AddSeconds(this._ObjMonitor.pIntervalo / 1000);

            if (Program.Parametros.pMrcNegociacaoAtiva) this.btoNegociacao_Click_1(sender, e);
            this._Bloqueado = Program.Parametros.pPossuiSenha;
            this.btnLockUnlock.Image = (this._Bloqueado) ? Properties.Resources.lock_2 : Properties.Resources.unlock_3;

            this._ObjMonitor.iniciar();
        }

        //--Função principal
        private void atualizarMonitor(object sender, EventArgs e)
        {
            int vIntValorBarraProgresso = 0;

            try
            {
                //--Verificando se o ObjetoMonitor deu erro;
                if (this._ObjMonitor.pErro != null)
                {
                    this.adicionarErroLog(this._ObjMonitor.pErro);
                    this._ObjMonitor.pErro = null;                    
                }
                
                //--Atualizando o período a ser consultado
                this._ObjMonitor.pDataInicio = DateTime.Now.AddDays(this._DiasLimite * -1);
                this._ObjMonitor.pDataFim = DateTime.Now.AddSeconds(this._ObjMonitor.pIntervalo / 1000);
                

                //--Atualizando o valor de compra e venda a ser negociado
                this._ObjMonitor.pValorCompra = Convert.ToDecimal(this.txtNegCompra.Text);
                this._ObjMonitor.pValorVenda = Convert.ToDecimal(this.txtNegVenda.Text);
                this._ObjMonitor.pPeriodoMaxMin = this.trbDias.Value;

                if (this._ObjMonitor.pObjTickerAtual != null)
                {
                    this.panConectando.Visible = false;

                    //--Preenchendo os labels 
                    this.lblValorCompra.Text = this._ObjMonitor.pObjTickerAtual.ticker.sell.ToString("C");
                    this.lblValorData.Text = this._ObjMonitor.pObjTickerAtual.ticker.verDataHora().ToString();
                    this.lblValorMaior.Text = this._ObjMonitor.pObjTickerAtual.ticker.high.ToString("C");
                    this.lblValorMenor.Text = this._ObjMonitor.pObjTickerAtual.ticker.low.ToString("C");
                    this.lblValorUltimo.Text = this._ObjMonitor.pObjTickerAtual.ticker.last.ToString("C");
                    this.lblValorVenda.Text = this._ObjMonitor.pObjTickerAtual.ticker.buy.ToString("C");
                    this.lblValorVolume.Text = this._ObjMonitor.pObjTickerAtual.ticker.vol.ToString();
                    this.lblVlrBitstamp.Text = this._ObjMonitor.pObjTickerBSAtual.ticker.last.ToString("C");

                    //--Preenchendo dados da carteira
                    this.lblValorMoeda.Text = Program.Carteira.pSaldoMoeda.ToString("C");
                    this.lblValorBitcoins.Text = Program.Carteira.pSaldoBitcoins.ToString("0.#####");

                    //--Carregando os dados de resultado apenas quando ocorrer algum evento
                    if (this.lblValorAtividade.Text != this.retornaAtividadeMonitor())
                        this.preencherResultado(enumTipoResultado.Ambos);

                    //--Preenchendo a atividade do monitor
                    this.lblValorAtividade.Text = this.retornaAtividadeMonitor();

                    //--Atualiando o texto do ícone do SystemTray
                    this.notifyIcon1.Text = "Compra: " + this._ObjMonitor.pObjTickerAtual.ticker.sell.ToString("C") + " / Venda: " + this._ObjMonitor.pObjTickerAtual.ticker.buy.ToString("C");

                    //--Preenchendo os DataGrids
                    this.dgvCompraVenda.DataSource = this._ObjMovimento.listarOperacoes(new DateTime(1984, 03, 27), new DateTime(2150, 12, 31));

                    //--Atualizando o Gráfico
                    this.preencherGrafico();

                    //--Atualizando o livro de ordens:
                    this.preencherLivroOrdens();

                    //--Habilitando ou não o botão de enviar Ordem:
                    this.btoEnviarOrdem.Enabled = (this._ObjMonitor.pAcaoAtual == clsCtrMonitor.enumAcao.MonitorandoCompra || this._ObjMonitor.pAcaoAtual == clsCtrMonitor.enumAcao.MonitorandoVenda);

                    //--Habilitando ou não o botão de cancelar Ordem
                    this.controlarBtoCancelar();

                    //--Preenchendo os valores máximos e mínimos da aba de operações
                    this.lblVlrMaiorPeriodo.Text = this._ObjMonitor.pObjTickerMaxMin.ticker.high.ToString("C"); 
                    this.lblVlrMenorPeriodo.Text = this._ObjMonitor.pObjTickerMaxMin.ticker.low.ToString("C");
                    this.lblVlrMaiorPeriodoBS.Text = this._ObjMonitor.pObjTickerMaxMinBS.ticker.high.ToString("C");
                    this.lblVlrMenorPeriodoBS.Text = this._ObjMonitor.pObjTickerMaxMinBS.ticker.low.ToString("C");


                    //--Atualizando a barra de distância do Máximo:                    
                    if ((this._ObjMonitor.pObjTickerMaxMin.ticker.high - this._ObjMonitor.pObjTickerMaxMin.ticker.low) > 0)
                        vIntValorBarraProgresso = Convert.ToInt16((this._ObjMonitor.pObjTickerAtual.ticker.last - this._ObjMonitor.pObjTickerMaxMin.ticker.low) / (this._ObjMonitor.pObjTickerMaxMin.ticker.high - this._ObjMonitor.pObjTickerMaxMin.ticker.low) * 100);
                    if(vIntValorBarraProgresso < 0) vIntValorBarraProgresso = 0;
                    if (vIntValorBarraProgresso > 100) vIntValorBarraProgresso = 100;
                    this.pgbDistanciaMinMax.Value = vIntValorBarraProgresso;
                    this.lblValorBarra.Text = this.pgbDistanciaMinMax.Value.ToString() + "%";
                }

            }
            catch(Exception ex)
            {
                this.adicionarErroLog(ex);
            }
        }                               
        
        //--System Tray
        private void frmViwMonitor_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                this.notifyIcon1.Visible = true;
            }
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        //--DataGrids
        private void dgvCompraVenda_DataSourceChanged(object sender, EventArgs e)
        {
            try
            {
                this.dgvCompraVenda.Columns[0].HeaderText = "ID";
                this.dgvCompraVenda.Columns[1].HeaderText = "Tipo";
                this.dgvCompraVenda.Columns[2].HeaderText = "Status";
                this.dgvCompraVenda.Columns[3].HeaderText = "Criação";
                this.dgvCompraVenda.Columns[4].HeaderText = "Atualização";
                this.dgvCompraVenda.Columns[5].HeaderText = "ID Consulta";
                this.dgvCompraVenda.Columns[6].HeaderText = "Valor Limite";
                this.dgvCompraVenda.Columns[7].HeaderText = "Qtd.";
                this.dgvCompraVenda.Columns[8].HeaderText = "Qtd. Executada";
                this.dgvCompraVenda.Columns[9].HeaderText = "Valor Médio Exec.";
                this.dgvCompraVenda.Columns[10].HeaderText = "Valor Taxa";

                
                this.dgvCompraVenda.Columns[5].Visible = false;
                
                this.dgvCompraVenda.Columns[0].CellTemplate.ValueType = typeof(int);
                this.dgvCompraVenda.Columns[1].CellTemplate.ValueType = typeof(string);
                this.dgvCompraVenda.Columns[2].CellTemplate.ValueType = typeof(int);
                this.dgvCompraVenda.Columns[3].CellTemplate.ValueType = typeof(DateTime);
                this.dgvCompraVenda.Columns[4].CellTemplate.ValueType = typeof(DateTime);
                this.dgvCompraVenda.Columns[5].CellTemplate.ValueType = typeof(int);
                this.dgvCompraVenda.Columns[6].CellTemplate.ValueType = typeof(decimal);
                this.dgvCompraVenda.Columns[7].CellTemplate.ValueType = typeof(double);
                this.dgvCompraVenda.Columns[8].CellTemplate.ValueType = typeof(double);
                this.dgvCompraVenda.Columns[9].CellTemplate.ValueType = typeof(decimal);
                this.dgvCompraVenda.Columns[10].CellTemplate.ValueType = typeof(double);

                
                this.dgvCompraVenda.Columns[3].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
                this.dgvCompraVenda.Columns[4].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
                this.dgvCompraVenda.Columns[6].DefaultCellStyle.Format = "C";
                this.dgvCompraVenda.Columns[9].DefaultCellStyle.Format = "C";


                this.dgvCompraVenda.Columns[0].Width = 90;
                this.dgvCompraVenda.Columns[1].Width = 80;
                this.dgvCompraVenda.Columns[2].Width = 90;
                this.dgvCompraVenda.Columns[3].Width = 130;
                this.dgvCompraVenda.Columns[4].Width = 130;
                this.dgvCompraVenda.Columns[6].Width = 110;
                this.dgvCompraVenda.Columns[7].Width = 65;
                this.dgvCompraVenda.Columns[8].Width = 125;
                this.dgvCompraVenda.Columns[9].Width = 145;
                this.dgvCompraVenda.Columns[10].Width = 95;

                this.dgvCompraVenda.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dgvCompraVenda.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dgvCompraVenda.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dgvCompraVenda.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dgvCompraVenda.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dgvCompraVenda.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dgvCompraVenda.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dgvCompraVenda.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dgvCompraVenda.Columns[9].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dgvCompraVenda.Columns[10].SortMode = DataGridViewColumnSortMode.NotSortable;

            }
            catch (Exception ex)
            {
                this.adicionarErroLog(ex);
            }
        }
        private void dgvOrderBookCompra_DataSourceChanged(object sender, EventArgs e)
        {
            this.dgvOrderBookCompra.Columns[0].HeaderText = "ID";
            this.dgvOrderBookCompra.Columns[1].HeaderText = "Qtd Bitcoins";
            this.dgvOrderBookCompra.Columns[2].HeaderText = "Minha";
            this.dgvOrderBookCompra.Columns[3].HeaderText = "Preço Limite";


            this.dgvOrderBookCompra.Columns[0].CellTemplate.ValueType = typeof(int);
            this.dgvOrderBookCompra.Columns[1].CellTemplate.ValueType = typeof(double);
            this.dgvOrderBookCompra.Columns[2].CellTemplate.ValueType = typeof(bool);
            this.dgvOrderBookCompra.Columns[3].CellTemplate.ValueType = typeof(decimal);


            this.dgvOrderBookCompra.Columns[1].DefaultCellStyle.Format = "N5";
            this.dgvOrderBookCompra.Columns[3].DefaultCellStyle.Format = "C";

            this.dgvOrderBookCompra.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            this.dgvOrderBookCompra.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            this.dgvOrderBookCompra.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            this.dgvOrderBookCompra.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            this.dgvOrderBookCompra.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dgvOrderBookCompra.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dgvOrderBookCompra.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dgvOrderBookCompra.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;

            this.dgvOrderBookCompra.Columns[0].Width = 109;
            this.dgvOrderBookCompra.Columns[1].Width = 109;
            this.dgvOrderBookCompra.Columns[2].Width = 109;
            this.dgvOrderBookCompra.Columns[3].Width = 109;


        }
        private void dgvOrderBookVenda_DataSourceChanged(object sender, EventArgs e)
        {
            this.dgvOrderBookVenda.Columns[0].HeaderText = "ID";
            this.dgvOrderBookVenda.Columns[1].HeaderText = "Qtd Bitcoins";
            this.dgvOrderBookVenda.Columns[2].HeaderText = "Minha";
            this.dgvOrderBookVenda.Columns[3].HeaderText = "Preço Limite";


            this.dgvOrderBookVenda.Columns[0].CellTemplate.ValueType = typeof(int);
            this.dgvOrderBookVenda.Columns[1].CellTemplate.ValueType = typeof(double);
            this.dgvOrderBookVenda.Columns[2].CellTemplate.ValueType = typeof(bool);
            this.dgvOrderBookVenda.Columns[3].CellTemplate.ValueType = typeof(decimal);


            this.dgvOrderBookVenda.Columns[1].DefaultCellStyle.Format = "N5";
            this.dgvOrderBookVenda.Columns[3].DefaultCellStyle.Format = "C";

            this.dgvOrderBookVenda.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            this.dgvOrderBookVenda.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            this.dgvOrderBookVenda.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            this.dgvOrderBookVenda.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            this.dgvOrderBookVenda.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dgvOrderBookVenda.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dgvOrderBookVenda.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dgvOrderBookVenda.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;

            this.dgvOrderBookVenda.Columns[0].Width = 109;
            this.dgvOrderBookVenda.Columns[1].Width = 109;
            this.dgvOrderBookVenda.Columns[2].Width = 109;
            this.dgvOrderBookVenda.Columns[3].Width = 109;
        }
        private void dgvResultado_DataSourceChanged(object sender, EventArgs e)
        {
            this.dgvResultado.Columns[0].HeaderText = "Data";
            this.dgvResultado.Columns[1].HeaderText = "Investido";
            this.dgvResultado.Columns[2].HeaderText = "Recebido";
            this.dgvResultado.Columns[3].HeaderText = "Taxa";

            this.dgvResultado.Columns[0].CellTemplate.ValueType = typeof(DateTime);
            this.dgvResultado.Columns[1].CellTemplate.ValueType = typeof(decimal);
            this.dgvResultado.Columns[2].CellTemplate.ValueType = typeof(decimal);
            this.dgvResultado.Columns[3].CellTemplate.ValueType = typeof(decimal);

            this.dgvResultado.Columns[0].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            this.dgvResultado.Columns[1].DefaultCellStyle.Format = "C";
            this.dgvResultado.Columns[2].DefaultCellStyle.Format = "C";
            this.dgvResultado.Columns[3].DefaultCellStyle.Format = "P2";

            this.dgvResultado.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dgvResultado.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dgvResultado.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dgvResultado.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dgvResultado.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dgvResultado.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.dgvResultado.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            this.dgvResultado.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            this.dgvResultado.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            this.dgvResultado.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            this.dgvResultado.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dgvResultado.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dgvResultado.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dgvResultado.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;

            this.dgvResultado.Columns[0].Width = 120;
            this.dgvResultado.Columns[1].Width = 116;
            this.dgvResultado.Columns[2].Width = 116;
            this.dgvResultado.Columns[3].Width = 100;

        }
        private void dgvRentabilidade_DataSourceChanged(object sender, EventArgs e)
        {
            this.dgvRentabilidade.Columns[1].HeaderText = "Mês";
            this.dgvRentabilidade.Columns[2].HeaderText = "%";

            this.dgvRentabilidade.Columns[0].Visible = false;

            this.dgvRentabilidade.Columns[1].CellTemplate.ValueType = typeof(string);
            this.dgvRentabilidade.Columns[2].CellTemplate.ValueType = typeof(double);

            this.dgvRentabilidade.Columns[2].DefaultCellStyle.Format = "P2";
            this.dgvRentabilidade.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dgvRentabilidade.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.dgvRentabilidade.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            this.dgvRentabilidade.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            this.dgvRentabilidade.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dgvRentabilidade.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;

            this.dgvRentabilidade.Columns[1].Width = 116;
            this.dgvRentabilidade.Columns[2].Width = 80;

        }


        //--Demais preenchimentos
        private void preencherGrafico()
        {
            try
            {
                if (this._ObjMonitor?.pObjHistorico == null)
                    return;

                if (this._ObjMonitor.pObjHistorico.Rows.Count > 0)
                {
                    this.graHistorico.DataSource = this._ObjMonitor.pObjHistorico;

                    this.graHistorico.Series["Vlr_Compra"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
                    this.graHistorico.Series["Vlr_Compra"].XValueMember = "dat_consulta";
                    this.graHistorico.Series["Vlr_Compra"].YValueMembers = "vlr_compra";
                    this.graHistorico.Series["Vlr_Venda"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
                    this.graHistorico.Series["Vlr_Venda"].XValueMember = "dat_consulta";
                    this.graHistorico.Series["Vlr_Venda"].YValueMembers = "Vlr_Venda";
                    this.graHistorico.Series["Vlr_Maximo"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
                    this.graHistorico.Series["Vlr_Maximo"].XValueMember = "dat_consulta";
                    this.graHistorico.Series["Vlr_Maximo"].YValueMembers = "Vlr_Maximo";
                    this.graHistorico.Series["Vlr_Minimo"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
                    this.graHistorico.Series["Vlr_Minimo"].XValueMember = "dat_consulta";
                    this.graHistorico.Series["Vlr_Minimo"].YValueMembers = "Vlr_Minimo";

                    //--Definindo características do eixo X
                    this.graHistorico.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "dd/MM HH:mm";
                    this.graHistorico.ChartAreas["ChartArea1"].AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Minutes;

                    //this.graHistorico.ChartAreas["ChartArea1"].AxisX.Minimum = Convert.ToDateTime(this._ObjMonitor.pObjHistorico.Rows[this._ObjMonitor.pObjHistorico.Rows.Count - 1]["dat_consulta"]).ToOADate();
                    //this.graHistorico.ChartAreas["ChartArea1"].AxisX.Maximum = Convert.ToDateTime(this._ObjMonitor.pObjHistorico.Rows[0]["dat_consulta"]).ToOADate();

                    //--Definindo características do eixo y
                    this.graHistorico.ChartAreas["ChartArea1"].AxisY.LabelStyle.Format = "N2";
                    this.graHistorico.ChartAreas["ChartArea1"].AxisY.LabelStyle.Interval = 1000;
                    this.graHistorico.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(this._ObjMonitor.pObjHistorico.Rows[0]["vlr_minimo"]) - 2000;
                    this.graHistorico.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(this._ObjMonitor.pObjHistorico.Rows[0]["vlr_maximo"]) + 2000;


                }
            }
            catch (Exception ex)
            {
                this.adicionarErroLog(ex);
            }
        }
        private void preencherResultado(enumTipoResultado aEnumTipoResultado)
        {
            clsCtrResultado vObjResultado;

            try
            {
                vObjResultado = new clsCtrResultado();

                if (aEnumTipoResultado == enumTipoResultado.Ambos || aEnumTipoResultado == enumTipoResultado.Anual)
                {
                    vObjResultado.carregarValoresAnual(this._DataAnoResultado.Year);
                    this.lblAnoResultado.Text = this._DataAnoResultado.Year.ToString();
                    this.preencherGraficoPatrimonio(vObjResultado.pObjDadosAnual);
                    this.dgvRentabilidade.DataSource = vObjResultado.pObjDadosRentabilidade;
                }
                if (aEnumTipoResultado == enumTipoResultado.Ambos || aEnumTipoResultado == enumTipoResultado.Mensal)
                {
                    vObjResultado.carregarValoresMensal(this._DataMesResultado.Year, this._DataMesResultado.Month);
                    this.dgvResultado.DataSource = vObjResultado.pObjDadosMensal;
                    this.lblMesAnoResultado.Text = clsTooUtil.buscarNomeMes(this._DataMesResultado.Month) + " - " + this._DataMesResultado.Year.ToString();
                    this.preencherGraficoGanhosPerdas(vObjResultado.pGanhos, vObjResultado.pPerdas);
                    this.preencherGraficoTaxas(vObjResultado.pObjDadosTaxas);
                }
                

            }
            catch (Exception ex)
            {
                this.adicionarErroLog(ex);
            }
        }
        private void preencherGraficoPatrimonio(DataTable aObjDados)
        {
            try
            {
                this.graPatrimonio.DataSource = aObjDados;
                this.graPatrimonio.Series["vlr_patrimonio"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
                this.graPatrimonio.Series["vlr_patrimonio"].XValueMember = "dat_ordem";
                this.graPatrimonio.Series["vlr_patrimonio"].YValueMembers = "vlr_recebido";
                
                //--Definindo características do eixo X
                this.graPatrimonio.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "dd/MM";
                this.graPatrimonio.ChartAreas["ChartArea1"].AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Weeks;

                //--Definindo características do eixo y
                this.graHistorico.ChartAreas["ChartArea1"].AxisY.LabelStyle.Format = "N2";


            }
            catch (Exception ex)
            {
                this.adicionarErroLog(ex);
            }
        }
        private void preencherGraficoGanhosPerdas(decimal aDecGanho, decimal aDecPerda)
        {
            try
            {
                this.graGanhosPerdas.Series["vlr_ganho"].Points.Clear();
                this.graGanhosPerdas.Series["vlr_perda"].Points.Clear();
                this.graGanhosPerdas.Series["vlr_ganho"].Points.AddY(aDecGanho);
                this.graGanhosPerdas.Series["vlr_perda"].Points.AddY(aDecPerda);
                this.graGanhosPerdas.ChartAreas["ChartArea1"].AxisY.LabelStyle.Format = "N2";
            }
            catch (Exception ex)
            {
                this.adicionarErroLog(ex);
            }
        }
        private void preencherGraficoTaxas(DataTable aObjDados)
        {
            try
            {
                this.graTaxas.Series.Clear();

                //--Adicioando a Série
                this.graTaxas.Series.Add("Valores");
                //--Configurando para pizza
                this.graTaxas.Series["Valores"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

                //--Adicioando os valores
                for (int i = 0; i < aObjDados.Rows.Count; i++)
                    this.graTaxas.Series["Valores"].Points.AddXY(Convert.ToDouble(aObjDados.Rows[i]["VLR_TAXA"]).ToString("P2"), Convert.ToInt16(aObjDados.Rows[i]["QTD_VEZES"]));
            }
            catch (Exception ex)
            {
                this.adicionarErroLog(ex);
            }
        }
        private void preencherLivroOrdens()
        {
            decimal vDecTotalCompras = 0;
            decimal vDecTotalVendas = 0;

            if (this._ObjMonitor == null) return;
            if (this._ObjMonitor.pObjOrderBookAtual == null) return;
            if (this._ObjMonitor.pObjOrderBookAtual.response_data == null) return;
            if (this._ObjMonitor.pObjOrderBookAtual.response_data.orderbook == null) return;

            DataTable vObjDadosCompra = new DataTable();
            DataTable vObjDadosVenda = new DataTable();
            DataRow vObjLinha;

            vObjDadosCompra.Columns.Add("order_id", typeof(int));
            vObjDadosCompra.Columns.Add("quantity", typeof(decimal));
            vObjDadosCompra.Columns.Add("is_owner", typeof(bool));
            vObjDadosCompra.Columns.Add("limit_price", typeof(decimal));

            vObjDadosVenda = vObjDadosCompra.Clone();

            foreach (clsApiOrderbook_bids_asks_data vObj in this._ObjMonitor.pObjOrderBookAtual.response_data.orderbook.bids)
            {
                vObjLinha = vObjDadosCompra.NewRow();
                vObjLinha["order_id"] = vObj.order_id;
                vObjLinha["quantity"] = clsTooUtil.converterStringDecimal_US(vObj.quantity);
                vObjLinha["is_owner"] = vObj.is_owner;
                vObjLinha["limit_price"] = clsTooUtil.converterStringDecimal_US(vObj.limit_price);
                vObjDadosCompra.Rows.Add(vObjLinha);

                //--Totalizador
                vDecTotalCompras += clsTooUtil.converterStringDecimal_US(vObj.quantity);
            }
            foreach (clsApiOrderbook_bids_asks_data vObj in this._ObjMonitor.pObjOrderBookAtual.response_data.orderbook.asks)
            {
                vObjLinha = vObjDadosVenda.NewRow();
                vObjLinha["order_id"] = vObj.order_id;
                vObjLinha["quantity"] = clsTooUtil.converterStringDecimal_US(vObj.quantity);
                vObjLinha["is_owner"] = vObj.is_owner;
                vObjLinha["limit_price"] = clsTooUtil.converterStringDecimal_US(vObj.limit_price);
                vObjDadosVenda.Rows.Add(vObjLinha);

                //--Totalizador
                vDecTotalVendas += clsTooUtil.converterStringDecimal_US(vObj.quantity);
            }

            this.dgvOrderBookCompra.DataSource = vObjDadosCompra;
            this.dgvOrderBookVenda.DataSource = vObjDadosVenda;
            this.lblTotalCompras.Text = "Quantidade Total: " + vDecTotalCompras.ToString();
            this.lblTotalVendas.Text = "Quantidade Total: " + vDecTotalVendas.ToString();


        }             
                
        //--Ações do Menu
        private void mnuParametros_Click(object sender, EventArgs e)
        {
            if (this._Bloqueado) return;
            view.frmViwParametros vObjForm = new view.frmViwParametros();
            vObjForm.ShowDialog();
        }
        private void mnuAlterarSenha_Click(object sender, EventArgs e)
        {
            if (this._Bloqueado) return;
            view.frmViwAlterarSenha vObjForm = new view.frmViwAlterarSenha();
            vObjForm.ShowDialog();
        }
        private void mnuSobre_Click(object sender, EventArgs e)
        {            
            view.frmViwSobre vObjForm = new view.frmViwSobre();
            vObjForm.ShowDialog();
        }
        private void mnuSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //--Ações dos botões
        private void btoApagarLogErro_Click(object sender, EventArgs e)
        {
            if (this._Bloqueado) return;
            this.txtLogErro.Text = String.Empty;
            this.tbpLogErro.Parent = null;
        }
        private void btoEnviarOrdem_Click(object sender, EventArgs e)
        {
            if (this._Bloqueado) return;
            try
            {
                if (MessageBox.Show("Deseja mesmo enviar uma ordem de Compra/Venda nos valores atuais?", "Enviar Ordem", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    this._ObjMonitor.enviarOrdem();
                    MessageBox.Show("Ordem enviada!!\n\nAguarde a atualização do monitor para verificar seu resultado.", "Enviar Ordem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                this.adicionarErroLog(ex);
            }
        }
        private void btoNegociacao_Click_1(object sender, EventArgs e)
        {
            if (this._Bloqueado) return;
            try
            {
                if (this._ObjMonitor.pAtivaNegociacao)
                {
                    this._ObjMonitor.pAtivaNegociacao = false;
                    this.btoNegociacao.Image = Properties.Resources.negociar;
                    this.btoNegociacao.ToolTipText = "Iniciar Negociação";                    
                }
                else
                {
                    this._ObjMonitor.pAtivaNegociacao = true;
                    this.btoNegociacao.Image = Properties.Resources.parar2;
                    this.btoNegociacao.ToolTipText = "Parar Negociação";
                }
            }
            catch (Exception ex)
            {
                this.adicionarErroLog(ex);
            }
        }
        private void btoCancelarOrdem_Click(object sender, EventArgs e)
        {
            if (this._Bloqueado) return;
            try
            {
                if (MessageBox.Show("Deseja mesmo enviar um pedido de CANCELAMENTO da ordem atual?", "Cancelamento de Ordem", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    this._ObjMovimento.cancelarOrdem(Convert.ToInt64(this.dgvCompraVenda[0, 0].Value.ToString()));
                    MessageBox.Show("Pedido de cancelamento enviado!\n\nAguarde a atualização do monitor para verificar se o cancelamento foi realizado com sucesso.", "Cancelamento de ordem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                this.adicionarErroLog(ex);
            }

        }
        private void btoExportarCompraVenda_Click(object sender, EventArgs e)
        {

        }
        private void btnLockUnlock_Click(object sender, EventArgs e)
        {
            bool vBooAcessoValidado = false;
            string vStrSenha = String.Empty;

            clsCtrSenha vObjSenha;

            try
            {
                vObjSenha = new clsCtrSenha();

                if (this._Bloqueado)
                {
                    using (frmViwEntrar vObjJanelaLogar = new frmViwEntrar())
                    {
                        while (vBooAcessoValidado == false)
                        {
                            vStrSenha = vObjJanelaLogar.pegarSenha();

                            if (vStrSenha == String.Empty)
                                return;

                            vBooAcessoValidado = vObjSenha.validarSenha(vStrSenha);
                            if (vBooAcessoValidado == false)
                                MessageBox.Show("Senha incorreta!", "Monitor Bitcoin", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }

                    this._Bloqueado = false;
                    this.btnLockUnlock.Image = Properties.Resources.unlock_3;
                }
                else
                {
                    if (!Program.Parametros.pPossuiSenha)
                    {
                        MessageBox.Show("Não é possível bloquear o monitor porque você ainda não definiu uma senha.\nEntre no menu Arquivo > Definir Senha e cadastre uma senha.", "Monitor Bitcoin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    this._Bloqueado = true;
                    this.btnLockUnlock.Image = Properties.Resources.lock_2;
                }
            }
            catch (Exception ex)
            {
                this.adicionarErroLog(ex);
            }
        }
        private void btnMesAnterior_Click(object sender, EventArgs e)
        {
            try
            {
                this._DataMesResultado = this._DataMesResultado.AddMonths(-1);
                this.preencherResultado(enumTipoResultado.Mensal);
            }
            catch (Exception ex)
            {
                this.adicionarErroLog(ex);
            }
        }
        private void btnMesProximo_Click(object sender, EventArgs e)
        {
            try
            {
                this._DataMesResultado = this._DataMesResultado.AddMonths(1);
                this.preencherResultado(enumTipoResultado.Mensal);
            }
            catch (Exception ex)
            {
                this.adicionarErroLog(ex);
            }
        }

        //--Genérico
        private void controlarBtoCancelar()
        {
            try
            {
                if (this.dgvCompraVenda.Rows.Count > 0)
                    this.btoCancelarOrdem.Enabled = (this.dgvCompraVenda[2, 0].Value.ToString() == "Aberta");
            }
            catch { throw; }
        }
        private string retornaAtividadeMonitor()
        {
            string vStrRetorno = String.Empty;

            if (this._ObjMonitor is null)
                vStrRetorno = "Inicializando";
            else
            {
                switch (this._ObjMonitor.pAcaoAtual)
                {
                    case clsCtrMonitor.enumAcao.Coletando:
                        vStrRetorno = "Coletando";
                        break;
                    case clsCtrMonitor.enumAcao.Comprando:
                        vStrRetorno = "Comprando";
                        break;
                    case clsCtrMonitor.enumAcao.MonitorandoCompra:
                        vStrRetorno = "Avaliando Compra";
                        break;
                    case clsCtrMonitor.enumAcao.MonitorandoVenda:
                        vStrRetorno = "Avaliando Venda";
                        break;
                    case clsCtrMonitor.enumAcao.Vendendo:
                        vStrRetorno = "Vendendo";
                        break;
                    default:
                        break;
                }
            }
            vStrRetorno += "...";

            return vStrRetorno;
        }
        private void adicionarErroLog(Exception ex)
        {
            string vStrMsg;

            vStrMsg = "***********************************************************************************************************\r\n";
            vStrMsg += DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ": " + ex.Message + "\r\n\r\n" + ex.StackTrace;

            this.txtLogErro.Text = vStrMsg + this.txtLogErro.Text;

            if (this.tbpLogErro.Parent == null)
                this.tbpLogErro.Parent = this.tabMonitor;
        }
        private void cmbPeriodoHistorico_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._Bloqueado) return;
            switch (this.cmbPeriodoHistorico.SelectedIndex)
            {
                case 0:
                    this._DiasLimite = 1;
                    break;
                case 1:
                    this._DiasLimite = 5;
                    break;
                case 2:
                    this._DiasLimite = 14;
                    break;
                case 3:
                    this._DiasLimite = 30;
                    break;
                case 4:
                    this._DiasLimite = 3650;
                    break;
            }

        }
        private void graPatrimonio_MouseMove(object sender, MouseEventArgs e)
        {
            var vObjPosicao = e.Location;
            bool vBooMostrar = false;

            try
            {

                if (this._PrevPosition.HasValue && vObjPosicao == this._PrevPosition.Value) return;

                this._PrevPosition = vObjPosicao;

                var vObjValores = this.graPatrimonio.HitTest(vObjPosicao.X, vObjPosicao.Y, false, ChartElementType.DataPoint);

                foreach (var vObjValor in vObjValores)
                {
                    if (vObjValor.ChartElementType == ChartElementType.DataPoint)
                    {
                        var vObjPonto = vObjValor.Object as DataPoint;
                        if (vObjPonto != null)
                        {
                            var pointXPixel = vObjValor.ChartArea.AxisX.ValueToPixelPosition(vObjPonto.XValue);
                            var pointYPixel = vObjValor.ChartArea.AxisY.ValueToPixelPosition(vObjPonto.YValues[0]);

                            // check if the cursor is really close to the point 
                            if (Math.Abs(vObjPosicao.X - pointXPixel) < 30 &&
                                Math.Abs(vObjPosicao.Y - pointYPixel) < 100)
                            {
                                if (this._PontoAnterior == null)
                                    vBooMostrar = true;
                                else if (this._PontoAnterior.XValue != vObjPonto.XValue)
                                {
                                    vBooMostrar = true;
                                    this._PontoAnterior.MarkerStyle = MarkerStyle.None;
                                }

                                if (vBooMostrar)
                                {
                                    vObjPonto.MarkerSize = 7;
                                    vObjPonto.MarkerStyle = MarkerStyle.Circle;
                                    vObjPonto.MarkerColor = Color.White;
                                    vObjPonto.MarkerBorderColor = Color.Black;

                                    this._ToolTip.Show(vObjPonto.YValues[0].ToString("C"), this.graPatrimonio, vObjPosicao.X, vObjPosicao.Y - 15);
                                    this._PontoAnterior = vObjPonto;
                                }
                            }
                            else
                            {
                                this._ToolTip.RemoveAll();
                                if (this._PontoAnterior != null) this._PontoAnterior.MarkerStyle = MarkerStyle.None;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.adicionarErroLog(ex);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void trbDias_Scroll(object sender, EventArgs e)
        {
            this._ObjMonitor.pPeriodoMaxMin = this.trbDias.Value;
            this._ObjMonitor.buscarMaxMinPeriodo();
        }
    }
}
