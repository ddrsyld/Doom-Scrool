using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CreditUi : MonoBehaviour
{
    [Header("Background Settings")]
    [SerializeField] private List<RawImage> backgroundList;
    [SerializeField] private float changeInterval = 5f;
    [SerializeField] private float fadeDuration = 1f;

    [Header("Text Settings")]
    [SerializeField] private List<TMP_Text> textList;
    [SerializeField] private float textChangeInterval = 4f;
    [SerializeField] private float textFadeDuration = 1f;

    [Header("Closing Settings")]
    [SerializeField] private RawImage closingBackground;
    [SerializeField] private RawImage closingImage;
    [SerializeField] private TMP_Text closingText;
    [SerializeField] private float closingFadeDuration = 1.5f;
    [SerializeField] private float closingDisplayDuration = 3f;

    [Header("Cinematic Movement")]
    [SerializeField] private float scrollSpeed = 0.05f;

    private int currentIndex = 0;
    private int currentTextIndex = 0;
    private Rect uvRect;
    private bool isCreditsFinished = false;

    private void Start()
    {
        if (backgroundList == null || backgroundList.Count == 0)
        {
            Debug.LogWarning("CreditUi: Background List is empty!");
            return;
        }

        // Initialize Backgrounds: Set all to transparent and setup UVs
        uvRect = new Rect(0, 0, 1, 1);
        foreach (var img in backgroundList)
        {
            if (img != null)
            {
                Color c = img.color;
                c.a = 0f;
                img.color = c;
                img.uvRect = uvRect;
            }
        }

        // Initialize Texts: Set all to transparent
        if (textList != null)
        {
            foreach (var txt in textList)
            {
                if (txt != null)
                {
                    Color c = txt.color;
                    c.a = 0f;
                    txt.color = c;
                }
            }
        }

        // Initialize Closing Elements
        if (closingBackground != null)
        {
            Color c = closingBackground.color;
            c.a = 0f;
            closingBackground.color = c;
            closingBackground.uvRect = uvRect;
        }
        if (closingImage != null)
        {
            Color c = closingImage.color;
            c.a = 0f;
            closingImage.color = c;
        }
        if (closingText != null)
        {
            Color c = closingText.color;
            c.a = 0f;
            closingText.color = c;
        }
        
        StartCoroutine(BackgroundRoutine());
        if (textList != null && textList.Count > 0)
        {
            StartCoroutine(TextRoutine());
        }
    }

    private void Update()
    {
        // Cinematic Panning
        uvRect.x += scrollSpeed * Time.deltaTime;
        
        if (uvRect.x > 100f) uvRect.x -= 100f;

        // Apply to all images so they stay in sync
        foreach (var img in backgroundList)
        {
            if (img != null) img.uvRect = uvRect;
        }

        if (closingBackground != null) closingBackground.uvRect = uvRect;
    }

    private IEnumerator BackgroundRoutine()
    {
        while (!isCreditsFinished)
        {
            // Reset position to prevent stretching and restart panning
            uvRect = new Rect(0, 0, 1, 1);

            RawImage currentImage = backgroundList.Count > 0 ? backgroundList[currentIndex] : null;

            // Fade In
            if (currentImage != null) yield return StartCoroutine(FadeImage(currentImage, 0f, 1f));

            // Wait Interval
            float elapsed = 0f;
            while (elapsed < changeInterval && !isCreditsFinished)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Fade Out
            if (currentImage != null) yield return StartCoroutine(FadeImage(currentImage, 1f, 0f));

            // Next Index
            if (backgroundList.Count > 0) currentIndex = (currentIndex + 1) % backgroundList.Count;
        }
    }

    private IEnumerator TextRoutine()
    {
        for (int i = 0; i < textList.Count; i++)
        {
            TMP_Text currentText = textList[i];

            // Fade In
            if (currentText != null) yield return StartCoroutine(FadeText(currentText, 0f, 1f));

            // Wait Interval
            yield return new WaitForSeconds(textChangeInterval);

            // Fade Out
            if (currentText != null) yield return StartCoroutine(FadeText(currentText, 1f, 0f));
        }

        // End of credits
        isCreditsFinished = true;
        StartCoroutine(ClosingRoutine());
    }

    private IEnumerator ClosingRoutine()
    {
        // 1. Fade In Closing Background
        if (closingBackground != null)
            yield return StartCoroutine(FadeImage(closingBackground, 0f, 1f));

        // 2. Fade In Closing Image
        if (closingImage != null)
        {
            yield return StartCoroutine(FadeImage(closingImage, 0f, 1f));
            yield return new WaitForSeconds(closingDisplayDuration);
            yield return StartCoroutine(FadeImage(closingImage, 1f, 0f));
        }

        // 3. Fade In Closing Text
        if (closingText != null)
        {
            yield return StartCoroutine(FadeText(closingText, 0f, 1f));
            yield return new WaitForSeconds(closingDisplayDuration);
            yield return StartCoroutine(FadeText(closingText, 1f, 0f));
        }

        // 4. Fade Out Closing Background (Optional)
        if (closingBackground != null)
            yield return StartCoroutine(FadeImage(closingBackground, 1f, 0f));
    }

    private IEnumerator FadeImage(RawImage img, float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color color = img.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            img.color = color;
            
            yield return null;
        }

        color.a = endAlpha;
        img.color = color;
    }

    private IEnumerator FadeText(TMP_Text txt, float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color color = txt.color;

        while (elapsed < textFadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / textFadeDuration);
            
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            txt.color = color;
            
            yield return null;
        }

        color.a = endAlpha;
        txt.color = color;
    }
}
