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
    
    public partial class pracownicy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public pracownicy()
        {
            this.wynajem = new HashSet<wynajem>();
        }
    
        public int id_pracownika { get; set; }
        public string imie { get; set; }
        public string nazwisko { get; set; }
        public string dzial { get; set; }
        public string stanowisko { get; set; }
        public Nullable<decimal> pensja { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<wynajem> wynajem { get; set; }
    }
}
