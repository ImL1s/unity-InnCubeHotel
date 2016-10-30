using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RevealPanel : MonoBehaviour
{
	private List<GameObject> instanceList = new List<GameObject>();

	[SerializeField] private GameObject contentGO;
	[SerializeField] private GameObject dataItemPrefab;



	public void SetData(List<Customer> customers)
	{
		if(instanceList.Count != 0)
		{
			foreach (var item in instanceList) 
			{
				GameObject.Destroy (item);	
			}	
		}

		foreach (var customer in customers) 
		{
			GameObject instance = GameObject.Instantiate (dataItemPrefab) as GameObject;
			instance.transform.parent = contentGO.transform;
			instance.transform.localScale = Vector3.one;
			instance.GetComponent<DataItem> ().SetData (customer);

			instanceList.Add (instance);
		}
	}

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
