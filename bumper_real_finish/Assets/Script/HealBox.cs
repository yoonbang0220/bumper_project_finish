using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealBox : MonoBehaviour
{
    public Image controlledImage; // �ν����Ϳ��� �Ҵ��� Image ������Ʈ

    void Start()
    {
        // Image Ȱ��ȭ
        controlledImage.enabled = true;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Play"))
        {

            // ������ �ڽ� ��Ȱ��ȭ �Ǵ� �ı�
            gameObject.SetActive(false);
            Destroy(gameObject);
            controlledImage.enabled = false; // Image ��Ȱ��ȭ
        }
    }
}
