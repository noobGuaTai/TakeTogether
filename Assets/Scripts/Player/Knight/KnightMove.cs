using System.Collections;
using System.Collections.Generic;
using Mirror;
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
    private PlayerManager playerManager;

    private bool isDashing;
    private float dashTimeLeft;
    private bool isAttacking;
    private float restoreSpeedMP = 1f;
    [SyncVar] public double lastRestoreMPTime;
    private float underAttackCooldownTime = 1.0f; // 受击冷却时间为1秒
    private double lastHitTime = 0.0f; // 上次被击中的时间


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerAttribute = GetComponent<PlayerAttribute>();
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        lastRestoreMPTime = NetworkTime.time;
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize(); // 保证对角移动时速度不会加倍

        Attack();
        Attack2();
        Attack3();

        if (isLocalPlayer && NetworkTime.time - lastRestoreMPTime > restoreSpeedMP)
        {
            UpdateMP(1f);
            lastRestoreMPTime = NetworkTime.time;
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
        if (!isLocalPlayer) return;
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);

        bool isMoving = moveInput.x != 0 || moveInput.y != 0;
        ShowPlayerAnimCommand("run", isMoving);

        if (isMoving)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    public override void Attack2()
    {
        if (!isLocalPlayer) return;
        if (Input.GetButtonDown("Attack2") && !isDashing && playerAttribute.MP >= playerAttribute.MPConsume)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        playerAttribute.isInvincible = true;//冲刺期间无敌
        UpdateMP(-playerAttribute.MPConsume);
        transform.Find("KnightAttack2Tect").GetComponent<Collider2D>().enabled = true;
        dashTimeLeft = dashTime;
        ShowPlayerAnimCommand("dash", true);
        rb.velocity = moveInput * dashSpeed;

        CommandForSpawnTrail();
        yield return new WaitForSeconds(dashTime / 2);
        CommandForSpawnTrail();
        yield return new WaitForSeconds(dashTime / 2);
        CommandForSpawnTrail();

        // 结束冲刺，重置状态
        rb.velocity = Vector2.zero;
        isDashing = false;
        playerAttribute.isInvincible = false;
        transform.Find("KnightAttack2Tect").GetComponent<Collider2D>().enabled = false;
        ShowPlayerAnimCommand("dash", false);
    }

    [Command]
    void CommandForSpawnTrail()// 客户端发起请求，里边的操作在服务器执行，然后调用ClientRpc同步到其他客户端
    {
        SpawnTrail();
    }

    [ClientRpc]
    void SpawnTrail()
    {
        // 创建一个新的Sprite GameObject作为残影
        GameObject trail = new GameObject("Trail");
        SpriteRenderer trailRenderer = trail.AddComponent<SpriteRenderer>();
        trailRenderer.sprite = GetComponent<SpriteRenderer>().sprite;
        trailRenderer.material = trailMaterial;
        trailRenderer.color = new Color(49f / 255f, 110f / 255f, 183f / 255f, 1f);
        //trail.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.01f);
        trail.transform.position = transform.position;
        trail.transform.localScale = transform.localScale;

        // 设置残影的渲染层级确保它在角色下方
        trailRenderer.sortingLayerID = GetComponent<SpriteRenderer>().sortingLayerID;
        trailRenderer.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;

        Destroy(trail, 1f);
    }

    public override void Attack()
    {
        if (!isLocalPlayer) return;
        if (Input.GetButton("Attack") && !isAttacking && !isDashing)
        {
            StartCoroutine(StartAttack());
        }
    }

    IEnumerator StartAttack()
    {
        isAttacking = true;
        ShowPlayerAnimCommand("attack", true);
        transform.Find("KnightAttackTect").GetComponent<Collider2D>().enabled = true;

        yield return new WaitForSeconds(attackDuration); // 等待攻击动画完成

        isAttacking = false;
        ShowPlayerAnimCommand("attack", false);
        transform.Find("KnightAttackTect").GetComponent<Collider2D>().enabled = false;
    }

    public override void Attack3()
    {
        if (!isLocalPlayer) return;
        if (Input.GetButton("Attack3") && playerAttribute.CTP >= 10f)
        {
            CommandForSlowTime();
        }
        if (playerManager.otherPlayer != null && playerAttribute.isCT == true && playerManager.otherPlayer.GetComponent<PlayerAttribute>().isCT == true)
        {
            StartAttack3();
        }

    }

    [Command]
    void CommandForSlowTime()
    {
        RpcForSlowTime();
    }

    [ClientRpc]
    void RpcForSlowTime()
    {
        StartCoroutine(SlowTime());
    }

    IEnumerator SlowTime()
    {
        Time.timeScale = 0.3f;
        playerAttribute.isCT = true;
        // playerAttribute.CTP -= 10f;
        yield return new WaitForSeconds(0.1f);
        playerAttribute.isCT = false;
        Time.timeScale = 1f;
    }

    void StartAttack3()
    {
        ShowPlayerAnimCommand("attack3", true);
        isAttacking = true;
        playerAttribute.isCT = false;
        StartCoroutine(MoveToPosition(transform, playerManager.otherPlayer.transform.position, 0.1f));
    }

    IEnumerator MoveToPosition(Transform playerTransform, Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0;
        Vector3 startingPosition = playerTransform.position;
        while (elapsedTime < duration)
        {
            playerTransform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        playerTransform.position = targetPosition;
        ShowPlayerAnimCommand("attack3", false);
        isAttacking = false;
    }

    [Command]
    void UpdateMP(float value)
    {
        playerAttribute.ChangeMP(value);
    }

    [Command]
    void ShowPlayerAnimCommand(string name, bool can)
    {
        ShowPlayerAnim(name, can);
    }

    [ClientRpc]
    void ShowPlayerAnim(string name, bool can)
    {
        if (anim != null)
        {
            anim.SetBool(name, can);
        }

    }

}
