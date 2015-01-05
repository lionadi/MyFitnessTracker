using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFitnessTrackerWebAPI.Models.ChartModels
{
    public class ChartExerciseMonthData
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float MonthRecordAverage { get; set; }
        public int ActivityCount { get; set; }
    }
}