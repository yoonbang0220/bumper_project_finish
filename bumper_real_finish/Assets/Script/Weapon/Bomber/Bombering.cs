using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombering : MonoBehaviour
{
    public GameObject[] spheres; // Ȱ��ȭ�� Sphere ������Ʈ��
    public float initialDelay = 30f; // �ʱ� ���� �ð�
    public float activationInterval = 0.1f; // Ȱ��ȭ ����
    public float activeDuration = 5f; // Ȱ��ȭ ���� �ð�
    public float growDuration = 1f; // ũ�� ������ �ɸ��� �ð�

    void Start()
    {
        // ��� Sphere ������Ʈ�� ó���� ��Ȱ��ȭ
        foreach (var sphere in spheres)
        {
            sphere.SetActive(false);
        }

        // �ʱ� ���� �� ���� Ȱ��ȭ ����
        StartCoroutine(ActivateSpheresAfterDelay(initialDelay));
    }

    IEnumerator ActivateSpheresAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Ȱ��ȭ �ð��� �����ϱ� ���� Ÿ�̸� ����
        float timer = 0f;

        // Ȱ��ȭ �� ��Ȱ��ȭ ����
        while (timer < activeDuration)
        {
            GameObject sphereToActivate = spheres[Random.Range(0, spheres.Length)];

            // Sphere Ȱ��ȭ �� ũ�� ���� ����
            StartCoroutine(GrowSphere(sphereToActivate));

            // ��� ��ٸ� �� ��Ȱ��ȭ
            yield return new WaitForSeconds(activationInterval);
            sphereToActivate.SetActive(false);
            sphereToActivate.transform.localScale = Vector3.one; // ũ�� �ʱ�ȭ

            // Ÿ�̸� ������Ʈ
            timer += activationInterval;
        }
    }

    IEnumerator GrowSphere(GameObject sphere)
    {
        // Sphere�� Light Ȱ��ȭ
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

        sphere.transform.localScale = new Vector3(10f, 10f, 10f); // ���� ũ��� ����

        // Sphere ��Ȱ��ȭ (Light�� �Բ� ��Ȱ��ȭ�˴ϴ�)
        sphere.SetActive(false);
        if (sphereLight != null)
        {
            sphereLight.enabled = false;
        }
    }
}
