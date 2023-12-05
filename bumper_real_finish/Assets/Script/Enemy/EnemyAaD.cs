using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyAaD : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    public float forceAmount = 2f;
    private Rigidbody rb;
    private int hitCount = 0; // 충돌 횟수를 추적하는 변수입니다.
    public Image[] hitImages; //HP
    public Canvas canvas; //HP Canvas

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

        // 캔버스가 항상 Y축으로 0도 회전되도록 유지
        if (canvas != null)
        {
            canvas.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 총을 맞거나 플레이어와 충돌하면 HP 감소
        if (collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Play")
        {
            hitCount++; // 충돌 횟수를 증가시킵니다.

            if (hitCount - 1 < hitImages.Length) // 인덱스 범위 확인
            {
                hitImages[hitImages.Length - hitCount].enabled = false; // 해당 HP 이미지를 숨깁니다.
            }

            if (hitCount >= 5) // 5번 이상 충돌했는지 확인합니다.
            {
                Destroy(gameObject); // 오브젝트를 파괴합니다.
            }
            else
            {
                agent.isStopped = true; // NavMeshAgent를 일시 중지합니다.

                Vector3 hitDirection = collision.contacts[0].point - transform.position;
                hitDirection = -hitDirection.normalized;

                rb.AddForce(hitDirection * forceAmount, ForceMode.Impulse);

                // 힘이 가해진 후 1초가 지나면 NavMeshAgent를 다시 활성화합니다.
                StartCoroutine(EnableAgentAfterDelay(1f));
            }
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
