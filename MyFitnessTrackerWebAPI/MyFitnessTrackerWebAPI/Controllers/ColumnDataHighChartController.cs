using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyFitnessTrackerWebAPI.Models;
using MyFitnessTrackerWebAPI.Models.HighChartModels;
using MyFitnessTrackerWebAPI.Models.ChartModels;
using MyFitnessTrackerWebAPI.Controllers.HighCharts;

namespace MyFitnessTrackerWebAPI.Controllers
{
    [Authorize]
    [Helpers.AuthenticationActionFilterHelper]
    public class ColumnDataHighChartController : ControllerBase
    {
        // GET: api/ColumnDataHighChart
        public ColumnDataHighChart Get(DateTime startDate, DateTime endDate)
        {
            ColumnDataHighChart hsData = null;

            ChartSetController chartSetDataBuilder = new ChartSetController();
            var chartDataContiner = chartSetDataBuilder.Get(startDate, endDate);
            if (chartDataContiner != null)
            {
                hsData = new ColumnDataHighChart();
                hsData.Title = Resources.ChartsTranslations.HighCharts_ColumnChartTitle;
                hsData.SubTitle = Resources.ChartsTranslations.HighCharts_ColumnChartSubTitle;
                hsData.yAxisTitle = Resources.ChartsTranslations.HighCharts_ColumnChartyAxisTitle;
                
                hsData.Series = new List<SeriesDataHighChart>();
                foreach (var chartSet in chartDataContiner.ChartSets)
                {
                    // One series corresponds to one set and data for each month
                    SeriesDataHighChart seriesData = new SeriesDataHighChart();

                    seriesData.ID = chartSet.Id;
                    seriesData.Name = chartSet.Name;
                    var seriesMonthsActivityCountData = (from monthActivityCount in chartSet.ChartSetMonthsData
                         select new object[] { monthActivityCount.ActivityCount as object });
                    seriesData.Data = new List<object>(seriesMonthsActivityCountData.ToArray());

                    hsData.Series.Add(seriesData);
                }

                // Add the xAxis Categories to the High Chart
                hsData.xAxisCategories = new List<String>();
                foreach (var month in chartDataContiner.MonthsInSets)
                    hsData.xAxisCategories.Add(month.Value);
                
            }

            return hsData;
        }

        // GET: api/ColumnDataHighChart/5
        public ColumnDataHighChart Get(long id)
        {
            ColumnDataHighChart hsData = new ColumnDataHighChart();

            return hsData;
        }
    }
}
