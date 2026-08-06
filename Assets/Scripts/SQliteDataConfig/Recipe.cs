using SQLite4Unity3d;

//合成表
[Table("Recipe")]
public class Recipe
{
    [PrimaryKey]
    public int RecipeID{get;set;}//配方的ID
    [Column("GenerateID")]
    public int GenerateID{get;set;}//生成物品的ID
    [Column("GenerateNumber")]
    public int GenerateNumber{get;set;}//产出数量
    [Column("RequireList")]
    public string RequireList{get;set;}//所需材料列表[ItemID1:NeedNumber|ItemID2:NeedNUmber]
}