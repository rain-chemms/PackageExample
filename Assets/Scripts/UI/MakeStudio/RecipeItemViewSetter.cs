using UnityEngine;
using UnityEngine.UI;
using TMPro;
[RequireComponent(typeof(RecipeItem))]
public class RecipeItemViewSetter : MonoBehaviour
{
    [SerializeField] private DiscriptionFresher discriptionFresher;//描述
    public DiscriptionFresher DiscriptionFresher { get => discriptionFresher; set => discriptionFresher = value; }
    [SerializeField] private RecipeItem recipeItem;
    void OnEnable()
    {
        if(recipeItem == null) recipeItem = GetComponent<RecipeItem>();
    }
    [SerializeField] private TMP_Text recipeText;
    public TMP_Text RecipeText { get => recipeText; }
    [SerializeField] private Image recipeImage;
    public Image Image { get => recipeImage; }
    public void FreshDisplay()
    {
        if(recipeText!=null) recipeText.text = recipeItem.RecipeName;
        Sprite sprite = ItemIDToSpriteSetter.instance?.GetSprite(recipeItem.RecipeID);
        recipeImage.sprite = sprite;
        discriptionFresher.GetComponent<Canvas>().enabled = true;
        discriptionFresher?.FreshDisplay();
    }
}
