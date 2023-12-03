using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public Transform target; // Ÿ���� Transform.
    public GameObject enemyBulletPrefab; // ���� �Ѿ� ������.
    public Transform[] bulletSpawnPoints; // �Ѿ��� �߻�� ��ġ���� �迭.
    public float fireRate = 5f; // �߻� ����(��).
    public float bulletSpeed = 1000f; // �Ѿ��� �ӵ�.
    public AudioClip shootingSound; // �� �߻� �Ҹ� Ŭ��.

    private AudioSource audioSource; // AudioSource ������Ʈ.
    private float nextFireTime; // ���� �߻� �ð�.

    void Start()
    {
        nextFireTime = Time.time + fireRate; // ���� �߻� �ð� �ʱ�ȭ.
        audioSource = GetComponent<AudioSource>(); // AudioSource ������Ʈ ����.
    }

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            FireBullets(); // ��� bulletSpawnPoints���� �Ѿ� �߻�.
            nextFireTime = Time.time + fireRate; // ���� �߻� �ð� ����.
        }
    }

    void FireBullets()
    {
        foreach (Transform spawnPoint in bulletSpawnPoints) // �� �߻� ��ġ�� ���� �ݺ�.
        {
            if (enemyBulletPrefab && spawnPoint && target)
            {
                // �Ѿ� �������� �߻� ��ġ���� �����մϴ�.
                GameObject bullet = Instantiate(enemyBulletPrefab, spawnPoint.position, spawnPoint.rotation);

                // �Ѿ��� �÷��̾��� ������ �ٶ󺸵��� ����
                bullet.transform.rotation = transform.rotation;

                // �� �߻� �Ҹ� ���
                if (audioSource && shootingSound)
                {
                    audioSource.PlayOneShot(shootingSound);
                }

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
