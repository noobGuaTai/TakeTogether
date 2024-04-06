using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class EnemyAttribute : NetworkBehaviour
{
    [SyncVar] public float HP;
    public float MAXHP;
    [SyncVar] public float ATK;
    public virtual void ChangeHP(float value){}
    public virtual void Die(){}

}
