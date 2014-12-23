//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Exercise
    {
        public Exercise()
        {
            this.ExerciseAttributes = new HashSet<ExerciseAttribute>();
            this.ExerciseRecords = new HashSet<ExerciseRecord>();
        }
    
        public long Id { get; set; }
        public string Name { get; set; }
        public double Target { get; set; }
        public string UserID { get; set; }
        public long SetId { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Set Set { get; set; }
        public virtual ICollection<ExerciseAttribute> ExerciseAttributes { get; set; }
        public virtual ICollection<ExerciseRecord> ExerciseRecords { get; set; }
    }
}