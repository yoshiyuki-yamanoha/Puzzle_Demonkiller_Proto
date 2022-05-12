using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    [SerializeField] float fadeSpeed = 0.01f;
    Image fadeImage;
    float rgb;
    float alpha;

    bool isFadeIn;

    // Start is called before the first frame update
    void Start()
    {
        FadeImage_Init();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isFadeIn == true)
            StartFadeIn();
    }

    void FadeImage_Init()
    {
        fadeImage = GetComponent<Image>();
        rgb = 0;
        alpha = fadeImage.color.a;
        isFadeIn = true;

        fadeImage.enabled = true;
    }

    void StartFadeIn()
    {
        alpha -= fadeSpeed;
        if (alpha <= 0)
        {
            alpha = 0;
            isFadeIn = false;
        }
        UpdateFadeImageColor();
    }

    void UpdateFadeImageColor()
    {
        fadeImage.color = new Color(rgb, rgb, rgb, alpha);
    }
}