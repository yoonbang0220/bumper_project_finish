using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRandom : MonoBehaviour
{
    public GameObject[] transformObjects; // 변환될 게임 오브젝트 배열

    void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트가 플레이어인지 확인
        if (collision.gameObject.tag == "Play")
        {
            TransformPlayer(collision.gameObject);
        }
    }

    void TransformPlayer(GameObject player)
    {
        int index = Random.Range(0, transformObjects.Length); // 랜덤 인덱스 생성
        GameObject selectedObject = Instantiate(transformObjects[index], player.transform.position, player.transform.rotation); // 선택된 오브젝트 인스턴스화
    }
}
