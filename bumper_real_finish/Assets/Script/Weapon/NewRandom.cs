using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRandom : MonoBehaviour
{
    public List<GameObject> gameObjects;

    void Start()
    {
        // element0라는 이름의 새 GameObject를 생성하고 리스트에 추가합니다.
        GameObject element0 = new GameObject("element0");
        gameObjects.Add(element0);

        // 모든 GameObject를 비활성화하고 element0만 활성화합니다.
        foreach (var go in gameObjects)
        {
            go.SetActive(false);
        }
        element0.SetActive(true);

        // Cube의 인덱스를 찾습니다.
        int cubeIndex = FindIndexOfCube();
        if (cubeIndex != -1)
        {
            Debug.Log("Cube의 인덱스: " + cubeIndex);
        }
        else
        {
            Debug.Log("Cube를 찾을 수 없습니다.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // "Random" 태그를 가진 오브젝트와 충돌 시 처리
        if (collision.gameObject.tag == "Random")
        {
            ActivateRandomGameObject();
        }
    }

    void ActivateRandomGameObject()
    {
        if (gameObjects.Count > 0)
        {
            // 모든 GameObject를 비활성화합니다.
            foreach (var go in gameObjects)
            {
                go.SetActive(false);
            }

            // 무작위로 GameObject 하나를 선택하여 활성화합니다.
            int randomIndex = Random.Range(0, gameObjects.Count);
            gameObjects[randomIndex].SetActive(true);
        }
    }

    // Cube의 인덱스를 찾는 함수입니다.
    int FindIndexOfCube()
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (gameObjects[i].name == "Cube")
            {
                return i; // Cube의 인덱스를 반환합니다.
            }
        }
        return -1; // Cube를 찾지 못한 경우 -1을 반환합니다.
    }
}
