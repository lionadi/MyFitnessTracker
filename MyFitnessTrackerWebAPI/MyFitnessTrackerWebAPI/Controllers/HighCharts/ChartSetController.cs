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

namespace MyFitnessTrackerWebAPI.Controllers.HighCharts
{
    [Authorize]
    [Helpers.AuthenticationActionFilterHelper]
    [Authorize(Roles = "Admin")]
    public class ChartSetController : ControllerBase
    {

        // GET: api/ChartSet
        public IEnumerable<ChartSet> Get(DateTime startDate, DateTime endDate)
        {
            List<ChartSet> chartSetsData = new List<ChartSet>();

            ExerciseRecordsController exerciseRecordsController = new ExerciseRecordsController();
            var exerciseRecordsData = exerciseRecordsController.GetExerciseRecords().Where(er => er.Date > startDate && er.Date < endDate && er.Exercise.Set.UserId.ToLower().CompareTo(this.user.Id.ToLower()) == 0);
            var setsData = exerciseRecordsData.DistinctBy(o => o.Exercise.SetId).Select( o => o.Exercise.Set);
            


            return chartSetsData;
        }

        // GET: api/ChartSet/5
        public string Get(int id)
        {
            return "value";
        }
    }
}
