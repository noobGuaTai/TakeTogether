using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerAttribute : NetworkBehaviour
{
    [SyncVar] public float HP;
    public float MAXHP;
    [SyncVar] public float MP;
    public float MAXMP;
    [SyncVar] public float MPConsume;
    [SyncVar] public float ATK;
    [SyncVar] public bool isInvincible;
    public GameObject enemyHPUI;
    public bool isReady;
    public virtual void ChangeHP(float value){}
    public virtual void ChangeMP(float value){}
    public virtual void ChangeTP(float value){}
}
