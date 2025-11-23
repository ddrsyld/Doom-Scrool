using UnityEngine;

public class HandManager : MonoBehaviour
{
    [SerializeField] Animator animator;
    public static HandManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void DoScrollAnimate()
    {
        animator.Play("scroolHand", 0, 0);
    }
}
