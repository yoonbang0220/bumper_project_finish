using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class NewBossAaD : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    public float forceAmount = 2f;
    private Rigidbody rb;
    private int hitCount = 0; // 충돌 횟수 추적 변수
    public Slider hpSlider; // HP 바로 사용할 Slider 컴포넌트
    public Canvas canvas; // HP Canvas
    public Image controlledImage; // 인스펙터에서 할당할 Image 컴포넌트
    public float speed = 5f; // 속도

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        agent.speed = speed;

        // Slider 초기 설정
        hpSlider.maxValue = 100; // HP 바의 최대 길이 설정
        hpSlider.value = hpSlider.maxValue; // 초기 HP 설정

        // Image 활성화
        controlledImage.enabled = true;
    }

    void Update()
    {
        if (target != null && !agent.isStopped)
        {
            agent.SetDestination(target.position);
        }

        if (canvas != null)
        {
            canvas.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Mine")
        {
            hitCount++; // 충돌 횟수 증가

            // HP 바 갱신
            hpSlider.value -= 2; // HP 감소 (각 충돌당 1씩 감소)

            if (hitCount >= 50) // 최대 충돌 횟수 도달 시
            {
                Destroy(gameObject); // 오브젝트 파괴
                controlledImage.enabled = false; // Image 비활성화
            }
            else
            {
                agent.isStopped = true; // NavMeshAgent 일시 중지

                Vector3 hitDirection = collision.contacts[0].point - transform.position;
                hitDirection = -hitDirection.normalized;

                rb.AddForce(hitDirection * forceAmount, ForceMode.Impulse);

                StartCoroutine(EnableAgentAfterDelay(1f)); // 1초 후 NavMeshAgent 활성화
            }

            // 지뢰와 충돌시 지뢰 제거
            if (collision.gameObject.tag == "Mine")
            {
                Destroy(collision.gameObject);
            }
        }
    }

    IEnumerator EnableAgentAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 1초 대기

        rb.velocity = Vector3.zero; // Rigidbody 속도 0으로 설정
        agent.isStopped = false; // NavMeshAgent 활성화
    }
}
