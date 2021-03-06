﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyFitnessTrackerWebAPI.Models.ChartModels
{
    public class ChartExercise
    {
        [Key]
        public Int64 Id { get; set; }
        public String Name { get; set; }
        public double TotalRecordAverage { get; set; }
        public float TotalActivityCount { get; set; }
        public float Target { get; set; }
        public IList<ChartExerciseWeekData> ChartWeeksData { get; set; }
        public IList<ChartExerciseMonthData> ChartMonthsData { get; set; }
        public IList<ChartExerciseRecord> ChartExerciseRecords { get; set; }
    }
}