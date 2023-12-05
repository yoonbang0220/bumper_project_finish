using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public GameObject bulletPrefab; // 총알 프리팹
    public Transform bulletSpawn; // 총알이 생성될 위치
    public float bulletSpeed = 20f; // 총알의 속도
    public float forceAmount = 2f; // 가해진 충격
    private Rigidbody rb;


    // Update is called once per frame
    void Update()
    {
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
