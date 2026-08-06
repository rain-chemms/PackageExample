using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Button))]
public class PackageGrid : MonoBehaviour
{
    [SerializeField] private int index;//背包格子的索引
    public int Index {get => index;set => index = value;}
    [SerializeField] private int number;//道具的数量
    public int Number{get => number;set => number = value;}
    [SerializeField] private int itemID;
    public int ItemID{get => itemID;set => itemID = value;}
    [SerializeField] private string itemName;//道具的名字
    public string ItemName{get => itemName;set => itemName = value;}
    [SerializeField] private string itemDiscription;//道具的描述
    public string ItemDiscription{get => itemDiscription;set => itemDiscription = value;}
}
