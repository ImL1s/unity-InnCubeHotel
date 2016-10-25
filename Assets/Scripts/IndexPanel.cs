using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IndexPanel : MonoBehaviour
{
	[SerializeField] private GameObject searchPanel;
	[SerializeField] private GameObject addPanel;
	[SerializeField] private GameObject deletePanel;

	[SerializeField] private Button searchBtn;
	[SerializeField] private Button addBtn;
	[SerializeField] private Button deleteBtn;

	// Use this for initialization
	void Start ()
	{
		searchBtn.onClick.AddListener (() => 
		{
				searchPanel.SetActive(true);
		});

		addBtn.onClick.AddListener (() => 
		{
				addPanel.SetActive(true);		
		});

		deleteBtn.onClick.AddListener (() => 
		{
				deletePanel.SetActive(true);		
		});
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
