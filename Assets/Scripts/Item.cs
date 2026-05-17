using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Базовый класс как для пассивных предметов так и для оружий.
/// </summary>
public abstract class Item : MonoBehaviour
{
    public int currentLevel = 1, maxLevel = 1;

    protected PlayerStats owner;

    public PlayerStats Owner { get { return owner; } }
    public virtual void Initialize(ItemData data)
    {
        maxLevel = data.maxLevel;
        owner = FindObjectOfType<PlayerStats>();
    }

    public virtual bool CanLevelUp()
    {
        return currentLevel <= maxLevel;
    }
    
    public virtual bool DoLevelUp()
    {
        return true;
    }

    public virtual void OnEquip() { }

    public virtual void OnUnequip() { }
}
