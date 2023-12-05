using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAssault : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    public float forceAmount = 10f;
    private Rigidbody rb;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (target != null && !agent.isStopped)
        {
            agent.SetDestination(target.position);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            agent.isStopped = true; // NavMeshAgent를 일시 중지합니다.

            Vector3 hitDirection = collision.contacts[0].point - transform.position;
            hitDirection = -hitDirection.normalized;

            rb.AddForce(hitDirection * forceAmount, ForceMode.Impulse);

            // 힘이 가해진 후 1초가 지나면 NavMeshAgent를 다시 활성화합니다.
            StartCoroutine(EnableAgentAfterDelay(1f));
        }
    }

    IEnumerator EnableAgentAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 1초 동안 대기

        // 1초 후 실행될 코드
        rb.velocity = Vector3.zero; // Rigidbody의 속도를 0으로 설정하여 멈춥니다.
        agent.isStopped = false; // NavMeshAgent를 다시 활성화하여 목표 추적을 계속합니다.
    }
}
