using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PropBase : MonoBehaviour
{
    public string propName = "unnamedProp";
    public int amount = 1;
    public bool canBeCollected = true;
    public bool isAttracted = false;

    public Tween tween = null;

    private void Awake()
    {
        isAttracted = false;
        tween = GetComponent<Tween>();
        if(transform.Find("/PropManager") != null)
        {
            transform.parent = transform.Find("/PropManager").gameObject.transform;
        }
    }

    public void _DropProcess(Vector3 position)
    {
        transform.position = position;
    }
    public void _DropCollect(float _) {
        canBeCollected = true;
    }
    
    public void Drop(Vector3 position, float dropRadius=16f, float collectDelay=0.6f) {
        canBeCollected = false;

        var randomAngle = UnityEngine.Random.Range(0, 2 * Mathf.PI);
        var randomVec3 = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0);
        var targetPos = transform.position + randomVec3 * dropRadius;

        tween.Clear();
        tween.AddTween(_DropProcess, transform.position, targetPos, collectDelay, Tween.TransitionType.QUAD, Tween.EaseType.OUT);
        tween.AddTween(_DropCollect, 0f, 0f, 0);
        tween.Play();
    }

    public void OnCollect(GameObject who) {
        var effectManagerObj = transform.Find("/EffectManager");
        var effectManager = effectManagerObj.GetComponent<EffectManager>();

        effectManager.GenEffect("Collect", transform.position);

        Destroy(gameObject);
    }

    public void _OnAttractProcess(float alpha, GameObject who, Vector3 startPosition) {
        alpha *= 3;
        transform.position = (1-alpha)*startPosition + alpha * who.transform.position;
    }
    public void OnAttract(GameObject who)
    {
        if (isAttracted)
            return;
        tween.Clear();
        var startPos = transform.position;
        tween.AddTween((float alpha) => _OnAttractProcess(alpha, who, startPos), 
            0f, 1f, 1, Tween.TransitionType.BACK, Tween.EaseType.IN);
        tween.Play();
        isAttracted = true;

        var effectManager = transform.Find("/EffectManager").GetComponent<EffectManager>();
        var obj = effectManager.GenEffect("Attract", transform.position);
        obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
    }
}
