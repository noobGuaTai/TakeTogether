using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Enemy2Attribute : EnemyAttribute
{

    protected override void Awake()
    {
        base.Awake();
        HP = 50f;
        MAXHP = 50f;
        ATK = 5f;
    }

    void Start()
    {
        SetParent();
    }

    protected override void Update()
    {
        base.Update();
        if (isDead && isServer)
        {
            while (propNums > 0)
            {
                GenProp("Coin_5");
                propNums--;
            }
        }
    }

}

