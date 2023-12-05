using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountBossTime : MonoBehaviour
{
    public TextMeshProUGUI countdownText; // ī��Ʈ�ٿ��� ǥ���� TextMeshProUGUI
    public int startTime = 60; // ī��Ʈ�ٿ� ���� �ð�

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

        countdownText.text = "0"; // ���������� 0 ǥ��

        // Ÿ�̸Ӱ� 0�� �������� ���� �߰� ����
        countdownText.gameObject.SetActive(false); // TextMeshProUGUI ��Ȱ��ȭ
                                                   // �ʿ��� ��� ���⼭ �ٸ� ������ �߰��� �� �ֽ��ϴ�.
    }
}
