using System;
using UnityEngine;

public class PartsManager : MonoBehaviour
{
    public static PartsManager Instance { get; private set; }
    public int currentParts { get; private set; } = 0;
    public event Action OnPartsChanged;

    private void Awake()
    {
        //using singleton pattern to avoid any possible duplication of the parts manager in the scene
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddParts(int amount)
    {
        currentParts += amount;
        OnPartsChanged?.Invoke();
    }

    public bool SpendParts(int upgradeCost)
    {
        if (CanAffordUpgrade(upgradeCost))
        {
            currentParts -= upgradeCost;
            OnPartsChanged?.Invoke();
            return true;
        }
        return false;


    }

    public bool CanAffordUpgrade(int upgradeCost)
    {
        return currentParts >= upgradeCost;
    }
}
