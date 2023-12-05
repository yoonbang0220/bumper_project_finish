using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayer : MonoBehaviour
{
    // 랜덤박스와 충돌 후 플레이어 전환
    public GameObject aObject; // 건
    public GameObject bObject; // 샷건
    public GameObject cObject; // 머신건
    public GameObject dObject; // 범퍼

    void OnCollisionEnter(Collision collision)
    {
        // 태그가 "Gun"인 오브젝트와 충돌했는지 확인
        if (collision.gameObject.tag == "Gun")
        {
            SwitchToObject(aObject);
            Destroy(collision.gameObject); // 충돌한 "Gun" 태그 오브젝트 파괴
            Destroy(gameObject); // 전 오브젝트 파괴
        }
        // 태그가 "ShotGun"인 오브젝트와 충돌했는지 확인
        else if (collision.gameObject.tag == "ShotGun")
        {
            SwitchToObject(bObject);
            Destroy(collision.gameObject); // 충돌한 "ShotGun" 태그 오브젝트 파괴
            Destroy(gameObject);
        }
        // 태그가 "MachineGun"인 오브젝트와 충돌했는지 확인
        else if (collision.gameObject.tag == "MachineGun")
        {
            SwitchToObject(cObject);
            Destroy(collision.gameObject); // 충돌한 "MachineGun" 태그 오브젝트 파괴
            Destroy(gameObject);
        }
        // 태그가 "Bumper"인 오브젝트와 충돌했는지 확인
        else if (collision.gameObject.tag == "Bumper")
        {
            SwitchToObject(dObject);
            Destroy(collision.gameObject); // 충돌한 "Bumper" 태그 오브젝트 파괴
            Destroy(gameObject);
        }
    }

    void SwitchToObject(GameObject newObject)
    {
        aObject.SetActive(false); // aObject 비활성화
        newObject.SetActive(true); // 전환될 오브젝트 활성화
    }
}
