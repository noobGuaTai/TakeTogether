using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttribute : MonoBehaviour
{
    public float HP;
    public float MAXHP;
    public float ATK;
    public virtual void ChangeHP(float value){}
    public virtual void Die(){}

}
