using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Enemy2Attribute : EnemyAttribute
{

    void Awake()
    {
        HP = 50f;
        MAXHP = 50f;
        ATK = 5f;
    }

    public override void Die()
    {
        var propManager = transform.Find("/PropManager").GetComponent<PropManager>();
        propManager.GenProp("Coin_5", transform.position);
        base.Die();
    }
}
