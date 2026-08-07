using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AddAndSubButtonFunctioner : MonoBehaviour
{
    [SerializeField] private Button button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ASButtonFunction);
    }

    void OnDisable()
    {
        button.onClick.RemoveListener(ASButtonFunction);
    }

    [SerializeField] bool isAdd = false;
    [SerializeField] private DiscriptionFresher disFresher;
    private void ASButtonFunction()
    {
        if (isAdd)
        {
            MakeFunctioner.instance.MakeNumber++;
        }
        else
        {
            MakeFunctioner.instance.MakeNumber--;
            if(MakeFunctioner.instance.MakeNumber < 1) MakeFunctioner.instance.MakeNumber = 1;    
        }
        disFresher?.FreshDisplay();
    }
}
