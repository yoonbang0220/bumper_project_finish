using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class NewPlayerAaD : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 100.0f;
    public GameObject bulletPrefab; // 총알 프리팹
    public Transform bulletSpawn; // 총알이 생성될 위치
    public float bulletSpeed = 20f; // 총알의 속도
    public float forceAmount = 2f; // 가해진 충격
    public Slider hpBar; // HP Bar
    public Image gameOverImage; // 게임 오버 이미지
    public Button replayButton; // 다시 시작 버튼
    private Rigidbody rb;
    private int hitCount = 0; // 충돌 횟수를 계산
    private const int MaxHits = 10; // 허용되는 최대 충돌 횟수
    private NavMeshAgent agent; // NavMeshAgent 참조
    private bool canBoost = true; //스페이스바 쿨타임
    private bool isBoosting = false;


    void Start()
    {
        //HP bar 초기화
        hpBar.maxValue = MaxHits; // Slider의 최대값 설정
        hpBar.value = MaxHits; // 초기 HP 설정
        gameOverImage.enabled = false; // 게임 오버 이미지 초기화
        replayButton.interactable = false; // 리플레이 버튼 초기화
        SetReplayButtonVisibility(false); // 시작 시 리플레이 버튼 숨기기

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(BoostSpeed());
        }

        // 스페이스바 입력 감지 및 canBoost 체크
        if (Input.GetKeyDown(KeyCode.Space) && canBoost)
        {
            StartCoroutine(BoostSpeed());
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

    // 스페이스바를 클릭하면 속도 부스트
    IEnumerator BoostSpeed()
    {
        canBoost = false; // 다음 부스트를 방지
        isBoosting = true; // Boost 상태 시작
        speed = 50f; // 속도 증가

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
        if (collision.gameObject.tag == "EnemyBullet")
        {
            hitCount++; // Increase the collision count
            hpBar.value = MaxHits - hitCount; // HP 갱신

            if (hitCount >= MaxHits) // Check if maximum collisions are reached
            {
                Destroy(gameObject); // Destroy the object
                gameOverImage.enabled = true; // 게임 오버 이미지 활성화
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
