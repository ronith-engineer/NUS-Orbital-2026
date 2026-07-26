using UnityEngine;
using UnityEngine.UI;

public class KnifeUI : MonoBehaviour
{
    private Image durabilityIndicator1;
    private Image durabilityIndicator2;
    private Image durabilityIndicator3;

    [SerializeField] private ItemData.ItemType weaponType;
    private Knife knife;

    private Image knifeIndicator; 
    private int knifeCurrentDurability;

    private void OnEnable()
    {
        knife = InventoryManager.Instance.GetSpawnedKnife(weaponType);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetUIReferences(); // Set UI references
        SetUIIndicatorsToGrey(); // Set all UI indicators to gray at the start
    }

    // Update is called once per frame
    void Update()
    {
        if (knife == null) return;
        knifeCurrentDurability = knife.knifeCurrentDurability; // Get the current durability from the Knife script
        UpdateDurabilityDisplay(); // Update the durability indicators based on the current durability

    }
    private void SetUIIndicatorsToGrey()
    {
        durabilityIndicator1.color = new Color32(125, 125, 125, 255);
        durabilityIndicator2.color = new Color32(125, 125, 125, 255);
        durabilityIndicator3.color = new Color32(125, 125, 125, 255);
    }

    private void SetUIReferences()
    {
        durabilityIndicator1 = transform.Find("DurabilityIndicator1").GetComponent<Image>();
        durabilityIndicator2 = transform.Find("DurabilityIndicator2").GetComponent<Image>();
        durabilityIndicator3 = transform.Find("DurabilityIndicator3").GetComponent<Image>();
        knifeIndicator = GetComponent<Image>();
    }

    private void UpdateDurabilityDisplay()
    {
        if (knifeCurrentDurability > 0)
        {
            durabilityIndicator1.color = new Color32(255, 255, 255, 255);
            if (knifeCurrentDurability > 1)
            {
                durabilityIndicator2.color = new Color32(255, 255, 255, 255);
                if (knifeCurrentDurability > 2)
                {
                    durabilityIndicator3.color = new Color32(255, 255, 255, 255);
                }
                else
                {
                    durabilityIndicator3.color = new Color32(125, 125, 125, 255);
                }
            }
            else
            {
                durabilityIndicator2.color = new Color32(125, 125, 125, 255);
            }
        }
        else
        {
            durabilityIndicator1.color = new Color32(125, 125, 125, 255);
            knifeIndicator.color = new Color32(125, 125, 125, 255);
        }
    }
}
