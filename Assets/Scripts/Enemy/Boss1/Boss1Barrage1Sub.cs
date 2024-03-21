using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Barrage1Sub : MonoBehaviour
{
    private ParticleSystem ps;
    private GameObject player;
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }


    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            ps.trigger.SetCollider(0, player.transform);
        }
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
            float atk = GameObject.FindGameObjectWithTag("Boss").GetComponent<EnemyAttribute>().ATK;
            player.GetComponent<PlayerAttribute>().ChangeHP(-atk * 3);
        }

        // 应用更改回粒子系统
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
    }
}
