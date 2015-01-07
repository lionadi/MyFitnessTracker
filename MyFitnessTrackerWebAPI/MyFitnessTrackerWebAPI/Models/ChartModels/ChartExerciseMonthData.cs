using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyFitnessTrackerWebAPI.Models.ChartModels
{
    public class ChartExerciseMonthData
    {
        [Key]
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float MonthRecordAverage { get; set; }
        public int ActivityCount { get; set; }
    }
}