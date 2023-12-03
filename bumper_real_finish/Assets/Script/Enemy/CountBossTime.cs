using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountBossTime : MonoBehaviour
{
    public TextMeshProUGUI countdownText; // 카운트다운을 표시할 TextMeshProUGUI
    public int startTime = 60; // 카운트다운 시작 시간

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
            countdownText.text = count.ToString(); // TextMeshProUGUI 텍스트 업데이트
            yield return new WaitForSeconds(1f); // 1초 대기
            count--; // 카운트다운 감소
        }

        countdownText.text = "0"; // 마지막으로 0 표시

        // 타이머가 0에 도달했을 때의 추가 동작
        countdownText.gameObject.SetActive(false); // TextMeshProUGUI 비활성화
                                                   // 필요한 경우 여기서 다른 동작을 추가할 수 있습니다.
    }
}
