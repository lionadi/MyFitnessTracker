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
    // NOTICE: When data is processed and we are using series data model like that for HighCharts there must be for each category a data. For example, each month or week below must have a value zero or empty otherwise in the case for highcharts the highchart might not work as expected
    public static class ChartsHelper
    {
        public static ChartDateRange GetUserSetMaximumDataDateRange()
        {
            ChartDateRange chartDateRange = null;



            return chartDateRange;
        }

        public static ChartSet PopulateChartSet(Set set, IEnumerable<int> monthsInSetsData, IEnumerable<int> weeksInSetsData, DateTime startDate, DateTime endDate)
        {
            ChartSet chartSet = new ChartSet();

            chartSet.Name = set.Name;
            chartSet.Id = set.Id;
            //#######################################################################
            // Start Building the set month statistics (NOTICE: that the calculation is intended to calculate by aggregating all activities by month regardless of the year the month belongs to)
            //#######################################################################
            chartSet.ChartSetMonthsData = new List<ChartSetMonthData>();
            foreach (var month in monthsInSetsData)
            {
                chartSet.ChartSetMonthsData.Add(new ChartSetMonthData
                {
                    ActivityCount = set.Exercises.Where(o => o.ExerciseRecords.Any(m => m.Date.Month == month)).Count(),
                    StartDate = DateTime.Now.StartOfMonth(month),
                    EndDate = DateTime.Now.EndOfMonth(month),
                    MonthNumber = month
                });
            }
            //chartSet.ChartSetMonthsData.OrderBy(o => o.MonthNumber);
            //#######################################################################

            //#######################################################################
            // Start Building the set weeks statistics (NOTICE: that the calculation is intended to calculate by aggregating all activities by week regardless of the year the week belongs to)
            //#######################################################################
            chartSet.ChartSetWeeksData = new List<ChartSetWeekData>();
            foreach (var week in weeksInSetsData)
            {
                //var er = set.Exercises.Select(o => o.ExerciseRecords.Where(m => m.Date.WeekOfDate() == week && m.Date >= startDate && m.Date <= endDate)).ElementAtOrDefault(0);
                var er = set.Exercises.Select(o => o.ExerciseRecords.FirstOrDefault( m => m.Date.WeekOfDate() == week)).ElementAtOrDefault(0);

                chartSet.ChartSetWeeksData.Add(new ChartSetWeekData
                {
                    ActivityCount = set.Exercises.Where(o => o.ExerciseRecords.Any(m => m.Date.WeekOfDate() == week)).Count(),
                    StartDate = DateTimeExtensions.StartOfWeekNumber(er != null ? er.Date.Year : DateTime.Now.Year, week),
                    EndDate = DateTimeExtensions.EndOfWeekNumber(er != null ? er.Date.Year : DateTime.Now.Year, week),
                    WeekNumber = week
                });
            }
            //chartSet.ChartSetWeeksData.OrderBy(o => o.WeekNumber);
            chartSet.TotalActivityCount = chartSet.ChartSetMonthsData.Sum(ac => ac.ActivityCount);
            //#######################################################################

            //#######################################################################
            // Start creating chart data for excercises
            //#######################################################################
            chartSet.ChartSetExercises = new List<ChartExercise>();

            foreach (var exercise in set.Exercises)
            {
                ChartExercise chartExercise = ChartsHelper.PopulateChartExercise(exercise, monthsInSetsData, weeksInSetsData, startDate, endDate);
                // Final step is to add the loaded exercise data into the corresponding set that is to be returned to the client
                chartSet.ChartSetExercises.Add(chartExercise);
            }
            //#######################################################################

            // Final step is to add the loaded set data that is to be returned to the client
            return (chartSet);
        }

        public static ChartExercise PopulateChartExercise(Exercise exercise, IEnumerable<int> monthsInSetsData, IEnumerable<int> weeksInSetsData, DateTime startDate, DateTime endDate)
        {
            ChartExercise chartExercise = new ChartExercise();
            chartExercise.Id = exercise.Id;
            chartExercise.Name = exercise.Name;
            chartExercise.Target = (float)exercise.Target;
            //#######################################################################
            // Start Building the exercise month statistics (NOTICE: that the calculation is intended to calculate by aggregating all activities by month regardless of the year the month belongs to)
            //#######################################################################
            chartExercise.ChartMonthsData = new List<ChartExerciseMonthData>();
            
            foreach (var month in monthsInSetsData)
            {
                chartExercise.ChartMonthsData.Add(new ChartExerciseMonthData
                {
                    ActivityCount = exercise.ExerciseRecords.Where(m => m.Date.Month == month).Count(),
                    StartDate = DateTime.Now.StartOfMonth(month),
                    EndDate = DateTime.Now.EndOfMonth(month),
                    MonthRecordAverage = new Func<double>(() => {
                    double averageRecord = 0;
                    var exerciseRecordByDateRange = exercise.ExerciseRecords.Where(m => m.Date.Month == month);
                    if (exerciseRecordByDateRange.Count() > 0)
                        averageRecord = exerciseRecordByDateRange.Average(a => a.Record);

                    return (averageRecord);
                    })(),
                    MonthNumber = month
                });
            }
            //chartExercise.ChartMonthsData.OrderBy(o => o.MonthNumber);
            //#######################################################################

            //#######################################################################
            // Start Building the exercise weeks statistics (NOTICE: that the calculation is intended to calculate by aggregating all activities by week regardless of the year the week belongs to)
            //#######################################################################
            chartExercise.ChartWeeksData = new List<ChartExerciseWeekData>();
            foreach (var week in weeksInSetsData)
            {
                var er = exercise.ExerciseRecords.FirstOrDefault(m => m.Date.WeekOfDate() == week);

                chartExercise.ChartWeeksData.Add(new ChartExerciseWeekData
                {
                    ActivityCount = exercise.ExerciseRecords.Where(m => m.Date.WeekOfDate() == week).Count(),
                    StartDate = DateTimeExtensions.StartOfWeekNumber(er != null ? er.Date.Year : DateTime.Now.Year, week),
                    EndDate = DateTimeExtensions.EndOfWeekNumber(er != null ? er.Date.Year : DateTime.Now.Year, week),
                    WeekRecordAverage = new Func<double>(() => {
                    double averageRecord = 0;
                    var exerciseRecordByDateRange = exercise.ExerciseRecords.Where(m => m.Date.WeekOfDate() == week);
                    if (exerciseRecordByDateRange.Count() > 0)
                        averageRecord = exerciseRecordByDateRange.Average(a => a.Record);

                    return (averageRecord);
                    })(),
                    WeekNumber = week
                });
            }
            //chartExercise.ChartWeeksData.OrderBy(o => o.WeekNumber);
            chartExercise.TotalActivityCount = chartExercise.ChartMonthsData.Sum(ac => ac.ActivityCount);
            chartExercise.TotalRecordAverage = new Func<double>(() => {
                    double averageRecord = 0;
                    var exerciseRecordByDateRange = exercise.ExerciseRecords.Where(m => m.Date >= startDate && m.Date <= endDate);
                    if (exerciseRecordByDateRange.Count() > 0)
                        averageRecord = exerciseRecordByDateRange.Average(a => a.Record);

                    return (averageRecord);
                    })();
            //#######################################################################
            // Start creating chart data for excercises
            //#######################################################################
            chartExercise.ChartExerciseRecords = new List<ChartExerciseRecord>();
            foreach (var exerciseRecord in exercise.ExerciseRecords.Where(m => m.Date >= startDate && m.Date <= endDate))
            {
                ChartExerciseRecord chartExerciseRecord = ChartsHelper.PopulateChartExerciseRecord(exerciseRecord, monthsInSetsData, weeksInSetsData, startDate, endDate);
                chartExercise.ChartExerciseRecords.Add(chartExerciseRecord);
            }
            //chartExercise.ChartExerciseRecords.OrderBy(o => o.Date);
            //#######################################################################
            return (chartExercise);
        }

        public static ChartExerciseRecord PopulateChartExerciseRecord(ExerciseRecord exerciseRecord, IEnumerable<int> monthsInSetsData, IEnumerable<int> weeksInSetsData, DateTime startDate, DateTime endDate)
        {
            ChartExerciseRecord chartExerciseRecord = new ChartExerciseRecord();

            chartExerciseRecord.Id = exerciseRecord.Id;
            chartExerciseRecord.Record = (float)exerciseRecord.Record;
            chartExerciseRecord.StartDate = exerciseRecord.StartDate;
            chartExerciseRecord.EndDate = exerciseRecord.EndDate;
            chartExerciseRecord.Date = exerciseRecord.Date;

            return (chartExerciseRecord);
        }
    }
}