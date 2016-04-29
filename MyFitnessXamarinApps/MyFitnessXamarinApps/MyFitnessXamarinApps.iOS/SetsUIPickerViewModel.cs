using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using MyFitnessXamarinApps.Core;

namespace MyFitnessXamarinApps.iOS
{
    public class SetsUIPickerViewModel : UIPickerViewModel
    {
        public SetsUIPickerViewModel(UIPickerView excercisesUIPicker)
        {
            this.excercisesUIPicker = excercisesUIPicker;
        }

        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 1;
        }

        private UIPickerView excercisesUIPicker;

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return ServiceOperator.GetInstance().GetUserSets().Count;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            return ServiceOperator.GetInstance().GetUserSets()[(int)row].Name;
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            this.excercisesUIPicker.Model = new ExcercisesUIPickerViewController(row);
        }
    }
}
