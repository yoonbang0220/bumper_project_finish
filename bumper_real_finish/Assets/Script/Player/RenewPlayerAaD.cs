using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class RenewPlayerAaD : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 100.0f;
    public GameObject[] weaponPrefabs; // weaponPrefab 배열
    public Transform weaponPosition; // 무기가 생성될 위치
    public GameObject bulletPrefab; // 총알 프리팹
    public Transform bulletSpawn; // 총알이 생성될 위치
    public float bulletSpeed = 20f; // 총알의 속도
    public float forceAmount = 2f; // 가해진 충격
    public Slider hpBar; // HP Bar
    public Image gameOverImage; // 게임 오버 이미지
    public Image gameOverBack; // 게임 오버 배경이미지
    public Button replayButton; // 다시 시작 버튼
    public GameObject mine;
    public int mineCount = 0; // 현재까지 생성된 mine의 수
    public int maxMineCount = 10; // 최대 mine 생성 횟수
    public AudioClip shootingSound;
    public GameObject HillEffect; // HillEffect 게임 오브젝트
    private Rigidbody rb;
    private GameObject currentWeapon; // 현재 장착된 무기
    private int hitCount = 0; // 충돌 횟수를 계산
    public int MaxHits = 20; // 허용되는 최대 충돌 횟수
    private NavMeshAgent agent; // NavMeshAgent 참조
    private bool canBoost = true; //스페이스바 쿨타임
    private bool isBoosting = false;
    private float lastGunShotTime = 0f; // 마지막 총알 발사 시간
    private float lastShotGunShotTime = 0f; // 마지막 ShotGun 총알 발사 시간
    private float lastMachineGunShotTime = 0f; // 마지막 MachineGun 총알 발사 시간
    private const float gunCooldown = 0.2f; // Gun의 총알 발사 쿨타임 (1초)
    private const float shotGunCooldown = 1f; // ShotGun 총알 발사 쿨타임 (0.5초)
    private const float machineGunCooldown = 2f; // MachineGun 총알 발사 쿨타임 (0.5초)


    void Start()
    {
        //HP bar 초기화
        hpBar.maxValue = MaxHits; // Slider의 최대값 설정
        hpBar.value = MaxHits; // 초기 HP 설정
        gameOverImage.enabled = false; // 게임 오버 이미지 초기화
        gameOverBack.enabled = false;
        replayButton.interactable = false; // 리플레이 버튼 초기화
        SetReplayButtonVisibility(false); // 시작 시 리플레이 버튼 숨기기

        // 오브젝트에 NavMeshAgent 컴포넌트가 있다고 가정하고 참조를 가져옵니다.
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        // 기본 총
        InstantiateGun();
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
        if (Input.GetMouseButton(0) && (currentWeapon == null || currentWeapon.name != "Bumper"))
        {
            ShootBullet();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(BoostSpeed());
        }

        // 스페이스바 입력 감지 및 canBoost 체크
        if (Input.GetKeyDown(KeyCode.Space) && canBoost)
        {
            StartCoroutine(BoostSpeed());
        }

        // 마우스 오른쪽 버튼 클릭 감지
        if (Input.GetMouseButtonDown(1)) // 마우스 오른쪽 버튼은 1번
        {
            CreateMineBehindPlayer();
        }
    }

    void InstantiateGun()
    {
        // 'Gun' 프리팹 찾기
        GameObject gunPrefab = null;
        foreach (var prefab in weaponPrefabs)
        {
            if (prefab != null && prefab.name == "Gun")
            {
                gunPrefab = prefab;
                break;
            }
        }

        // Gun 프리팹이 있는 경우 해당 위치에 생성
        if (gunPrefab != null)
        {
            Instantiate(gunPrefab, weaponPosition.position, weaponPosition.rotation, weaponPosition);
        }

        // 현재 무기 설정
        currentWeapon = gunPrefab;
    }

    void ShootBullet()
    {
        // Bumper일 때 총알을 발사하지 않음
        if (currentWeapon != null && currentWeapon.name == "Bumper")
        {
            return;
        }

        // 총알 인스턴스 생성
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        // 총알이 플레이어의 방향을 바라보도록 설정
        bullet.transform.rotation = transform.rotation;

        // 사운드 재생
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && shootingSound != null)
        {
            audioSource.PlayOneShot(shootingSound);
        }

        // 총알에 속도 부여
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            float currentBulletSpeed = bulletSpeed;

            // 현재 무기의 이름에 따라 총알 속도 설정
            if (currentWeapon != null)
            {
                // Gun 총알 발사 쿨타임 체크
                if (currentWeapon != null && currentWeapon.name == "Gun")
                {
                    if (Time.time >= lastGunShotTime + gunCooldown)
                    {
                        currentBulletSpeed = 20f;
                        forceAmount = 2f;
                        lastGunShotTime = Time.time; // 마지막 총알 발사 시간 업데이트
                    }
                    else
                    {
                        Destroy(bullet); // 쿨타임 동안 총알 발사하지 않음
                        return;
                    }
                }
                // ShotGun 총알 발사 쿨타임 체크
                else if (currentWeapon.name == "ShotGun")
                {
                    if (Time.time >= lastShotGunShotTime + shotGunCooldown)
                    {
                        currentBulletSpeed = 50f;
                        forceAmount = 20f;
                        lastShotGunShotTime = Time.time; // 마지막 ShotGun 총알 발사 시간 업데이트

                        // ShotGun의 경우, 5개의 총알 발사 로직...
                    }
                    else
                    {
                        Destroy(bullet); // 쿨타임 동안 총알 발사하지 않음
                        return;
                    }
                }
                // MachineGun 총알 발사 쿨타임 체크
                else if (currentWeapon.name == "MachineGun")
                {
                    if (Time.time >= lastMachineGunShotTime + machineGunCooldown)
                    {
                        currentBulletSpeed = 200f;
                        forceAmount = 50f;
                        lastMachineGunShotTime = Time.time; // 마지막 MachineGun 총알 발사 시간 업데이트
                    }
                    else
                    {
                        Destroy(bullet); // 쿨타임 동안 총알 발사하지 않음
                        return;
                    }
                }
            }

            rb.velocity = bulletSpawn.forward * currentBulletSpeed;
        }

        // 총알을 몇 초 후에 자동으로 파괴(옵션)
        Destroy(bullet, 2f); // 2초 후에 총알 인스턴스 파괴
    }

    // 스페이스바를 클릭하면 속도 부스트
    IEnumerator BoostSpeed()
    {
        canBoost = false; // 다음 부스트를 방지
        isBoosting = true; // Boost 상태 시작
        speed = 60f; // 속도 증가

        yield return new WaitForSeconds(1f); // 1초 기다림

        speed = 10f; // 속도 감소
        isBoosting = false; // Boost 상태 종료

        yield return new WaitForSeconds(4f); // 추가 4초 기다림

        canBoost = true; // 부스트 가능
    }

    void SetReplayButtonVisibility(bool isActive)
    {
        replayButton.gameObject.SetActive(isActive); // 버튼의 GameObject 활성화/비활성화
        replayButton.interactable = isActive; // 버튼의 상호작용 가능 여부 설정
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "EnemyBullet" || collision.gameObject.tag == "Boss")
        {
            hitCount++; // Increase the collision count
            hpBar.value = MaxHits - hitCount; // HP 갱신

            if (hitCount >= MaxHits) // Check if maximum collisions are reached
            {
                Destroy(gameObject); // Destroy the object
                gameOverImage.enabled = true; // 게임 오버 이미지 활성화
                gameOverBack.enabled = true;
                replayButton.interactable = true; // 리플레이 버튼 활성화
                SetReplayButtonVisibility(true); // 리플레이 버튼 보이게 설정
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
            // HP 및 hitCount를 초기화
            hpBar.maxValue = MaxHits; // Slider의 최대값을 다시 설정
            hpBar.value = MaxHits; // HP를 최대치로 재설정
            hitCount = 0; // hitCount를 0으로 초기화
            Destroy(collision.gameObject); // HilItem 파괴

            // HillEffect 활성화
            ShowHillEffect();
        }

        // 부스트 중 충돌하면 힘 가함
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
            Destroy(collision.gameObject); // 파괴
        }
    }

    void ChangeWeapon()
    {
        if (weaponPrefabs.Length == 0) return; // weaponPrefab 배열이 비어있으면 아무 작업도 하지 않음

        int randomIndex = Random.Range(0, weaponPrefabs.Length); // 무작위 인덱스 선택
        GameObject selectedPrefab = weaponPrefabs[randomIndex]; // 선택된 prefab

        // 기존에 weaponPosition에 있는 무기를 파괴
        foreach (Transform child in weaponPosition)
        {
            Destroy(child.gameObject);
        }

        // 무작위로 선택된 무기 생성
        GameObject newWeapon = Instantiate(selectedPrefab, weaponPosition.position, weaponPosition.rotation, weaponPosition);

        // 현재 무기 업데이트
        currentWeapon = newWeapon;
    }

    // 지뢰 설치
    void CreateMineBehindPlayer()
    {
        if (mineCount >= maxMineCount) // 최대 mine 생성 횟수에 도달한 경우
        {
            return; // 더 이상 mine을 생성하지 않음
        }

        // 플레이어 뒤쪽 5 유닛 떨어진 위치 계산
        Vector3 minePosition = transform.position - transform.forward * 5;
        minePosition.y = 0.5f; // y축 위치 설정

        // Mine 인스턴스화
        Instantiate(mine, minePosition, Quaternion.identity);

        mineCount++; // mine 생성 횟수 증가
    }

    void ShowHillEffect()
    {
        if (HillEffect != null)
        {
            // HillEffect 게임 오브젝트를 활성화하고, 필요한 경우 추가 로직을 실행합니다.
            HillEffect.SetActive(true);

            // 예: 일정 시간 후에 이펙트를 자동으로 비활성화합니다.
            StartCoroutine(DisableEffectAfterTime(HillEffect, 2f)); // 2초 후에 이펙트 비활성화
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
