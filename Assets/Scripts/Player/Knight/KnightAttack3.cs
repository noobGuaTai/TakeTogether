using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class KnightAttack3 : NetworkBehaviour
{
    private PlayerAttribute playerAttribute;

    void Start()
    {
        playerAttribute = GetComponent<PlayerAttribute>();
    }

    void Update()
    {

    }

    // public void Attack3()
    // {
    //     if (!isLocalPlayer) return;
    //     if (Input.GetButton("Attack3") && playerAttribute.CTP >= 10f)
    //     {
    //         // CommandForSlowTime();
    //     }
    //     if (playerManager.otherPlayer != null && playerAttribute.isCT == true && playerManager.otherPlayer.GetComponent<PlayerAttribute>().isCT == true)
    //     {
    //         StartAttack3();
    //     }

    // }
}
