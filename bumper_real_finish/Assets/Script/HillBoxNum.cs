using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HillBoxNum : MonoBehaviour
{
    public List<GameObject> objectsToCheck; // �˻��� GameObject ����Ʈ
    public TextMeshProUGUI textDisplay; // Ȱ��ȭ�� GameObject ���� ǥ���� TextMeshProUGUI

    void Update()
    {
        // �ı��� GameObject ����
        objectsToCheck.RemoveAll(item => item == null);

        int activeCount = 0;
        foreach (var obj in objectsToCheck)
        {
            if (obj.activeSelf) // GameObject�� Ȱ��ȭ�� ���
            {
                activeCount++;
            }
        }

        textDisplay.text = $"{activeCount}"; // TextMeshProUGUI ������Ʈ
    }
}
