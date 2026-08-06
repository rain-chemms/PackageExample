using UnityEngine;
using UnityEngine.UI;
using SQLite;
using SQLite4Unity3d;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

public class PackageItemDisplayer : MonoBehaviour
{
    [SerializeField] private PackageGrid gridPrefab;//背包格子预制体
    public PackageGrid GridPrefab{ get=>gridPrefab; }
    [SerializeField] private ScrollRect itemView;
    public ScrollRect ItemView{ get=>itemView; }
    async void Start()
    {
        //读取当前的玩家存档信息
        await FreshDisplay();
    }
    async void OnEnable()
    {
        //读取当前的玩家存档信息
        await FreshDisplay();
    }
    //依据数据刷新背包显示
    public async Task FreshDisplay()
    {
        await DBManager.Instance.Initialize();
        await PackageDataGetter.instance.GetDataFromDB();
        SQLiteAsyncConnection link = DBManager.Instance.GetConnection();
        int packageSize = (int)PackageDataGetter.instance?.PackageSize;
        List<PackageData> packageItems = PackageDataGetter.instance?.PackageItems;
        //清除当前所有的UI子物体
        foreach (Transform child in itemView?.content.transform)
        {
            Destroy(child.gameObject);
        }
        //创建新的UI子物体
        for(int i = 1; i <= packageSize ;i++)
        {
            PackageGrid grid = Instantiate(gridPrefab, transform);
            Button button = grid.GetComponent<Button>();//禁用按钮
            if(button!=null) button.interactable = false;
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
                        if(button!=null) button.interactable = true;//启用按钮
                        grid.GetComponent<PackageGridSpriteSetter>().FreshDisplay();
                    }
                    break;
                }
            }
            grid.transform.SetParent(itemView?.content.transform,false);//设置父物体
        }
    }
}
