using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShoot : MonoBehaviour
{
    public Transform target; // 타겟의 Transform.
    public GameObject bulletPrefab; // 발사할 총알 프리팹.
    public Transform[] bulletSpawnPoints; // 발사 지점들을 저장할 배열.
    public float fireRate = 5f; // 발사 간격(초).
    public float bulletSpeed = 1000f; // 총알의 속도.

    private float nextFireTime; // 다음 발사 시간.

    void Start()
    {
        nextFireTime = Time.time + fireRate; // 최초 발사 시간 초기화.
    }

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            FireBullet();
            nextFireTime = Time.time + fireRate; // 다음 발사 시간 설정.
        }
    }

    void FireBullet()
    {
        if (bulletPrefab && bulletSpawnPoints.Length > 0 && target)
        {
            foreach (Transform spawnPoint in bulletSpawnPoints)
            {
                // 각 발사 지점에서 총알 프리팹을 생성합니다.
                GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);

                // 총알이 플레이어의 방향을 바라보도록 설정
                bullet.transform.rotation = transform.rotation;

                // 총알의 Rigidbody 컴포넌트를 가져와 힘을 가합니다.
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                if (rb)
                {
                    // 총알을 타겟 방향으로 발사합니다.
                    Vector3 direction = (target.position - spawnPoint.position).normalized;
                    rb.AddForce(direction * bulletSpeed);
                }

                // 총알이 일정 시간 후에 사라지도록 설정합니다.
                Destroy(bullet, 5f); // 예를 들어 5초 후에 총알을 파괴합니다.
            }
        }
    }
}
