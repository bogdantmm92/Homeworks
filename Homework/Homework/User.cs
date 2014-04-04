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
    
    public partial class User
    {
        public User()
        {
            this.Comentarius = new HashSet<Comentariu>();
            this.Participas = new HashSet<Participa>();
            this.Ratings = new HashSet<Rating>();
            this.Submits = new HashSet<Submit>();
            this.Temas = new HashSet<Tema>();
        }
    
        public int id_user { get; set; }
        public string nume { get; set; }
        public string prenume { get; set; }
        public string email { get; set; }
        public string parola { get; set; }
        public int tip { get; set; }
        public int id_liceu { get; set; }
        public string clasa { get; set; }
        public Nullable<int> an_studiu { get; set; }
        public int activ { get; set; }
    
        public virtual ICollection<Comentariu> Comentarius { get; set; }
        public virtual Liceu Liceu { get; set; }
        public virtual ICollection<Participa> Participas { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Submit> Submits { get; set; }
        public virtual ICollection<Tema> Temas { get; set; }
    }
}
