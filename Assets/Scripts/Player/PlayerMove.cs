using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMove : NetworkBehaviour
{
    public GameObject HPUI;
    public virtual void Move(){}

    public virtual void Attack(){}

    public virtual void Attack2(){}
}
