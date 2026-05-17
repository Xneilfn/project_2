using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceWeapon : ProjectileWeapon
{
    protected override float GetSpawnAngle()
    {
        return Random.Range(0f, 360f);
    }

}
