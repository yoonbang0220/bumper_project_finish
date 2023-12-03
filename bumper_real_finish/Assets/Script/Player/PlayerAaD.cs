using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PlayerAaD : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 100.0f;
    public GameObject bulletPrefab; // �Ѿ� ������
    public Transform bulletSpawn; // �Ѿ��� ������ ��ġ
    public float bulletSpeed = 20f; // �Ѿ��� �ӵ�
    public float forceAmount = 2f; // ������ ���
    public Image[] hitImages; // HP
    private Rigidbody rb;
    private int hitCount = 0; // �浹 Ƚ���� ���
    private const int MaxHits = 10; // ���Ǵ� �ִ� �浹 Ƚ��
    private NavMeshAgent agent; // NavMeshAgent ����
    private bool canBoost = true; //�����̽��� ��Ÿ��
    private bool isBoosting = false;


    void Start()
    {
        // ������Ʈ�� NavMeshAgent ������Ʈ�� �ִٰ� �����ϰ� ������ �����ɴϴ�.
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // ������Ʈ �̵�
        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        // �̵��ÿ��� ������Ʈ ȸ��
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        // ���콺 ��ư(�Ϲ������� ���� ��ư) Ŭ�� �� �Ѿ� �߻�
        if (Input.GetButtonDown("Fire1"))
        {
            ShootBullet();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(BoostSpeed());
        }

        // �����̽��� �Է� ���� �� canBoost üũ
        if (Input.GetKeyDown(KeyCode.Space) && canBoost)
        {
            StartCoroutine(BoostSpeed());
        }
    }

    void ShootBullet()
    {
        // �Ѿ� �ν��Ͻ� ����
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        // �Ѿ˿� �ӵ� �ο�
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = bulletSpawn.forward * bulletSpeed;
        }

        // �Ѿ��� �� �� �Ŀ� �ڵ����� �ı�(�ɼ�)
        Destroy(bullet, 2f); // 2�� �Ŀ� �Ѿ� �ν��Ͻ� �ı�
    }

    // �����̽��ٸ� Ŭ���ϸ� �ӵ� �ν�Ʈ
    IEnumerator BoostSpeed()
    {
        canBoost = false; // ���� �ν�Ʈ�� ����
        isBoosting = true; // Boost ���� ����
        speed = 50f; // �ӵ� ����

        yield return new WaitForSeconds(1f); // 1�� ��ٸ�

        speed = 10f; // �ӵ� ����
        isBoosting = false; // Boost ���� ����

        yield return new WaitForSeconds(4f); // �߰� 4�� ��ٸ�

        canBoost = true; // �ν�Ʈ ����
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "EnemyBullet")
        {
            hitCount++; // Increase the collision count
            if (hitCount - 1 < hitImages.Length)
            {
                hitImages[hitCount - 1].enabled = false; // HP discount
            }
            if (hitCount >= MaxHits) // Check if maximum collisions are reached
            {
                Destroy(gameObject); // Destroy the object
            }
            else
            {
                Vector3 hitDirection = collision.contacts[0].point - transform.position;
                hitDirection = -hitDirection.normalized;

                // Start the coroutine to apply force over time
                StartCoroutine(ApplyForceOverTime(hitDirection, 5f)); // Apply force over 5 seconds

                // Disable further physics interactions for 1 second
                rb.isKinematic = true;

                rb.AddForce(hitDirection * forceAmount, ForceMode.Impulse);

                // After 1 second, reset the velocity and resume physics simulation
                StartCoroutine(ResetVelocityAfterDelay(1f));
            }
        }

        // �ν�Ʈ �� �浹�ϸ� �� ����
        if (isBoosting && collision.gameObject.tag == "Enemy")
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            if (enemyRb != null)
            {
                Vector3 forceDirection = collision.contacts[0].point - transform.position;
                forceDirection = -forceDirection.normalized;
                enemyRb.AddForce(forceDirection * 2f, ForceMode.Impulse);
            }
        }
    }

    IEnumerator ResetVelocityAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the delay

        // After the delay, reset the velocity and re-enable physics interactions
        rb.isKinematic = false; // Re-enable physics simulation
        rb.velocity = Vector3.zero; // Reset the velocity
    }

    IEnumerator ApplyForceOverTime(Vector3 forceDirection, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            rb.AddForce(forceDirection * (forceAmount / duration) * Time.deltaTime, ForceMode.Impulse);
            time += Time.deltaTime;
            yield return null; // Wait until the next frame
        }
    }
}