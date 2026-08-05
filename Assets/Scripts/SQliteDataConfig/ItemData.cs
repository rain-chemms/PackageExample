using SQLite4Unity3d;

[Table("ItemData")]
public class ItemData
{
    /// <summary>
    /// 物品的信息,作为主键不能重复
    /// </summary>
    [PrimaryKey]
    public int ItemID {get;set;}
    /// <summary>
    /// 物品的名字
    /// </summary>
    [Column("ItemName")]
    public string ItemName {get;set;}
    /// <summary>
    /// 物品的描述
    /// </summary>
    [Column("ItemDiscription")]
    public string ItemDiscription {get;set;}
    /// <summary>
    /// 物品最大堆叠数,小于等于0代表堆叠数为1
    /// </summary>
    [Column("MaxStack")]
    public int MaxStack {get;set;}
}