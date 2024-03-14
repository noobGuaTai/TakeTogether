using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttribute : MonoBehaviour
{
    public float HP;
    public float ATK;
    public bool isInvincible;
    public virtual void ChangeHP(float value){}
    public virtual void ChangeMP(float value){}
    public virtual void ChangeTP(float value){}
}
