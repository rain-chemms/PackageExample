using UnityEngine;

//单例:玩家状态设置器,便于设置玩家ID以区分不同的背包系统
public class PlayerStateSetter : MonoBehaviour
{
    public static PlayerStateSetter instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    [SerializeField] private string playerId;//当前的玩家ID
    public string PlayerId{get => playerId;set => playerId = value;}
}
