using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Boss1Attribute : EnemyAttribute
{
   

    void Awake()
    {
        HP = 300f;
        MAXHP = 300f;
        ATK = 10f;
    }

    
}
