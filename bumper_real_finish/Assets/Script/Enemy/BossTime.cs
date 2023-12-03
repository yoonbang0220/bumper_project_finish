using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTime : MonoBehaviour
{
    public GameObject objectToActivate; // Ȱ��ȭ�� ������Ʈ
    public float activationDelay = 60f; // Ȱ��ȭ������ ���� �ð�

    void Start()
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(false); // ���� �� ������Ʈ�� ��Ȱ��ȭ
            StartCoroutine(ActivateObjectAfterDelay(activationDelay)); // ������ ���� �ð� �Ŀ� ������Ʈ�� Ȱ��ȭ
        }
    }

    IEnumerator ActivateObjectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // ������ �ð���ŭ ���
        objectToActivate.SetActive(true); // ������Ʈ�� Ȱ��ȭ
    }
}
