using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class EnemyAttribute : NetworkBehaviour
{
    [SyncVar] public float HP;
    public float MAXHP;
    [SyncVar] public float ATK;
    public GameObject damageTextPrefab;
    [SyncVar] public bool isDead = false;
    [SyncVar] public int propNums = 1;// 掉落金币数量

    private Animator anim;
    private Rigidbody2D rb;
    public Transform UI;
    private GameObject enemies;


    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        UI = GameObject.Find("UI").transform;
    }


    protected virtual void Update()
    {
        if (HP <= 0 && isServer && !isDead)
        {
            Die();

        }
        if (HP <= 0)// 不知道为什么 把EnemyMove设置为false了，总是又变成true了
        {
            GetComponent<EnemyMove>().enabled = false;
            GetComponent<EnemyMove>().isAttacking = false;
            rb.velocity = Vector2.zero;
        }
    }

    [Server]
    public virtual void ChangeHP(float value)
    {
        HP += value;

        // 实例化伤害文本预设
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + 8f);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(pos);
        GameObject damageTextInstance = Instantiate(damageTextPrefab, screenPosition, Quaternion.identity, UI.transform);
        damageTextInstance.transform.SetParent(UI);
        // 设置伤害值
        damageTextInstance.GetComponent<EnemyUnderAttackText>().SetText(System.Math.Abs(value).ToString());
    }

    [ClientRpc]
    public virtual void Die()
    {
        anim.SetBool("death", true);
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 1f);
        // gameObject.SetActive(false);
        isDead = true;
    }

    [Server]
    public virtual void GenProp(string PropName)
    {
        StartCoroutine(GenPropCoroutine(PropName));
    }

    IEnumerator GenPropCoroutine(string PropName)
    {
        yield return new WaitForSeconds(0.9f);
        var propManager = transform.Find("/PropManager").GetComponent<PropManager>();
        propManager.GenProp(PropName, transform.position);
    }

    public void SetParent()
    {
        if (transform.Find("/Enemies") != null)
        {
            enemies = transform.Find("/Enemies").gameObject;
            transform.parent = enemies.transform;
        }
    }

}
