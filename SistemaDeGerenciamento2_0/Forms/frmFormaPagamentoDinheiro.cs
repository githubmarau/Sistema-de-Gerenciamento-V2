﻿using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using SistemaDeGerenciamento2_0.Class;
using SistemaDeGerenciamento2_0.Context;
using SistemaDeGerenciamento2_0.Models;
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
    public partial class frmFormaPagamentoDinheiro : DevExpress.XtraEditors.XtraForm
    {
        public decimal valorPagoNoProduto = 0;
        public decimal valorJuros = 0;
        public decimal valorFinalPago = 0;

        private string numeroNF;

        private frmPDV frmPDV;

        private frmPagamento frmPagamento;

        private frmClienteCPF frmClienteCPF;

        private frmTelaPrincipal frmTelaPrincipal;

        private PermissoesUsuario permissoesUsuario = new PermissoesUsuario();

        public frmFormaPagamentoDinheiro(string _valorFinalPago, string _numeroNF, decimal _valorPagoNoProduto,
            decimal _valorJuros, frmTelaPrincipal _frmTelaPrincipal, frmPagamento _frmPagamento, frmPDV _frmPDV)
        {
            InitializeComponent();

            lblValorTotal.Text = _valorFinalPago;

            numeroNF = _numeroNF;

            valorPagoNoProduto = _valorPagoNoProduto;

            valorJuros = _valorJuros;

            frmPDV = _frmPDV;

            frmPagamento = _frmPagamento;

            frmTelaPrincipal = _frmTelaPrincipal;

            valorFinalPago = Convert.ToDecimal(_valorFinalPago.Replace("R$", ""));

            lblNomeUsuario.Text = frmLogin.UsuarioLogado.ToUpper();
        }

        private void txtValorEntregue_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtValorEntregue.Text != string.Empty)
            {
                lblTroco.Text = (Convert.ToDecimal(txtValorEntregue.Text.Replace("R$", "")) - Convert.ToDecimal(lblValorTotal.Text.Replace("R$", ""))).ToString("C2");
            }
        }

        private void btn1FinalizarVenda_Click(object sender, EventArgs e)
        {
            FinalizarVenda();
        }

        private void FinalizarVenda()
        {
            NFSaida.NotaFiscalSaida(numeroNF, valorPagoNoProduto, valorJuros, valorFinalPago, "Dinheiro");

            AlterarEstoque.AlterandoEstoque();

            btn1CancelarVenda.Enabled = false;
            btn1FinalizarVenda.Enabled = false;
            txtValorEntregue.Enabled = false;

            ImprimirCupomFiscal(numeroNF);

            frmPDV.ZerandoTodosCampos();

            frmPagamento.Close();

            this.Close();
        }

        private void ImprimirCupomFiscal(string _numeroNF)
        {
            frmCupomFiscal frmCupomFiscal = new frmCupomFiscal();
            frmCupomFiscal.Parameters["parameter1"].Value = _numeroNF;
            frmCupomFiscal.ShowPreviewDialog();
        }

        private void btn1CancelarVenda_Click(object sender, EventArgs e)
        {
            CancelarVenda();
        }

        private void CancelarVenda()
        {
            permissoesUsuario.ReloadData(frmTelaPrincipal, frmLogin.UsuarioLogado);
            permissoesUsuario.VerificarCancelarVendaTelaPDV("Cancelar Venda Tela PDV");

            if (frmPDV.permissaoCancelarVenda == true)
            {
                DialogResult OpcaoDoUsuario = new DialogResult();
                OpcaoDoUsuario = MessageBox.Show("Realmente Cancela a Venda?", "Atenção!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (OpcaoDoUsuario == DialogResult.Yes)
                {
                    frmPDV.permissaoCancelarVenda = false;

                    frmPagamento.Close();

                    frmPDV.ZerandoTodosCampos();

                    frmPagamento.Close();

                    this.Close();
                }
            }
        }

        private void frmFormaPagamentoDinheiro_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                FinalizarVenda();
            }
            else if (e.KeyCode == Keys.F9)
            {
                CancelarVenda();
            }
        }
    }
}