using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRandom : MonoBehaviour
{
    public List<GameObject> gameObjects;

    void Start()
    {
        // element0��� �̸��� �� GameObject�� �����ϰ� ����Ʈ�� �߰��մϴ�.
        GameObject element0 = new GameObject("element0");
        gameObjects.Add(element0);

        // ��� GameObject�� ��Ȱ��ȭ�ϰ� element0�� Ȱ��ȭ�մϴ�.
        foreach (var go in gameObjects)
        {
            go.SetActive(false);
        }
        element0.SetActive(true);

        // Cube�� �ε����� ã���ϴ�.
        int cubeIndex = FindIndexOfCube();
        if (cubeIndex != -1)
        {
            Debug.Log("Cube�� �ε���: " + cubeIndex);
        }
        else
        {
            Debug.Log("Cube�� ã�� �� �����ϴ�.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // "Random" �±׸� ���� ������Ʈ�� �浹 �� ó��
        if (collision.gameObject.tag == "Random")
        {
            ActivateRandomGameObject();
        }
    }

    void ActivateRandomGameObject()
    {
        if (gameObjects.Count > 0)
        {
            // ��� GameObject�� ��Ȱ��ȭ�մϴ�.
            foreach (var go in gameObjects)
            {
                go.SetActive(false);
            }

            // �������� GameObject �ϳ��� �����Ͽ� Ȱ��ȭ�մϴ�.
            int randomIndex = Random.Range(0, gameObjects.Count);
            gameObjects[randomIndex].SetActive(true);
        }
    }

    // Cube�� �ε����� ã�� �Լ��Դϴ�.
    int FindIndexOfCube()
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (gameObjects[i].name == "Cube")
            {
                return i; // Cube�� �ε����� ��ȯ�մϴ�.
            }
        }
        return -1; // Cube�� ã�� ���� ��� -1�� ��ȯ�մϴ�.
    }
}
