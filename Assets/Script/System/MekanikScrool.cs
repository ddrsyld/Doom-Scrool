using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Model.Content;

public class MekanikScrool : MonoBehaviour
{
    [SerializeField] int index = 0;
    [SerializeField] GameObject CameraRenderContent;
    [SerializeField] float everyScroolMoveY = -19.7f;
    [SerializeField] float durationScrool = 0.4f;
    bool isCanScrool = true, isCanLikeFeed = true;
    ContentData CurrentCont;

    Vector3 initialPOS;
    public static MekanikScrool Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (CameraRenderContent == null)
        {
            Debug.LogError("MekanikScrool: CameraRenderContent is not assigned in the inspector.");
            initialPOS = transform.position;
        }
        else
        {
            initialPOS = CameraRenderContent.transform.position;
        }

        if (ContentsScript.Instance != null)
            CurrentCont = ContentsScript.Instance.GetContentById(index);
        else
            Debug.LogWarning("MekanikScrool: ContentsScript.Instance is null. Ensure ContentsScript exists and its Awake() ran.");
    }
    void Update()
    {
        // cek movement scrool
        if (isCanScrool && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
            DOScrool();
    }

    string GetTextMood(int mood)
    {
        if (mood == 1) return "Mood+";
        if (mood == 2) return "Mood++";
        if (mood == 3) return "Mood+++";
        if (mood == -1) return "Mood-";
        if (mood == -2) return "Mood--";
        if (mood == -3) return "Mood---";

        return "???";
    }

    void DOScrool()
    {
        if (CameraRenderContent == null)
        {
            Debug.LogError("MekanikScrool.DOScrool: CameraRenderContent is null, cannot scroll.");
            return;
        }

        isCanScrool = false;
        HandManager.Instance.DoScrollAnimate();

        CurrentCont = ContentsScript.Instance.GetContentById(index + 1);
        ContentsScript.Instance.PrepareContentFeed(CurrentCont);

        if (CurrentCont.effect_name == "mood")
        {
            string teksMood = GetTextMood(CurrentCont.effect);
            GameManager01.Instance.ShowTextPopup(teksMood);
            GameManager01.Instance.SetMoodVal(CurrentCont.effect);
        }

        Vector3 lastPosCam = CameraRenderContent.transform.position;
        CameraRenderContent.transform.DOMoveY(lastPosCam.y + everyScroolMoveY, durationScrool).OnComplete(() =>
        {
            // kalo index sudah di 5 loading
            if (index == 5 - 1)
            {
                StartCoroutine(DoLoading());
                return;
            }

            StartCoroutine(SetIsCanScroolinSecond());
            index++;

            isCanLikeFeed = true;

            // HUDManager.Instance.GetCGLIke().interactable = true;
            // HUDManager.Instance.GetCGLIke().blocksRaycasts = true;

        });

    }

    public void DoLike()
    {
        if (!isCanLikeFeed) return;

        isCanLikeFeed = false;
        AudioManager.Instance.aS_like.Stop();
        AudioManager.Instance.aS_like.Play();
        // do effect

        // HUDManager.Instance.SetTextureLike();
        HUDManager.Instance.likeSpriteRenderer.sprite = HUDManager.Instance.spriteLikeActive;
        GameManager01.Instance.SetMoodVal(1);
        GameManager01.Instance.ShowTextPopup("mood+");

        // HUDManager.Instance.GetCGLIke().interactable = false;
        // HUDManager.Instance.GetCGLIke().blocksRaycasts = false;
    }

    IEnumerator SetIsCanScroolinSecond()
    {
        yield return new WaitForSeconds(0.5f);
        isCanScrool = true;
    }

    IEnumerator DoLoading()
    {
        index = 0;
        isCanScrool = false;

        ContentsScript.Instance.LoadFyp(5); // load new 5 content

        yield return new WaitForSeconds(0.7f);
        CameraRenderContent.transform.position = initialPOS;
        isCanScrool = true;
    }
}
