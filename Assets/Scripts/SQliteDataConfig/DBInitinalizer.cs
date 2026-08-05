using System.Threading.Tasks;
using UnityEngine;

public class DBInitinalizer : MonoBehaviour
{
    async void Awake()
    {
        await DatabaseManager.Instance.Initialize();
    }   

    async void OnDestroy()
    {
        await DatabaseManager.Instance.Close();
    } 
}
