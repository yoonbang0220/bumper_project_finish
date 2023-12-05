using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShoot : MonoBehaviour
{
    public Transform target; // Ÿ���� Transform.
    public GameObject bulletPrefab; // �߻��� �Ѿ� ������.
    public Transform[] bulletSpawnPoints; // �߻� �������� ������ �迭.
    public float fireRate = 5f; // �߻� ����(��).
    public float bulletSpeed = 1000f; // �Ѿ��� �ӵ�.

    private float nextFireTime; // ���� �߻� �ð�.

    void Start()
    {
        nextFireTime = Time.time + fireRate; // ���� �߻� �ð� �ʱ�ȭ.
    }

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            FireBullet();
            nextFireTime = Time.time + fireRate; // ���� �߻� �ð� ����.
        }
    }

    void FireBullet()
    {
        if (bulletPrefab && bulletSpawnPoints.Length > 0 && target)
        {
            foreach (Transform spawnPoint in bulletSpawnPoints)
            {
                // �� �߻� �������� �Ѿ� �������� �����մϴ�.
                GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);

                // �Ѿ��� �÷��̾��� ������ �ٶ󺸵��� ����
                bullet.transform.rotation = transform.rotation;

                // �Ѿ��� Rigidbody ������Ʈ�� ������ ���� ���մϴ�.
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                if (rb)
                {
                    // �Ѿ��� Ÿ�� �������� �߻��մϴ�.
                    Vector3 direction = (target.position - spawnPoint.position).normalized;
                    rb.AddForce(direction * bulletSpeed);
                }

                // �Ѿ��� ���� �ð� �Ŀ� ��������� �����մϴ�.
                Destroy(bullet, 5f); // ���� ��� 5�� �Ŀ� �Ѿ��� �ı��մϴ�.
            }
        }
    }
}
