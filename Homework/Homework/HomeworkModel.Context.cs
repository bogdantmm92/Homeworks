﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HomeworkContext : DbContext
    {
        public HomeworkContext()
            : base("name=HomeworkContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Comentariu> Comentarius { get; set; }
        public DbSet<Fisier> Fisiers { get; set; }
        public DbSet<Liceu> Liceus { get; set; }
        public DbSet<Participa> Participas { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Submit> Submits { get; set; }
        public DbSet<Tema> Temas { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
