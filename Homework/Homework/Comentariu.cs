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
    
    public partial class Comentariu
    {
        public int id_comentariu { get; set; }
        public int id_tema { get; set; }
        public System.DateTime data { get; set; }
        public int id_user { get; set; }
        public string text { get; set; }
    
        public virtual Tema Tema { get; set; }
        public virtual User User { get; set; }
    }
}
