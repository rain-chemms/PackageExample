using UnityEngine;

public class RecipeItem : MonoBehaviour
{
    [SerializeField] private int recipeId;
    public int RecipeID { get => recipeId; set => recipeId = value; }
    [SerializeField] private string recipeName;
    public string RecipeName { get => recipeName; set => recipeName = value; }
    [SerializeField] private int generateId;//产生物品的ID
    public int GenerateID { get => generateId; set => generateId = value; }
    [SerializeField] private string requireList;
    public string RequireList { get => requireList; set => requireList = value; }
    [SerializeField] public int generateNumber;
    public int GenerateNumber { get => generateNumber; set => generateNumber = value; }
}
