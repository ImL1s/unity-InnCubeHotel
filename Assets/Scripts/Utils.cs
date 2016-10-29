using System;
using UI.Dates;
using UnityEngine;

namespace YSFramework.Utils
{
	public class Utils
	{
		public Utils ()
		{
		}

		public static void SetDataPicker(DatePicker datePicker)
		{

			datePicker.VisibleDate = new SerializableDate () 
			{
				Date = DateTime.Now
			};

			datePicker.Config.Sizing.OverrideTransformHeight = true;
			datePicker.Config.Sizing.PreferredHeight = Screen.height/2 - 10;
		}
	}
}

