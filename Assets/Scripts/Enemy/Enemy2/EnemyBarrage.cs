using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBarrage : MonoBehaviour
{
    public float shootInterval = 2.0f; // 发射间隔时间，以秒为单位
    private ParticleSystem ps; // 粒子系统组件
    private float timer = 0f; // 计时器
    private GameObject player;
    private Transform playerTransform; // 玩家的Transform

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        ps.trigger.SetCollider(0, playerTransform);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (playerTransform != null)
        {
            Vector3 directionToPlayer = playerTransform.position - transform.position;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0f, 0f, angle - 15);
        }

        if (timer >= shootInterval)
        {
            EmitParticlesTowardsPlayer();
            timer = 0f;
        }

    }


    void EmitParticlesTowardsPlayer()
    {
        // 确保ParticleSystem.Emit的参数正确配置以发射3个粒子
        var emitParams = new ParticleSystem.EmitParams();
        ps.Emit(emitParams, 3);
    }

    void OnParticleTrigger()
    {
        List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);

        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = inside[i];
            // 设置粒子的剩余生命时间为0，使其立即消失
            p.remainingLifetime = 0;
            inside[i] = p;
            float atk = GetComponentInParent<EnemyAttribute>().ATK;
            player.GetComponent<PlayerAttribute>().ChangeHP(-atk);
        }

        // 应用更改回粒子系统
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
    }


}
