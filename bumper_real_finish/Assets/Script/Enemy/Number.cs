using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Number : MonoBehaviour
{
    public Image[] images; // 확인할 Image 컴포넌트들
    public TextMeshProUGUI countText; // 숫자를 표시할 TextMeshProUGUI

    void Update()
    {
        UpdateImageCount();
    }

    void UpdateImageCount()
    {
        int activeImageCount = 0; // 활성화된 이미지의 수
        foreach (var image in images)
        {
            if (image.enabled)
            {
                activeImageCount++;
            }
        }

        countText.text = activeImageCount.ToString(); // TextMeshPro 텍스트 업데이트
    }
}
