using UnityEngine;
using System.Collections.Generic;
using SQLite;
using System.Threading.Tasks;

public class PackageDataGetter : MonoBehaviour
{
    public static PackageDataGetter instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    [SerializeField] private int packageSize; 
    public int PackageSize{get => packageSize;set => packageSize = value;}
    [SerializeField] private List<PackageData> packageItems = new List<PackageData>();
    public List<PackageData> PackageItems{get => packageItems;set => packageItems = value;}
    async void OnEnable()
    {
        await DBManager.Instance.Initialize();
        await GetDataFromDB();
    }
    /*
    async void OnDisable()
    {
        await UpdateDataToDB();
    }
    */
    /// <summary>
    /// 将当前的背包数据写入数据库
    /// </summary>
    /// <returns>是一个异步操作</returns>
    private async Task UpdateDataToDB()
    {
        foreach (PackageData item in packageItems)
        {
            await DBManager.Instance.GetConnection().UpdateAsync(item);
        }
    }
    /// <summary>
    /// 获取当前PlayerStateSetter中玩家ID对应的背包数据
    /// </summary>
    /// <returns>是一个异步操作</returns>

    public async Task GetDataFromDB()
    {
        //获取PlayerID
        string playerId = PlayerStateSetter.instance?.PlayerId;
        //读取数据库获取玩家的背包大小
        SQLiteAsyncConnection link = DBManager.Instance.GetConnection();//获取数据库连接
        var state = await link.Table<PlayerState>()
            .Where(x => x.PlayerID.Equals(playerId))
            .FirstOrDefaultAsync();
        //获取当前的背包大小
        if(state == null) Debug.LogError($"[PackageItemDisplayer]: 没有寻找到当前ID玩家: {playerId},");
        else packageSize = state.PackageSize;  
        packageItems = await link.Table<PackageData>()
            .Where(x => x.PlayerID.Equals(playerId) && x.SlotIndex <= packageSize)
            .ToListAsync();//获取所有有效的背包数据
        Debug.Log($"[PackageItemDisplayer]: 玩家ID为: {playerId}, 格子大小为{packageSize}");
    }
}
