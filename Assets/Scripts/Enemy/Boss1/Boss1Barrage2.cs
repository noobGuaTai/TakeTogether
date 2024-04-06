using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Boss1Barrage2 : EnemyMove
{
    public ParticleSystem ps;
    public float speed = 5f;
    public int particlesToEmit = 10;
    public bool canEmission = false;
    public float cooldownTime = 1.0f; // 冷却时间为1秒
    private double lastHitTime = 0.0f; // 上次被击中的时间

    void Start()
    {
        Init();
        Emission();
    }

    public void Update()
    {
        Init();


        if (canEmission == true)
        {
            Emission();
            canEmission = false;
        }
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


    public void Emission()
    {
        if (closedPlayer != null)
        {
            Vector3 directionToPlayer = closedPlayer.transform.position - transform.position;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        EmitParticlesTowardsPlayer();

    }

    void EmitParticlesTowardsPlayer()
    {
        var emitParams = new ParticleSystem.EmitParams();
        ps.Emit(emitParams, particlesToEmit);

        StartCoroutine(RotateOverTime(6f));//旋转父物体

    }

    IEnumerator RotateOverTime(float duration)
    {
        float startRotation = transform.eulerAngles.z; // 获取当前Z轴旋转角度
        float endRotation = startRotation + 360f; // 最终旋转角度
        float currentTime = 0f;

        while (currentTime <= duration)
        {
            float t = currentTime / duration; // 计算当前时间占总时长的比例
            float angle = Mathf.Lerp(startRotation, endRotation, t); // 根据比例计算当前应旋转到的角度
            transform.rotation = Quaternion.Euler(0f, 0f, angle); // 应用旋转
            currentTime += Time.deltaTime; // 更新已过时间
            yield return null; // 等待下一帧
        }

        transform.rotation = Quaternion.Euler(0f, 0f, startRotation);
    }



    void OnParticleTrigger()
    {
        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = enter[i];
            // 设置粒子的剩余生命时间为0立即消失
            p.remainingLifetime = 0;
            enter[i] = p;
            // float atk = GameObject.FindGameObjectWithTag("Boss").GetComponent<EnemyAttribute>().ATK;
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
            float atk = GameObject.FindGameObjectWithTag("Boss1").GetComponent<EnemyAttribute>().ATK;
            other.GetComponent<PlayerAttribute>().ChangeHP(-atk * 3);
            lastHitTime = NetworkTime.time;
        }
    }
}
