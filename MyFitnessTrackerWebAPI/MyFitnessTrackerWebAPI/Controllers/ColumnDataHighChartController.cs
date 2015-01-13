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
        public ColumnDataHighChart Get(long setId, DateTime startDate, DateTime endDate)
        {
            ColumnDataHighChart hsData = null;

            ChartSetController chartSetDataBuilder = new ChartSetController();
            var chartDataContiner = chartSetDataBuilder.Get(startDate, endDate);
            if (chartDataContiner != null)
            {
                hsData = new ColumnDataHighChart();
                hsData.Title = Resources.ChartsTranslations.HighCharts_ColumnChartTitle;
                hsData.SubTitle = Resources.ChartsTranslations.HighCharts_ColumnCharty_SetTitle;
                hsData.yAxisTitle = Resources.ChartsTranslations.HighCharts_ColumnChartyAxisTitle;

                hsData.Series = new List<SeriesDataHighChart>();
                
                var chartSet = chartDataContiner.ChartSets.SingleOrDefault( o => o.Id == setId);

                if(chartSet != null)
                {
                    foreach (var chartSetExercise in chartSet.ChartSetExercises)
                    {
                        // One series corresponds to one set and data for each month
                        SeriesDataHighChart seriesData = new SeriesDataHighChart();

                        seriesData.ID = chartSetExercise.Id;
                        seriesData.Name = chartSetExercise.Name;
                        var seriesMonthsActivityCountData = (from monthActivityCount in chartSetExercise.ChartMonthsData
                                                             select new object[] { monthActivityCount.ActivityCount as object });
                        seriesData.Data = new List<object>(seriesMonthsActivityCountData.ToArray());

                        hsData.Series.Add(seriesData);
                    }
                }

                // Add the xAxis Categories to the High Chart
                hsData.xAxisCategories = new List<String>();
                foreach (var month in chartDataContiner.MonthsInSets)
                    hsData.xAxisCategories.Add(month.Value);
            }

            return hsData;
        }

        public ColumnDataHighChart Get(long setId, long exerciseId, DateTime startDate, DateTime endDate)
        {
            // Make the chart to take into consideration if t here is 1.  one week, 2. more than one week, 3. more than a month
            ColumnDataHighChart hsData = null;

            ChartSetController chartSetDataBuilder = new ChartSetController();
            var chartDataContiner = chartSetDataBuilder.Get(startDate, endDate);
            if (chartDataContiner != null)
            {
                hsData = new ColumnDataHighChart();
                hsData.Title = Resources.ChartsTranslations.HighCharts_ColumnChartTitle;
                hsData.SubTitle = Resources.ChartsTranslations.HighCharts_ColumnCharty_ExerciseTitle;
                hsData.yAxisTitle = Resources.ChartsTranslations.HighCharts_ColumnChartyAxisTitle;

                hsData.Series = new List<SeriesDataHighChart>();

                var chartSet = chartDataContiner.ChartSets.SingleOrDefault(o => o.Id == setId);
                
                if (chartSet != null)
                {
                    var chartSetExercise = chartSet.ChartSetExercises.SingleOrDefault(o => o.Id == exerciseId);

                    if (chartSetExercise != null)
                    {
                        hsData.xAxisCategories = new List<String>();
                        if (chartDataContiner.MonthsInSets.Count() > 1)
                        {
                            SeriesDataHighChart seriesData = new SeriesDataHighChart();
                            var seriesMonthsActivityCountData = (from monthActivityCount in chartSetExercise.ChartMonthsData
                                                                 select new object[] { monthActivityCount.MonthRecordAverage as object });
                            seriesData.Data = new List<object>(seriesMonthsActivityCountData.ToArray());
                            hsData.Series.Add(seriesData);

                            // Add the xAxis Categories to the High Chart
                            
                            foreach (var month in chartDataContiner.MonthsInSets)
                                hsData.xAxisCategories.Add(month.Value);
                        }
                        else if (chartDataContiner.MonthsInSets.Count() <= 1 && chartDataContiner.WeeksInSets.Count() > 1)
                        {
                            SeriesDataHighChart seriesData = new SeriesDataHighChart();
                            var seriesMonthsActivityCountData = (from monthActivityCount in chartSetExercise.ChartWeeksData
                                                                 select new object[] { monthActivityCount.WeekRecordAverage as object });
                            seriesData.Data = new List<object>(seriesMonthsActivityCountData.ToArray());
                            hsData.Series.Add(seriesData);

                            foreach (var week in chartDataContiner.WeeksInSets)
                                hsData.xAxisCategories.Add(Resources.ChartsTranslations.HighCharts_ColumnChartExerciseWeekTitle + week.Value);
                        }
                        else if (chartDataContiner.WeeksInSets.Count() == 1)
                        {
                            foreach (var chartSetExerciseRecord in chartSetExercise.ChartExerciseRecords)
                            {
                                // One series corresponds to one set and data for each month
                                SeriesDataHighChart seriesData = new SeriesDataHighChart();

                                seriesData.ID = chartSetExerciseRecord.Id;
                                seriesData.Name = chartSetExerciseRecord.Date.ToShortDateString();
                                seriesData.Data = new List<object>();
                                seriesData.Data.Add(chartSetExerciseRecord.Record);
                               
                                hsData.Series.Add(seriesData);
                            }
                            hsData.xAxisCategories.Add(Resources.ChartsTranslations.HighCharts_ColumnChartExerciseWeekTitle + chartDataContiner.WeeksInSets.First().Value);
                        }
                        
                    }
                }

                
            }

            return hsData;
        }
    }
}
