﻿using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using SistemaDeGerenciamento2_0.Class;
using System.Diagnostics;

namespace SistemaDeGerenciamento2_0.Forms
{
    public partial class frmCadastroProduto : DevExpress.XtraEditors.XtraForm
    {
        private int X = 0;
        private int Y = 0;

        private List<GrupoStruct> Grupo = new List<GrupoStruct>();

        public frmCadastroProduto()
        {
            InitializeComponent();

            var tarefa = Task.Run(() =>
            {
                PreencherComboBoxGrupo();
            });
        }

        private class GrupoStruct
        {
            public int idGrupo { get; set; }
            public string nomeGrupo { get; set; }
            public string nomeAgrupador { get; set; }
        }

        private async Task PreencherComboBoxGrupo()
        {
            try
            {
                using (SistemaDeGerenciamento2_0Entities3 db = new SistemaDeGerenciamento2_0Entities3())
                {
                    Grupo = db.tb_grupo.Where(x => !string.IsNullOrEmpty(x.gp_nome_grupo))
                       .Select(x => new GrupoStruct { idGrupo = x.id_grupo, nomeGrupo = x.gp_nome_grupo, nomeAgrupador = x.gp_nome_agrupador })
                       .Distinct()
                       .ToList();

                    cmbGrupo.Properties.DataSource = Grupo;
                    cmbGrupo.Properties.DisplayMember = "nomeGrupo";
                    cmbGrupo.Properties.ValueMember = "idGrupo";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void frmCadastroProduto_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            this.Left = X + MousePosition.X;
            this.Top = Y + MousePosition.Y;
        }

        private void frmCadastroProduto_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            X = this.Left - MousePosition.X;
            Y = this.Top - MousePosition.Y;
        }

        private void VerificacaoFecharTela()
        {
            if (txtNome.Text != string.Empty || cmbGrupo.Text != string.Empty || cmbFinalidade.Text != string.Empty ||
                  cmbTipoProduto.Text != string.Empty)
            {
                MensagemAtencao.MensagemCancelar(this);
            }
            else
            {
                this.Close();
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            VerificacaoFecharTela();
        }

        private void frmCadastroProduto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                VerificacaoFecharTela();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            VerificacaoFecharTela();
        }

        private void btnAcessarGrupoSubGrupo_Click(object sender, EventArgs e)
        {
            frmCadastroGrupoSubGrupo frmCadastroGrupoSubGrupo = new frmCadastroGrupoSubGrupo();
            frmCadastroGrupoSubGrupo.ShowDialog();
        }

        private void txtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            ManipulacaoTextBox.TeclaDigitadaFoiLetrasOuNumeros(e, txtCodigo);
        }

        private void txtCodigoDeBarras_KeyPress(object sender, KeyPressEventArgs e)
        {
            ManipulacaoTextBox.TeclaDigitadaFoiNumero(e, txtCodigoDeBarras);
        }

        private void txtNome_KeyPress(object sender, KeyPressEventArgs e)
        {
            ManipulacaoTextBox.TeclaDigitadaFoiLetras(e, txtNome);
        }

        private void txtTipoUnidade_KeyPress(object sender, KeyPressEventArgs e)
        {
            ManipulacaoTextBox.TeclaDigitadaFoiLetras(e, txtTipoUnidade);
        }

        private void txtEstoqueMinimo_KeyPress(object sender, KeyPressEventArgs e)
        {
            ManipulacaoTextBox.TeclaDigitadaFoiNumero(e, txtEstoqueMinimo);
        }

        private void txtCusto_KeyPress(object sender, KeyPressEventArgs e)
        {
            ManipulacaoTextBox.FormatoDinheiro(e, sender, txtCusto);
        }

        private void txtPreco_KeyPress(object sender, KeyPressEventArgs e)
        {
            ManipulacaoTextBox.FormatoDinheiro(e, sender, txtPreco);
        }

        private void txtMargemLucro_KeyPress(object sender, KeyPressEventArgs e)
        {
            ManipulacaoTextBox.PreenchimentoPorcentagem(e, sender, txtMargemLucro);
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        private void Salvar()
        {
            if (txtNome.Text != string.Empty || cmbGrupo.Text != string.Empty || cmbFinalidade.Text != string.Empty ||
                cmbTipoProduto.Text != string.Empty || txtTipoUnidade.Text != "Ex.: Peça, Un, Kg")
            {
                ConexaoSalvar();
            }
            else
            {
                MensagemAtencao.MensagemPreencherCampos(this);
            }
        }

        private void ConexaoSalvar()
        {
            try
            {
                using (SistemaDeGerenciamento2_0Entities3 db = new SistemaDeGerenciamento2_0Entities3())
                {
                    var CadastroProduto = new tb_produto()
                    {
                        pd_codigo = txtCodigo.Text,
                        pd_codigo_barras = txtCodigoDeBarras.Text,
                        pd_finalidade = cmbFinalidade.Text,
                        pd_nome = txtNome.Text,
                        pd_estoque_minimo = Convert.ToDecimal(txtEstoqueMinimo.Text),
                        pd_custo = Convert.ToDecimal(txtCusto.Text.Replace("R$ ", "")),
                        pd_margem = Convert.ToDecimal(txtMargemLucro.Text.Replace("%", "")),
                        pd_preco = Convert.ToDecimal(txtPreco.Text.Replace("R$ ", "")),
                        pd_tipo_produto = cmbTipoProduto.Text,
                        pd_tipo_unidade = txtTipoUnidade.Text,
                        pd_observacoes = txtObservacoes.Text,
                        fk_grupo = Convert.ToInt32(cmbGrupo.Properties.GetKeyValueByDisplayValue(cmbGrupo.Text)),
                    };

                    db.tb_produto.Add(CadastroProduto);
                    db.SaveChanges();
                }
            }
            catch (Exception x)
            {
                LogErros.EscreverArquivoDeLog($"Erro ao Cadastrar Produto {x.ToString()}");

                MensagemErros.ErroAoCadastroProduto(x);
            }
        }

        private void txtCusto_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtCusto.Text != string.Empty && txtCusto.Text != "R$ 0,00" && txtCusto.Text != "R$ " && txtCusto.Text != "R$" && txtCusto.Text != "R")
            {
                CalcularPreco();
            }
        }

        private void CalcularPreco()
        {
            decimal valorCusto = Convert.ToDecimal(txtCusto.Text.Replace("R$", ""));

            decimal margemLucro = Convert.ToDecimal(txtMargemLucro.Text.Replace("%", ""));

            decimal valorPreco = 0;

            valorPreco = (valorCusto * margemLucro / 100) + valorCusto;

            txtPreco.Text = valorPreco.ToString("C");
        }

        private void txtMargemLucro_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtMargemLucro.Text != string.Empty)
            {
                CalcularPreco();
            }
            else
            {
                txtPreco.Text = txtCusto.Text;
            }
        }

        private void txtPreco_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtPreco.Text != string.Empty && txtPreco.Text != "R$ 0,00" && txtPreco.Text != "R$ " && txtPreco.Text != "R$" && txtPreco.Text != "R")
            {
                CalcularMargem();
            }
        }

        private void CalcularMargem()
        {
            decimal valorCusto = Convert.ToDecimal(txtCusto.Text.Replace("R$", ""));

            decimal valorPreco = Convert.ToDecimal(txtPreco.Text.Replace("R$", "")); ;

            decimal margemLucro = 0;

            margemLucro = ((valorPreco * 100) / valorCusto) / 100;

            txtMargemLucro.Text = (margemLucro - 1).ToString("P");
        }
    }
}