using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BomberTime : MonoBehaviour
{
    public TextMeshProUGUI countdownText; // ī��Ʈ�ٿ��� ǥ���� TextMeshProUGUI
    public int startTime = 30; // ī��Ʈ�ٿ� ���� �ð�

    void Start()
    {
        if (countdownText != null)
        {
            StartCoroutine(StartCountdown(startTime));
        }
    }

    IEnumerator StartCountdown(int time)
    {
        int count = time;

        while (count > 0)
        {
            countdownText.text = count.ToString(); // TextMeshProUGUI �ؽ�Ʈ ������Ʈ
            yield return new WaitForSeconds(1f); // 1�� ���
            count--; // ī��Ʈ�ٿ� ����
        }

        // ī��Ʈ�ٿ��� ������ "Bomber" ǥ��
        countdownText.text = "Raid";
        yield return new WaitForSeconds(5f); // "Bomber"�� 5�� ���� ǥ��

        // ���⼭ "Bomber" ǥ�� ���� �߰� ������ ������ �� �ֽ��ϴ�.
        countdownText.text = ""; // �ؽ�Ʈ�� ���
    }
}
