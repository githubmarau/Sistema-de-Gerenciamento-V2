﻿using SistemaDeGerenciamento2_0.Forms;
using System.Windows.Forms;

namespace SistemaDeGerenciamento2_0
{
    internal class MensagemAtencao
    {
        public static void MensagemCancelar(Form form)
        {
            DialogResult OpcaoDoUsuario = new DialogResult();
            OpcaoDoUsuario = MessageBox.Show("Realmente Deseja Cancelar?", "Atenção!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (OpcaoDoUsuario == DialogResult.Yes)
            {
                form.Close();
            }
        }

        public static void MensagemPreencherCampos()
        {
            MessageBox.Show("Por Favor, Preencher Campos Obrigatorios!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void MensagemProdutoJaExistente()
        {
            MessageBox.Show("Desculpe, codigo de produto ou nome de produto ja existente, por favor digite um valor diferente!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void MensagemCampoDigitadoInvalido(string _corpo)
        {
            MessageBox.Show($"Por Favor Digite Um {_corpo} válido!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void MensagemDataNasciemntoInvalida()
        {
            MessageBox.Show("Desculpe, Cliente / Fornecedor menor de Idade!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void MensagemNaoCadastrado(string _corpo)
        {
            MessageBox.Show($"Desculpe, {_corpo} não cadastrado!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void MensagemJaExistente(string _corpoMensagem)
        {
            MessageBox.Show($"Desculpe, {_corpoMensagem} ja existente, por favor digite um valor diferente!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void MensagemNaoAceitoValoresNegativos()
        {
            MessageBox.Show("Desculpe, não é aceitos valores negativos!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void MensagemProdutoComValoresDivergentes()
        {
            MessageBox.Show("Desculpe, Produtos com Valores Divergentes do Cadastrado!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void MensagemSenhasDivergentes()
        {
            MessageBox.Show("Desculpe, Senhas Divergentes Por Favor Verique!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void MensagemUsuarioSenhasNaoEncontrados()
        {
            MessageBox.Show("Usuário/Senhas Divergentes Por Favor Verique!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void MensagemUsuarioSemPermissao()
        {
            MessageBox.Show("Desculpe, Usuário Sem Permissão!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void MensagemQuantidadeIndisponivel()
        {
            MessageBox.Show("Desculpe, Quantidade Solicitada Indisponivel!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void MensagemDataVencimentoMaiorQueLancamento()
        {
            MessageBox.Show("Atenção, Data de Vencimento é maior que Data de Lançamento!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void MensagemPreenchimentoCampoPeriocidade()
        {
            MessageBox.Show("Atenção, Campo de Periocidade deve estar Preenchido!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void MensagemDataFinalMaiorQueDataInicial()
        {
            MessageBox.Show("Atenção, Data Final informada maior que a data incial!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void MensagemExcluir(frmFinanceiro frmFinanceiro)
        {
            DialogResult OpcaoDoUsuario = new DialogResult();
            OpcaoDoUsuario = MessageBox.Show("Realmente Deseja Excluir?", "Atenção!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (OpcaoDoUsuario == DialogResult.Yes)
            {
                frmFinanceiro.RemoverDespesaConta();
            }
        }

        public static void MensagemConfirmar(frmPagamentoDespesa frmPagamentoDespesa)
        {
            DialogResult OpcaoDoUsuario = new DialogResult();
            OpcaoDoUsuario = MessageBox.Show("Realmente Deseja Realizar o Pagamento?", "Atenção!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (OpcaoDoUsuario == DialogResult.Yes)
            {
                frmPagamentoDespesa.RealizarPagamento();
            }
        }
    }
}