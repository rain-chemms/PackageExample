using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(PackageGrid))]
public class PackageGridSpriteSetter : MonoBehaviour
{
    [SerializeField] private PackageGrid grid;
    void OnEnable()
    {
        if(grid != null) grid = GetComponent<PackageGrid>();
        FreshDisplay();
    }

    void Start()
    {
        FreshDisplay();
    }
    
    [SerializeField] private TMP_Text itemName;//名称
    [SerializeField] private TMP_Text itemNumber;
    [SerializeField] private Image itemImage;
    [SerializeField] private SerializableDictionary<int,Sprite> itemIDToSprite = new SerializableDictionary<int, Sprite>();

    public void FreshDisplay()
    {
        if(itemName!=null) itemName.text = grid?.ItemName;
        if(itemNumber!=null) itemNumber.text = grid?.Number.ToString();
        Sprite sprite = null;
        if((bool)itemIDToSprite?.TryGetValue((int)grid?.ItemID,out sprite))
        {
            itemImage.sprite = sprite;
        }
    }
    
}
