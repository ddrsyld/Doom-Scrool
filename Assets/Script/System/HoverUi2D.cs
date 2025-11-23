using UnityEngine;

public class HoverUi2D : MonoBehaviour
{
    SpriteRenderer sr;
    Color originalColor;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr) originalColor = sr.color;
    }

    public void OnMouseEnterCustom()
    {
        Debug.Log($"{name} : Hover masuk");
        if (sr) sr.color = new Color(0.9f, 0.9f, 1f);
    }

    public void OnMouseExitCustom()
    {
        Debug.Log($"{name} : Hover keluar");
        if (sr) sr.color = originalColor;
    }

    public void OnMouseClickCustom()
    {
        Debug.Log($"{name} : Diklik");
    }
}
