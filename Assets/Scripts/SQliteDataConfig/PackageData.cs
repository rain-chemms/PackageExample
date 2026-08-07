using SQLite4Unity3d;

[Table("PackageData")]
public class PackageData
{
    /// <summary>
    /// 背包的格子索引,作为主键不能重复
    /// </summary>
    [PrimaryKey,AutoIncrement]
    public int ID {get;set;}

    [Column("PlayerID"),Indexed]
    public string PlayerID {get;set;}
    [Column("SlotIndex"),Indexed]
    public int SlotIndex {get;set;}
    /// <summary>
    /// 存储的物品ID,为-1的时候代表当前格子为空
    /// </summary>
    [Column("ItemID")]
    public int ItemID {get;set;}
    [Column("ItemCount")]
    public int ItemCount {get;set;}
}
