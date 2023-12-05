using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MineNumber : MonoBehaviour
{
    public TMP_Text textMeshPro;
    public int count = 10;

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && count > 0) // 마우스 오른쪽 버튼 클릭 감지 (0은 왼쪽, 1은 오른쪽)
        {
            count--;
            textMeshPro.text = count.ToString();
        }
    }
}
