using System;
using System.Collections.Generic;
using System.Text;

namespace MyFitnessXamarinApps.iOS
{
    using UIKit;
    using MyFitnessXamarinApps.Core;

    public class ExcercisesUIPickerViewController : UIPickerViewModel
    {
        public nint SetID { get; set; }
        public ExcercisesUIPickerViewController(nint setID)
        {
            this.SetID = setID;
        }
        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 1;
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return ServiceOperator.GetInstance().GetUserSets()[(int)this.SetID].Exercises.Count;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            return ServiceOperator.GetInstance().GetUserSets()[(int)this.SetID].Exercises[(int)row].Name;
        }

    }
}
