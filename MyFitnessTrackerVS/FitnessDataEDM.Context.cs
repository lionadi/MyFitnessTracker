﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyFitnessTrackerVS
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MyFitnessTrackerDBEntities : DbContext
    {
        public MyFitnessTrackerDBEntities()
            : base("name=MyFitnessTrackerDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Exercise> Exercises { get; set; }
        public virtual DbSet<ExerciseRecord> ExerciseRecords { get; set; }
        public virtual DbSet<Set> Sets { get; set; }
        public virtual DbSet<ExerciseAttribute> ExerciseAttributes { get; set; }
    }
}
