using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFitnessTrackerWebAPI.Models.ChartModels
{
    public class ChartSetMonthData
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ActivityCount { get; set; }
    }
}