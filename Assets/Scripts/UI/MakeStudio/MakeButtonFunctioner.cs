using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MakeButtonFunctioner : MonoBehaviour
{
    [SerializeField] private Button button;
    void OnEnable()
    {
        if(button == null) button = GetComponent<Button>();
        button.onClick.AddListener(FuncLink);
    }
    [SerializeField] private DiscriptionFresher fresher;
    void OnDisable()
    {
        button.onClick.RemoveListener(FuncLink);
    }

    async public void FuncLink()
    {
        MakeFunctioner.instance.MakeNewItem();
        //fresher.FreshDisplay();
        await PackageDataGetter.instance.GetDataFromDB();//刷新数据
    }
}
