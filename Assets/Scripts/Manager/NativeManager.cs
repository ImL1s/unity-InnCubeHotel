using UnityEngine;
using System.Collections;
using System;

public class NativeManager : MonoBehaviour
{
	/// <summary>
	/// 顯示Android本地的Toast
	/// </summary>
	/// <param name="str">String.</param>
	/// <param name="isLong">If set to <c>true</c> is long.</param>
	public static void ShowToast (string str, bool isLong = true)
	{
		if (Application.platform == RuntimePlatform.Android) 
		{ 
			AndroidJavaClass ajc2 = new AndroidJavaClass ("com.iml1s.androidplugins.utils.UToast");
			ajc2.CallStatic ("showToastA", str, true);
			
		}
		else if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			// TODO IPhonePlayer
		}
		else
		{
			Debug.Log (string.Format ("Toast Log Length-{0} :{1}", isLong ? "Long" : "short", str));
		}
	}


}
