using System.Threading.Tasks;
using UnityEngine;

public class DBInitinalizer : MonoBehaviour
{
    public static DBInitinalizer instance;
    async void Awake()
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
        await DBManager.Instance.Initialize();
    }   

    async void OnDestroy()
    {
        await DBManager.Instance.Close();
    } 
}
