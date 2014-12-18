using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFitnessTracker.Models
{
    public class Exercise
    {
        public int ExerciseId { get; set; }
        public List<int> Targets { get; set; }
        public String Name { get; set; }
    }
}