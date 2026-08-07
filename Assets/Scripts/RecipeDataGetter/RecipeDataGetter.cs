using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using SQLite;

public class RecipeDataGetter : MonoBehaviour
{
    public static RecipeDataGetter instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // 启动时
    async void OnEnable()
    {
        await GetDataFromDB();
    }

    [SerializeField] private List<Recipe> recipes;
    public List<Recipe> Recipes{ get=> recipes; }
    public async Task GetDataFromDB()
    {
        //读取数据库获取玩家的背包大小
        SQLiteAsyncConnection link = DBManager.Instance.GetConnection();//获取数据库连接
        //获取所有的配方数据
        recipes = await link.Table<Recipe>().ToListAsync();
    }
}
