using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Enemy1Attribute : EnemyAttribute
{

    protected override void Awake()
    {
        base.Awake();
        HP = 100f;
        MAXHP = 100f;
        ATK = 10f;
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
            while(propNums > 0)
            {
                GenProp("Coin_1");
                propNums--;
            }
            
        }
    }


    public override void Die()
    {
        base.Die();
    }
}
