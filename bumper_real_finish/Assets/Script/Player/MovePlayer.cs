using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovePlayer : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 100.0f;
    public GameObject bulletPrefab; // 총알 프리팹
    public Transform bulletSpawn; // 총알이 생성될 위치
    public float bulletSpeed = 20f; // 총알의 속도
    private int hitCount = 0; // 충돌 횟수를 계산
    private const int MaxHits = 10; // 허용되는 최대 충돌 횟수
    private NavMeshAgent agent; // NavMeshAgent 참조
    private bool canMove = true; //총 맞으면 잠

    void Start()
    {
        // 오브젝트에 NavMeshAgent 컴포넌트가 있다고 가정하고 참조를 가져옵니다.
        agent = GetComponent<NavMeshAgent>();
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
        if (collision.gameObject.tag == "EnemyBullet")
        {
            hitCount++;
            if (hitCount >= MaxHits)
            {
                Destroy(gameObject);
            }
            else
            {
                if (agent != null)
                {
                    // NavMeshAgent의 이동을 1초 동안 중지합니다.
                    agent.isStopped = true;
                    // 1초 후에 NavMeshAgent의 이동을 다시 활성화합니다.
                    StartCoroutine(ResumeAfterDelay(1f));
                }
            }
        }
    }

    IEnumerator ResumeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // 1초 후 실행될 코드
        if (agent != null)
        {
            // NavMeshAgent의 이동을 다시 활성화합니다.
            agent.isStopped = false;
        }
    }
}
