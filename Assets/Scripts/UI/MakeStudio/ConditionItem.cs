using UnityEngine;

public class ConditionItem : MonoBehaviour
{
    [SerializeField] private int requireItemID;//用于获取物品ID
    
    public int RequireItemID { get => requireItemID; set => requireItemID = value; }
    /*
    [SerializeField] private string requireItemName;//用于获取物品名称
    public string RequireItemName { get => requireItemName; set => requireItemName = value; }
    [SerializeField] private int nowHaveItemNumber;//已经获取的物品数量
    public int NowHaveItemNumber { get => nowHaveItemNumber; set => nowHaveItemNumber = value; }
    */
    [SerializeField] private int requireItemNumber;//用于当前所需的物品数量
    public int RequireItemNumber { get => requireItemNumber; set => requireItemNumber = value; }
    
}
