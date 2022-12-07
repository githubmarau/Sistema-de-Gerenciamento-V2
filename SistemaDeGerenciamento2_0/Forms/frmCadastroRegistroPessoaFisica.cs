﻿using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Filtering.Templates;
using DevExpress.XtraPrinting.Native;
using DevExpress.XtraVerticalGrid.ViewInfo;
using SistemaDeGerenciamento2_0.Class;
using SistemaDeGerenciamento2_0.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;
using static DevExpress.XtraPrinting.Export.Pdf.PdfImageCache;

namespace SistemaDeGerenciamento2_0.Forms
{
    public partial class frmCadastroRegistroPessoaFisica : DevExpress.XtraEditors.XtraForm
    {
        private int FK_Endereco = 0;

        private int FK_InformacoesComerciais = 0;

        private string tipoCadastro = string.Empty;

        private bool isCPFJaExistente = false;

        private ApiCorreios Api = new ApiCorreios();

        private Form telaRegistro = null;

        public frmCadastroRegistroPessoaFisica(string _tipoCadastro, Form _telaRegistro)
        {
            InitializeComponent();

            tipoCadastro = _tipoCadastro;

            telaRegistro = _telaRegistro;

            SetandoDados();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            bool IsCampoEnderecoPreenchido = VerificandoPreenchimentoCampoEndereco();

            bool IsCampoBasicoPreenchido = VerificandoPreenchimentoBasico();

            if (IsCampoEnderecoPreenchido == true && IsCampoBasicoPreenchido == true)
            {
                VerificarExitenciaCPF();

                if (isCPFJaExistente == false)
                {
                    SalvarInformacoesComerciais();

                    SalvarEndereco();

                    SalvarRegisto();

                    ChamandoAlertaSucessoNoCantoInferiorDireito();

                    LimparCampos.LimpaCampos(this.Controls);

                    txtObservacoes.Text = string.Empty;
                }
                else
                {
                    MensagemAtencao.MensagemValorJaExistente("CPF");
                }
            }
            else
            {
                MensagemAtencao.MensagemPreencherCampos();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            MensagemAtencao.MensagemCancelar(telaRegistro);
        }

        private void btnBuscarPorCEP_Click(object sender, EventArgs e)
        {
            PreenchimentoPorCEP(txtCEP);
        }

        private void txtDataNascimento_KeyPress(object sender, KeyPressEventArgs e)
        {
            ManipulacaoTextBox.FormatoData(txtDataNascimento);
        }

        private void txtCPF_KeyPress(object sender, KeyPressEventArgs e)
        {
            ManipulacaoTextBox.FormatoCPF(e, txtCPF);
        }

        private void txtRG_KeyPress(object sender, KeyPressEventArgs e)
        {
            ManipulacaoTextBox.FormatoRG(e, txtRG);
        }

        private void txtNome_KeyPress(object sender, KeyPressEventArgs e)
        {
            ManipulacaoTextBox.DigitarApenasLetras(e, txtNome);
        }

        private void txtEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            ManipulacaoTextBox.FormatoEmail(e, txtEmail);
        }

        private void txtDataNascimento_Leave(object sender, EventArgs e)
        {
            if (txtDataNascimento.Text != string.Empty)
            {
                if (Validacoes.IsCampoPreenchido(txtDataNascimento) == true)
                {
                    if (Validacoes.IsDataValida(txtDataNascimento) == true)
                    {
                        VerificarIdade();
                    }
                }
            }
        }

        private void txtCelular_KeyPress(object sender, KeyPressEventArgs e)
        {
            ManipulacaoTextBox.FormatoCelular(e, txtCelular);
        }

        private void txtResidencial_KeyPress(object sender, KeyPressEventArgs e)
        {
            ManipulacaoTextBox.FormatoTelefone(e, txtTelefoneResidencial);
        }

        private void txtCPF_Leave(object sender, EventArgs e)
        {
            if (txtCPF.Text != string.Empty)
            {
                if (Validacoes.IsCampoPreenchido(txtCPF) == true)
                {
                    if (Validacoes.IsCpfValido(txtCPF.Text) == false)
                    {
                        MensagemAtencao.MensagemCampoDigitadoInvalido("CPF");

                        txtCPF.BackColor = Color.LightGray;

                        txtCPF.Focus();
                    }
                    else
                    {
                        txtCPF.BackColor = Color.FromArgb(0, 255, 255, 255);
                    }
                }
            }
        }

        private void txtRG_Leave(object sender, EventArgs e)
        {
            if (txtRG.Text != string.Empty)
            {
                if (Validacoes.IsCampoPreenchido(txtRG) == true)
                {
                    if (Validacoes.IsRGValido(txtRG.Text) == false)
                    {
                        MensagemAtencao.MensagemCampoDigitadoInvalido("RG");

                        txtRG.BackColor = Color.LightGray;

                        txtRG.Focus();
                    }
                    else
                    {
                        txtRG.BackColor = Color.FromArgb(0, 255, 255, 255);
                    }
                }
            }
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (txtEmail.Text != string.Empty)
            {
                if (Validacoes.IsEmailValido(txtEmail.Text) == false)
                {
                    MensagemAtencao.MensagemCampoDigitadoInvalido("Email");

                    txtEmail.BackColor = Color.LightGray;

                    txtEmail.Focus();
                }
                else
                {
                    txtEmail.BackColor = Color.FromArgb(0, 255, 255, 255);
                }
            }
        }

        private void txtCEP_KeyPress(object sender, KeyPressEventArgs e)
        {
            ManipulacaoTextBox.DigitarApenasNumero(e, txtCEP);

            ManipulacaoTextBox.FormatoCEP(txtCEP);
        }

        private void txtCEP_Leave(object sender, EventArgs e)
        {
            Validacoes.IsCampoPreenchido(txtCEP);
        }

        private void txtNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            ManipulacaoTextBox.DigitarApenasLetrasOuNumeros(e, txtNumero);
        }

        private void txtComplemento_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void txtCidade_KeyPress(object sender, KeyPressEventArgs e)
        {
            ManipulacaoTextBox.DigitarApenasLetrasOuNumeros(e, txtCidade);
        }

        private void txtBairro_KeyPress(object sender, KeyPressEventArgs e)
        {
            ManipulacaoTextBox.DigitarApenasLetrasOuNumeros(e, txtBairro);
        }

        private void txtLimiteCredito_KeyPress(object sender, KeyPressEventArgs e)
        {
            ManipulacaoTextBox.FormatoDinheiro(e, sender, txtLimiteCredito);
        }

        private void SetandoDados()
        {
            lblDataCadastro.Text = DateTime.Now.ToString();

            PreenchimentoComboBoxEstado();

            LayoutTipoCadastro(tipoCadastro);
        }

        private void ChamandoAlertaSucessoNoCantoInferiorDireito()
        {
            DadosMensagemAlerta msg = new DadosMensagemAlerta("\n   Sucesso!", Resources.salvar_verde50);
            AlertaSalvar.Show(this, $"{msg.titulo}", msg.texto, string.Empty, msg.image, msg);
        }

        private void VerificarExitenciaCPF()
        {
            try
            {
                using (SistemaDeGerenciamento2_0Entities5 db = new SistemaDeGerenciamento2_0Entities5())
                {
                    var CPFCadastrado = db.tb_registro.Where(x => x.rg_cpf == txtCPF.Text).Select(x => x.rg_cpf).ToList();

                    if (CPFCadastrado.Count > 0)
                    {
                        isCPFJaExistente = true;
                    }
                    else
                    {
                        isCPFJaExistente = false;
                    }
                }
            }
            catch (Exception x)
            {
                LogErros.EscreverArquivoDeLog($"{DateTime.Now} - Erro ao Buscar CPF para verifica a exitencia no cadastro - Registro Pessoa Fisica | {x.Message} | {x.StackTrace}");

                MensagemErros.ErroAoBuscarCPFParaVerificacaoExistencia(x);
            }
        }

        private void VerificarIdade()
        {
            var dataNascimentoInformada = Convert.ToDateTime(txtDataNascimento.Text);

            if (dataNascimentoInformada.AddYears(18) > DateTime.Today)
            {
                txtDataNascimento.BackColor = Color.LightGray;

                MensagemAtencao.MensagemDataNasciemntoInvalida();

                txtDataNascimento.Focus();
            }
            else
            {
                txtDataNascimento.BackColor = Color.FromArgb(0, 255, 255, 255);
            }
        }

        private void LayoutTipoCadastro(string _tipoCadastro)
        {
            if (_tipoCadastro == "Fornecedor" || _tipoCadastro == "Funcionario")
            {
                cmbPrioridade.Enabled = false;
                cmbSituacao.Enabled = false;
                txtLimiteCredito.Enabled = false;
            }
            else
            {
                cmbPrioridade.Enabled = true;
                cmbSituacao.Enabled = true;
                txtLimiteCredito.Enabled = true;
            }
        }

        private void PreenchimentoComboBoxEstado()
        {
            ListaEstados.PreechendoListaEstado();

            cmbEstado.Properties.DataSource = ListaEstados.listaEstados;
            cmbEstado.Properties.DisplayMember = "Sigla";
            cmbEstado.Properties.ValueMember = "NomeEstado";
        }

        private void PreenchimentoPorCEP(DevExpress.XtraEditors.TextEdit _txtCEP)
        {
            if (txtCEP.Text != string.Empty)
            {
                if (Validacoes.IsCampoPreenchido(txtCEP) == true)
                {
                    var tarefa = Api.APICorreios((_txtCEP.Text).Replace("-", ""));
                    var esperador = tarefa.GetAwaiter();

                    esperador.OnCompleted(() =>
                    {
                        var item = Api.RetornoApi();

                        if (item.uf != null)
                        {
                            PreenchendoCamposCEP(item);

                            txtCEP.BackColor = Color.FromArgb(0, 255, 255, 255);
                        }
                        else
                        {
                            _txtCEP.Focus();

                            txtCEP.BackColor = Color.LightGray;

                            MensagemAtencao.MensagemCampoDigitadoInvalido("CEP");
                        }
                    });
                }
            }
            else
            {
                LimparCamposPreenchidoPorCEP();
            }
        }

        private void LimparCamposPreenchidoPorCEP()
        {
            cmbEstado.Text = string.Empty;
            txtCidade.Text = string.Empty;
            txtBairro.Text = string.Empty;
            txtLogradouro.Text = string.Empty;
            txtNumero.Text = string.Empty;
            txtComplemento.Text = string.Empty;
        }

        private void PreenchendoCamposCEP(DadosCEP _item)
        {
            txtLogradouro.Text = _item.logradouro;
            txtComplemento.Text = _item.complemento;
            txtBairro.Text = _item.bairro;
            txtCidade.Text = _item.localidade;
            cmbEstado.Text = _item.uf;
        }

        private bool VerificandoPreenchimentoBasico()
        {
            if (txtCPF.Text != string.Empty && txtNome.Text != string.Empty)
            {
                SetandoCorFundoComponentesBasico(Color.FromArgb(0, 255, 255, 255));

                return true;
            }
            else
            {
                SetandoCorFundoComponentesBasico(Color.LightGray);

                return false;
            }
        }

        private void SetandoCorFundoComponentesBasico(Color _corFundo)
        {
            txtCPF.BackColor = _corFundo;
            txtNome.BackColor = _corFundo;
        }

        private bool VerificandoPreenchimentoCampoEndereco()
        {
            if (cmbTipoEndereco.Text != string.Empty && cmbEstado.Text != string.Empty && txtCidade.Text != string.Empty &&
                txtBairro.Text != string.Empty && txtLogradouro.Text != string.Empty && txtNumero.Text != string.Empty)
            {
                SetandoCorFundoComponetesEndereco(Color.FromArgb(0, 255, 255, 255));

                return true;
            }
            else
            {
                SetandoCorFundoComponetesEndereco(Color.LightGray);

                return false;
            }
        }

        private void SetandoCorFundoComponetesEndereco(Color _corFundo)
        {
            cmbTipoEndereco.BackColor = _corFundo;
            txtCEP.BackColor = _corFundo;
            cmbEstado.BackColor = _corFundo;
            txtCidade.BackColor = _corFundo;
            txtBairro.BackColor = _corFundo;
            txtLogradouro.BackColor = _corFundo;
            txtNumero.BackColor = _corFundo;
        }

        private void SalvarInformacoesComerciais()
        {
            try
            {
                using (SistemaDeGerenciamento2_0Entities5 db = new SistemaDeGerenciamento2_0Entities5())
                {
                    var informacaoComercialPessoaFisica = new tb_informacoes_comeciais()
                    {
                        ic_limite_credito = Convert.ToDecimal(txtLimiteCredito.Text.Replace("R$ ", "")),
                        ic_prioridade = cmbPrioridade.Text,
                        ic_situacao = cmbSituacao.Text,
                        ic_vendedor = "Nome Usuario que esta cadastrando"
                    };

                    db.tb_informacoes_comeciais.Add(informacaoComercialPessoaFisica);
                    db.SaveChanges();

                    FK_InformacoesComerciais = informacaoComercialPessoaFisica.id_informacao_comercial;
                }
            }
            catch (Exception x)
            {
                LogErros.EscreverArquivoDeLog($"{DateTime.Now} - Erro ao Cadastrar Registro - Informações Comerciais - Pessoa Fisica | {x.Message} | {x.StackTrace}");

                MensagemErros.ErroAoCadastroRegistroInformacoesComerciaisPessoaFisica(x);
            }
        }

        private void SalvarEndereco()
        {
            try
            {
                using (SistemaDeGerenciamento2_0Entities5 db = new SistemaDeGerenciamento2_0Entities5())
                {
                    var enderecoPessoaFisica = new tb_enderecos()
                    {
                        ed_tipo = cmbTipoEndereco.Text,
                        ed_cep = txtCEP.Text,
                        ed_estado = cmbEstado.Text,
                        ed_cidade = txtCidade.Text,
                        ed_bairro = txtBairro.Text,
                        ed_locgradouro = txtLogradouro.Text,
                        ed_numero = txtNumero.Text,
                        ed_complemento = txtComplemento.Text,
                    };

                    db.tb_enderecos.Add(enderecoPessoaFisica);
                    db.SaveChanges();

                    FK_Endereco = enderecoPessoaFisica.id_endereco;
                }
            }
            catch (Exception x)
            {
                LogErros.EscreverArquivoDeLog($"{DateTime.Now} - Erro ao Cadastrar Registro - Endereço - Pessoa Fisica | {x.Message} | {x.StackTrace}");

                MensagemErros.ErroAoCadastroRegistroEnderecoPessoaFisica(x);
            }
        }

        private void SalvarRegisto()
        {
            try
            {
                using (SistemaDeGerenciamento2_0Entities5 db = new SistemaDeGerenciamento2_0Entities5())
                {
                    var pessoaFisica = new tb_registro()
                    {
                        rg_tipo_cadastro = tipoCadastro,
                        rg_data_cadastro = Convert.ToDateTime(lblDataCadastro.Text),
                        rg_categoria = "Pessoa Fisica",
                        rg_cpf = txtCPF.Text,
                        rg_rg = txtRG.Text,
                        rg_nome = txtNome.Text,
                        rg_sexo = cmbSexo.Text,
                        rg_data_nascimento = Convert.ToDateTime(txtDataNascimento.Text),
                        rg_email = txtEmail.Text,
                        rg_celular = txtCelular.Text,
                        rg_telefone_fixo = txtTelefoneResidencial.Text,
                        fk_endereco = FK_Endereco,
                        fk_informacao_comercial = FK_InformacoesComerciais,
                    };

                    db.tb_registro.Add(pessoaFisica);
                    db.SaveChanges();
                }
            }
            catch (Exception x)
            {
                LogErros.EscreverArquivoDeLog($"{DateTime.Now} - Erro ao Cadastrar Registro | {x.Message} | {x.StackTrace}");

                MensagemErros.ErroAoCadastroRegistroPessoaFisica(x);
            }
        }

        private void txtLimiteCredito_Leave(object sender, EventArgs e)
        {
            decimal valorCredito = Convert.ToDecimal(txtLimiteCredito.Text.Replace("R$ ", ""));

            if (valorCredito < 0)
            {
                MensagemAtencao.MensagemNaoAceitoValoresNegativos();

                txtLimiteCredito.Focus();
            }
        }

        private void txtCelular_Leave(object sender, EventArgs e)
        {
            if (txtCelular.Text != string.Empty)
            {
                if (Validacoes.IsCampoPreenchido(txtCelular) == false)
                {
                    txtCelular.BackColor = Color.LightGray;

                    MensagemAtencao.MensagemCampoDigitadoInvalido("Celular");

                    txtCelular.Focus();
                }
                else
                {
                    txtCelular.BackColor = Color.FromArgb(0, 255, 255, 255);
                }
            }
        }

        private void txtTelefoneResidencial_Leave(object sender, EventArgs e)
        {
            if (txtTelefoneResidencial.Text != string.Empty)
            {
                if (Validacoes.IsCampoPreenchido(txtTelefoneResidencial) == false)
                {
                    txtTelefoneResidencial.BackColor = Color.LightGray;

                    MensagemAtencao.MensagemCampoDigitadoInvalido("Telefone");

                    txtTelefoneResidencial.Focus();
                }
                else
                {
                    txtTelefoneResidencial.BackColor = Color.FromArgb(0, 255, 255, 255);
                }
            }
        }
    }
}