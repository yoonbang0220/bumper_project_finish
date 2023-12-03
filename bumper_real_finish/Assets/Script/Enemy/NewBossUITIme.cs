using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewBossUITIme : MonoBehaviour
{
    public TextMeshProUGUI textToActivate; // Ȱ��ȭ�� �ؽ�Ʈ
    public GameObject objectToActivate; // Ȱ��ȭ�� ���� ������Ʈ
    public float activationDelay = 60f; // Ȱ��ȭ������ ���� �ð�

    void Start()
    {
        if (textToActivate != null)
        {
            textToActivate.gameObject.SetActive(false); // ���� �� �ؽ�Ʈ�� ��Ȱ��ȭ
        }

        if (objectToActivate != null)
        {
            objectToActivate.SetActive(false); // ���� �� ���� ������Ʈ�� ��Ȱ��ȭ
        }

        StartCoroutine(ActivateAfterDelay(activationDelay)); // ������ ���� �ð� �Ŀ� �ؽ�Ʈ�� ���� ������Ʈ�� Ȱ��ȭ
    }

    IEnumerator ActivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // ������ �ð���ŭ ���

        if (textToActivate != null)
        {
            textToActivate.gameObject.SetActive(true); // �ؽ�Ʈ�� Ȱ��ȭ
        }

        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true); // ���� ������Ʈ�� Ȱ��ȭ
        }
    }
}
