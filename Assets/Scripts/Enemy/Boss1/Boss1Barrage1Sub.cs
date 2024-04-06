using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Boss1Barrage1Sub : EnemyMove
{
    private ParticleSystem ps;
    private GameObject player;
    public float cooldownTime = 1.0f; // 冷却时间为1秒
    private double lastHitTime = 0.0f; // 上次被击中的时间
    void Start()
    {
        Init();
        Emission();
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
        var emitParams = new ParticleSystem.EmitParams();
        ps.Emit(emitParams, 8);
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
            // float atk = GameObject.FindGameObjectWithTag("Boss").GetComponent<EnemyAttribute>().ATK;
            // player.GetComponent<PlayerAttribute>().ChangeHP(-atk * 3);
        }

        // 应用更改回粒子系统
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Player" && NetworkTime.time - lastHitTime > cooldownTime)
        {
            float atk = GameObject.FindGameObjectWithTag("Boss1").GetComponent<EnemyAttribute>().ATK;
            other.GetComponent<PlayerAttribute>().ChangeHP(-atk);
            lastHitTime = NetworkTime.time;
        }
    }

}
