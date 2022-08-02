using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeIOManager : MonoBehaviour
{
    public static FadeIOManager instance;
    public CanvasGroup canvasGroup;
    private float fadeTime = 1f;

    private void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FadeIn()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(0, fadeTime)
            .OnComplete( () => canvasGroup.blocksRaycasts = false);
    }
    public void FadeOut()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(1, fadeTime)
            .OnComplete( () => canvasGroup.blocksRaycasts = false);
    }

    public void FadeOutToIn(TweenCallback action)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(1, fadeTime)
            .OnComplete( () => {
                action();
                FadeIn();
            });
    }
}
