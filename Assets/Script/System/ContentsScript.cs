using UnityEngine;
using System.Collections.Generic;
using Model.Content;
public class ContentsScript : MonoBehaviour
{
    public ContentRoot Collection_content;
    [SerializeField] List<ContentData> contentDatas;
    [SerializeField] List<Sprite> contentSpriteList;

    [SerializeField] List<ContentData> fyp;

    // skeleton
    public static ContentsScript Instance;

    void Awake()
    {
        Instance = this;

        if (fyp == null)
            fyp = new List<ContentData>();

    }
    void Start()
    {
        LoadJson();

        if (Collection_content != null && Collection_content.contents != null)
            Debug.Log("Total content: " + Collection_content.contents.Count);
        else
            Debug.LogWarning("ContentsScript: collection content is null or empty after loading JSON.");
    }
    void LoadJson()
    {
        TextAsset json = Resources.Load<TextAsset>("content");
        if (json == null)
        {
            Debug.LogError("ContentsScript: 'content' TextAsset not found in Resources folder.");
            contentDatas = new List<ContentData>();
            return;
        }

        Collection_content = JsonUtility.FromJson<ContentRoot>(json.text);
        if (Collection_content == null || Collection_content.contents == null)
        {
            Debug.LogError("ContentsScript: Failed to parse content JSON or no contents found.");
            contentDatas = new List<ContentData>();
            return;
        }

        contentDatas = Collection_content.contents; // ambil semua item
        LoadFyp(5);
    }

    public ContentData GetContentById(int id)
    {
        ContentData content = contentDatas.Find((data) => data.id == id);
        return content;
    }

    public List<int> GetRandomUniqueList(int count, int lo = 1, int hi = 30)
    {
        List<int> numbers = new List<int>();

        // isi angka 1-30
        for (int i = lo; i <= hi; i++)
            numbers.Add(i);

        // shuffle
        System.Random rnd = new System.Random();
        for (int i = numbers.Count - 1; i > 0; i--)
        {
            int randomIndex = rnd.Next(0, i + 1);
            int temp = numbers[i];
            numbers[i] = numbers[randomIndex];
            numbers[randomIndex] = temp;
        }

        // ambil jumlah yang dibutuhkan
        return numbers.GetRange(0, count);
    }

    public void LoadFyp(int count)
    {
        // GameManager01.Instance.SetIsCanDialog(true);

        if (fyp == null)
            fyp = new List<ContentData>();
        else
            fyp.Clear();

        if (contentDatas == null || contentDatas.Count == 0)
        {
            Debug.LogWarning("ContentsScript.LoadFyp: contentDatas is empty, nothing to load into fyp.");
            return;
        }

        List<int> id_contents = GetRandomUniqueList(count, 1, 15);
        foreach (int cont in id_contents)
        {
            ContentData data = GetContentById(cont);
            if (data != null)
                fyp.Add(data);
            else
                Debug.LogWarning($"ContentsScript.LoadFyp: content with id {cont} not found.");
        }

        LoadContent();
    }

    void LoadContent()
    {
        int i = 0;
        foreach (SpriteRenderer spriteRenderer in HUDManager.Instance.spriteRenderersContent)
        {
            spriteRenderer.sprite = contentSpriteList[fyp[i].id - 1];
            i++;
        }
    }

    public void PrepareContentFeed(ContentData contentData)
    {
        // load all content
        // reset like
        // HUDManager.Instance.ResetTextureLike();
        HUDManager.Instance.likeSpriteRenderer.sprite = HUDManager.Instance.spriteLike;
        // prepare comment

        // prepare content text
        HUDManager.Instance.textContent.text = contentData.description;

        // prepare dialog
        if (GameManager01.Instance.GetIsCanDialog() && contentData.dialogue != null)
        {   
            StartCoroutine(HUDManager.Instance.StartAnimationTextDialogue(contentData.dialogue[Random.Range(0, contentData.dialogue.Count)]));
        }
        // prepare audio
        // AudioManager audioManager = AudioManager.Instance;
        // audioManager.aS_content.Stop();
        // audioManager.SetClip(audioManager.aS_content, audioManager.audioClipsContent[contentData.id]);
        // audioManager.aS_content.Play();
    }

}
