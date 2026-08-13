using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{

    public override void Initialize()
    {
        baseClipCapacity = 4;
        baseAttackDamage = 2;
        base.Initialize();
    }




}
