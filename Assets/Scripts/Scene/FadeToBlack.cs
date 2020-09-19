using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{
    public static FadeToBlack instance;

    private Image blackPanel;

    private void Awake()
    {
        blackPanel = GetComponent<Image>();
        instance = this;
    }

    private void Start()
    {
        FadeOutOfBlack();
    }

    public void FadeIntoBlack(System.Action onComplete)
    {
        LeanTween.cancel(gameObject.GetComponent<RectTransform>());
        blackPanel.enabled = true;
        LeanTween.alpha(gameObject.GetComponent<RectTransform>(), 1, 0.5f).setOnComplete(onComplete);
    }

    public void FadeOutOfBlack()
    {
        LeanTween.cancel(gameObject.GetComponent<RectTransform>());
        blackPanel.enabled = true;
        gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        LeanTween.alpha(gameObject.GetComponent<RectTransform>(), 0, 0.5f).setOnComplete(DisableObject);
    }

    private void DisableObject()
    {
        blackPanel.enabled = false;
    }
}