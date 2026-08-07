using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using SQLite;
using System.Linq;

[RequireComponent(typeof(ConditionItem))]
public class ConditionItemViewSetter : MonoBehaviour
{
    [SerializeField] private ConditionItem conditionItem;
    async void OnEnable()
    {
        if(conditionItem == null) conditionItem = GetComponent<ConditionItem>();
        await FreshDisplay();
    }
    [SerializeField] private TMP_Text conditionNumber;
    public TMP_Text ConditionNumber { get => conditionNumber; }
    [SerializeField] private TMP_Text conditionName;
    public TMP_Text ConditionName { get => conditionName; }
    [SerializeField] private Image conditionImage;
    public Image ConditionImage { get => conditionImage; }
    async public Task FreshDisplay()
    {
        await DBManager.Instance.Initialize();
        await PackageDataGetter.instance.GetDataFromDB();
        //获取当前ID对应的物品名称
        SQLiteAsyncConnection link = DBManager.Instance.GetConnection();
        conditionName.text = (await (link.Table<ItemData>().Where(x => x.ItemID == conditionItem.RequireItemID).FirstOrDefaultAsync())).ItemName;
        conditionImage.sprite = ItemIDToSpriteSetter.instance?.GetSprite(conditionItem.RequireItemID);
        int nowNumber = 0;
        foreach(PackageData pd in PackageDataGetter.instance.PackageItems)
        {
            if(pd.ItemID == conditionItem.RequireItemID)
            {
                nowNumber += pd.ItemCount;
            }
        }
        conditionNumber.text = "(" + nowNumber.ToString() + "/" + conditionItem.RequireItemNumber.ToString() + ")";
    }
}