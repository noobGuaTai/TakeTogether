using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBackpack : MonoBehaviour
{
    public Dictionary<String, int> props = new Dictionary<String, int>();

    public void Collect(String name, int amount) {
        if (!props.ContainsKey(name))
        {
            props.Add(name, amount);
        }
        else props[name] += amount;
    }

    public void TryCollect(Collider2D collision)
    {
        var obj = collision.gameObject;
        if(obj.layer != LayerMask.NameToLayer("Prop"))
        {
            return;
        }
        var prop = obj.GetComponent<PropBase>();

        if (prop.canBeCollected)
        {
            Collect(prop.propName, prop.amount);
            prop.OnCollect(gameObject);

            Destroy(prop.gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryCollect(collision);
    }
}
