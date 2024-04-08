using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Boss1Attribute : EnemyAttribute
{
    protected override void Awake()
    {
        base.Awake();
        HP = 300f;
        MAXHP = 300f;
        ATK = 10f;
    }

    void Start()
    {
        SetParent();
    }

}
