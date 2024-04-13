using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Enemy2Barrage : EnemyMove
{
    public float shootInterval = 2.0f; // 发射间隔时间，以秒为单位
    private ParticleSystem ps; // 粒子系统组件
    private float timer = 0f; // 计时器
    public float cooldownTime = 1.0f; // 冷却时间为1秒
    private double lastHitTime = 0.0f; // 上次被击中的时间
    private float atk = 0f;

    void Start()
    {
        Init();
        Emission();

        atk = GameObject.FindGameObjectWithTag("Enemy2").GetComponent<EnemyAttribute>().ATK;
    }

    void Update()
    {
        Init();
    }

    void Init()
    {
        ps = GetComponent<ParticleSystem>();
        allPlayers = FindAllPlayers();
        closedPlayer = FindClosestPlayer();
        if (closedPlayer != null)
        {
            int i = 0;
            foreach (GameObject player in allPlayers)
            {
                ps.trigger.SetCollider(i, player.transform);
                i += 1;
            }
        }
    }

    void Emission()
    {
        // timer += Time.deltaTime;

        if (closedPlayer != null)
        {
            Vector3 directionToPlayer = closedPlayer.transform.position - transform.position;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0f, 0f, angle - 15);
            var emitParams = new ParticleSystem.EmitParams();
            ps.Emit(emitParams, 3);

        }

    }

    void OnParticleTrigger()
    {
        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = enter[i];
            // 设置粒子的剩余生命时间为0，使其立即消失
            p.remainingLifetime = 0;
            enter[i] = p;
            // float atk = GetComponentInParent<EnemyAttribute>().ATK;
            // player.GetComponent<PlayerAttribute>().ChangeHP(-atk);
        }

        // 应用更改回粒子系统
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Player" && NetworkTime.time - lastHitTime > cooldownTime)
        {
            // 粒子碰撞到了玩家
            // 在这里处理碰撞逻辑，比如减少玩家的HP
            other.GetComponent<PlayerAttribute>().ChangeHP(-atk * 3);
            lastHitTime = NetworkTime.time;
        }
    }


}
