using SQLite4Unity3d;
using SQLite;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

//单例模式
public class DatabaseManager
{
    private static DatabaseManager _instance;
    public static DatabaseManager Instance => _instance ??= new DatabaseManager();

    private SQLiteAsyncConnection _db;
    
    // 移动端安全路径，PC端也可用
    private string DbPath => "D:/PROJECT/Unity/PackageExample/Assets/DataBase/AresVirus_PackageExample.db";/*Path.Combine(
        Application.persistentDataPath, 
        "AresVirus_PackageExample.db"
    );*/

    /// <summary>
    /// 游戏启动时调用一次
    /// </summary>
    public async Task Initialize()
    {
        if (_db != null) return;

        // 1.创建/打开数据库连接
        _db = new SQLiteAsyncConnection(DbPath);
        
        // 2.开启WAL模式,提升并发读写性能
        //await _db.ExecuteAsync("PRAGMA journal_mode=WAL;");

        // 3.自动建表（已存在则跳过，不会清空数据）
        await _db.CreateTableAsync<PackageData>();
        await _db.CreateTableAsync<PlayerState>();
        await _db.CreateTableAsync<ItemData>();

        Debug.Log($"[DBManager]: 数据库初始化完成: {DbPath}");
    }

    /// <summary>
    /// 获取数据库连接（供各业务模块使用）
    /// </summary>
    public SQLiteAsyncConnection GetConnection()
    {
        if (_db == null)
            throw new System.Exception("[DBManager]: 数据库未初始化,请先调用Initialize()");
        return _db;
    }

    /// <summary>
    /// 游戏退出时调用
    /// </summary>
    public async Task Close()
    {
        if (_db != null)
        {
            await _db.CloseAsync();
            _db = null;
        }
    }
}
