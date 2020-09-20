using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowCredits()
    {
        SoundManager.PlaySound(SoundManager.Sound.Click);
        gameObject.SetActive(true);
        image.color = new Color(0, 0, 0, 1);
    }

    public void CloseCredits()
    {
        SoundManager.PlaySound(SoundManager.Sound.Click);
        gameObject.SetActive(false);
        image.color = new Color(0, 0, 0, 0);
    }
}
