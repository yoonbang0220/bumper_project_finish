using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealBox : MonoBehaviour
{
    public Image controlledImage; // 인스펙터에서 할당할 Image 컴포넌트

    void Start()
    {
        // Image 활성화
        controlledImage.enabled = true;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Play"))
        {

            // 아이템 박스 비활성화 또는 파괴
            gameObject.SetActive(false);
            Destroy(gameObject);
            controlledImage.enabled = false; // Image 비활성화
        }
    }
}
