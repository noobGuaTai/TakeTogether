using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttribute : MonoBehaviour
{
    public float HP;
    public float ATK;
    public virtual void ChangeHP(){}
    public virtual void ChangeMP(){}
    public virtual void ChangeTP(){}
}
