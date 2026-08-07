using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using UnityEngine.InputSystem;

//制作功能器
//内部包含所有制作所需的数据
public class MakeFunctioner : MonoBehaviour
{
    public static MakeFunctioner instance;
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
    [SerializeField] private int generateNumber;//单次制作产生的数量
    public int GenerateNumber {get => generateNumber;set => generateNumber = value;} 
    [SerializeField] private int makeNumber = 1;// 制作数量
    public int MakeNumber {get => makeNumber; set => makeNumber = value; }
    [SerializeField] private int generateID = 0;// 生成的物品ID
    public int GenerateID {get => generateID; set => generateID = value; }
    [SerializeField] private string recipe = "";//当前合成的配方
    public string Recipe {get => recipe; set => recipe = value; }
    [SerializeField] Dictionary<int,int> requireList = new Dictionary<int, int>();
    public Dictionary<int,int> RequireList {get => requireList; set => requireList = value; }
    
    public void FreshRequireList()
    {
        requireList.Clear();
        List<string> rec = recipe.Split('|').ToList();   
        foreach(string r in rec)
        {
            List<string> rc = r.Split(':').ToList();
            if(rc.Count >= 2)
            {
                int id = 0;
                int number = 0;
                if(int.TryParse(rc[0],out id) && int.TryParse(rc[1],out number))
                {
                    if(requireList.ContainsKey(id)) requireList[id] += number;
                    else requireList.Add(id,number);
                }
            }
        }
    }

    async public void MakeNewItem()
    {
        //获取配方
        //requireList = new Dictionary<int, int>();
        //检擦材料是否充足
        Dictionary<int,int> haveList = new Dictionary<int, int>();
        foreach(var item in PackageDataGetter.instance.PackageItems)
        {
            if(item != null)
            {
                if(haveList.ContainsKey(item.ItemID)) haveList[item.ItemID] += item.ItemCount;
                else haveList.Add(item.ItemID,item.ItemCount);
            }
        }
        foreach(KeyValuePair<int,int> kv in requireList)
        {
            int key = kv.Key;
            int value = kv.Value;
            if(haveList.TryGetValue(key,out int have))
            {
                if(value * makeNumber > have) 
                {
                    Debug.LogWarning("[MakeFunctioner]: 缺少材料");
                    return ;
                }//材材料不足
            }            
            else {Debug.LogWarning("[MakeFunctioner]: 缺少材料"); return ;}//没有当前的材料
        }
        
        //计算所需的背包空间,不足则返回
        await DBManager.Instance.Initialize();
        SQLiteAsyncConnection link = DBManager.Instance.GetConnection();
        //计算制作后消耗了多少格背包空间
        int consume = 0;
        foreach(var item in requireList)
        {
            int key = item.Key;
            int need = item.Value * makeNumber;
            var st = await link.Table<ItemData>().Where(x => x.ItemID == key).FirstOrDefaultAsync();
            int stack = (int)st?.MaxStack;
            consume += need / stack;
        }
        //获取当前物品的最大堆叠数
        var state = await link.Table<ItemData>().Where(x => x.ItemID == generateID).FirstOrDefaultAsync();
        int maxStack = (int)state?.MaxStack;
        if(maxStack <= 0) maxStack = 1;
        int emptySpace = PackageDataGetter.instance.PackageSize;
        foreach(var item in PackageDataGetter.instance.PackageItems)
        {
            if(item.SlotIndex < PackageDataGetter.instance.PackageSize && item.SlotIndex > 0)
            {
                emptySpace--;
            }
        }
        int needSpace = makeNumber * GenerateNumber / maxStack + (((makeNumber * GenerateNumber)%maxStack) == 0? 0 : 1);
        if(needSpace > emptySpace + consume) 
        {
            Debug.Log("[MakeFunctioner]: 背包空间不足");
            return;//背包空间不足
        }
        
        //将背包中的材料移除
        List<PackageData> deleteItems = new List<PackageData>();
        List<PackageData> updateItems = new List<PackageData>();
        foreach(KeyValuePair<int,int> kv in requireList)
        {
            int need = kv.Value * makeNumber;
            int id = kv.Key;
            foreach(PackageData item in PackageDataGetter.instance.PackageItems.ToList())
            {
                if(item == null) continue;
                if(need <= 0) break;
                if(item.ItemID == id)
                {
                    if(item.ItemCount <= need)
                    {
                        //移除当前表项
                        deleteItems.Add(item);
                        //await link.DeleteAsync<PackageData>(item.ID);
                    }
                    else
                    {
                        item.ItemCount -= need;
                        //刷新当前表项
                        //await link.InsertOrReplaceAsync(item);
                        updateItems.Add(item);
                    }
                    need -= item.ItemCount;
                }
            }
            foreach(PackageData dp in deleteItems)
            {
                if(dp == null) continue;
                await link.ExecuteAsync("DELETE FROM PackageData WHERE ID = ?",dp.ID);
            }

            foreach(PackageData dp in updateItems)
            {
                if(dp == null) continue;
                await link.ExecuteAsync("UPDATE PackageData SET ItemCount = ? WHERE ID = ?", dp.ItemCount, dp.ID);
            }
        }

        //将新的物品加入背包中
        //寻找背包的空位
        await PackageDataGetter.instance.GetDataFromDB();//获取背包数据
        List<int> indexList = new List<int>();
        for(int i = 1; i <= PackageDataGetter.instance.PackageSize; i++)
        {
            if(PackageDataGetter.instance.PackageItems.Find(x => x.SlotIndex == i) == null)
            {
                indexList.Add(i);
            }
        }
        //maxStack
        //needSpace
        int makeNum = makeNumber * GenerateNumber;
        int empIdx = 0;
        while(makeNum > 0)
        {
            PackageData newItem = new PackageData();
            newItem.ItemID = generateID;
            newItem.ItemCount = makeNum > maxStack ? maxStack : makeNum;
            newItem.SlotIndex = indexList[empIdx];
            newItem.PlayerID = PlayerStateSetter.instance.PlayerId;
            await link.ExecuteAsync(
                "INSERT INTO PackageData (PlayerID, SlotIndex ,ItemID ,ItemCount) VALUES (?, ? ,? ,?)",
                newItem.PlayerID, newItem.SlotIndex, newItem.ItemID, newItem.ItemCount
            );
            empIdx++;
            makeNum -= maxStack;
        }
        //制作完成
    }
}
