using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 네임스페이스 추가
using UnityEngine.SceneManagement; // SceneManagement 네임스페이스 추가

public class RestartGame : MonoBehaviour
{
    public Button restartButton; // 에디터에서 할당할 버튼 참조
    public GameObject[] HealItems; // 힐템 배열
    public int NumberOfObjectsToActivate = 5; // 활성화할 오브젝트의 수

    void Start()
    {
        restartButton.onClick.AddListener(RePlayGame); // 버튼에 리스너 추가
        ActivateRandomObjects(); // 힐템 랜덤배치
    }

    void ActivateRandomObjects()
    {
        // 모든 게임 오브젝트를 비활성화
        foreach (var obj in HealItems)
        {
            obj.SetActive(false);
        }

        // 랜덤으로 오브젝트를 활성화하기 위한 리스트 생성
        List<GameObject> objectsToActivate = new List<GameObject>(HealItems);

        // 5개의 오브젝트를 무작위로 활성화
        for (int i = 0; i < NumberOfObjectsToActivate; i++)
        {
            if (objectsToActivate.Count > 0)
            {
                int randomIndex = Random.Range(0, objectsToActivate.Count);
                objectsToActivate[randomIndex].SetActive(true);
                objectsToActivate.RemoveAt(randomIndex); // 중복 방지를 위해 리스트에서 제거
            }
        }
    }

    void RePlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 씬 재로드
    }
}
