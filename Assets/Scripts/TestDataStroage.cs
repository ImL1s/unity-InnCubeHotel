using UnityEngine;
using System.Collections;
using System;

public class TestDataStroage : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		TestAdd ();
//		TestGet ();
//		TestDelete ();

        
	}

	private void TestAdd ()
	{
		Customer customer = new Customer () {
			Name = "皮卡丘",
			CheckInTime = System.DateTime.Now,
			Gender = EGender.Female,
			Identity = "A127456789",
			National = ENational.Austria,
			RoomInfo = "5F A區"
		};

		StorageManager.AddCoustomer (customer);
		Debug.Log ("Add a customer: " +customer.Name);
	}

	private void TestGet ()
	{
		var list = StorageManager.GetAllCoustomer ();
		
		foreach (var item in list) {
			Debug.Log (item.Name);
			Debug.Log (item.CheckInTime);
		}
	}

	private void TestDelete()
	{
		bool succ = StorageManager.DeleteCoustomer (DateTime.Now.Date);
		Debug.Log ("刪除結果: " + (succ ? "成功" : "失敗"));
	}


	// Update is called once per frame
	void Update ()
	{
	
	}
}
