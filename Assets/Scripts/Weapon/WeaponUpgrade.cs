using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponUpgrade", menuName = "Weapons/Weapon Upgrade")]

public class WeaponUpgrade : ScriptableObject
{
    public float statIncrease;
    public int partsCost;

    public UpgradeStatType statType;
    public enum UpgradeStatType
    { 
        Damage,
        ClipCapacity,
        Silencer

    }


}
