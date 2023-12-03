using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaitingBoss : MonoBehaviour
{
    public TextMeshProUGUI textToDeactivate; // 비활성화할 TextMeshProUGUI

    void Start()
    {
        if (textToDeactivate != null)
        {
            StartCoroutine(DeactivateAfterDelay(60f)); // 60초 후에 비활성화 코루틴 시작
        }
    }

    IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정된 시간만큼 대기
        textToDeactivate.gameObject.SetActive(false); // TextMeshProUGUI 비활성화
    }
}
