//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SistemaDeGerenciamento2_0
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_despesa
    {
        public int id_despesa { get; set; }
        public System.DateTime dp_data { get; set; }
        public string dp_observacao { get; set; }
        public decimal dp_sub_valor_total { get; set; }
        public decimal dp_desconto { get; set; }
        public decimal dp_valor_lancamento { get; set; }
        public string dp_forma_pagamento { get; set; }
        public string dp_pagamento_em { get; set; }
        public System.DateTime dp_vencimento { get; set; }
        public int fk_registro { get; set; }
        public int fk_grupo { get; set; }
    
        public virtual tb_registro tb_registro { get; set; }
        public virtual tb_grupo tb_grupo { get; set; }
    }
}
