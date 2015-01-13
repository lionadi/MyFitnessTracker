using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyFitnessTrackerWebAPI.Models;
using MyFitnessTrackerWebAPI.Models.HighChartModels;
using MyFitnessTrackerWebAPI.Models.ChartModels;
using MyFitnessTrackerWebAPI.Controllers;
using MyFitnessTrackerLibrary.Globals;
using System.Globalization;

namespace MyFitnessTrackerWebAPI.Controllers.HighCharts
{
    [Authorize]
    [Helpers.AuthenticationActionFilterHelper]
    //[Authorize(Roles = "Admin")]
    public class ChartSetController : ControllerBase
    {
        public ChartDateRange Get()
        {
            ChartDateRange chartDateRange = new ChartDateRange();


            ExerciseRecordsController exerciseRecordsController = new ExerciseRecordsController();
            chartDateRange.StartDate = exerciseRecordsController.GetExerciseRecords().Min(o => o.Date);
            chartDateRange.EndDate = exerciseRecordsController.GetExerciseRecords().Max(o => o.Date);

            return (chartDateRange);
        }

        // GET: api/ChartSet
        public ChartDataContainer Get(DateTime startDate, DateTime endDate)
        {
            ChartDataContainer chartDataContainer = new ChartDataContainer();
            chartDataContainer.ChartSets = new List<ChartSet>();

            ExerciseRecordsController exerciseRecordsController = new ExerciseRecordsController();
            var exerciseRecordsData = exerciseRecordsController.GetExerciseRecords().Where(er => er.Date >= startDate && er.Date <= endDate);
            var setsData = exerciseRecordsData.DistinctBy(o => o.Exercise.SetId).Select( o => o.Exercise.Set);

            // We do these steps to narrow down the calculations to actual existing data based on submitted dates
            var monthsInSetsData = exerciseRecordsData.DistinctBy(m => m.Date.Month).Select(ms => ms.Date.Month).OrderBy(o => o);
            var weeksInSetsData = exerciseRecordsData.DistinctBy(m => m.Date.WeekOfDate()).Select(ms => ms.Date.WeekOfDate()).OrderBy(o => o);

            chartDataContainer.MonthsInSets = new Dictionary<int, String>();
            foreach (var month in monthsInSetsData)
                chartDataContainer.MonthsInSets.Add(month, new DateTime(DateTime.Now.Year, month, 1).ToString("MMMM", CultureInfo.InvariantCulture));

            chartDataContainer.WeeksInSets = new Dictionary<int, String>();
            foreach (var week in weeksInSetsData)
                chartDataContainer.WeeksInSets.Add(week, week.ToString());

            //#######################################################################
            // Start Building the chart data by going through each found set
            //#######################################################################
            foreach(var set in setsData)
            {
                ChartSet chartSet = ChartsHelper.PopulateChartSet(set, monthsInSetsData, weeksInSetsData, startDate, endDate);
                // Final step is to add the loaded set data that is to be returned to the client
                chartDataContainer.ChartSets.Add(chartSet);
            }
            //#######################################################################

            return chartDataContainer;
        }

        // GET: api/ChartSet/5
        public ChartDataContainer Get(long id, DateTime startDate, DateTime endDate)
        {
            ChartDataContainer chartDataContainer = new ChartDataContainer();
            chartDataContainer.ChartSets = new List<ChartSet>();

            ChartSet chartSet = null;
            ExerciseRecordsController exerciseRecordsController = new ExerciseRecordsController();
            var exerciseRecordsData = exerciseRecordsController.GetExerciseRecords().Where(er => er.Date >= startDate && er.Date <= endDate && er.Exercise.Set.UserId.ToLower().CompareTo(this.user.Id.ToLower()) == 0 && er.Exercise.SetId == id);
            var setsData = exerciseRecordsData.DistinctBy(o => o.Exercise.SetId).Select( o => o.Exercise.Set);

            // We do these steps to narrow down the calculations to actual existing data based on submitted dates
            var monthsInSetsData = exerciseRecordsData.DistinctBy(o => o.Exercise.SetId).DistinctBy(m => m.Date.Month).Select(ms => ms.Date.Month);
            var weeksInSetsData = exerciseRecordsData.DistinctBy(o => o.Exercise.SetId).DistinctBy(m => m.Date.WeekOfDate()).Select(ms => ms.Date.WeekOfDate());
            

            //#######################################################################
            // Start Building the chart data by going through each found set
            //#######################################################################
            foreach(var set in setsData)
            {
                chartSet = ChartsHelper.PopulateChartSet(set, monthsInSetsData, weeksInSetsData, startDate, endDate);
            }

            chartDataContainer.ChartSets.Add(chartSet);
            //#######################################################################

            return (chartDataContainer);
        }
    }
}
