using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryChecker : MonoBehaviour
{
    public Image[] images; // 게임 내 모든 이미지 배열
    public Image victoryImage; // 승리 이미지
    public Image victoryBack; // 승리 배경이미지
    public Button replayButton; // 다시 시작 버튼

    void Start()
    {
        victoryImage.enabled = false; // 시작 시 승리 이미지 숨기기
        victoryBack.enabled = false;
        replayButton.gameObject.SetActive(false); // 시작 시 리플레이 버튼 숨기기
    }

    void Update()
    {
        CheckVictoryCondition();
    }

    void CheckVictoryCondition()
    {
        foreach (Image img in images)
        {
            if (img.enabled) // 활성화된 이미지가 하나라도 있다면
            {
                return; // 함수를 종료합니다
            }
        }

        // 모든 이미지가 비활성화되었다면
        victoryImage.enabled = true; // 승리 이미지 활성화
        victoryBack.enabled = true;
        replayButton.gameObject.SetActive(true); // 리플레이 버튼 활성화
    }
}
