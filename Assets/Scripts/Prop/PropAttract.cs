using Assets.Scripts.Tool.Utils;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PropAttract : MonoBehaviour
{
    //public GameObject master = null;
    //public Vector3 offset = Vector3.zero;

    //private void Awake()
    //{
    //    master = transform.parent.gameObject;
    //    offset = transform.position - master.transform.position;   
    //    transform.parent = transform.Find("/Global");
    //}

    //private void Update()
    //{
    //    transform.position = master.GetComponent<Transform>().position + offset;
    //}
    public Dictionary<Collider2D, bool> inRangeNotCollected = new Dictionary<Collider2D, bool>();

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
            prop.OnAttract(gameObject);
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
