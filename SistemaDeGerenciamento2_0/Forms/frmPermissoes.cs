﻿using DevExpress.Data.PivotGrid;
using DevExpress.Utils.Extensions;
using DevExpress.XtraSplashScreen;
using SistemaDeGerenciamento2_0.Class;
using SistemaDeGerenciamento2_0.Context;
using SistemaDeGerenciamento2_0.Models;
using SistemaDeGerenciamento2_0.Properties;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SistemaDeGerenciamento2_0.Forms
{
    public partial class frmPermissoes : DevExpress.XtraEditors.XtraForm
    {
        private frmCadastroUsuario form = new frmCadastroUsuario();

        private int FK_Permissoes = 0;

        private int IDRegistro = 0;

        public frmPermissoes(frmCadastroUsuario _form)
        {
            InitializeComponent();

            form = _form;

            IDRegistro = Convert.ToInt32(form.cmbFuncionario.Properties.GetKeyValueByDisplayValue(form.cmbFuncionario.Text));

            PreencherCheckBox();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            FechaTela();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        private void FechaTela()
        {
            if (form.txtNomeUsuario.Text == string.Empty && form.txtSenha.Text == string.Empty && form.txtNomeUsuario.Text == string.Empty)
            {
                form.Close();
            }
            else
            {
                MensagemAtencao.MensagemCancelar(form);
            }
        }

        private void Salvar()
        {
            if (IsTextBoxPreenchidos() == true
                && IsSenhaIgualConfimacaoSenha() == true)
            {
                if (form.FK_Permissoes != null)
                {
                    AtualizarPermissoes();

                    AlertaConfirmacao.ChamandoAlertaSucessoNoCantoInferiorDireito(AlertaSalvar, this);
                }
                else
                {
                    SalvarPermissoes();

                    CadastrarLoginUsuario();

                    AlertaConfirmacao.ChamandoAlertaSucessoNoCantoInferiorDireito(AlertaSalvar, this);
                }
            }
        }

        private void SalvarPermissoes()
        {
            try
            {
                using (SistemaDeGerenciamento2_0Context db = new SistemaDeGerenciamento2_0Context())
                {
                    var checkBoxPreenchidas = new tb_permissoes()
                    {
                        ////PDV
                        pm_acesso_pdv = Convert.ToBoolean(chkAcessoAoPDV.Checked),
                        pm_vendas_do_dias = Convert.ToBoolean(chkVendasDoDia.Checked),
                        pm_todas_vendas = Convert.ToBoolean(chkTodasVendas.Checked),
                        pm_remover_venda = Convert.ToBoolean(chkRemoverVenda.Checked),
                        pm_cancelar_venda = Convert.ToBoolean(chkCancelarVenda.Checked),
                        pm_realizar_devolucao = Convert.ToBoolean(chkRealizarDevolucao.Checked),

                        ////Produto
                        pm_adicionar_produto = Convert.ToBoolean(chkCadastrarProduto.Checked),
                        pm_remover_produto = Convert.ToBoolean(chkRemoverProduto.Checked),
                        pm_editar_produto = Convert.ToBoolean(cnkEditarProduto.Checked),
                        pm_visualizar_todos_produtos = Convert.ToBoolean(chkTodosProdutos.Checked),
                        pm_alerta_estoque_baixo = Convert.ToBoolean(chkAlertaEstoqueBaixo.Checked),

                        ////Cadastro
                        pm_efetuar_cadastro = Convert.ToBoolean(chkEfetuarCadastros.Checked),
                        pm_editar_cadastro = Convert.ToBoolean(chkEditarCadastros.Checked),
                        pm_remover_cadastro = Convert.ToBoolean(chkRemoverCadastros.Checked),
                        pm_visualizar_cadastro_completo = Convert.ToBoolean(chkVisualizarCadastrosCompleto.Checked),

                        ////Financeiro
                        pm_alerta_contas_atrasadas = Convert.ToBoolean(chkAlertaContasAtrasadas.Checked),
                        pm_visualizar_contas_pagar = Convert.ToBoolean(chkVisualizarContasReceberPagar.Checked),
                        pm_pagar_contas = Convert.ToBoolean(chkPermitePagarReceberContas.Checked),
                        pm_visualizar_fluxo_caixa = Convert.ToBoolean(chkVisualizarFluxoCaixa.Checked),
                        pm_acesso_financeiro = Convert.ToBoolean(chkAcessoFinanceiro.Checked),
                        pm_cadastrar_conta = Convert.ToBoolean(chkCadastrarCategoriaDespesa.Checked),
                        pm_cadastrar_despesa = Convert.ToBoolean(chkCadastrarDespesa.Checked),
                        pm_importar_xml = Convert.ToBoolean(chkPermissaoImportarXML.Checked),

                        ////Configuração
                        pm_criar_editar_usuario = Convert.ToBoolean(chkCriarEditarUsuarios.Checked),
                        pm_configuracoes_empresa = Convert.ToBoolean(chkConfiguracaoEmpresa.Checked),
                        pm_configuracoes_perfil = Convert.ToBoolean(chkConfiguracaoPerfil.Checked),
                        pm_configuracoes_financeira = Convert.ToBoolean(chkConfiguracaoFinanceira.Checked),
                        pm_configuracoes_despesa = Convert.ToBoolean(chkConfiguracaoDespesa.Checked),

                        ////Relatorio
                        pm_visualizar_visao_geral = Convert.ToBoolean(chkVisualizarVisaoGeral.Checked),
                        pm_visualizar_dre = Convert.ToBoolean(chkVisualizarDRE.Checked),
                        pm_criar_visualizar_relatorios = Convert.ToBoolean(chkCriarVisualizarRelatorios.Checked),
                        pm_visualizar_faturamento = Convert.ToBoolean(chkVisualizarFaturamento.Checked),
                        pm_indicadores_venda = Convert.ToBoolean(chkIndicadoresDeVenda.Checked),
                        pm_historico_venda = Convert.ToBoolean(chkHistoricoVenda.Checked),
                        pm_visao_geral = Convert.ToBoolean(chkVisaoGeral.Checked),
                    };

                    db.tb_permissoes.Add(checkBoxPreenchidas);
                    db.SaveChanges();
                    FK_Permissoes = checkBoxPreenchidas.id_permissoes;
                };
            }
            catch (Exception x)
            {
                LogErros.EscreverArquivoDeLog($"{DateTime.Now} - Erro ao Salvar Permissões de Acesso - | {x.Message} | {x.StackTrace}");

                MensagemErros.ErroAoCadastroPermissoes(x);
            }
        }

        private void CadastrarLoginUsuario()
        {
            try
            {
                using (SistemaDeGerenciamento2_0Context db = new SistemaDeGerenciamento2_0Context())
                {
                    var dadosUsuario = db.tb_registro.Where(x => x.id_registro.Equals(IDRegistro));

                    foreach (var item in dadosUsuario)
                    {
                        item.rg_login = form.txtNomeUsuario.Text;
                        item.rg_senha = form.txtSenha.Text;
                        item.fk_permissoes = FK_Permissoes;
                    }

                    db.SaveChanges();
                }
            }
            catch (Exception x)
            {
                LogErros.EscreverArquivoDeLog($"{DateTime.Now} - Erro ao Cadastrar Usuário - | {x.Message} | {x.StackTrace}");

                MensagemErros.ErroAoCadastroUsuario(x);
            }
        }

        private void PreencherCheckBox()
        {
            try
            {
                int? fk_permissoes = form.FK_Permissoes;

                if (fk_permissoes != 0)
                {
                    using (SistemaDeGerenciamento2_0Context db = new SistemaDeGerenciamento2_0Context())
                    {
                        var dadosConfiguracoes = db.tb_permissoes.Where(x => x.id_permissoes == fk_permissoes).ToList();

                        if (dadosConfiguracoes.Count > 0)
                        {
                            foreach (var item in dadosConfiguracoes)
                            {
                                //PDV
                                chkAcessoAoPDV.Checked = Convert.ToBoolean(item.pm_acesso_pdv);
                                chkVendasDoDia.Checked = Convert.ToBoolean(item.pm_vendas_do_dias);
                                chkTodasVendas.Checked = Convert.ToBoolean(item.pm_todas_vendas);
                                chkRemoverVenda.Checked = Convert.ToBoolean(item.pm_remover_venda);
                                chkCancelarVenda.Checked = Convert.ToBoolean(item.pm_cancelar_venda);
                                chkRealizarDevolucao.Checked = Convert.ToBoolean(item.pm_realizar_devolucao);

                                //Produto
                                chkCadastrarProduto.Checked = Convert.ToBoolean(item.pm_adicionar_produto);
                                cnkEditarProduto.Checked = Convert.ToBoolean(item.pm_editar_produto);
                                chkRemoverProduto.Checked = Convert.ToBoolean(item.pm_remover_produto);
                                chkTodosProdutos.Checked = Convert.ToBoolean(item.pm_visualizar_todos_produtos);
                                chkAlertaEstoqueBaixo.Checked = Convert.ToBoolean(item.pm_alerta_estoque_baixo);

                                //Cadastro
                                chkEfetuarCadastros.Checked = Convert.ToBoolean(item.pm_efetuar_cadastro);
                                chkEditarCadastros.Checked = Convert.ToBoolean(item.pm_editar_cadastro);
                                chkRemoverCadastros.Checked = Convert.ToBoolean(item.pm_remover_cadastro);
                                chkVisualizarCadastrosCompleto.Checked = Convert.ToBoolean(item.pm_visualizar_cadastro_completo);

                                //Financeiro
                                chkAlertaContasAtrasadas.Checked = Convert.ToBoolean(item.pm_alerta_contas_atrasadas);
                                chkVisualizarContasReceberPagar.Checked = Convert.ToBoolean(item.pm_visualizar_contas_pagar);
                                chkPermitePagarReceberContas.Checked = Convert.ToBoolean(item.pm_pagar_contas);
                                chkVisualizarFluxoCaixa.Checked = Convert.ToBoolean(item.pm_visualizar_fluxo_caixa);
                                chkAcessoFinanceiro.Checked = Convert.ToBoolean(item.pm_acesso_financeiro);
                                chkCadastrarCategoriaDespesa.Checked = Convert.ToBoolean(item.pm_cadastrar_conta);
                                chkCadastrarDespesa.Checked = Convert.ToBoolean(item.pm_cadastrar_despesa);
                                chkPermissaoImportarXML.Checked = Convert.ToBoolean(item.pm_importar_xml);

                                //Configuração
                                chkCriarEditarUsuarios.Checked = Convert.ToBoolean(item.pm_criar_editar_usuario);
                                chkConfiguracaoEmpresa.Checked = Convert.ToBoolean(item.pm_configuracoes_empresa);
                                chkConfiguracaoPerfil.Checked = Convert.ToBoolean(item.pm_configuracoes_perfil);
                                chkConfiguracaoFinanceira.Checked = Convert.ToBoolean(item.pm_configuracoes_financeira);
                                chkConfiguracaoDespesa.Checked = Convert.ToBoolean(item.pm_configuracoes_despesa);

                                //Relatorio
                                chkVisualizarVisaoGeral.Checked = Convert.ToBoolean(item.pm_visualizar_visao_geral);
                                chkVisualizarDRE.Checked = Convert.ToBoolean(item.pm_visualizar_dre);
                                chkCriarVisualizarRelatorios.Checked = Convert.ToBoolean(item.pm_criar_visualizar_relatorios);
                                chkVisualizarFaturamento.Checked = Convert.ToBoolean(item.pm_visualizar_faturamento);
                                chkIndicadoresDeVenda.Checked = Convert.ToBoolean(item.pm_indicadores_venda);
                                chkHistoricoVenda.Checked = Convert.ToBoolean(item.pm_historico_venda);
                                chkVisaoGeral.Checked = Convert.ToBoolean(item.pm_visao_geral);
                            }
                        }
                    }
                }
            }
            catch (Exception x)
            {
                LogErros.EscreverArquivoDeLog($"{DateTime.Now} - Erro ao Buscar Permissões Para Preencher CheckBox - | {x.Message} | {x.StackTrace}");

                MensagemErros.ErroAoBuscarPermissoesUsuario(x);
            }
        }

        private void AtualizarPermissoes()
        {
            try
            {
                using (SistemaDeGerenciamento2_0Context db = new SistemaDeGerenciamento2_0Context())
                {
                    int? fk_permissoes = Convert.ToInt32(form.FK_Permissoes);

                    var checkBoxPreenchidas = db.tb_permissoes.FirstOrDefault(x => x.id_permissoes == fk_permissoes);

                    ////PDV
                    checkBoxPreenchidas.pm_acesso_pdv = Convert.ToBoolean(chkAcessoAoPDV.Checked);
                    checkBoxPreenchidas.pm_vendas_do_dias = Convert.ToBoolean(chkVendasDoDia.Checked);
                    checkBoxPreenchidas.pm_todas_vendas = Convert.ToBoolean(chkTodasVendas.Checked);
                    checkBoxPreenchidas.pm_remover_venda = Convert.ToBoolean(chkRemoverVenda.Checked);
                    checkBoxPreenchidas.pm_cancelar_venda = Convert.ToBoolean(chkCancelarVenda.Checked);
                    checkBoxPreenchidas.pm_realizar_devolucao = Convert.ToBoolean(chkRealizarDevolucao.Checked);

                    ////Produto
                    checkBoxPreenchidas.pm_adicionar_produto = Convert.ToBoolean(chkCadastrarProduto.Checked);
                    checkBoxPreenchidas.pm_remover_produto = Convert.ToBoolean(chkRemoverProduto.Checked);
                    checkBoxPreenchidas.pm_editar_produto = Convert.ToBoolean(cnkEditarProduto.Checked);
                    checkBoxPreenchidas.pm_visualizar_todos_produtos = Convert.ToBoolean(chkTodosProdutos.Checked);
                    checkBoxPreenchidas.pm_alerta_estoque_baixo = Convert.ToBoolean(chkAlertaEstoqueBaixo.Checked);

                    ////Cadastro
                    checkBoxPreenchidas.pm_efetuar_cadastro = Convert.ToBoolean(chkEfetuarCadastros.Checked);
                    checkBoxPreenchidas.pm_editar_cadastro = Convert.ToBoolean(chkEditarCadastros.Checked);
                    checkBoxPreenchidas.pm_remover_cadastro = Convert.ToBoolean(chkRemoverCadastros.Checked);
                    checkBoxPreenchidas.pm_visualizar_cadastro_completo = Convert.ToBoolean(chkVisualizarCadastrosCompleto.Checked);

                    ////Financeiro
                    checkBoxPreenchidas.pm_alerta_contas_atrasadas = Convert.ToBoolean(chkAlertaContasAtrasadas.Checked);
                    checkBoxPreenchidas.pm_visualizar_contas_pagar = Convert.ToBoolean(chkVisualizarContasReceberPagar.Checked);
                    checkBoxPreenchidas.pm_pagar_contas = Convert.ToBoolean(chkPermitePagarReceberContas.Checked);
                    checkBoxPreenchidas.pm_visualizar_fluxo_caixa = Convert.ToBoolean(chkVisualizarFluxoCaixa.Checked);
                    checkBoxPreenchidas.pm_acesso_financeiro = Convert.ToBoolean(chkAcessoFinanceiro.Checked);
                    checkBoxPreenchidas.pm_cadastrar_conta = Convert.ToBoolean(chkCadastrarCategoriaDespesa.Checked);
                    checkBoxPreenchidas.pm_cadastrar_despesa = Convert.ToBoolean(chkCadastrarDespesa.Checked);
                    checkBoxPreenchidas.pm_importar_xml = Convert.ToBoolean(chkPermissaoImportarXML.Checked);

                    ////Configuração

                    checkBoxPreenchidas.pm_criar_editar_usuario = Convert.ToBoolean(chkCriarEditarUsuarios.Checked);
                    checkBoxPreenchidas.pm_configuracoes_empresa = Convert.ToBoolean(chkConfiguracaoEmpresa.Checked);
                    checkBoxPreenchidas.pm_configuracoes_perfil = Convert.ToBoolean(chkConfiguracaoPerfil.Checked);
                    checkBoxPreenchidas.pm_configuracoes_financeira = Convert.ToBoolean(chkConfiguracaoFinanceira.Checked);
                    checkBoxPreenchidas.pm_configuracoes_despesa = Convert.ToBoolean(chkConfiguracaoDespesa.Checked);

                    ////Relatorio
                    checkBoxPreenchidas.pm_visualizar_visao_geral = Convert.ToBoolean(chkVisualizarVisaoGeral.Checked);
                    checkBoxPreenchidas.pm_visualizar_dre = Convert.ToBoolean(chkVisualizarDRE.Checked);
                    checkBoxPreenchidas.pm_criar_visualizar_relatorios = Convert.ToBoolean(chkCriarVisualizarRelatorios.Checked);
                    checkBoxPreenchidas.pm_visualizar_faturamento = Convert.ToBoolean(chkVisualizarFaturamento.Checked);
                    checkBoxPreenchidas.pm_indicadores_venda = Convert.ToBoolean(chkIndicadoresDeVenda.Checked);
                    checkBoxPreenchidas.pm_historico_venda = Convert.ToBoolean(chkHistoricoVenda.Checked);
                    checkBoxPreenchidas.pm_visao_geral = Convert.ToBoolean(chkVisaoGeral.Checked);

                    db.SaveChanges();
                };
            }
            catch (Exception x)
            {
                LogErros.EscreverArquivoDeLog($"{DateTime.Now} - Erro ao Atualizar Permissões - | {x.Message} | {x.StackTrace}");

                MensagemErros.ErroAoAtualizarPermissoesUsuario(x);
            }
        }

        private bool IsSenhaIgualConfimacaoSenha()
        {
            if (form.txtConfirmacaoSenha.Text == form.txtSenha.Text)
            {
                form.txtConfirmacaoSenha.BackColor = Color.FromArgb(0, 255, 255, 255);

                return true;
            }
            else
            {
                MensagemAtencao.MensagemSenhasDivergentes();

                form.txtConfirmacaoSenha.BackColor = Color.LightGray;

                form.txtConfirmacaoSenha.Focus();

                return false;
            }
        }

        private bool IsTextBoxPreenchidos()
        {
            if (form.txtConfirmacaoSenha.Text != string.Empty && form.txtNomeUsuario.Text != string.Empty && form.txtSenha.Text != string.Empty)
            {
                SetandoCorFundoTextBox(Color.FromArgb(0, 255, 255, 255));

                return true;
            }
            else
            {
                MensagemAtencao.MensagemPreencherCampos();

                form.txtNomeUsuario.Focus();

                SetandoCorFundoTextBox(Color.LightGray);

                return false;
            }
        }

        private void SetandoCorFundoTextBox(Color _corFundo)
        {
            form.txtConfirmacaoSenha.BackColor = _corFundo;
            form.txtSenha.BackColor = _corFundo;
            form.txtNomeUsuario.BackColor = _corFundo;
        }

        private void frmPermissoes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                FechaTela();
            }
            else if (e.KeyCode == Keys.F10)
            {
                Salvar();
            }
        }
    }
}