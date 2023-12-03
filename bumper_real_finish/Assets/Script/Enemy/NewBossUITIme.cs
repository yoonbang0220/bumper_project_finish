using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewBossUITIme : MonoBehaviour
{
    public TextMeshProUGUI textToActivate; // 활성화할 텍스트
    public GameObject objectToActivate; // 활성화할 게임 오브젝트
    public float activationDelay = 60f; // 활성화까지의 지연 시간

    void Start()
    {
        if (textToActivate != null)
        {
            textToActivate.gameObject.SetActive(false); // 시작 시 텍스트를 비활성화
        }

        if (objectToActivate != null)
        {
            objectToActivate.SetActive(false); // 시작 시 게임 오브젝트를 비활성화
        }

        StartCoroutine(ActivateAfterDelay(activationDelay)); // 설정된 지연 시간 후에 텍스트와 게임 오브젝트를 활성화
    }

    IEnumerator ActivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정된 시간만큼 대기

        if (textToActivate != null)
        {
            textToActivate.gameObject.SetActive(true); // 텍스트를 활성화
        }

        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true); // 게임 오브젝트를 활성화
        }
    }
}
