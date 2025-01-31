﻿using SistemaDeGerenciamento2_0.Class;
using SistemaDeGerenciamento2_0.Context;
using SistemaDeGerenciamento2_0.Models;
using SistemaDeGerenciamento2_0.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SistemaDeGerenciamento2_0.Forms
{
    public partial class frmCadastroGrupo : DevExpress.XtraEditors.XtraForm
    {
        private int X = 0;
        private int Y = 0;

        public frmCadastroGrupo()
        {
            InitializeComponent();

            PreencherComboBoxAgrupador();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            FecharTela();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        private void Salvar()
        {
            if (txtNomeGrupo.Text != string.Empty && cmbAgrupador.Text != string.Empty)
            {
                if (IsGrupoComMesmoNomeEAgrupadorExiste() == false)
                {
                    ConexaoSalvar();

                    txtNomeGrupo.BackColor = Color.FromArgb(0, 255, 255, 255);

                    cmbAgrupador.BackColor = Color.FromArgb(0, 255, 255, 255);
                }
                else
                {
                    MensagemAtencao.MensagemJaExistente("Grupo e Agrupador");
                }
            }
            else
            {
                txtNomeGrupo.BackColor = Color.LightGray;

                cmbAgrupador.BackColor = Color.LightGray;

                MensagemAtencao.MensagemPreencherCampos();

                txtNomeGrupo.Focus();
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            FecharTela();
        }

        private void frmAdicionarGrupo_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            this.Left = X + MousePosition.X;
            this.Top = Y + MousePosition.Y;
        }

        private void frmAdicionarGrupo_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            X = this.Left - MousePosition.X;
            Y = this.Top - MousePosition.Y;
        }

        private void frmAdicionarGrupo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                FecharTela();
            }
            else if (e.KeyCode == Keys.F10)
            {
                Salvar();
            }
        }

        private void PreencherComboBoxAgrupador()
        {
            try
            {
                using (SistemaDeGerenciamento2_0Context db = new SistemaDeGerenciamento2_0Context())
                {
                    List<string> listaSubGrupo = new List<string>();

                    listaSubGrupo = db.tb_grupo.Select(x => x.gp_nome_agrupador).Distinct().ToList();

                    listaSubGrupo.ForEach(x => cmbAgrupador.Properties.Items.Add(x));
                }
            }
            catch (Exception x)
            {
                LogErros.EscreverArquivoDeLog($"{DateTime.Now} - Erro ao Buscar Agrupador na tela Grupo - | {x.Message} | {x.StackTrace}");

                MensagemErros.ErroAoBuscarAgrupadorTelaGrupo(x);
            }
        }

        private void FecharTela()
        {
            if (txtNomeGrupo.Text == string.Empty && cmbAgrupador.Text == string.Empty)
            {
                this.Close();
            }
            else
            {
                MensagemAtencao.MensagemCancelar(this);
            }
        }

        private bool IsGrupoComMesmoNomeEAgrupadorExiste()
        {
            try
            {
                using (SistemaDeGerenciamento2_0Context db = new SistemaDeGerenciamento2_0Context())
                {
                    var IsExisteGrupo = db.tb_grupo.Where(x => x.gp_nome_grupo.Equals(txtNomeGrupo.Text)).Any();

                    return IsExisteGrupo;
                }
            }
            catch (Exception x)
            {
                LogErros.EscreverArquivoDeLog($"{DateTime.Now} - Erro ao Buscar Grupo e Agrupador - | {x.Message} | {x.StackTrace}");

                MensagemErros.ErroAoBuscarGrupoEAgrupador(x);

                return false;
            }
        }

        private void ConexaoSalvar()
        {
            try
            {
                var grupoProduto = new tb_grupo() { gp_nome_grupo = txtNomeGrupo.Text, gp_nome_agrupador = cmbAgrupador.Text };

                using (SistemaDeGerenciamento2_0Context db = new SistemaDeGerenciamento2_0Context())
                {
                    db.tb_grupo.Add(grupoProduto);
                    db.SaveChanges();

                    AlertaConfirmacao.ChamandoAlertaSucessoNoCantoInferiorDireito(AlertaSalvar, this);

                    txtNomeGrupo.Text = string.Empty;
                    cmbAgrupador.Text = string.Empty;
                }
            }
            catch (Exception x)
            {
                LogErros.EscreverArquivoDeLog($"{DateTime.Now} - Erro ao Cadastrar Grupo - | {x.Message} | {x.StackTrace}");

                MensagemErros.ErroAoCadastroGrupo(x);
            }
        }

        private void cmbAgrupador_KeyPress(object sender, KeyPressEventArgs e)
        {
            ManipulacaoTextBox.DigitarApenasLetras(e, cmbAgrupador);
        }
    }
}