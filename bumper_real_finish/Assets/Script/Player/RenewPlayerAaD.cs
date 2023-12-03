using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class RenewPlayerAaD : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 100.0f;
    public GameObject[] weaponPrefabs; // weaponPrefab �迭
    public Transform weaponPosition; // ���Ⱑ ������ ��ġ
    public GameObject bulletPrefab; // �Ѿ� ������
    public Transform bulletSpawn; // �Ѿ��� ������ ��ġ
    public float bulletSpeed = 20f; // �Ѿ��� �ӵ�
    public float forceAmount = 2f; // ������ ���
    public Slider hpBar; // HP Bar
    public Image gameOverImage; // ���� ���� �̹���
    public Image gameOverBack; // ���� ���� ����̹���
    public Button replayButton; // �ٽ� ���� ��ư
    public GameObject mine;
    public int mineCount = 0; // ������� ������ mine�� ��
    public int maxMineCount = 10; // �ִ� mine ���� Ƚ��
    public AudioClip shootingSound;
    public GameObject HillEffect; // HillEffect ���� ������Ʈ
    private Rigidbody rb;
    private GameObject currentWeapon; // ���� ������ ����
    private int hitCount = 0; // �浹 Ƚ���� ���
    public int MaxHits = 20; // ���Ǵ� �ִ� �浹 Ƚ��
    private NavMeshAgent agent; // NavMeshAgent ����
    private bool canBoost = true; //�����̽��� ��Ÿ��
    private bool isBoosting = false;
    private float lastGunShotTime = 0f; // ������ �Ѿ� �߻� �ð�
    private float lastShotGunShotTime = 0f; // ������ ShotGun �Ѿ� �߻� �ð�
    private float lastMachineGunShotTime = 0f; // ������ MachineGun �Ѿ� �߻� �ð�
    private const float gunCooldown = 0.2f; // Gun�� �Ѿ� �߻� ��Ÿ�� (1��)
    private const float shotGunCooldown = 1f; // ShotGun �Ѿ� �߻� ��Ÿ�� (0.5��)
    private const float machineGunCooldown = 2f; // MachineGun �Ѿ� �߻� ��Ÿ�� (0.5��)


    void Start()
    {
        //HP bar �ʱ�ȭ
        hpBar.maxValue = MaxHits; // Slider�� �ִ밪 ����
        hpBar.value = MaxHits; // �ʱ� HP ����
        gameOverImage.enabled = false; // ���� ���� �̹��� �ʱ�ȭ
        gameOverBack.enabled = false;
        replayButton.interactable = false; // ���÷��� ��ư �ʱ�ȭ
        SetReplayButtonVisibility(false); // ���� �� ���÷��� ��ư �����

        // ������Ʈ�� NavMeshAgent ������Ʈ�� �ִٰ� �����ϰ� ������ �����ɴϴ�.
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        // �⺻ ��
        InstantiateGun();
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
        if (Input.GetMouseButton(0) && (currentWeapon == null || currentWeapon.name != "Bumper"))
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

        // ���콺 ������ ��ư Ŭ�� ����
        if (Input.GetMouseButtonDown(1)) // ���콺 ������ ��ư�� 1��
        {
            CreateMineBehindPlayer();
        }
    }

    void InstantiateGun()
    {
        // 'Gun' ������ ã��
        GameObject gunPrefab = null;
        foreach (var prefab in weaponPrefabs)
        {
            if (prefab != null && prefab.name == "Gun")
            {
                gunPrefab = prefab;
                break;
            }
        }

        // Gun �������� �ִ� ��� �ش� ��ġ�� ����
        if (gunPrefab != null)
        {
            Instantiate(gunPrefab, weaponPosition.position, weaponPosition.rotation, weaponPosition);
        }

        // ���� ���� ����
        currentWeapon = gunPrefab;
    }

    void ShootBullet()
    {
        // Bumper�� �� �Ѿ��� �߻����� ����
        if (currentWeapon != null && currentWeapon.name == "Bumper")
        {
            return;
        }

        // �Ѿ� �ν��Ͻ� ����
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        // �Ѿ��� �÷��̾��� ������ �ٶ󺸵��� ����
        bullet.transform.rotation = transform.rotation;

        // ���� ���
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && shootingSound != null)
        {
            audioSource.PlayOneShot(shootingSound);
        }

        // �Ѿ˿� �ӵ� �ο�
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            float currentBulletSpeed = bulletSpeed;

            // ���� ������ �̸��� ���� �Ѿ� �ӵ� ����
            if (currentWeapon != null)
            {
                // Gun �Ѿ� �߻� ��Ÿ�� üũ
                if (currentWeapon != null && currentWeapon.name == "Gun")
                {
                    if (Time.time >= lastGunShotTime + gunCooldown)
                    {
                        currentBulletSpeed = 20f;
                        forceAmount = 2f;
                        lastGunShotTime = Time.time; // ������ �Ѿ� �߻� �ð� ������Ʈ
                    }
                    else
                    {
                        Destroy(bullet); // ��Ÿ�� ���� �Ѿ� �߻����� ����
                        return;
                    }
                }
                // ShotGun �Ѿ� �߻� ��Ÿ�� üũ
                else if (currentWeapon.name == "ShotGun")
                {
                    if (Time.time >= lastShotGunShotTime + shotGunCooldown)
                    {
                        currentBulletSpeed = 50f;
                        forceAmount = 20f;
                        lastShotGunShotTime = Time.time; // ������ ShotGun �Ѿ� �߻� �ð� ������Ʈ

                        // ShotGun�� ���, 5���� �Ѿ� �߻� ����...
                    }
                    else
                    {
                        Destroy(bullet); // ��Ÿ�� ���� �Ѿ� �߻����� ����
                        return;
                    }
                }
                // MachineGun �Ѿ� �߻� ��Ÿ�� üũ
                else if (currentWeapon.name == "MachineGun")
                {
                    if (Time.time >= lastMachineGunShotTime + machineGunCooldown)
                    {
                        currentBulletSpeed = 200f;
                        forceAmount = 50f;
                        lastMachineGunShotTime = Time.time; // ������ MachineGun �Ѿ� �߻� �ð� ������Ʈ
                    }
                    else
                    {
                        Destroy(bullet); // ��Ÿ�� ���� �Ѿ� �߻����� ����
                        return;
                    }
                }
            }

            rb.velocity = bulletSpawn.forward * currentBulletSpeed;
        }

        // �Ѿ��� �� �� �Ŀ� �ڵ����� �ı�(�ɼ�)
        Destroy(bullet, 2f); // 2�� �Ŀ� �Ѿ� �ν��Ͻ� �ı�
    }

    // �����̽��ٸ� Ŭ���ϸ� �ӵ� �ν�Ʈ
    IEnumerator BoostSpeed()
    {
        canBoost = false; // ���� �ν�Ʈ�� ����
        isBoosting = true; // Boost ���� ����
        speed = 60f; // �ӵ� ����

        yield return new WaitForSeconds(1f); // 1�� ��ٸ�

        speed = 10f; // �ӵ� ����
        isBoosting = false; // Boost ���� ����

        yield return new WaitForSeconds(4f); // �߰� 4�� ��ٸ�

        canBoost = true; // �ν�Ʈ ����
    }

    void SetReplayButtonVisibility(bool isActive)
    {
        replayButton.gameObject.SetActive(isActive); // ��ư�� GameObject Ȱ��ȭ/��Ȱ��ȭ
        replayButton.interactable = isActive; // ��ư�� ��ȣ�ۿ� ���� ���� ����
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "EnemyBullet" || collision.gameObject.tag == "Boss")
        {
            hitCount++; // Increase the collision count
            hpBar.value = MaxHits - hitCount; // HP ����

            if (hitCount >= MaxHits) // Check if maximum collisions are reached
            {
                Destroy(gameObject); // Destroy the object
                gameOverImage.enabled = true; // ���� ���� �̹��� Ȱ��ȭ
                gameOverBack.enabled = true;
                replayButton.interactable = true; // ���÷��� ��ư Ȱ��ȭ
                SetReplayButtonVisibility(true); // ���÷��� ��ư ���̰� ����
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

        if (collision.gameObject.tag == "HilItem")
        {
            // HP �� hitCount�� �ʱ�ȭ
            hpBar.maxValue = MaxHits; // Slider�� �ִ밪�� �ٽ� ����
            hpBar.value = MaxHits; // HP�� �ִ�ġ�� �缳��
            hitCount = 0; // hitCount�� 0���� �ʱ�ȭ
            Destroy(collision.gameObject); // HilItem �ı�

            // HillEffect Ȱ��ȭ
            ShowHillEffect();
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

        else if (collision.gameObject.tag == "Random")
        {
            ChangeWeapon();
            Destroy(collision.gameObject); // �ı�
        }
    }

    void ChangeWeapon()
    {
        if (weaponPrefabs.Length == 0) return; // weaponPrefab �迭�� ��������� �ƹ� �۾��� ���� ����

        int randomIndex = Random.Range(0, weaponPrefabs.Length); // ������ �ε��� ����
        GameObject selectedPrefab = weaponPrefabs[randomIndex]; // ���õ� prefab

        // ������ weaponPosition�� �ִ� ���⸦ �ı�
        foreach (Transform child in weaponPosition)
        {
            Destroy(child.gameObject);
        }

        // �������� ���õ� ���� ����
        GameObject newWeapon = Instantiate(selectedPrefab, weaponPosition.position, weaponPosition.rotation, weaponPosition);

        // ���� ���� ������Ʈ
        currentWeapon = newWeapon;
    }

    // ���� ��ġ
    void CreateMineBehindPlayer()
    {
        if (mineCount >= maxMineCount) // �ִ� mine ���� Ƚ���� ������ ���
        {
            return; // �� �̻� mine�� �������� ����
        }

        // �÷��̾� ���� 5 ���� ������ ��ġ ���
        Vector3 minePosition = transform.position - transform.forward * 5;
        minePosition.y = 0.5f; // y�� ��ġ ����

        // Mine �ν��Ͻ�ȭ
        Instantiate(mine, minePosition, Quaternion.identity);

        mineCount++; // mine ���� Ƚ�� ����
    }

    void ShowHillEffect()
    {
        if (HillEffect != null)
        {
            // HillEffect ���� ������Ʈ�� Ȱ��ȭ�ϰ�, �ʿ��� ��� �߰� ������ �����մϴ�.
            HillEffect.SetActive(true);

            // ��: ���� �ð� �Ŀ� ����Ʈ�� �ڵ����� ��Ȱ��ȭ�մϴ�.
            StartCoroutine(DisableEffectAfterTime(HillEffect, 2f)); // 2�� �Ŀ� ����Ʈ ��Ȱ��ȭ
        }
    }

    IEnumerator DisableEffectAfterTime(GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        effect.SetActive(false);
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
