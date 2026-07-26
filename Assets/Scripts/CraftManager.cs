using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance;

    [Header("Craft Windows")]
    [SerializeField] private GameObject craftWindow1;
    [SerializeField] private GameObject craftWindow2;

    [Header("Result Items")]
    [SerializeField] private ItemData molotovItem;
    [SerializeField] private ItemData bandageItem;
    [SerializeField] private ItemData knifeItem;

    private InventorySlot slotA;
    private InventorySlot slotB;

    private void Awake()
    {
        Instance = this;
        if (craftWindow1 != null)
            craftWindow1.SetActive(false);
        if (craftWindow2 != null)
            craftWindow2.SetActive(false);
    }

    public bool TryCraft(InventorySlot from, InventorySlot to)
    {
        ItemData itemA = from.GetItem();
        ItemData itemB = to.GetItem();

        if (itemA == null || itemB == null) return false;

        slotA = from;
        slotB = to;

        if (IsPair(itemA, itemB, ItemData.ItemType.Alcohol, ItemData.ItemType.Rags))
        {
            from.ResetIcon();
            to.ResetIcon();
            craftWindow1.SetActive(true);
            InventoryManager.Instance.HideInventory();
            return true;
        }

        if (IsPair(itemA, itemB, ItemData.ItemType.MetalScrap, ItemData.ItemType.Rags))
        {
            from.ResetIcon();
            to.ResetIcon();
            craftWindow2.SetActive(true);
            InventoryManager.Instance.HideInventory();
            return true;
        }

        return false;
    }

    private void CompleteCraft(ItemData result)
    {
        InventoryManager.Instance.ShowInventory();
        slotA.RemoveOne();
        slotB.RemoveOne();
        InventoryManager.Instance.AddItem(result);
    }

    private bool IsPair(ItemData a, ItemData b, ItemData.ItemType type1, ItemData.ItemType type2)
    {
        return (a.itemType == type1 && b.itemType == type2)
            || (a.itemType == type2 && b.itemType == type1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (craftWindow1.activeSelf || craftWindow2.activeSelf)
            {
                craftWindow1.SetActive(false);
                craftWindow2.SetActive(false);
                InventoryManager.Instance.ShowInventory();
            }
        }
    }

    public void CraftMolotov()
    {
        craftWindow1.SetActive(false);
        CompleteCraft(molotovItem);
    }

    public void CraftBandage()
    {
        craftWindow1.SetActive(false);
        CompleteCraft(bandageItem);
    }

    public void CraftKnife()
    {
        craftWindow2.SetActive(false);
        CompleteCraft(knifeItem);
    }
}