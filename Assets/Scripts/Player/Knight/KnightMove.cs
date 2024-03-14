using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMove : PlayerMove
{
    public float moveSpeed = 5f;
    public float dashSpeed = 20f; // 冲刺速度
    public float dashTime = 0.2f; // 冲刺持续时间
    public float attackDuration = 0.3f; // 攻击动画持续时间
    public Material trailMaterial; // 用于残影的材质
    public float trailSpawnInterval = 0.05f; // 残影生成间隔时间

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator anim;
    private PlayerAttribute playerAttribute;

    private bool isDashing;
    private float dashTimeLeft;
    private bool isAttacking;
    private float restoreSpeedMP = 1f;
    private float lastRestoreMPTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerAttribute = GetComponent<PlayerAttribute>();
        lastRestoreMPTime = Time.time;
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize(); // 保证对角移动时速度不会加倍

        Attack();
        Attack2();

        if(Time.time - lastRestoreMPTime > restoreSpeedMP)
        {
            playerAttribute.ChangeMP(1f);
            lastRestoreMPTime = Time.time;
        }
    }

    void FixedUpdate()
    {
        if (!isDashing && !isAttacking)
        {
            Move();
        }

    }

    public override void Move()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);

        bool isMoving = moveInput.x != 0 || moveInput.y != 0;
        anim.SetBool("run", isMoving);

        if (isMoving)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    public override void Attack2()
    {
        if (Input.GetButtonDown("Attack2") && !isDashing && playerAttribute.MP >= playerAttribute.MPConsume)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        playerAttribute.isInvincible = true;//冲刺期间无敌
        playerAttribute.ChangeMP(-playerAttribute.MPConsume);
        GameObject.Find("KnightAttack2Tect").GetComponent<Collider2D>().enabled = true;
        dashTimeLeft = dashTime;
        anim.SetBool("dash", true);
        rb.velocity = moveInput* dashSpeed;

        SpawnTrail();
        yield return new WaitForSeconds(dashTime / 2);
        SpawnTrail();
        yield return new WaitForSeconds(dashTime / 2);
        SpawnTrail();

        // 结束冲刺，重置状态
        rb.velocity = Vector2.zero;
        isDashing = false;
        playerAttribute.isInvincible = false;
        GameObject.Find("KnightAttack2Tect").GetComponent<Collider2D>().enabled = false;
        anim.SetBool("dash", false);
    }


    void SpawnTrail()
    {
        // 创建一个新的Sprite GameObject作为残影
        GameObject trail = new GameObject("Trail");
        SpriteRenderer trailRenderer = trail.AddComponent<SpriteRenderer>();
        trailRenderer.sprite = GetComponent<SpriteRenderer>().sprite;
        trailRenderer.material = trailMaterial;
        trailRenderer.color = new Color(49f/255f, 110f/255f, 183f/255f, 1f);
        //trail.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.01f);
        trail.transform.position = transform.position;
        trail.transform.localScale = transform.localScale;

        // 设置残影的渲染层级确保它在角色下方
        trailRenderer.sortingLayerID = GetComponent<SpriteRenderer>().sortingLayerID;
        trailRenderer.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;

        // 使残影在短时间后消失
        Destroy(trail, 1f);
    }

    public override void Attack()
    {
        if (Input.GetButton("Attack") && !isAttacking && !isDashing)
        {
            StartCoroutine(StartAttack());
        }
    }

    IEnumerator StartAttack()
    {
        isAttacking = true;
        anim.SetBool("attack", true);
        GameObject.Find("KnightAttackTect").GetComponent<Collider2D>().enabled = true;

        yield return new WaitForSeconds(attackDuration); // 等待攻击动画完成

        isAttacking = false;
        anim.SetBool("attack", false);
        GameObject.Find("KnightAttackTect").GetComponent<Collider2D>().enabled = false;
    }
}
