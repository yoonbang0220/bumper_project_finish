using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossUITime : MonoBehaviour
{
    public Image imageToActivate; // 활성화할 이미지
    public float activationDelay = 60f; // 활성화까지의 지연 시간

    void Start()
    {
        if (imageToActivate != null)
        {
            imageToActivate.gameObject.SetActive(false); // 시작 시 이미지를 비활성화
            StartCoroutine(ActivateImageAfterDelay(activationDelay)); // 설정된 지연 시간 후에 이미지를 활성화
        }
    }

    IEnumerator ActivateImageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정된 시간만큼 대기
        imageToActivate.gameObject.SetActive(true); // 이미지를 활성화
    }
}
