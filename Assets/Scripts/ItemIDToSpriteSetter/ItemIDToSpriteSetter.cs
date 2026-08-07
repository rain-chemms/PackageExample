using UnityEngine;

public class ItemIDToSpriteSetter : MonoBehaviour
{
    public static ItemIDToSpriteSetter instance;
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
    [SerializeField] private SerializableDictionary<int,Sprite> itemIDToSprite = new SerializableDictionary<int, Sprite>();
    public SerializableDictionary<int, Sprite> ItemIDToSprite {get => itemIDToSprite;}

    public Sprite GetSprite(int itemID)
    {
        if(itemIDToSprite.Contains(itemID)) return itemIDToSprite[itemID];
        return null;
    }
}