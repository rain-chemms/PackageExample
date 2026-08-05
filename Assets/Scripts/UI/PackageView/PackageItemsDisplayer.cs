using UnityEngine;
using UnityEngine.UI;
using SQLite;
using SQLite4Unity3d;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(GridLayoutGroup))]
public class PackageItemDisplayer : MonoBehaviour
{
    [SerializeField] private PackageGrid gridPrefab;//背包格子预制体
    async void OnEnable()
    {
        //读取当前的玩家存档信息
        await GetDataByPlayerState();
        await FreshDisplay();
    }

    [NonSerialized] private int packageSize; 
    [NonSerialized] private List<PackageData> packageItems = new List<PackageData>();
    /// <summary>
    /// 获取当前PlayerStateSetter中玩家ID对应的背包数据
    /// </summary>
    /// <returns>是一个异步操作</returns>
    private async Task GetDataByPlayerState()
    {
        //获取PlayerID
        string playerId = PlayerStateSetter.instance?.PlayerId;
        //读取数据库获取玩家的背包大小
        SQLiteAsyncConnection link = DatabaseManager.Instance.GetConnection();//获取数据库连接
        var state = await link.Table<PlayerState>()
            .Where(x => x.PlayerID.Equals(playerId))
            .FirstOrDefaultAsync();
        //获取当前的背包大小
        Debug.Log($"[PackageItemDisplayer]: 玩家ID为: {playerId}");
        if(state == null) Debug.LogError($"[PackageItemDisplayer]: 玩家ID为空: {playerId}");
        else packageSize = state.PackageSize;  
        packageItems = await link.Table<PackageData>()
            .Where(x => x.PlayerID.Equals(playerId) && x.SlotIndex <= packageSize)
            .ToListAsync();//获取所有有效的背包数据
    }

    //依据数据刷新背包显示
    public async Task FreshDisplay()
    {
        SQLiteAsyncConnection link = DatabaseManager.Instance.GetConnection();
        //清除当前所有的UI子物体
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        //创建新的UI子物体
        for(int i = 1; i <= packageSize ;i++)
        {
            PackageGrid grid = Instantiate(gridPrefab, transform);
            grid.GetComponent<Button>().enabled = false;//禁用按钮
            grid.Index = i;//设置索引
            //尝试获取数据
            foreach(PackageData item in packageItems)
            {
                if(item == null) continue;
                grid.ItemID = item.ItemID;
                if(item.SlotIndex == i)
                {
                    //设置UI控制的数据
                    ItemData idat = await link.Table<ItemData>().Where(x => x.ItemID == item.ItemID).FirstOrDefaultAsync();
                    grid.Number = item.ItemCount;     
                    if(idat!=null)
                    {
                        grid.ItemName = idat.ItemName;                   
                        grid.ItemDiscription = idat.ItemDiscription;
                        grid.GetComponent<Button>().enabled = true;//启用按钮
                        grid.GetComponent<PackageGridViewSetter>().FreshDisplay();
                    }
                    break;
                }
            }
            grid.transform.SetParent(transform,false);//设置父物体
        }
    }
}
