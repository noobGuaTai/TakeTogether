using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Barrage2 : MonoBehaviour
{
    public ParticleSystem ps;
    public float speed = 5f;
    public int particlesToEmit = 10;
    public bool canEmission = false;

    private GameObject player;
    private Transform playerTransform;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
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

        if (canEmission == true)
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
        List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);

        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = inside[i];
            // 设置粒子的剩余生命时间为0立即消失
            p.remainingLifetime = 0;
            inside[i] = p;
            float atk = GameObject.FindGameObjectWithTag("Boss").GetComponent<EnemyAttribute>().ATK;
            player.GetComponent<PlayerAttribute>().ChangeHP(-atk);
        }

        // 应用更改回粒子系统
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
    }
}
