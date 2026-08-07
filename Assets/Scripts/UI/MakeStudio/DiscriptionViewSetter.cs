using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SQLite;
using System.Threading.Tasks;

[RequireComponent(typeof(DiscriptionView))]
public class DiscriptionViewSetter : MonoBehaviour
{
    [SerializeField] private DiscriptionView discriptionView;
    async void OnEnable()
    {
        if(discriptionView == null) discriptionView = GetComponent<DiscriptionView>();
        await FreshDisplay();
    }
    [SerializeField] private Image discriptImg;//描述图片
    [SerializeField] private TMP_Text itemName;//物品名称
    [SerializeField] private TMP_Text storageNumber;//库存数量
    [SerializeField] private TMP_Text discribe;//描述文字
    async public Task FreshDisplay()
    {
        await DBManager.Instance.Initialize();
        int id = discriptionView.GenerateID;    
        discriptImg.sprite = ItemIDToSpriteSetter.instance.GetSprite(id);//设置图片
        SQLiteAsyncConnection link = DBManager.Instance.GetConnection();
        ItemData state = await link.Table<ItemData>().Where(x => x.ItemID == id).FirstOrDefaultAsync();
        itemName.text = state?.ItemName.ToString();//设置名称
        discribe.text = state?.ItemDiscription.ToString();//设置描述文字
        decimal sum = await link.ExecuteScalarAsync<decimal>(
            "SELECT COALESCE(SUM(ItemCount), 0) FROM PackageData WHERE ItemID = ?", 
            id
        ); 
        storageNumber.text = sum.ToString();
    }
}
