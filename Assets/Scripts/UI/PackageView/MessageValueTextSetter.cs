using UnityEngine;
using SQLite;
using System.Threading.Tasks;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class MessageValueTextSetter : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void OnEnable()
    {
        if(text == null) text = GetComponent<TMP_Text>();
        await FreshTextDisplay();
    }

    [SerializeField] private int maxSize = 0;
    [SerializeField] private int nowSize = 0;
    public async Task FreshTextDisplay()
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
        else maxSize = state.PackageSize;  
        nowSize = (await link.Table<PackageData>()
            .Where(x => x.PlayerID.Equals(playerId) && x.SlotIndex <= maxSize)
            .ToListAsync()
        ).Count;//获取所有有效的背包数据
        //设置文本
        if(text != null) text.text = $"({nowSize}/{maxSize})";
    }

}
