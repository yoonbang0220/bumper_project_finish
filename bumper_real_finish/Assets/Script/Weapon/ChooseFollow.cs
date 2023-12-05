using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseFollow : MonoBehaviour
{
    public GameObject[] weapons; // ���� ������ �迭

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // �÷��̾�� �浹 ��
        {
            int weaponIndex = Random.Range(0, weapons.Length); // ���� ���� ����
            GameObject newWeapon = Instantiate(weapons[weaponIndex], other.transform.position + other.transform.forward, Quaternion.identity); // ���� ���� �� ��ġ ����
            newWeapon.transform.parent = other.transform; // �÷��̾��� �ڽ����� ����
        }
    }
}
