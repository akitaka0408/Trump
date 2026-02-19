using UnityEngine;
using UnityEngine.Audio;

// BGMの操作
public class BGMManager : MonoBehaviour
{
    // フィールド定義
    private static BGMManager instance;    // シングルトン管理用
    public AudioSource bgmSource;          // BGM用AudioSource(BGMの情報)
    public AudioClip[] bgmClips;           // BGM用の配列
    public AudioMixer mixer;               // オーディオミキサー

    private int currentIndex = -1;         // 現在選ばれている音楽

    // 一番最初に呼び出される
    private void Awake()
    {
        // Instanceがない場合
        if (Instance == null)
        {
            // 自身をシングルトンとして登録
            Instance = this;
            // シーンをまたいでも壊れないようにする
            DontDestroyOnLoad(gameObject); 
        }
        // Instanceがある場合
        else
        {
            // シングルトンを破壊する
            Destroy(gameObject);
        }
    }

    // Awakeの次に呼び出される
    private void Start()
    {
        int savedIndex = GameDataManager.Instance.data.bgmIndex;
        PlayBGM(savedIndex);
    }

    // シングルトンを参照する
    public static BGMManager Instance    
    {
        get
        {
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    // 音量設定
    public void SetVolume(float value)
    {
        if (value <= 0.0001f)
        {
            mixer.SetFloat("BGMVolume", -80f);
        }
        else
        {
            mixer.SetFloat("BGMVolume", Mathf.Log10(value) * 20);
        }
    }

    // 指定番号のBGMを再生
    public void PlayBGM(int index)
    {
        // 番号が正しい
        if (index < 0 || index >= bgmClips.Length)
        {
            return;
        }

        // 同じ曲なら何もしない
        if (currentIndex == index && bgmSource.isPlaying)
        {
            return;
        }

        currentIndex = index;
        bgmSource.clip = bgmClips[index];
        bgmSource.loop = true;
        // 曲を流す
        bgmSource.Play();

        GameDataManager.Instance.data.bgmIndex = index;
        GameDataManager.Instance.Save();
    }
}
