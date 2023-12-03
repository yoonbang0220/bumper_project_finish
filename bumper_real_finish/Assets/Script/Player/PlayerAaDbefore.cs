using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAaDbefore : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 100.0f;
    public GameObject bulletPrefab; // 총알 프리팹
    public Transform bulletSpawn; // 총알이 생성될 위치
    public float bulletSpeed = 20f; // 총알의 속도
    public float forceAmount = 2f; // 가해진 충격
    private Rigidbody rb;
    private int hitCount = 0; // 충돌 횟수를 계산
    private const int MaxHits = 10; // 허용되는 최대 충돌 횟수
    private NavMeshAgent agent; // NavMeshAgent 참조


    void Start()
    {
        // 오브젝트에 NavMeshAgent 컴포넌트가 있다고 가정하고 참조를 가져옵니다.
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // 오브젝트 이동
        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        // 이동시에만 오브젝트 회전
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        // 마우스 버튼(일반적으로 왼쪽 버튼) 클릭 시 총알 발사
        if (Input.GetButtonDown("Fire1"))
        {
            ShootBullet();
        }
    }

    void ShootBullet()
    {
        // 총알 인스턴스 생성
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        // 총알에 속도 부여
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = bulletSpawn.forward * bulletSpeed;
        }

        // 총알을 몇 초 후에 자동으로 파괴(옵션)
        Destroy(bullet, 2f); // 2초 후에 총알 인스턴스 파괴
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            hitCount++; // Increase the collision count
            Debug.Log("Hit count: " + hitCount);
            if (hitCount >= MaxHits) // Check if maximum collisions are reached
            {
                Destroy(gameObject); // Destroy the object
            }
            else
            {
                // Get the direction from which the hit was received
                Vector3 hitDirection = collision.contacts[0].point - transform.position;
                hitDirection = -hitDirection.normalized;

                // Apply the impulse force
                rb.AddForce(hitDirection * forceAmount, ForceMode.Impulse);

                // Disable further physics interactions for 1 second
                rb.isKinematic = true;

                // After 1 second, reset the velocity and resume physics simulation
                StartCoroutine(ResetVelocityAfterDelay(1f));
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
}
