using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public Slider loadingBar; // 로딩 바

    void Start()
    {
        StartCoroutine(LoadWithProgress());
    }

    IEnumerator LoadWithProgress()
    {
        // 0에서 0.7까지 1초 동안 이동
        yield return StartCoroutine(MoveSliderOverTime(loadingBar, 0f, 0.7f, 0.2f));

        // 2초간 대기
        yield return new WaitForSeconds(2f);

        // 0.7에서 1까지 1초 동안 이동
        yield return StartCoroutine(MoveSliderOverTime(loadingBar, 0.7f, 1f, 0.5f));

        // 1초간 딜레이
        yield return new WaitForSeconds(1f);

        // Intro 씬으로 전환
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
