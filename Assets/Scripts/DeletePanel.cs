using UnityEngine;
using System.Collections;
using UI.Dates;
using UnityEngine.UI;
using System;

public class DeletePanel : MonoBehaviour
{

	[SerializeField] private DatePicker picker;
	[SerializeField] private Button submitBtn;

	// Use this for initialization
	void Start ()
	{
		YSFramework.Utils.Utils.SetDataPicker (picker);

		submitBtn.onClick.AddListener (() => 
		{
				bool succ = StorageManager.DeleteCoustomer(picker.SelectedDate.Date);

				print("Delete :"+succ);
		});
	}
	
	// Update is called once per frame
	void Update ()
	{
		CheckInput ();
	}

	private void CheckInput ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			this.gameObject.SetActive (false);
		}
	}
}
