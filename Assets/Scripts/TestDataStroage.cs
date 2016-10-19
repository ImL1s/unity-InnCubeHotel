using UnityEngine;
using System.Collections;

public class TestDataStroage : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        StorageManager.AddCoustomer(new Customer()
        {
            Name = "皮卡丘",
            CheckInTime = System.DateTime.Now,
            Gender = EGender.Female,
            Identity = "A127456789",
            National = ENational.Austria,
            RoomInfo = "5F A區"
        });

        var list = StorageManager.GetAllCoustomer();

        foreach (var item in list)
        {
            Debug.Log(item.Name);
            Debug.Log(item.CheckInTime);
        }
        
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
