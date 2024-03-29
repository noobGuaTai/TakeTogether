using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Barrage1 : MonoBehaviour
{
    public GameObject subEmitterPrefab;
    public float subEmitInterval = 2.0f; // 子粒子系统发射间隔
    public int mainParticleNums = 3;
    public bool canEmission = false;

    private ParticleSystem ps; 
    private GameObject player;
    private Transform playerTransform;
    private EnemyMove enemyMove;


    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        enemyMove = GetComponentInParent<EnemyMove>();
        var main = ps.main;
        main.startSpeed = 2f;
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
                ps.trigger.SetCollider(0, playerTransform);
            }
        }

        if (enemyMove.isAttacking == true && canEmission == true)
        {
            Emission();
            canEmission = false;
        }
    }

    void Emission()
    {
        if (playerTransform != null)
        {
            Vector3 directionToPlayer = playerTransform.position - transform.position;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        EmitParticlesTowardsPlayer();
        
    }


    void EmitParticlesTowardsPlayer()
    {
        var emitParams = new ParticleSystem.EmitParams();
        ps.Emit(emitParams, mainParticleNums);
        
        //子粒子启动
        StartCoroutine(TriggerSubEmittersAfterDelay(subEmitInterval));
    }

    IEnumerator TriggerSubEmittersAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[mainParticleNums];
        if (subEmitterPrefab != null)
        {
            int numParticlesAlive = ps.GetParticles(particles);
            for (int i = 0; i < numParticlesAlive; i++)
            {
                Debug.Log(particles[i].position);
                // 实例化预制体，并设置位置
                GameObject instance = Instantiate(subEmitterPrefab, particles[i].position, Quaternion.identity);
                ParticleSystem subPs = instance.GetComponent<ParticleSystem>();
                if (subPs != null)
                {
                    // 发射粒子
                    var emitParams = new ParticleSystem.EmitParams();
                    //emitParams.position = transform.position;
                    subPs.Emit(emitParams, 8); // 这里的8是要发射的粒子数量，根据需要调整
                }
                Destroy(instance, 6f);
            }

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
            float atk = GetComponentInParent<EnemyAttribute>().ATK;
            player.GetComponent<PlayerAttribute>().ChangeHP(-atk * 3);
        }

        // 应用更改回粒子系统
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
    }


}
