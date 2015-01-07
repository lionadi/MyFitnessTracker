using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace MyFitnessTrackerWebAPI.Models.HighChartModels
{
    public class ColumnDataHighChart
    {
        public ColumnDataHighChart()
        {
            this.Series = new List<SeriesDataHighChart>();
        }

        [Key]
        public int ID { get; set; }
        public String Title { get; set; }
        public String SubTitle { get; set; }
        public IList<String> xAxisCategories { get; set; }
        public String yAxisTitle { get; set; }
        public IList<SeriesDataHighChart> Series { get; set; }

    }
}