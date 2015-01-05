using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFitnessTrackerWebAPI.Models.ChartModels
{
    public class ChartSet
    {
        public Int64 Id { get; set; }
        public String Name { get; set; }
        public IList<ChartSetWeekData> ChartSetWeeksData { get; set; }
        public IList<ChartSetMonthData> ChartSetMonthsData { get; set; }
        public IList<ChartExercise> ChartSetExercises { get; set; }
    }
}