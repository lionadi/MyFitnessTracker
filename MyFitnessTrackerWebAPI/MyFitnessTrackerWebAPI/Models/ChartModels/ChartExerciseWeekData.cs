using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFitnessTrackerWebAPI.Models.ChartModels
{
    public class ChartExerciseWeekData
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float WeekRecordAverage { get; set; }
        public int ActivityCount { get; set; }
    }
}