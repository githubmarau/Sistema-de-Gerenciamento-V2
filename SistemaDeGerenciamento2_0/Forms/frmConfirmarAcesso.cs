﻿using DevExpress.Utils.Extensions;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using SistemaDeGerenciamento2_0.Class;
using SistemaDeGerenciamento2_0.Context;
using SistemaDeGerenciamento2_0.Models;
using SistemaDeGerenciamento2_0.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaDeGerenciamento2_0.Forms
{
    public partial class frmConfirmarAcesso : DevExpress.XtraEditors.XtraForm
    {
        private int X = 0;
        private int Y = 0;

        private string tela;

        private string tipoCadastro = string.Empty;

        private string CPFouCNPJCadastrado = string.Empty;

        private bool IsUsuarioPossuiAcesso = false;

        private frmCadastro frmCadastro;

        private frmProdutos frmProdutos;

        private frmTelaPrincipal frmTelaPrincipal = new frmTelaPrincipal();

        private PermissoesUsuario permissoesUsuario = new PermissoesUsuario();

        private frmFinanceiro frmFinanceiro;

        public frmConfirmarAcesso(frmTelaPrincipal _frmTelaPrincipal, string _tela)
        {
            InitializeComponent();

            frmTelaPrincipal = _frmTelaPrincipal;

            tela = _tela;
        }

        public frmConfirmarAcesso(frmFinanceiro _frmFinanceiro, string _tela)
        {
            InitializeComponent();

            frmFinanceiro = _frmFinanceiro;

            tela = _tela;
        }

        public frmConfirmarAcesso(string _tipoCadastro, frmTelaPrincipal _frmTelaPrincipal, string _CPFouCNPJCadastrado, string _tela)
        {
            InitializeComponent();

            frmTelaPrincipal = _frmTelaPrincipal;

            tela = _tela;

            tipoCadastro = _tipoCadastro;

            CPFouCNPJCadastrado = _CPFouCNPJCadastrado;
        }

        private void btnLogar_Click(object sender, EventArgs e)
        {
            RealizarLogin();
        }

        private void RealizarLogin()
        {
            if (IsLoginExiste() == true)
            {
                permissoesUsuario.BuscarPermissoesUsuario(txtUsuario.Text);
                //permissoesUsuario.ReloadData(frmTelaPrincipal, txtUsuario.Text);

                if (tela == "ConfigUsuario")
                {
                    VerificarAcessoConfigUsuario();
                }
                else if (tela == "Cliente" && CPFouCNPJCadastrado == string.Empty
                    || tela == "Funcionario" && CPFouCNPJCadastrado == string.Empty
                    || tela == "Fornecedor" && CPFouCNPJCadastrado == string.Empty)
                {
                    VerificarAcessoNovoCadastro(tela);
                }
                else if (tela == "Todos Os Cadastros")
                {
                    VerificarAcessoTodosCadastros();
                }
                else if (tela == "Cliente" && CPFouCNPJCadastrado != string.Empty
                    || tela == "Funcionario" && CPFouCNPJCadastrado == string.Empty
                    || tela == "Fornecedor" && CPFouCNPJCadastrado != string.Empty)
                {
                    VerificarAcessoAlterarCadastro();
                }
                else if (tela == "Apagar Cadastro")
                {
                    DialogResult OpcaoDoUsuario = new DialogResult();
                    OpcaoDoUsuario = MessageBox.Show("Realmente Deletar Cadastro?", "Atenção!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (OpcaoDoUsuario == DialogResult.Yes)
                    {
                        VerificarAcessoDeletarCadastro();
                    }
                }
                else if (tela == "Adiconar Produto")
                {
                    VerificarAcessoAdicionarProduto();
                }
                else if (tela == "Todos Produtos")
                {
                    VerificarAcessoTodosProdutos();
                }
                else if (tela == "Editar Produto")
                {
                    VerificarAcessoEditarProdutos();
                }
                else if (tela == "Apagar Produto")
                {
                    DialogResult OpcaoDoUsuario = new DialogResult();
                    OpcaoDoUsuario = MessageBox.Show("Realmente Deletar Cadastro do Produto?", "Atenção!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (OpcaoDoUsuario == DialogResult.Yes)
                    {
                        VerificarAcessoDeletarProdutos();
                    }
                }
                else if (tela == "ConfigEmpresa")
                {
                    VerificarAcessoConfiguracoesEmpresa();
                }
                else if (tela == "ConfigPerfil")
                {
                    VerificarAcessoConfiguracoesPerfil();
                }
                else if (tela == "ConfigFinanceiro")
                {
                    VerificarAcessoConfiguracoesFinanceiro();
                }
                else if (tela == "ConfigDespesa")
                {
                    VerificarAcessoConfiguracoesDespesa();
                }
                else if (tela == "Config Empresa Acesso Rapido")
                {
                    VerificarAcessoConfiguracoesEmpresaAcessoRapido();
                }
                else if (tela == "Config Financeiro Acesso Rapido")
                {
                    VerificarAcessoConfiguracoesFinanceiraAcessoRapido();
                }
                else if (tela == "Acesso PDV")
                {
                    VerificarAcessoPDV();
                }
                else if (tela == "Remover Item Tela PDV")
                {
                    VerificarRemoverProdutoTelaPDV();
                }
                else if (tela == "Cancelar Venda Tela PDV")
                {
                    VerificarCancelarVendaTelaPDV();
                }
                else if (tela == "Acesso Importar XML")
                {
                    VerificarAcessoFinanceiroImportarXML();
                }
                else if (tela == "Acesso Despesa")
                {
                    VerificarAcessoCadastrarDespesas();
                }
                else if (tela == "Acesso Categoria")
                {
                    VerificarAcessoFinanceiroAcessoCategoria();
                }
                else if (tela == "Pagar Contas")
                {
                    VerificarAcessoFinanceiroPagarConta(frmFinanceiro);
                }
                else if (tela == "Faturamento Por Dia" || tela == "Faturamento Por Vendedor")
                {
                    VerificarAcessoRelatorioFaturamentoPorDia();
                }
                else if (tela == "Indicadores de Venda")
                {
                    VerificarAcessoIndicadoresDeVenda();
                }
                else if (tela == "Historico de Venda")
                {
                    VerificarAcessoHistoricoDeVenda();
                }
                else if (tela == "Visao Geral")
                {
                    VerificarAcessoVisaoGeral();
                }
            }
            else
            {
                MensagemAtencao.MensagemUsuarioSenhasNaoEncontrados();

                txtUsuario.Focus();
            }
        }

        private void VerificarAcessoVisaoGeral()
        {
            permissoesUsuario.listaPermissoesUsuario.ForEach(x => IsUsuarioPossuiAcesso = x.pm_visao_geral);

            if (IsUsuarioPossuiAcesso == true && tela == "Visao Geral")
            {
                this.Hide();

                frmTelaPrincipal.TelaVisaoGeral();

                this.Close();
            }
            else
            {
                MenssagemUsuarioSemPermissao();
            }
        }

        private void VerificarAcessoHistoricoDeVenda()
        {
            permissoesUsuario.listaPermissoesUsuario.ForEach(x => IsUsuarioPossuiAcesso = x.pm_historico_venda);

            if (IsUsuarioPossuiAcesso == true && tela == "Historico de Venda")
            {
                this.Hide();

                frmHistoricoVenda frmHistoricoVenda = new frmHistoricoVenda();
                frmHistoricoVenda.ShowDialog();

                this.Close();
            }
            else
            {
                MenssagemUsuarioSemPermissao();
            }
        }

        private void VerificarAcessoIndicadoresDeVenda()
        {
            permissoesUsuario.listaPermissoesUsuario.ForEach(x => IsUsuarioPossuiAcesso = x.pm_indicadores_venda);

            if (IsUsuarioPossuiAcesso == true && tela == "Indicadores de Venda")
            {
                this.Hide();

                frmIndicadorVenda frmIndicadorVenda = new frmIndicadorVenda(frmTelaPrincipal);
                frmIndicadorVenda.ShowDialog();

                this.Close();
            }
            else
            {
                MenssagemUsuarioSemPermissao();
            }
        }

        private void VerificarAcessoRelatorioFaturamentoPorDia()
        {
            permissoesUsuario.listaPermissoesUsuario.ForEach(x => IsUsuarioPossuiAcesso = x.pm_visualizar_faturamento);

            if (IsUsuarioPossuiAcesso == true && tela == "Faturamento Por Dia")
            {
                this.Hide();

                frmFaturamentoPorDia frmFaturamentoPorDia = new frmFaturamentoPorDia(frmTelaPrincipal);
                frmFaturamentoPorDia.ShowDialog();

                this.Close();
            }
            else if (IsUsuarioPossuiAcesso == true && tela == "Faturamento Por Vendedor")
            {
                this.Hide();

                frmFaturamentoPorVendedor frmFaturamentoPorVendedor = new frmFaturamentoPorVendedor(frmTelaPrincipal);
                frmFaturamentoPorVendedor.ShowDialog();

                this.Close();
            }
            else
            {
                MenssagemUsuarioSemPermissao();
            }
        }

        private void VerificarAcessoCadastrarDespesas()
        {
            permissoesUsuario.listaPermissoesUsuario.ForEach(x => IsUsuarioPossuiAcesso = x.pm_pagar_contas);

            if (IsUsuarioPossuiAcesso == true)
            {
                this.Hide();

                frmFinanceiro frmFinanceiro = new frmFinanceiro(frmTelaPrincipal);
                frmFinanceiro.ShowDialog();

                this.Close();
            }
            else
            {
                MenssagemUsuarioSemPermissao();
            }
        }

        private void VerificarAcessoFinanceiroPagarConta(frmFinanceiro frmFinanceiro)
        {
            permissoesUsuario.listaPermissoesUsuario.ForEach(x => IsUsuarioPossuiAcesso = x.pm_pagar_contas);

            if (IsUsuarioPossuiAcesso == true)
            {
                this.Hide();

                frmPagamentoDespesa frmPagamentoDespesa = new frmPagamentoDespesa(frmFinanceiro.idDespesa, frmFinanceiro.parcela,
                frmFinanceiro.nomeEmpresa, frmFinanceiro.categoria, frmFinanceiro.valor, frmFinanceiro.vencimento);
                frmPagamentoDespesa.ShowDialog();

                this.Close();
            }
            else
            {
                MenssagemUsuarioSemPermissao();
            }
        }

        private void VerificarAcessoFinanceiroAcessoCategoria()
        {
            permissoesUsuario.listaPermissoesUsuario.ForEach(x => IsUsuarioPossuiAcesso = x.pm_cadastrar_despesa);

            if (IsUsuarioPossuiAcesso == true)
            {
                this.Hide();

                AcessoTelaDespesa();

                this.Close();
            }
            else
            {
                MenssagemUsuarioSemPermissao();
            }
        }

        public void AcessoTelaDespesa()
        {
            frmTelaPrincipal.pnlTelaPrincipal.Controls.Clear();
            frmDespesas frmDespesas = new frmDespesas(frmTelaPrincipal);
            frmDespesas.TopLevel = false;
            frmTelaPrincipal.pnlTelaPrincipal.Controls.Add(frmDespesas);
            frmTelaPrincipal.pnlTelaPrincipal.Tag = frmDespesas;
            frmDespesas.Show();
        }

        private void VerificarAcessoFinanceiroImportarXML()
        {
            permissoesUsuario.listaPermissoesUsuario.ForEach(x => IsUsuarioPossuiAcesso = x.pm_importar_xml);

            if (IsUsuarioPossuiAcesso == true)
            {
                this.Hide();

                frmEntradaNF frm = new frmEntradaNF(frmTelaPrincipal);
                frm.ShowDialog();

                this.Close();
            }
            else
            {
                MenssagemUsuarioSemPermissao();
            }
        }

        private void VerificarCancelarVendaTelaPDV()
        {
            permissoesUsuario.listaPermissoesUsuario.ForEach(x => IsUsuarioPossuiAcesso = x.pm_cancelar_venda);

            if (IsUsuarioPossuiAcesso == true)
            {
                this.Hide();

                frmPDV.permissaoCancelarVenda = true;

                this.Close();
            }
            else
            {
                MenssagemUsuarioSemPermissao();
            }
        }

        private void VerificarRemoverProdutoTelaPDV()
        {
            permissoesUsuario.listaPermissoesUsuario.ForEach(x => IsUsuarioPossuiAcesso = x.pm_remover_venda);

            if (IsUsuarioPossuiAcesso == true)
            {
                this.Hide();

                frmPDV.permissaoRemoverItem = true;

                this.Close();
            }
            else
            {
                MenssagemUsuarioSemPermissao();
            }
        }

        private void VerificarAcessoPDV()
        {
            permissoesUsuario.listaPermissoesUsuario.ForEach(x => IsUsuarioPossuiAcesso = x.pm_acesso_pdv);

            if (IsUsuarioPossuiAcesso == true)
            {
                this.Hide();

                frmPDV frmPDV = new frmPDV(frmTelaPrincipal);
                frmPDV.ShowDialog();

                this.Close();
            }
            else
            {
                MenssagemUsuarioSemPermissao();
            }
        }

        private void VerificarAcessoConfiguracoesFinanceiraAcessoRapido()
        {
            permissoesUsuario.listaPermissoesUsuario.ForEach(x => IsUsuarioPossuiAcesso = x.pm_configuracoes_financeira);

            if (IsUsuarioPossuiAcesso == true)
            {
                this.Hide();

                frmConfiguracaoFinanceira frmConfiguracaoFinanceira = new frmConfiguracaoFinanceira(frmTelaPrincipal);
                frmConfiguracaoFinanceira.AutoScroll = false;
                frmConfiguracaoFinanceira.btnCancelar.Visible = true;
                frmConfiguracaoFinanceira.KeyPreview = true;
                frmConfiguracaoFinanceira.btnFechar.Visible = true;
                frmConfiguracaoFinanceira.ShowDialog();

                this.Close();
            }
            else
            {
                MenssagemUsuarioSemPermissao();
            }
        }

        private void VerificarAcessoConfiguracoesEmpresaAcessoRapido()
        {
            permissoesUsuario.listaPermissoesUsuario.ForEach(x => IsUsuarioPossuiAcesso = x.pm_configuracoes_empresa);

            if (IsUsuarioPossuiAcesso == true)
            {
                this.Hide();

                frmDadosEmpresa frmDadosEmpresa = new frmDadosEmpresa(frmTelaPrincipal);
                frmDadosEmpresa.btnFechar.Visible = true;
                frmDadosEmpresa.btnSalvar.TabIndex = 0;
                frmDadosEmpresa.ShowDialog();

                this.Close();
            }
            else
            {
                MenssagemUsuarioSemPermissao();
            }
        }

        private void VerificarAcessoConfiguracoesDespesa()
        {
            permissoesUsuario.listaPermissoesUsuario.ForEach(x => IsUsuarioPossuiAcesso = x.pm_configuracoes_despesa);

            if (IsUsuarioPossuiAcesso == true)
            {
                this.Hide();

                permissoesUsuario.AcessoTelaDespesa();

                this.Close();
            }
            else
            {
                MenssagemUsuarioSemPermissao();
            }
        }

        private void VerificarAcessoConfiguracoesFinanceiro()
        {
            permissoesUsuario.listaPermissoesUsuario.ForEach(x => IsUsuarioPossuiAcesso = x.pm_configuracoes_financeira);

            if (IsUsuarioPossuiAcesso == true)
            {
                this.Hide();

                permissoesUsuario.TelaConfiguracoes("ConfigFinanceiro");

                this.Close();
            }
            else
            {
                MenssagemUsuarioSemPermissao();
            }
        }

        private void VerificarAcessoConfiguracoesPerfil()
        {
            permissoesUsuario.listaPermissoesUsuario.ForEach(x => IsUsuarioPossuiAcesso = x.pm_configuracoes_perfil);

            if (IsUsuarioPossuiAcesso == true)
            {
                this.Hide();

                permissoesUsuario.TelaConfiguracoes("ConfigPerfil");

                this.Close();
            }
            else
            {
                MenssagemUsuarioSemPermissao();
            }
        }

        private void VerificarAcessoConfiguracoesEmpresa()
        {
            permissoesUsuario.listaPermissoesUsuario.ForEach(x => IsUsuarioPossuiAcesso = x.pm_configuracoes_empresa);

            if (IsUsuarioPossuiAcesso == true)
            {
                this.Hide();

                permissoesUsuario.TelaConfiguracoes("ConfigEmpresa");

                this.Close();
            }
            else
            {
                MenssagemUsuarioSemPermissao();
            }
        }

        private void VerificarAcessoDeletarProdutos()
        {
            permissoesUsuario.listaPermissoesUsuario.ForEach(x => IsUsuarioPossuiAcesso = x.pm_remover_produto);

            if (IsUsuarioPossuiAcesso == true)
            {
                this.Hide();

                DeletarDados.DeletarCadastroProduto(frmProdutos.codigoProduto);

                this.Close();
            }
            else
            {
                MenssagemUsuarioSemPermissao();
            }
        }

        private void VerificarAcessoEditarProdutos()
        {
            permissoesUsuario.listaPermissoesUsuario.ForEach(x => IsUsuarioPossuiAcesso = x.pm_editar_produto);

            if (IsUsuarioPossuiAcesso == true)
            {
                this.Hide();

                string codigoProduto = frmProdutos.codigoProduto;

                frmCadastroProduto frmCadastroProduto = new frmCadastroProduto(codigoProduto);
                frmCadastroProduto.ShowDialog();

                this.Close();
            }
            else
            {
                MenssagemUsuarioSemPermissao();
            }
        }

        private void VerificarAcessoTodosProdutos()
        {
            permissoesUsuario.listaPermissoesUsuario.ForEach(x => IsUsuarioPossuiAcesso = x.pm_visualizar_todos_produtos);

            if (IsUsuarioPossuiAcesso == true)
            {
                this.Hide();

                frmProdutos frmProdutos = new frmProdutos(frmTelaPrincipal);
                frmProdutos.ShowDialog();

                this.Close();
            }
            else
            {
                MenssagemUsuarioSemPermissao();
            }
        }

        private void VerificarAcessoAdicionarProduto()
        {
            permissoesUsuario.listaPermissoesUsuario.ForEach(x => IsUsuarioPossuiAcesso = x.pm_adicionar_produto);

            if (IsUsuarioPossuiAcesso == true)
            {
                this.Hide();

                frmCadastroProduto frmCadastroProduto = new frmCadastroProduto();
                frmCadastroProduto.ShowDialog();

                this.Close();
            }
            else
            {
                MenssagemUsuarioSemPermissao();
            }
        }

        private void VerificarAcessoDeletarCadastro()
        {
            permissoesUsuario.listaPermissoesUsuario.ForEach(x => IsUsuarioPossuiAcesso = x.pm_remover_cadastro);

            if (IsUsuarioPossuiAcesso == true)
            {
                this.Hide();

                DeletarDados.DeletarCadastro(frmCadastro.CNPJouCPF);

                this.Close();
            }
            else
            {
                MenssagemUsuarioSemPermissao();
            }
        }

        private void VerificarAcessoAlterarCadastro()
        {
            permissoesUsuario.listaPermissoesUsuario.ForEach(x => IsUsuarioPossuiAcesso = x.pm_editar_cadastro);

            if (IsUsuarioPossuiAcesso == true)
            {
                this.Hide();
                frmCadastroRegistros frmCadastroRegistros = new frmCadastroRegistros(tipoCadastro, frmTelaPrincipal, CPFouCNPJCadastrado, tela);
                frmCadastroRegistros.ShowDialog();

                this.Close();
            }
            else
            {
                MenssagemUsuarioSemPermissao();
            }
        }

        private void VerificarAcessoTodosCadastros()
        {
            permissoesUsuario.listaPermissoesUsuario.ForEach(x => IsUsuarioPossuiAcesso = x.pm_visualizar_cadastro_completo);

            if (IsUsuarioPossuiAcesso == true)
            {
                this.Hide();
                frmCadastro frmCadastro = new frmCadastro(frmTelaPrincipal);
                frmCadastro.ShowDialog();

                this.Close();
            }
            else
            {
                MenssagemUsuarioSemPermissao();
            }
        }

        private void VerificarAcessoConfigUsuario()
        {
            permissoesUsuario.listaPermissoesUsuario.ForEach(x => IsUsuarioPossuiAcesso = x.pm_criar_editar_usuario);

            if (IsUsuarioPossuiAcesso == true)
            {
                ReloadDataConfigUsuario();
            }
            else
            {
                MenssagemUsuarioSemPermissao();
            }
        }

        private void VerificarAcessoNovoCadastro(string _Cadastro)
        {
            permissoesUsuario.listaPermissoesUsuario.ForEach(x => IsUsuarioPossuiAcesso = x.pm_editar_cadastro);

            if (IsUsuarioPossuiAcesso == true)
            {
                this.Hide();
                frmCadastroRegistros frmCadastroRegistros = new frmCadastroRegistros(_Cadastro, frmTelaPrincipal);
                frmCadastroRegistros.ShowDialog();

                this.Close();
            }
            else
            {
                MenssagemUsuarioSemPermissao();
            }
        }

        private void MenssagemUsuarioSemPermissao()
        {
            MensagemAtencao.MensagemUsuarioSemPermissao();
            txtUsuario.Focus();
        }

        private void ReloadDataConfigUsuario()
        {
            using (var handle = SplashScreenManager.ShowOverlayForm(frmTelaPrincipal))
            {
                frmTelaPrincipal.TelaConfiguracoes(tela);

                this.Close();
            }
        }

        private bool IsLoginExiste()
        {
            try
            {
                using (SistemaDeGerenciamento2_0Context db = new SistemaDeGerenciamento2_0Context())
                {
                    var login = db.tb_registro.Where(x => x.rg_login == txtUsuario.Text && x.rg_senha == txtSenha.Text).Select(x => x.rg_login).Any();

                    return login;
                }
            }
            catch (Exception x)
            {
                LogErros.EscreverArquivoDeLog($"{DateTime.Now} - Erro ao Verificar Login Para Acesso Restrito - | {x.Message} | {x.StackTrace}");

                MensagemErros.ErroAoBuscarUsuarioSenhaAcessoRestrito(x);

                return false;
            }
        }

        private void txtUsuario_KeyPress(object sender, KeyPressEventArgs e)
        {
            ManipulacaoTextBox.DigitarApenasLetrasOuNumeros(e, txtUsuario);
        }

        private void txtSenha_KeyPress(object sender, KeyPressEventArgs e)
        {
            ManipulacaoTextBox.DigitarApenasLetrasOuNumeros(e, txtSenha);
        }

        private void pcbExibirSenha_MouseUp(object sender, MouseEventArgs e)
        {
            txtSenha.Properties.UseSystemPasswordChar = true;
            pcbExibirSenha.Image = Resources.olho_20;
        }

        private void pcbExibirSenha_MouseDown(object sender, MouseEventArgs e)
        {
            txtSenha.Properties.UseSystemPasswordChar = false;
            pcbExibirSenha.Image = Resources.olho_aberto_20;
        }

        private void frmConfirmarAcesso_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            X = this.Left - MousePosition.X;
            Y = this.Top - MousePosition.Y;
        }

        private void frmConfirmarAcesso_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            this.Left = X + MousePosition.X;
            this.Top = Y + MousePosition.Y;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            MensagemAtencao.MensagemCancelar(this);
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmConfirmarAcesso_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                MensagemAtencao.MensagemCancelar(this);
            }
            else if (e.KeyCode == Keys.F10)
            {
                RealizarLogin();
            }
        }
    }
}