//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Homework
{
    using System;
    using System.Collections.Generic;
    
    public partial class Fisier
    {
        public Fisier()
        {
            this.Submits = new HashSet<Submit>();
            this.Temas = new HashSet<Tema>();
            this.Temas1 = new HashSet<Tema>();
        }
    
        public int id_fisier { get; set; }
        public string cale { get; set; }
    
        public virtual ICollection<Submit> Submits { get; set; }
        public virtual ICollection<Tema> Temas { get; set; }
        public virtual ICollection<Tema> Temas1 { get; set; }
    }
}
