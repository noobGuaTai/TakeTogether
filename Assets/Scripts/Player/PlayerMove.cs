using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 20f; // 冲刺速度
    public float dashTime = 0.2f; // 冲刺持续时间

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator anim;

    private bool isDashing;
    private float dashTimeLeft;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize(); // 保证对角移动时速度不会加倍

        // 检测冲刺键（例如空格键）是否被按下
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
        {
            isDashing = true;
            dashTimeLeft = dashTime;
        }

        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                rb.velocity = moveInput * dashSpeed;
                dashTimeLeft -= Time.deltaTime;
            }
            else
            {
                isDashing = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (!isDashing) // 只在非冲刺状态下应用正常移动
        {
            Move();
        }
    }

    void Move()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);

        bool isMoving = moveInput.x != 0 || moveInput.y != 0;
        anim.SetBool("run", isMoving);

        if (isMoving)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}
