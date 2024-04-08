using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Enemy1Attribute : EnemyAttribute
{

    void Awake()
    {
        HP = 100f;
        MAXHP = 100f;
        ATK = 10f;
    }

    void Start()
    {
        SetParent();
    }


    public override void Die()
    {
        var propManager = transform.Find("/PropManager").GetComponent<PropManager>();
        propManager.GenProp("Coin_1", transform.position);
        base.Die();
    }
}
