//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyFitnessTracker
{
    using System;
    using System.Collections.Generic;
    
    public partial class Set
    {
        public Set()
        {
            this.SetExcercises = new HashSet<SetExcercis>();
        }
    
        public int SetId { get; set; }
        public string Name { get; set; }
        public string UserID { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual ICollection<SetExcercis> SetExcercises { get; set; }
    }
}
