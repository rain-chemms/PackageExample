using UnityEngine;
using SQLite;
using System.Threading.Tasks;
using TMPro;


public class ItemNumberValueTextSetter : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    public TMP_Text Text { get=>text; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void OnEnable()
    {
        if(text == null) text = GetComponent<TMP_Text>();
        await FreshTextDisplay();
    }
    
    public async Task FreshTextDisplay()
    {
        await DBManager.Instance.Initialize();
        await PackageDataGetter.instance.GetDataFromDB();
        int maxSize = (int)PackageDataGetter.instance?.PackageSize;  
        int nowSize = (int)PackageDataGetter.instance?.PackageItems?.Count;
        //设置文本
        if(text != null) text.text = $"({nowSize}/{maxSize})";
    }

}
