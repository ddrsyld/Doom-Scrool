using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Rendering;
public class HUDManager : MonoBehaviour
{
    // Aps content
    public List<SpriteRenderer> spriteRenderersContent;

    public TMP_Text textDialogueUI, textContent, textMood, textHour;
    public Image fillEnergy, fillAudio;
    [SerializeField] public RawImage rawImageLike, rawImageAudio;

    public static HUDManager Instance;

    public Texture2D textureLike, textureLikeActive, textureAudio, textureAudioMute;
    public Sprite spriteLike, spriteLikeActive;
    public SpriteRenderer likeSpriteRenderer;

    bool isFullScreen = false;

    [SerializeField] CanvasGroup CG_audio, CG_pause, CG_topBar, CG_bottomBar, CG_like, CG_bg;
    [SerializeField] Animator animatorPauseMenu, animatorTextDialog;
    public Animator animatorBgEnv, animatorAudioMenu;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        isFullScreen = false;
    }

    void Update()
    {

    }

    public void UpdateFillImage(Image fillImage, int newVal, int max = 8)
    {
        fillImage.fillAmount = (float)newVal / max;
    }

    public CanvasGroup GetCGLIke()
    {
        return CG_like;
    }

    public CanvasGroup GetCGBG()
    {
        return CG_bg;
    }

    public void ResetTextureLike()
    {
        rawImageLike.texture = textureLike;
    }

    public void SetTextureLike()
    {
        rawImageLike.texture = textureLikeActive;
    }

    public void SetTextMood(int moodValue)
    {
        string mood;
        // if()
        if (moodValue >= 80) mood = "Happy";
        else if (moodValue >= 60) mood = "Neutral";
        else if (moodValue >= 40) mood = "Upset";
        else if (moodValue >= 20) mood = "Sad";
        else
            mood = "Broken";
        textMood.text = $"Mood : {mood}";
    }

    public IEnumerator StartAnimationTextDialogue(string text, float targetDur = 1.2f, float delayInStart = 1.1f)
    {
        Debug.LogWarning(text);
        GameManager01.Instance.SetIsCanDialog(false);

        yield return new WaitForSeconds(delayInStart);

        textDialogueUI.text = "";
        AudioManager audioManager = AudioManager.Instance;
        audioManager.PlayDialogAudio();

        animatorTextDialog.Play("play", 0, 0); // audio

        foreach (char c in text)
        {
            yield return new WaitForSeconds((targetDur / text.Length));
            textDialogueUI.text += c;
            // audioManager.SetPitchRand(audioManager.aS_dialogue);
        }

        // foreach (char c in text)
        // {
        //     yield return new WaitForSeconds((targetDur / text.Length) / 2);
        //     audioManager.SetPitchRand(audioManager.aS_dialogue);
        // }

        audioManager.StopAudio(audioManager.aS_dialogue);

        yield return new WaitForSeconds(1.5f);
        GameManager01.Instance.SetIsCanDialog(true);
    }

    public void LoadSettingScene()
    {
        SceneManager.LoadSceneAsync("OptionUI", LoadSceneMode.Additive);
    }

    public void LoadControlScene()
    {
        SceneManager.LoadSceneAsync("Tutorial", LoadSceneMode.Additive);
    }

    // lets make functionality for game
    public void PauseGameUI()
    {
        AudioManager.Instance.aS_choose.Stop();
        AudioManager.Instance.aS_choose.Play();
        CG_pause.DOFade(1f, 0.3f).OnComplete(() =>
        {
            CG_pause.blocksRaycasts = true;
            CG_pause.interactable = true;
            Time.timeScale = 0;
        });
    }

    public void UnPauseGameUI()
    {
        AudioManager.Instance.aS_choose.Stop();
        AudioManager.Instance.aS_choose.Play();

        Time.timeScale = 1;
        CG_pause.DOFade(0, 0.3f).OnComplete(() =>
        {
            CG_pause.blocksRaycasts = false;
            CG_pause.interactable = false;
        });
    }

    public void ChangeScreenUI()
    {
        isFullScreen = !isFullScreen;
        AudioManager.Instance.aS_choose.Stop();
        AudioManager.Instance.aS_choose.Play();

        if (isFullScreen == false) // ada item2
        {
            // set full

            CG_bottomBar.DOFade(1, 0.4f);
            CG_topBar.DOFade(1, 0.4f).OnComplete(() =>
            {
                CG_bottomBar.interactable = true;
                CG_bottomBar.blocksRaycasts = true;

                CG_topBar.interactable = true;
                CG_topBar.blocksRaycasts = true;
            });
        }
        else // gada item2
        {

            CG_bottomBar.DOFade(0, 0.4f);
            CG_topBar.DOFade(0, 0.4f).OnComplete(() =>
            {
                CG_bottomBar.interactable = false;
                CG_bottomBar.blocksRaycasts = false;

                CG_topBar.interactable = false;
                CG_topBar.blocksRaycasts = false;
            });
        }
    }

    public void ToggleMuteMusic()
    {
        AudioManager.Instance.aS_choose.Stop();
        AudioManager.Instance.aS_choose.Play();

        if (PlayerPrefs.GetInt("isMuteMusic", 0) == 0) // kalo mute
        {
            AudioManager.Instance.SetMute(AudioManager.Instance.aS_music);
            PlayerPrefs.SetInt("isMuteMusic", 1);
            PlayerPrefs.Save();
            MuteMusicUI(true);
        }
        else // kalo bersuara
        {
            AudioManager.Instance.SetMute(AudioManager.Instance.aS_music, false);
            PlayerPrefs.SetInt("isMuteMusic", 0);
            PlayerPrefs.Save();
            MuteMusicUI(false);
        }
    }

    public void MuteMusicUI(bool isMute)
    {
        if (isMute) rawImageAudio.texture = textureAudioMute;
        else rawImageAudio.texture = textureAudio;
    }

    public void SnapUI()
    {
        AudioManager.Instance.aS_camerasnap.Stop();
        AudioManager.Instance.aS_camerasnap.Play();
    }

}
