using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPosition : MonoBehaviour
{
    // GameObject 리스트를 선언합니다.
    public List<GameObject> gameObjects;

    void Start()
{
    // element0라는 이름의 새 GameObject를 생성하고 리스트에 추가합니다.
    GameObject element0 = new GameObject("element0");
    gameObjects.Add(element0);

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
