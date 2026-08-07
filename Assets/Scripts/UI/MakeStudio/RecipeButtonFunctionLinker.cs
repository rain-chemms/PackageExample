using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RecipeItem))]
[RequireComponent(typeof(Button))]
public class RecipeButtonFunctionLinker : MonoBehaviour
{
    [SerializeField] private RecipeItem recipeItem;
    [SerializeField] private Button button;
    void OnEnable()
    {
        if(button == null) button = GetComponent<Button>();
        if(recipeItem == null) recipeItem = GetComponent<RecipeItem>();
        button.onClick.AddListener(AfterButtonClick);
    }

    void OnDisable()
    {
        button.onClick.RemoveListener(AfterButtonClick);
    }

    private void AfterButtonClick()
    {
        MakeFunctioner.instance.Recipe = recipeItem.RequireList;
        MakeFunctioner.instance.GenerateID = recipeItem.GenerateID;
        MakeFunctioner.instance.GenerateID = recipeItem.GenerateID;
        MakeFunctioner.instance.GenerateNumber = recipeItem.GenerateNumber;
        recipeItem?.GetComponent<RecipeItemViewSetter>()?.FreshDisplay();
    }


}
