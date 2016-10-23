using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RevealPanel : MonoBehaviour
{
	[SerializeField] private GameObject contentGO;
	[SerializeField] private GameObject dataItemPrefab;



	public void SetData(List<Customer> customers)
	{

		foreach (var customer in customers) 
		{
			GameObject instance = GameObject.Instantiate (dataItemPrefab) as GameObject;
			instance.transform.parent = contentGO.transform;
			instance.transform.localScale = Vector3.one;
			instance.GetComponent<DataItem> ().SetData (customer);
		}
	}
}
