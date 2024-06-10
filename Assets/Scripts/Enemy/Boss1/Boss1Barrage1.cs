using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.Basic;
using UnityEngine;

public class Boss1Barrage1 : EnemyMove
{
    public ParticleSystem ps;
    public GameObject subEmitterPrefab;//子粒子系统的prefab
    public float speed = 15f;
    public int particlesToEmit = 10;
    public bool canEmission = false;
    public float coolDownTime = 1.0f; // 冷却时间为1秒
    private double lastHitTime = 0.0f; // 上次被击中的时间
    [SyncVar] private bool barrage1subHasSpawn = false;
    private float atk = 0f;


    void Start()
    {
        Init();
        Emission();
        transform.localScale = new Vector3(9, 9, 1);

        atk = GameObject.FindGameObjectWithTag("Boss1").GetComponent<EnemyAttribute>().ATK;
    }

    void Update()
    {
        // Init();
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
        if (closedPlayer != null)
        {
            Vector3 directionToPlayer = closedPlayer.transform.position - transform.position;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        var emitParams = new ParticleSystem.EmitParams();
        ps.Emit(emitParams, 3);
        StartSubEmit();
    }

    void StartSubEmit()
    {
        //子粒子启动
        StartCoroutine(TriggerSubEmittersAfterDelay(2));//2秒后发射子粒子
    }

    IEnumerator TriggerSubEmittersAfterDelay(float delay)
    {
        // barrage1subHasSpawn = true;
        yield return new WaitForSeconds(delay);

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[3];
        if (subEmitterPrefab != null)
        {
            int numParticlesAlive = ps.GetParticles(particles);
            for (int i = 0; i < numParticlesAlive; i++)
            {
                // 实例化预制体，并设置位置
                GameObject instance = Instantiate(subEmitterPrefab, particles[i].position, Quaternion.identity);
                //NetworkServer.Spawn(instance);
                // ParticleSystem subPs = instance.GetComponent<ParticleSystem>();
                // if (subPs != null)
                // {
                //     // 发射粒子
                //     var emitParams = new ParticleSystem.EmitParams();
                //     //emitParams.position = transform.position;
                //     subPs.Emit(emitParams, 8); // 这里的8是要发射的粒子数量，根据需要调整
                // }
                Destroy(instance, 6f);
            }

        }
    }


    // void OnParticleTrigger()
    // {
    //     List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    //     int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

    //     for (int i = 0; i < numEnter; i++)
    //     {
    //         ParticleSystem.Particle p = enter[i];
    //         // 设置粒子的剩余生命时间为0，使其立即消失
    //         p.remainingLifetime = 0;
    //         enter[i] = p;
    //         // float atk = GetComponentInParent<EnemyAttribute>().ATK;
    //         // player.GetComponent<PlayerAttribute>().ChangeHP(-atk * 3);
    //     }

    //     // 应用更改回粒子系统
    //     ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    // }

    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Player" && NetworkTime.time - lastHitTime > coolDownTime)// 有点问题，应该在玩家身上判断
        {
            other.GetComponent<PlayerAttribute>().ChangeHP(-atk);
            lastHitTime = NetworkTime.time;
        }
    }

}
