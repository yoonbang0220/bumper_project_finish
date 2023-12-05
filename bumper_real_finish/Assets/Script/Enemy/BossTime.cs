using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTime : MonoBehaviour
{
    public GameObject objectToActivate; // 활성화할 오브젝트
    public float activationDelay = 60f; // 활성화까지의 지연 시간

    void Start()
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(false); // 시작 시 오브젝트를 비활성화
            StartCoroutine(ActivateObjectAfterDelay(activationDelay)); // 설정된 지연 시간 후에 오브젝트를 활성화
        }
    }

    IEnumerator ActivateObjectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정된 시간만큼 대기
        objectToActivate.SetActive(true); // 오브젝트를 활성화
    }
}
