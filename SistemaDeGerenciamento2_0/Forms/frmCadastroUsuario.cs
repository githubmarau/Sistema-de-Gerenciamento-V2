﻿using DevExpress.XtraGrid;
using DevExpress.XtraSplashScreen;
using SistemaDeGerenciamento2_0.Class;
using SistemaDeGerenciamento2_0.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static SistemaDeGerenciamento2_0.Class.Estados;

namespace SistemaDeGerenciamento2_0.Forms
{
    public partial class frmCadastroUsuario : DevExpress.XtraEditors.XtraForm
    {
        private int X = 0;
        private int Y = 0;

        public int? FK_Permissoes = 0;

        private int? IDCadastro = 0;

        private string nomeFuncionario = string.Empty;

        private Form frmTelaPrincipal;

        public frmCadastroUsuario(int? _IDCadastro, Form _frmTelaPrincipal, string _nomeFuncionario)
        {
            InitializeComponent();
            // This line of code is generated by Data Source Configuration Wizard
            // Fill the SqlDataSource asynchronously
            sqlDataSource2.FillAsync();

            txtSenha.Properties.UseSystemPasswordChar = true;

            txtConfirmacaoSenha.Properties.UseSystemPasswordChar = true;

            frmTelaPrincipal = _frmTelaPrincipal;

            IDCadastro = _IDCadastro;

            nomeFuncionario = _nomeFuncionario;

            ReloadData();

            BuscarFKPermissoes();

            PreenchendoComboBoxFunionarioSelecionado();
        }

        public class IdNomeFuncionario
        {
            public int ID { get; set; }
            public string NomeFuncionario { get; set; }
        }

        private List<IdNomeFuncionario> listaUsuario = new List<IdNomeFuncionario>();

        public frmCadastroUsuario()
        {
            InitializeComponent();
            // This line of code is generated by Data Source Configuration Wizard
            // Fill the SqlDataSource asynchronously
            sqlDataSource2.FillAsync();

            txtSenha.Properties.UseSystemPasswordChar = true;
            txtConfirmacaoSenha.Properties.UseSystemPasswordChar = true;
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            FecharTela();
        }

        private void FecharTela()
        {
            MensagemAtencao.MensagemCancelar(this);
        }

        private void txtSenha_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            ManipulacaoTextBox.DigitoValidoParaSenha(e);
        }

        private void txtConfirmacaoSenha_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            ManipulacaoTextBox.DigitoValidoParaSenha(e);
        }

        private void txtConfirmacaoSenha_Leave(object sender, EventArgs e)
        {
            if (txtConfirmacaoSenha.Text != string.Empty)
            {
                if (txtSenha.Text != txtConfirmacaoSenha.Text)
                {
                    MensagemAtencao.MensagemSenhasDivergentes();

                    txtConfirmacaoSenha.BackColor = Color.LightGray;
                }
                else
                {
                    txtConfirmacaoSenha.BackColor = Color.FromArgb(0, 255, 255, 255);
                }
            }
        }

        private void cmbFuncionario_TextChanged_1(object sender, EventArgs e)
        {
            IDCadastro = Convert.ToInt32(cmbFuncionario.Properties.GetKeyValueByDisplayValue(cmbFuncionario.Text));

            ReloadData();

            BuscarFKPermissoes();

            TelaPermissoes();
        }

        private void pcbExibirSenha_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            txtSenha.Properties.UseSystemPasswordChar = false;
            txtConfirmacaoSenha.Properties.UseSystemPasswordChar = false;

            pcbExibirSenha.Image = Resources.olho_aberto_20;
        }

        private void pcbExibirSenha_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            txtSenha.Properties.UseSystemPasswordChar = true;

            txtConfirmacaoSenha.Properties.UseSystemPasswordChar = true;

            pcbExibirSenha.Image = Resources.olho_20;
        }

        private void frmCadastroUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                FecharTela();
            }
        }

        private void frmCadastroUsuario_Shown(object sender, EventArgs e)
        {
            TelaPermissoes();

            cmbFuncionario2.ItemIndex = 0;
        }

        private void PreenchendoComboBoxFunionarioSelecionado()
        {
            listaUsuario.Clear();
            listaUsuario.Add(new IdNomeFuncionario { ID = Convert.ToInt32(IDCadastro), NomeFuncionario = nomeFuncionario });

            cmbFuncionario2.Visible = true;
            cmbFuncionario.Visible = false;

            cmbFuncionario2.Properties.DataSource = null;
            cmbFuncionario2.Properties.DataSource = listaUsuario;
            cmbFuncionario2.Properties.DisplayMember = "NomeFuncionario";
            cmbFuncionario2.Properties.ValueMember = "ID";
        }

        private void TelaPermissoes()
        {
            pnlPermissoes.Controls.Clear();
            frmPermissoes frmPermissoes = new frmPermissoes(this);
            frmPermissoes.TopLevel = false;
            pnlPermissoes.Controls.Add(frmPermissoes);
            pnlPermissoes.Tag = frmPermissoes;
            frmPermissoes.Show();
        }

        private void ReloadData()
        {
            if (frmTelaPrincipal != null)
            {
                using (var handle = SplashScreenManager.ShowOverlayForm(frmTelaPrincipal))
                {
                    BuscarFKPermissoes();
                }
            }
            else
            {
                using (var handle = SplashScreenManager.ShowOverlayForm(this))
                {
                    BuscarFKPermissoes();
                }
            }
        }

        private void BuscarFKPermissoes()
        {
            try
            {
                using (SistemaDeGerenciamento2_0Entities7 db = new SistemaDeGerenciamento2_0Entities7())
                {
                    var FKPermissoes = db.tb_registro.Where(x => x.id_registro == IDCadastro)
                    .Select(x => new { x.fk_permissoes, x.rg_login, x.rg_senha }).ToList();

                    foreach (var item in FKPermissoes)
                    {
                        txtNomeUsuario.Text = item.rg_login;
                        txtSenha.Text = item.rg_senha;
                        txtConfirmacaoSenha.Text = item.rg_senha;
                        FK_Permissoes = item.fk_permissoes;
                    }
                }
            }
            catch (Exception x)
            {
                LogErros.EscreverArquivoDeLog($"{DateTime.Now} - Erro ao Buscar Usuario e Senha na tela de Cadastro Novo Usuário - | {x.Message} | {x.StackTrace}");

                MensagemErros.ErroAoBuscarDadosNovoUsuario(x);
            }
        }

        private void frmCadastroUsuario_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            this.Left = X + MousePosition.X;
            this.Top = Y + MousePosition.Y;
        }

        private void frmCadastroUsuario_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            X = this.Left - MousePosition.X;
            Y = this.Top - MousePosition.Y;
        }
    }
}