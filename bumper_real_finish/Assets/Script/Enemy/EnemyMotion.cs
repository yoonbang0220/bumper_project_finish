using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMotion : MonoBehaviour
{
    public float forceAmount = 10f; // 밀려나는 힘의 크기를 조절합니다.

    void OnCollisionEnter(Collision collision)
    {
        // 총알과의 충돌을 감지합니다.
        if (collision.gameObject.tag == "Bullet") // 'Bullet'은 총알의 태그입니다.
        {
            Vector3 hitDirection = collision.contacts[0].point - transform.position; // 충돌 지점과 적의 위치 차이를 계산합니다.
            hitDirection = -hitDirection.normalized; // 반대 방향으로 정규화합니다.

            GetComponent<Rigidbody>().AddForce(hitDirection * forceAmount, ForceMode.Impulse); // 힘을 추가합니다.
        }
    }
}
