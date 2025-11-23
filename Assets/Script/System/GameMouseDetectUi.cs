using UnityEngine;

public class GameMouseDetectUi : MonoBehaviour
{
    public GameObject lastHover;

    void Update()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float mx = mouseWorld.x;
        float my = mouseWorld.y;

        Debug.Log($"{mouseWorld.x}, {mouseWorld.y}");
        
        if (mx >= 5.5f && mx <= 6f &&
            my >= -0.25f && my <= 0.25f &&
            Input.GetMouseButtonDown(0))
        {
            // Do Like
            Debug.LogWarning("OKKK");
            MekanikScrool.Instance.DoLike();
        }

        if (mx >= 5.75f && mx <= 6f &&
           my >= -0.6f && my <= -0.4f &&
           Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse berada di area COMMENT!");
            // taruh kode apapun yang kamu mau di sini
        }

    }
}
