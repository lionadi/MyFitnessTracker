using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFitnessTrackerWebAPI.Models.ChartModels
{
    public class ChartDataContainer
    {
        public IList<ChartSet> ChartSets { get; set; }
        public System.Collections.Generic.IDictionary<int,String> MonthsInSets { get; set; }
        public System.Collections.Generic.IDictionary<int, String> WeeksInSets { get; set; }
    }
}