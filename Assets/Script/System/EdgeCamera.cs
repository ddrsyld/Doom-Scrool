using UnityEngine;

public class EdgeCamera : MonoBehaviour
{
 public float peekDistance = 1.5f;      // Seberapa jauh kamera boleh nengok
    public float edgeSize = 50f;           // Area pinggir layar (px)
    public float smooth = 5f;              // Kecepatan smoothing balik

    private Vector3 defaultPos;

    void Start()
    {
        defaultPos = transform.position;
    }

    void Update()
    {
        Vector3 target = defaultPos;
        Vector3 mouse = Input.mousePosition;

        // KANAN
        if (mouse.x > Screen.width - edgeSize)
            target.x = defaultPos.x + peekDistance;

        // KIRI
        if (mouse.x < edgeSize)
            target.x = defaultPos.x - peekDistance;

        // ATAS
        if (mouse.y > Screen.height - edgeSize)
            target.y = defaultPos.y + peekDistance;

        // // BAWAH
        // if (mouse.y < edgeSize)
        //     target.y = defaultPos.y - peekDistance;

        // Gerakin kamera dengan lembut
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * smooth);
    }
}
