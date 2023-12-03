using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    public Button quitButton; // 에디터에서 할당할 버튼 참조

    void Start()
    {
        quitButton.onClick.AddListener(QuitPlayGame); // 버튼에 리스너 추가
    }

    void QuitPlayGame()
    {
        Application.Quit(); // 게임 종료
    }
}
