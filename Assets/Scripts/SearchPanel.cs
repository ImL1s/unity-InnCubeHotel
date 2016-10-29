using UnityEngine;
using System.Collections;
using UI.Dates;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class SearchPanel : MonoBehaviour
{
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

        submitButton.onClick.AddListener(OnSubmitButtonClick);
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

		DateTime checkInTime = datePicker.SelectedDate.Date;
		EGender gender = toggleF.isOn ? EGender.Female : EGender.Male;
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
	}

	private void CheckInput()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			this.gameObject.SetActive (false);
		}
	}

}
