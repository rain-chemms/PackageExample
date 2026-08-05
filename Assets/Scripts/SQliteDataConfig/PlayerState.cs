using SQLite4Unity3d;

[Table("PlayerState")]
public class PlayerState
{
    [PrimaryKey]
    public string PlayerID {get;set;}//玩家的ID
    [Column("PackageSize")]
    public int PackageSize {get;set;}
}