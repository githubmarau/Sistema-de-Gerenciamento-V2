﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDeGerenciamento2_0.Models
{
    public partial class tb_permissoes
    {
        public tb_permissoes()
        {
            tb_registro = new HashSet<tb_registro>();
        }

        [Key]
        public int id_permissoes { get; set; }
        public bool pm_vendas_do_dias { get; set; }
        public bool pm_receber_conta { get; set; }
        public bool pm_todas_vendas { get; set; }
        public bool pm_remover_venda { get; set; }
        public bool pm_cancelar_venda { get; set; }
        public bool pm_realizar_devolucao { get; set; }
        public bool pm_alerta_contas_atrasadas { get; set; }
        public bool pm_visualizar_contas_pagar { get; set; }
        public bool pm_receber_contas { get; set; }
        public bool pm_visualizar_fluxo_caixa { get; set; }
        public bool pm_criar_editar_usuario { get; set; }
        public bool pm_visualizar_visao_geral { get; set; }
        public bool pm_visualizar_dre { get; set; }
        public bool pm_criar_visualizar_relatorios { get; set; }
        public bool pm_adicionar_produto { get; set; }
        public bool pm_editar_produto { get; set; }
        public bool pm_remover_produto { get; set; }
        public bool pm_visualizar_todos_produtos { get; set; }
        public bool pm_alerta_estoque_baixo { get; set; }
        public bool pm_efetuar_cadastro { get; set; }
        public bool pm_editar_cadastro { get; set; }
        public bool pm_remover_cadastro { get; set; }
        public bool pm_visualizar_cadastro_completo { get; set; }
        public bool pm_acesso_pdv { get; set; }

        [InverseProperty("fk_permissoesNavigation")]
        public virtual ICollection<tb_registro> tb_registro { get; set; }
    }
}