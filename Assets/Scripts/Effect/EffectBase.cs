using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EffectBase : MonoBehaviour
{
    public bool singlePlaye;

    public virtual void OnAnimationEnds() { 
        if(singlePlaye)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

}
