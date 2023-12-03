using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BomberTime : MonoBehaviour
{
    public TextMeshProUGUI countdownText; // 카운트다운을 표시할 TextMeshProUGUI
    public int startTime = 30; // 카운트다운 시작 시간

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

        // 카운트다운이 끝나면 "Bomber" 표시
        countdownText.text = "Raid";
        yield return new WaitForSeconds(5f); // "Bomber"를 5초 동안 표시

        // 여기서 "Bomber" 표시 후의 추가 동작을 구현할 수 있습니다.
        countdownText.text = ""; // 텍스트를 비움
    }
}
