using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public Slider loadingBar; // �ε� ��

    void Start()
    {
        StartCoroutine(LoadWithProgress());
    }

    IEnumerator LoadWithProgress()
    {
        // 0���� 0.7���� 1�� ���� �̵�
        yield return StartCoroutine(MoveSliderOverTime(loadingBar, 0f, 0.7f, 0.2f));

        // 2�ʰ� ���
        yield return new WaitForSeconds(2f);

        // 0.7���� 1���� 1�� ���� �̵�
        yield return StartCoroutine(MoveSliderOverTime(loadingBar, 0.7f, 1f, 0.5f));

        // 1�ʰ� ������
        yield return new WaitForSeconds(1f);

        // Intro ������ ��ȯ
        SceneManager.LoadScene("Intro");
    }

    IEnumerator MoveSliderOverTime(Slider slider, float startValue, float endValue, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            slider.value = Mathf.Lerp(startValue, endValue, time / duration);
            yield return null;
        }
        slider.value = endValue;
    }
}
