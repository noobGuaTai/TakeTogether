using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAttackTect : MonoBehaviour
{
    private EnemyAttribute enemyAttribute;
    private float ATK;
    
    void Start()
    {
        ATK = GetComponentInParent<PlayerAttribute>().ATK;
    }

    
    void Update()
    {

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        enemyAttribute = other.GetComponent<EnemyAttribute>();
        if(enemyAttribute != null)
        {
            enemyAttribute.ChangeHP(-ATK);
            Debug.Log(enemyAttribute.HP);
        }
        
    }
}
