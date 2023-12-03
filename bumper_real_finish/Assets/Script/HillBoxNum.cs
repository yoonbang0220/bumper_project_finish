using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HillBoxNum : MonoBehaviour
{
    public List<GameObject> objectsToCheck; // 검사할 GameObject 리스트
    public TextMeshProUGUI textDisplay; // 활성화된 GameObject 수를 표시할 TextMeshProUGUI

    void Update()
    {
        // 파괴된 GameObject 제거
        objectsToCheck.RemoveAll(item => item == null);

        int activeCount = 0;
        foreach (var obj in objectsToCheck)
        {
            if (obj.activeSelf) // GameObject가 활성화된 경우
            {
                activeCount++;
            }
        }

        textDisplay.text = $"{activeCount}"; // TextMeshProUGUI 업데이트
    }
}
