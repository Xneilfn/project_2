using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Game Weapon Data", menuName = "Weapon Data")]
public class WeaponData : ItemData
{


    [HideInInspector] public string behaviour;
    public Weapon.Stats baseStats;
    public Weapon.Stats[] linearGrowth;
    public Weapon.Stats[] randomGrowth;

    public Weapon.Stats GetLevelData(int level)
    {
        //выбираем характеристики со следующего уровня
        if (level - 2 < linearGrowth.Length)
            return linearGrowth[level - 2];
        // в другом случае выбираем статы из случайного набора
        if (randomGrowth.Length > 0)
            return randomGrowth[Random.Range(0, randomGrowth.Length)];

        Debug.LogWarning(string.Format("Weapon doesn't have its level up stats configured for level{0}!", level));
        return new Weapon.Stats();
    }
}
