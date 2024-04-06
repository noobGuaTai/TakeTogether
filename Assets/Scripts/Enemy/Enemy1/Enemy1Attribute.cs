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

   

}
