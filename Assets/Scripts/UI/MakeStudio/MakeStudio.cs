using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Collections.Generic;
using SQLite;

public class MakeStudio : MonoBehaviour
{
    [SerializeField] private RecipeItem recipeItemPrefab;//预制体
    async void OnEnable()
    {
       await FreshRecipeBar();
    }
    [SerializeField] private DiscriptionFresher discriptionFresher;//描述界面
    [SerializeField] private ScrollRect recipeBar;
    async public Task FreshRecipeBar()
    {
        //数据库初始化
        await DBManager.Instance.Initialize();
        await RecipeDataGetter.instance?.GetDataFromDB();
        SQLiteAsyncConnection link = DBManager.Instance.GetConnection();
        //清空列表
        foreach(Transform child in recipeBar?.content.transform)
        {
            Destroy(child.gameObject);
        }
        
        List<Recipe> recipes = RecipeDataGetter.instance?.Recipes;
        foreach(Recipe recipe in recipes)
        {
            RecipeItem recipeItem = Instantiate(recipeItemPrefab, recipeBar.content.transform);
            recipeItem.RecipeID = recipe.RecipeID;
            recipeItem.GenerateID = recipe.GenerateID;
            recipeItem.RequireList = recipe.RequireList;
            recipeItem.GenerateNumber = recipe.GenerateNumber;
            //读取数据库以获取生成的物品的名字
            var state = await link.Table<ItemData>().Where(x => x.ItemID == recipe.GenerateID).FirstOrDefaultAsync();
            recipeItem.RecipeName = state?.ItemName;
            RecipeItemViewSetter viewSetter = recipeItem.GetComponent<RecipeItemViewSetter>();
            if(viewSetter != null)
            {
                viewSetter.DiscriptionFresher = discriptionFresher;
                viewSetter.FreshDisplay();        
            }
        }
    }
}
