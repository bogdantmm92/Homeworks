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
    
    public partial class Rating
    {
        public int id_user { get; set; }
        public int id_tema { get; set; }
        public int id_rating { get; set; }
        public int rating1 { get; set; }
    
        public virtual Tema Tema { get; set; }
        public virtual User User { get; set; }
    }
}
