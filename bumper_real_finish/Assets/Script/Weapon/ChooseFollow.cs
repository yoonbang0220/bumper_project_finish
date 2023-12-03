using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseFollow : MonoBehaviour
{
    public GameObject[] weapons; // 무기 프리팹 배열

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // 플레이어와 충돌 시
        {
            int weaponIndex = Random.Range(0, weapons.Length); // 랜덤 무기 선택
            GameObject newWeapon = Instantiate(weapons[weaponIndex], other.transform.position + other.transform.forward, Quaternion.identity); // 무기 생성 및 위치 조정
            newWeapon.transform.parent = other.transform; // 플레이어의 자식으로 설정
        }
    }
}
