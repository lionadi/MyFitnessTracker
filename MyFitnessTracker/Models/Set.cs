using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFitnessTracker.Models
{
    public class Set
    {
        public int SetId { get; set; }
        public String Name { get; set; }
        public List<Exercise> Excercises { get; set; }
    }
}