using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;

public class HomeScreen : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private RawImage logoImage;
    [SerializeField] private List<RectTransform> menuItems;

    [Header("Animation Settings")]
    [SerializeField] private float slideDuration = 0.5f;
    [SerializeField] private float delayBetweenItems = 0.1f;
    [SerializeField] private float startOffsetX = 500f; // Distance to slide from right
    [SerializeField] private AnimationCurve slideCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    [Header("Logo Settings")]
    [SerializeField] private float logoFadeDuration = 1f;
    [SerializeField] private float logoDelay = 0f;

    [Header("Hover Settings")]
    [SerializeField] private Color hoverColor = Color.white;
    [SerializeField] private Vector3 hoverScale = new Vector3(5f, 2.5f, 2f);
    [SerializeField] private float hoverTransitionDuration = 0.2f;

    private List<Vector2> originalPositions = new List<Vector2>();

    private void Awake()
    {
        // Setup Logo
        if (logoImage != null)
        {
            Color c = logoImage.color;
            c.a = 0;
            logoImage.color = c;
        }

        // Store original positions and move items to start position
        foreach (var item in menuItems)
        {
            if (item != null)
            {
                originalPositions.Add(item.anchoredPosition);
                // Set initial position (off-screen to the right)
                item.anchoredPosition += new Vector2(startOffsetX, 0);

                // Add interaction component
                var interaction = item.gameObject.AddComponent<HomeScreenItemInteraction>();
                interaction.Initialize(hoverColor, hoverScale, hoverTransitionDuration, originalPositions[originalPositions.Count - 1]);
            }
            else
            {
                originalPositions.Add(Vector2.zero); // Placeholder
            }
        }
    }

    private void Start()
    {
        if (logoImage != null)
        {
            StartCoroutine(FadeInLogo());
        }
        StartCoroutine(AnimateMenu());
    }

    private IEnumerator FadeInLogo()
    {
        if (logoDelay > 0) yield return new WaitForSeconds(logoDelay);

        float elapsed = 0f;
        Color startColor = logoImage.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        while (elapsed < logoFadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / logoFadeDuration);
            logoImage.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }
        logoImage.color = targetColor;
    }

    private IEnumerator AnimateMenu()
    {
        for (int i = 0; i < menuItems.Count; i++)
        {
            if (menuItems[i] != null)
            {
                StartCoroutine(SlideItem(menuItems[i], originalPositions[i]));
                yield return new WaitForSeconds(delayBetweenItems);
            }
        }
    }

    private IEnumerator SlideItem(RectTransform item, Vector2 targetPos)
    {
        Vector2 startPos = item.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / slideDuration);
            float curveValue = slideCurve.Evaluate(t);

            item.anchoredPosition = Vector2.Lerp(startPos, targetPos, curveValue);
            yield return null;
        }

        item.anchoredPosition = targetPos;
    }
}

public class HomeScreenItemInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Color hoverColor;
    private Vector3 hoverScale;
    private float duration;
    private Vector2 restingPosition;

    private TMP_Text textComponent;
    private RectTransform rectTransform;
    private Vector3 originalScale;
    private Color originalColor;

    public void Initialize(Color color, Vector3 scale, float dur, Vector2 pos)
    {
        hoverColor = color;
        hoverScale = scale;
        duration = dur;
        restingPosition = pos;
    }

    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
        rectTransform = GetComponent<RectTransform>();
        if (textComponent == null) textComponent = GetComponentInChildren<TMP_Text>();
        originalScale = transform.localScale;
        if (textComponent != null) originalColor = textComponent.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(Animate(hoverScale, hoverColor, true));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(Animate(originalScale, originalColor, false));
    }

    private IEnumerator Animate(Vector3 targetScale, Color targetColor, bool isHovering)
    {
        float elapsed = 0f;
        Vector3 startScale = transform.localScale;
        Vector2 startPos = rectTransform.anchoredPosition;
        Color startColor = textComponent != null ? textComponent.color : Color.white;

        // Calculate target position to simulate "expand to left" (Right Pivot behavior)
        // If pivot is 0.5, we need to move left by (DeltaScale * Width * 0.5)
        // Formula: Offset = (TargetScale.x - OriginalScale.x) * Width * (Pivot.x - 1.0f)
        // If isHovering is false, we return to restingPosition (offset 0)
        
        Vector2 targetPos = restingPosition;
        if (isHovering)
        {
            float width = rectTransform.rect.width;
            float pivotX = rectTransform.pivot.x;
            float scaleDeltaX = targetScale.x - originalScale.x;
            float xOffset = scaleDeltaX * width * (pivotX - 1f);
            targetPos = restingPosition + new Vector2(xOffset, 0);
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            
            if (textComponent != null) textComponent.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }
        
        transform.localScale = targetScale;
        rectTransform.anchoredPosition = targetPos;
        if (textComponent != null) textComponent.color = targetColor;
    }
}
