using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaitingBoss : MonoBehaviour
{
    public TextMeshProUGUI textToDeactivate; // ��Ȱ��ȭ�� TextMeshProUGUI

    void Start()
    {
        if (textToDeactivate != null)
        {
            StartCoroutine(DeactivateAfterDelay(60f)); // 60�� �Ŀ� ��Ȱ��ȭ �ڷ�ƾ ����
        }
    }

    IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // ������ �ð���ŭ ���
        textToDeactivate.gameObject.SetActive(false); // TextMeshProUGUI ��Ȱ��ȭ
    }
}
