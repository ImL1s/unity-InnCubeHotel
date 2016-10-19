/*
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

    #endregion

}
