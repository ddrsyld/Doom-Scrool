using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text continueText;
    [SerializeField] private RawImage logoImage;

    [Header("Animation Settings")]
    // Hex: #6B3B00
    [SerializeField] private Color normalColor = new Color32(107, 59, 0, 255); 
    // Hex: #E17C08
    [SerializeField] private Color pulseColor = new Color32(225, 124, 8, 255); 
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float pulseScaleMultiplier = 1.1f;

    [Header("Intro Settings")]
    [SerializeField] private float appearDelay = 1f;
    [SerializeField] private float appearDuration = 0.3f;
    [SerializeField] private Vector3 targetScale = Vector3.one;

    [Header("Logo Settings")]
    [SerializeField] private float logoAppearDelay = 0.5f;
    [SerializeField] private float logoFadeDuration = 1f;

    private void Update()
    {
        float time = Time.time;

        // Logo Animation
        if (logoImage != null)
        {
            Color logoColor = logoImage.color;
            if (time < logoAppearDelay)
            {
                logoColor.a = 0;
            }
            else if (time < logoAppearDelay + logoFadeDuration)
            {
                logoColor.a = (time - logoAppearDelay) / logoFadeDuration;
            }
            else
            {
                logoColor.a = 1;
            }
            logoImage.color = logoColor;
        }

        // Text Animation
        if (continueText != null)
        {
            float appearEndTime = appearDelay + appearDuration;

            // Calculate the base pulse color
            float pulseT = Mathf.PingPong(time * pulseSpeed, 1f);
            Color currentPulseColor = Color.Lerp(normalColor, pulseColor, pulseT);

            if (time < appearDelay)
            {
                // Before 1s: Invisible and scaled down
                currentPulseColor.a = 0;
                continueText.color = currentPulseColor;
                continueText.transform.localScale = Vector3.zero;
            }
            else if (time < appearEndTime)
            {
                // Between 1s and 1.3s: Fade in and Scale up
                float progress = (time - appearDelay) / appearDuration;
                
                // Scale from 0 to targetScale
                continueText.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, progress);
                
                // Fade alpha from 0 to 1
                currentPulseColor.a = progress;
                continueText.color = currentPulseColor;
            }
            else
            {
                // After 1.3s: Normal pulse behavior
                // Pulse scale along with color
                continueText.transform.localScale = Vector3.Lerp(targetScale, targetScale * pulseScaleMultiplier, pulseT);
                continueText.color = currentPulseColor;
            }
        }
    }
}
