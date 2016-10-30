using UnityEngine;
using System.Collections;
using System;

public class TestAndroidPlugins : MonoBehaviour {

	private string JarClassName = "com.Unity.Test";
	private int Ord = 10;
	private int Add = 5;
	private int result = 0;
	private int Mutiplie = 10;
	private int Sub = 15;
	private AndroidJavaObject mCal = null;
	private object[] arglist;
	private string ec = "";

	// Use this for initialization
	void Start ()
	{
		//AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		//AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		//string str = jo.CallStatic<string>("Test","ssttrr");
		//Debug.Log(str);

		// 產生AndroidJavaObject物件


		if (Application.platform == RuntimePlatform.Android)
		{
			this.mCal = new AndroidJavaObject(JarClassName + ".UnityTest");
			//建立傳入參數陣列  
			arglist = new object[2];
			arglist[0] = (object)this.Ord;
			arglist[1] = (object)this.Add;
			//呼叫方法，並取得回傳值  
			this.result = this.mCal.Call<int>("add", arglist);

			//輸出測試結果顯示在 Log上
			Debug.Log("Add :" + this.Ord.ToString());

			StartCoroutine(func());
		}
	}

	IEnumerator func()
	{
		yield return new WaitForSeconds(2);

		try
		{
			//AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			//AndroidJavaObject unityActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

			AndroidJavaClass ajc1 = new AndroidJavaClass(JarClassName + ".UnityTest");

			ajc1.CallStatic("showToast","HelloWorld1" ,true);

			//mCal.Call("max", 1, 2);
			//mCal.Call("makeToast", unityActivity, 0);

		}
		catch(Exception e)
		{
			this.ec = e.ToString();
		}

		yield return new WaitForSeconds (3);

		try
		{
			AndroidJavaClass ajc2 = new AndroidJavaClass("com.iml1s.androidplugins.utils.UToast");
			ajc2.CallStatic("showToastA","HelloWorld2" ,true);
		}
		catch(Exception e)
		{
			this.ec = e.ToString();
		}
	}

	// Update is called once per frame
	void Update ()
	{
		//#if  !UNITY_EDITOR && UNITY_ANDROID
		//        result++;
		//#endif
	}

	void OnGUI()
	{
		GUI.Label(new Rect(30, 30, 300, 100), result.ToString(), new GUIStyle() { fontSize = 60 });
		GUI.Label(new Rect(30, 60, 300, 100), ec.ToString(), new GUIStyle() { fontSize = 20 });
	}
}
