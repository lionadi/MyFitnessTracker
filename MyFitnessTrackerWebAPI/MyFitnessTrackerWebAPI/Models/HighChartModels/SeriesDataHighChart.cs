using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace MyFitnessTrackerWebAPI.Models.HighChartModels
{
    public class SeriesDataHighChart
    {
        [Key]
        public int ID { get; set; }
        public String Name { get; set; }
        public IList<object> Data { get; set; }
    }
}