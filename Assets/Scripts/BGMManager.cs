using UnityEngine;

// BGMの操作
public class BGMManager : MonoBehaviour
{
    // フィールド定義
    private static BGMManager instance;    // シングルトン管理用
    public AudioSource bgmSource;          // BGM用AudioSource(BGMの情報)
    public AudioClip[] bgmClips;           // BGM用の配列

    private bool bgmEnabled = true;        // BGMが有効かどうか
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

    // BGMの有効状態を参照
    public bool BGMEnabled　　　　　　　　 
    {
        get
        {
            return bgmEnabled;
        }
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

    // 指定番号のBGMを再生
    public void PlayBGM(int index)
    {
        // BGMをオンにする
        bgmEnabled = true;

        // BGMが有効の場合
        if (!bgmEnabled)
        {
            return;
        }
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
    }

    // BGMを停止
    public void StopBGM()
    {
        // bgmEnabledをOFFにする
        bgmEnabled = false;

        // bgmSourceがあり、bgmがONの場合
        if (bgmSource != null && bgmSource.isPlaying)
        {
            // BGMを止める
            bgmSource.Stop();
        }
    }
}
