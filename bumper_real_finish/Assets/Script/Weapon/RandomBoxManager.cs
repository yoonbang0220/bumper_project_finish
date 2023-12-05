using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBoxManager : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Play"))
        {

            // 아이템 박스 비활성화 또는 파괴
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
