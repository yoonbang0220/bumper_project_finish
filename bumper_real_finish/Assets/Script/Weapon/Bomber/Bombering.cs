using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombering : MonoBehaviour
{
    public GameObject[] spheres; // 활성화할 Sphere 오브젝트들
    public float initialDelay = 30f; // 초기 지연 시간
    public float activationInterval = 0.1f; // 활성화 간격
    public float activeDuration = 5f; // 활성화 지속 시간
    public float growDuration = 1f; // 크기 증가에 걸리는 시간

    void Start()
    {
        // 모든 Sphere 오브젝트를 처음에 비활성화
        foreach (var sphere in spheres)
        {
            sphere.SetActive(false);
        }

        // 초기 지연 후 랜덤 활성화 시작
        StartCoroutine(ActivateSpheresAfterDelay(initialDelay));
    }

    IEnumerator ActivateSpheresAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 활성화 시간을 추적하기 위한 타이머 시작
        float timer = 0f;

        // 활성화 및 비활성화 루프
        while (timer < activeDuration)
        {
            GameObject sphereToActivate = spheres[Random.Range(0, spheres.Length)];

            // Sphere 활성화 및 크기 조정 시작
            StartCoroutine(GrowSphere(sphereToActivate));

            // 잠시 기다린 후 비활성화
            yield return new WaitForSeconds(activationInterval);
            sphereToActivate.SetActive(false);
            sphereToActivate.transform.localScale = Vector3.one; // 크기 초기화

            // 타이머 업데이트
            timer += activationInterval;
        }
    }

    IEnumerator GrowSphere(GameObject sphere)
    {
        // Sphere와 Light 활성화
        sphere.SetActive(true);
        Light sphereLight = sphere.GetComponent<Light>();
        if (sphereLight != null)
        {
            sphereLight.enabled = true;
        }

        float currentTime = 0f;
        while (currentTime < growDuration)
        {
            float scale = Mathf.Lerp(1f, 10f, currentTime / growDuration);
            sphere.transform.localScale = new Vector3(scale, scale, scale);
            currentTime += Time.deltaTime;
            yield return null;
        }

        sphere.transform.localScale = new Vector3(10f, 10f, 10f); // 최종 크기로 설정

        // Sphere 비활성화 (Light도 함께 비활성화됩니다)
        sphere.SetActive(false);
        if (sphereLight != null)
        {
            sphereLight.enabled = false;
        }
    }
}
