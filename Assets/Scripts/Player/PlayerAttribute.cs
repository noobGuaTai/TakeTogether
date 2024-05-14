using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerAttribute : NetworkBehaviour
{
    [SyncVar] public float HP;
    public float MAXHP = 100;
    [SyncVar] public float MP;
    public float MAXMP = 100;
    [SyncVar] public float CTP;// 连携攻击值
    [SyncVar] public bool isCT;// 连携攻击确认
    [SyncVar] public float MPConsume;
    [SyncVar] public float ATK;
    [SyncVar] public bool isInvincible;
    public GameObject enemyHPUI;
    public GameObject bossHPUI;
    public bool isReady;
    public virtual void ChangeHP(float value){}
    public virtual void ChangeMP(float value){}
    public virtual void ChangeTP(float value){}
}
