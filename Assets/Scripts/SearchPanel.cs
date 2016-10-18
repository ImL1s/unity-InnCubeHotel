using UnityEngine;
using System.Collections;
using UI.Dates;
using UnityEngine.UI;
using System;

public class SearchPanel : MonoBehaviour
{
    /// <summary>
    /// 國家
    /// </summary>
    public enum National
    {
        None = 0,
        China = 1,
        HongKong = 2,
        Japan = 3,
        Korea = 4,
        Thailand = 5,
        Mongolia = 6,
        Vietnam = 7,
        Indonesia = 8,
        Singapore = 9,
        Malaysia = 10,
        England = 11,
        French = 12,
        Germany = 13,
        Poland = 14,
        Denmark = 15,
        Sweden = 16,
        Norway = 17,
        Russia = 18,
        Turkey = 19,
        Spain = 20,
        Portugal = 21,
        Italy = 22,
        Austria = 23,
        America = 24,
        Mexico = 25,
        Canada = 26
    }

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

        submitButton.onClick.AddListener(OnSubmitButtonClick);


    }

    /// <summary>
    /// 初始化國家下拉列表
    /// </summary>
    private void InitNationalityDropdown()
    {
        Array nationalArray = Enum.GetValues(typeof(National));

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
	
	}

}
