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
    
    public partial class tb_estoque
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_estoque()
        {
            this.tb_nota_fiscal_saida = new HashSet<tb_nota_fiscal_saida>();
        }
    
        public int id_estoque { get; set; }
        public decimal ep_quantidade { get; set; }
        public System.DateTime ep_data_entrada { get; set; }
        public Nullable<System.DateTime> ep_data_exclusao { get; set; }
        public string ep_codigo_barras { get; set; }
        public string ep_status_situacao { get; set; }
        public int fk_produto { get; set; }
        public int fk_nota_fiscal_entrada { get; set; }
    
        public virtual tb_produto tb_produto { get; set; }
        public virtual tb_nota_fiscal_entrada tb_nota_fiscal_entrada { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_nota_fiscal_saida> tb_nota_fiscal_saida { get; set; }
    }
}
