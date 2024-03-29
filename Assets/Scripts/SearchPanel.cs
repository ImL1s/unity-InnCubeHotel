﻿using UnityEngine;
using System.Collections;
using UI.Dates;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class SearchPanel : MonoBehaviour
{
	private bool isSelectedDate = false;

	[SerializeField] private GameObject revealPanelGO;
	[SerializeField] private RevealPanel revealPanel;

    [SerializeField] private DatePicker datePicker;
    [SerializeField] private Toggle toggleM;
    [SerializeField] private Toggle toggleF;
    [SerializeField] private InputField nameInputField;
    [SerializeField] private Dropdown nationalityDropdown;
    [SerializeField] private InputField passportInputField;
    [SerializeField] private Button submitButton;


    void Awake()
	{
        datePicker = transform.Find("DatePicker - Popup").GetComponent<DatePicker>();
    }
		
	

	void Start ()
    {
        InitNationalityDropdown();

		YSFramework.Utils.Utils.SetDataPicker (datePicker);
		datePicker.Config.Events.OnDaySelected.AddListener (time => {
			isSelectedDate = true;	
		});

        submitButton.onClick.AddListener(OnSubmitButtonClick);
    }

	void OnEnable()
	{
		SerializableDate date = new SerializableDate();
		datePicker.SelectedDate = date;
		datePicker.UpdateDisplay ();
		toggleM.isOn = false;
		toggleF.isOn = false;
		nameInputField.text = "";
		nationalityDropdown.value = 0;
		passportInputField.text = "";

	}

    /// <summary>
    /// 初始化國家下拉列表
    /// </summary>
    private void InitNationalityDropdown()
    {
        Array nationalArray = Enum.GetValues(typeof(ENational));

        foreach (var national in nationalArray)
        {
            nationalityDropdown.options.Add(new Dropdown.OptionData(national.ToString()));
        }
    }

    private void OnSubmitButtonClick()
    {
        if (!Check())
        {
            print("Check faild");
            return;
        }

        print("Succ");

		DateTime checkInTime = isSelectedDate ? datePicker.SelectedDate.Date : default(DateTime);
		EGender gender = GetGender ();
		String name = nameInputField.text.Trim();
		ENational national = (ENational)nationalityDropdown.value;
		String identity = passportInputField.text.Trim();

		Debug.Log (string.Format ("CheckInTime: {0} Gender: {1} Name: {2} National: {3} Identity: {4}", 
			checkInTime.ToString (),
			gender.ToString (),
			name,
			national.ToString (),
			identity));

		Customer searchCustomer = new Customer () 
		{
			Name = name,
			Gender = gender,
			CheckInTime = checkInTime,
			National = national,
			Identity = identity
		};

		List<Customer> assignList = StorageManager.GetAssign (searchCustomer);
		revealPanelGO.gameObject.SetActive (true);
		revealPanel.SetData (assignList);
    }

	private EGender GetGender()
	{
		if (!toggleF.isOn && !toggleM.isOn)
			return EGender.None;

		return toggleF.isOn ? EGender.Female : EGender.Male;
	}


    /// <summary>
    /// 檢查欄位是否正確
    /// </summary>
    /// <returns></returns>
    private bool Check()
    {
        //if (!datePicker.SelectedDate.HasValue) return false;
        //if (!toggleF.isOn && !toggleM.isOn) return false;
        //if (string.IsNullOrEmpty(nameInputField.text.Trim())) return false;
        //if (nationalityDropdown.value == (int)National.None) return false;
        //if (string.IsNullOrEmpty(passportInputField.text.Trim())) return false;

        return true;
    }



    void Update ()
    {
		CheckInput ();
//		DateTime a;
//		DateTime b = default(DateTime);
//		print (a = datePicker.SelectedDate.Date);
	}

	private void CheckInput()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			datePicker.Hide ();
			this.gameObject.SetActive (false);
		}
	}

}
