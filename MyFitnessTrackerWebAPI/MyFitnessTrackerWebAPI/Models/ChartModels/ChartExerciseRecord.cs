using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyFitnessTrackerWebAPI.Models.ChartModels
{
    public class ChartExerciseRecord
    {
        [Key]
        public Int64 Id { get; set; }
        public float Record { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}