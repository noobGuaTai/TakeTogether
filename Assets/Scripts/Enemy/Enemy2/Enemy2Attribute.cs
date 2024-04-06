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

}
