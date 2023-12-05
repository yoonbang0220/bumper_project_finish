using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public string targetTag = "Play"; // 이 태그를 가진 오브젝트와 충돌 시 파괴됩니다.

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject); // 총알 자신을 파괴
    }
}
