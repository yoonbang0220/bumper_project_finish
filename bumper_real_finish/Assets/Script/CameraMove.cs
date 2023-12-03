using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public List<Transform> targets; // 카메라가 따라갈 대상들의 목록
    public Vector3 offset; // 대상과 카메라 사이의 오프셋
    private int currentIndex = 0; // 현재 추적 중인 대상의 인덱스

    void LateUpdate()
    {
        if (targets.Count == 0) return; // 대상이 없으면 아무것도 하지 않음

        // 현재 인덱스의 대상이 살아있는지 확인
        Transform currentTarget = targets[currentIndex];
        if (currentTarget != null && currentTarget.gameObject.activeSelf)
        {
            // 살아있으면 추적
            transform.position = currentTarget.position + offset;
            transform.LookAt(currentTarget);
        }
        // 추가적인 로직으로 인덱스 변경 가능 (예: 키 입력에 따른 변경)
    }

    // 인덱스를 변경하는 메서드 (예: 다음 대상으로 이동)
    public void NextTarget()
    {
        if (targets.Count > 0)
        {
            currentIndex = (currentIndex + 1) % targets.Count;
        }
    }
}
