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
    
    public partial class tb_grupo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_grupo()
        {
            this.tb_configuracao_financeira = new HashSet<tb_configuracao_financeira>();
            this.tb_produto = new HashSet<tb_produto>();
        }
    
        public int id_grupo { get; set; }
        public string gp_nome_grupo { get; set; }
        public string gp_nome_agrupador { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_configuracao_financeira> tb_configuracao_financeira { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_produto> tb_produto { get; set; }
    }
}
