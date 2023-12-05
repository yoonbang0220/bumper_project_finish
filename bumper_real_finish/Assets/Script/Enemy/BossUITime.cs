using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossUITime : MonoBehaviour
{
    public Image imageToActivate; // Ȱ��ȭ�� �̹���
    public float activationDelay = 60f; // Ȱ��ȭ������ ���� �ð�

    void Start()
    {
        if (imageToActivate != null)
        {
            imageToActivate.gameObject.SetActive(false); // ���� �� �̹����� ��Ȱ��ȭ
            StartCoroutine(ActivateImageAfterDelay(activationDelay)); // ������ ���� �ð� �Ŀ� �̹����� Ȱ��ȭ
        }
    }

    IEnumerator ActivateImageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // ������ �ð���ŭ ���
        imageToActivate.gameObject.SetActive(true); // �̹����� Ȱ��ȭ
    }
}
