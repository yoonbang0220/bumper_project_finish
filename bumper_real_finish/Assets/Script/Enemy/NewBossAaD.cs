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
    private int hitCount = 0; // �浹 Ƚ�� ���� ����
    public Slider hpSlider; // HP �ٷ� ����� Slider ������Ʈ
    public Canvas canvas; // HP Canvas
    public Image controlledImage; // �ν����Ϳ��� �Ҵ��� Image ������Ʈ
    public float speed = 5f; // �ӵ�

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        agent.speed = speed;

        // Slider �ʱ� ����
        hpSlider.maxValue = 100; // HP ���� �ִ� ���� ����
        hpSlider.value = hpSlider.maxValue; // �ʱ� HP ����

        // Image Ȱ��ȭ
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
            hitCount++; // �浹 Ƚ�� ����

            // HP �� ����
            hpSlider.value -= 2; // HP ���� (�� �浹�� 1�� ����)

            if (hitCount >= 50) // �ִ� �浹 Ƚ�� ���� ��
            {
                Destroy(gameObject); // ������Ʈ �ı�
                controlledImage.enabled = false; // Image ��Ȱ��ȭ
            }
            else
            {
                agent.isStopped = true; // NavMeshAgent �Ͻ� ����

                Vector3 hitDirection = collision.contacts[0].point - transform.position;
                hitDirection = -hitDirection.normalized;

                rb.AddForce(hitDirection * forceAmount, ForceMode.Impulse);

                StartCoroutine(EnableAgentAfterDelay(1f)); // 1�� �� NavMeshAgent Ȱ��ȭ
            }

            // ���ڿ� �浹�� ���� ����
            if (collision.gameObject.tag == "Mine")
            {
                Destroy(collision.gameObject);
            }
        }
    }

    IEnumerator EnableAgentAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 1�� ���

        rb.velocity = Vector3.zero; // Rigidbody �ӵ� 0���� ����
        agent.isStopped = false; // NavMeshAgent Ȱ��ȭ
    }
}
