//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectC_github
{
    using System;
    using System.Collections.Generic;
    
    public partial class samochody
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public samochody()
        {
            this.rej_samochody = new HashSet<rej_samochody>();
        }
    
        public int id_samochodu { get; set; }
        public string nr_rejestracyjny { get; set; }
        public string marka { get; set; }
        public string model { get; set; }
        public string wersja { get; set; }
        public string rocznik { get; set; }
        public string poj_silnika { get; set; }
        public string rodzaj_paliwa { get; set; }
        public int przebieg { get; set; }
        public int id_cennik { get; set; }
    
        public virtual cennik cennik { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rej_samochody> rej_samochody { get; set; }
    }
}
