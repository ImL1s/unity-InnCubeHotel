using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DataItem : MonoBehaviour
{
	[SerializeField] Text nameTxt;
	[SerializeField] Text nationalTxt;
	[SerializeField] Text genderTxt;
	[SerializeField] Text roomNumberTxt;
	[SerializeField] Text passportNumberTxt;
	[SerializeField] Text dateTxt;

	void Awake()
	{
		nameTxt = transform.Find ("NameText").GetComponent<Text> ();
		nationalTxt = transform.Find ("NationalText").GetComponent<Text> ();
		genderTxt = transform.Find ("GenderText").GetComponent<Text> ();
		roomNumberTxt = transform.Find ("RoomNumberText").GetComponent<Text> ();
		passportNumberTxt = transform.Find ("PassportNumberText").GetComponent<Text> ();
		dateTxt = transform.Find ("DateText").GetComponent<Text> ();
	}

	public void SetData(Customer customer)
	{
		this.nameTxt.text = customer.Name;
		this.nationalTxt.text = customer.National.ToString ();
		this.genderTxt.text = customer.Gender.ToString ();
		this.roomNumberTxt.text = customer.RoomInfo;
		this.passportNumberTxt.text = customer.Identity;
		this.dateTxt.text = customer.CheckInTime.ToString ();
	}
	
}
