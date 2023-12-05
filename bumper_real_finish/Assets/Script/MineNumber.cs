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
        if (Input.GetMouseButtonDown(1) && count > 0) // ���콺 ������ ��ư Ŭ�� ���� (0�� ����, 1�� ������)
        {
            count--;
            textMeshPro.text = count.ToString();
        }
    }
}
