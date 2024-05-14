using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMove : NetworkBehaviour
{
    public GameObject HPUI;
    public bool isPlayerInBossRoom;
    public virtual void Move(){}

    public virtual void Attack(){}

    public virtual void Attack2(){}

    public virtual void Attack3(){}
}
