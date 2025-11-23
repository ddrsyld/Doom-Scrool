using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
   public void PlayDialog(string text)
    {
        StartCoroutine(HUDManager.Instance.StartAnimationTextDialogue(text));
    }
}
