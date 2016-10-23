﻿/*
 *  Author:ImL1s
 *  
 *  Date:2016/10/20 12:07
 *  
 *  Desc:儲存資料管理器
 * 
 */

using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

public class StorageManager
{

    #region field

    /// <summary>
    /// 顧客資料儲存時使用的Key值
    /// </summary>
    public const string CustomerKey = "customer";

    private static StorageManager instance = null;

    #endregion

    #region property

    public static StorageManager Instance
    {
        get
        {
            if (instance == null) instance = new StorageManager();
            return instance;
        }
    }

    #endregion


    #region static method 

    /// <summary>
    /// 加入一筆顧客資料
    /// </summary>
    /// <param name="customer"></param>
    public static void AddCoustomer(Customer customer)
    {
        Instance.InnerAddCoustomer(customer);
    }

    /// <summary>
    /// 取得所有顧客資料
    /// </summary>
    /// <returns></returns>
    public static List<Customer> GetAllCoustomer()
    {
        return Instance.InnerGetAllCoustomer();
    }

	/// <summary>
	/// 取得指定的客戶資料
	/// </summary>
	/// <returns>The assign.</returns>
	public static List<Customer> GetAssign(Customer customer)
	{
		return Instance.InnerGetAssign (customer);
	}

	/// <summary>
	/// 刪除客戶資料
	/// </summary>
	/// <returns><c>true</c>, if coustomer was deleted, <c>false</c> otherwise.</returns>
	/// <param name="dateTime">Date time.</param>
	public static bool DeleteCoustomer(DateTime dateTime)
	{
		return Instance.InnerDeleteCoustomer (dateTime);
	}

    #endregion


    #region private method

    /// <summary>
    /// 單例模式，禁止亂New
    /// </summary>
    private StorageManager() { }


    /// <summary>
    /// 儲存String資料
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    private void SaveStringData(string key,string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    /// <summary>
    /// 取得String資料，默認為Null
    /// </summary>
    /// <param name="key"></param>
    /// <returns>默認為Null</returns>
    private string GetStringData(string key,string defalutValue = null)
    {
        return PlayerPrefs.GetString(CustomerKey, defalutValue);
    }


    /// <summary>
    /// 加入一筆顧客資料
    /// </summary>
    /// <param name="customer"></param>
    private void InnerAddCoustomer(Customer customer)
    {
        string jsonCoustomers = GetStringData(CustomerKey);
        List<Customer> coustomerList;

        // 第一次加入資料的操作
        if (string.IsNullOrEmpty(jsonCoustomers))
        {
            coustomerList = new List<Customer>();
            coustomerList.Add(customer);
        }
        // 已經有資料存在的操作
        else
        {
            coustomerList = JsonConvert.DeserializeObject<List<Customer>>(jsonCoustomers);
            coustomerList.Add(customer);
        }

        string json = JsonConvert.SerializeObject(coustomerList);
        SaveStringData(CustomerKey, json);
    }

    /// <summary>
    /// 取得所有顧客資料
    /// </summary>
    /// <returns></returns>
    private List<Customer> InnerGetAllCoustomer()
    {
        string currentCoustomerJson = GetStringData(CustomerKey);

        return JsonConvert.DeserializeObject<List<Customer>>(currentCoustomerJson);
    }

	/// <summary>
	/// 刪除客戶資料
	/// </summary>
	/// <returns><c>true</c>, if delete coustomer was innered, <c>false</c> otherwise.</returns>
	/// <param name="dateTime">Date time.</param>
	private bool InnerDeleteCoustomer (DateTime dateTime)
	{
		List<Customer> customerList = GetCustomers ();

		List<Customer> saveCustomerList = customerList.FindAll (c => !(c.CheckInTime.Date == dateTime.Date) );

		if (saveCustomerList.Count == customerList.Count)
			return false;

		string json = JsonConvert.SerializeObject(saveCustomerList);
		SaveStringData(CustomerKey, json);

		return true;
	}


	/// <summary>
	/// 取得之前存在的顧客資料
	/// </summary>
	/// <returns>The customers.</returns>
	private List<Customer> GetCustomers()
	{
		string jsonCoustomers = GetStringData(CustomerKey);

		List<Customer> coustomerList;

		// 第一次加入資料的操作
		if (string.IsNullOrEmpty(jsonCoustomers))
		{
			coustomerList = new List<Customer>();
		}
		else
		{
			coustomerList = JsonConvert.DeserializeObject<List<Customer>>(jsonCoustomers);
		}

		return coustomerList;
	}

	/// <summary>
	/// 取得指定的客戶資料
	/// </summary>
	/// <returns>The assign.</returns>
	private List<Customer> InnerGetAssign(Customer assignCustomer)
	{
		List<Customer> allCustomerList = InnerGetAllCoustomer ();
		List<Customer> assignCustomerList = allCustomerList.FindAll
			(c => (c.CheckInTime.Date == assignCustomer.CheckInTime.Date || c.Identity == assignCustomer.Identity || c.Name == assignCustomer.Name) );

		return assignCustomerList;
	}

    #endregion

}
