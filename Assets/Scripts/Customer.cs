using System;

/// <summary>
/// 性別
/// </summary>
public enum EGender
{
	None,
    Female,
    Male
}

/// <summary>
/// 國家
/// </summary>
public enum ENational
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

/// <summary>
/// 顧客
/// </summary>
public class Customer
{
    public string Name { get; set; }

    public EGender Gender { get; set; }

    public DateTime CheckInTime { get; set; }

    public ENational National { get; set; }

    public string Identity { get; set; }

    public string RoomInfo { get; set; }
}
