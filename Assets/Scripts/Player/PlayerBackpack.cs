using Assets.Scripts.Tool.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBackpack : MonoBehaviour
{
    public Dictionary<String, int> props = new Dictionary<String, int>();
    public Dictionary<Collider2D, bool> inRangeNotCollected = new Dictionary<Collider2D, bool>();
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

        }
    }

    public void BuySkill(string skillName, int cost) { 
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Utils.ExitRange(inRangeNotCollected, collision);
    }

    void Update()
    {
        Utils.UpdateRange(inRangeNotCollected, (Collider2D collision) =>
        {
            var prop = collision.GetComponent<PropBase>();
            if (!prop.canBeCollected) return false;
            TryCollect(collision);
            return true;
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Utils.EnterRange(inRangeNotCollected, collision, (Collider2D col) =>
        {
            var prop = collision.gameObject.GetComponent<PropBase>();
            return prop != null;
        });
    }
}
