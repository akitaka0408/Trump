using UnityEngine;

// BGMの操作
public class BGMManager : MonoBehaviour
{
    // フィールド定義
    private static BGMManager instance;    // シングルトン管理用
    public AudioSource bgmSource;          // SE用AudioSource(SEの情報)
    private bool bgmEnabled = true;        // BGMが有効かどうか

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

    // BGMを再生
    public void PlayBGM()
    {
        // bgmEnabledをONにする
        bgmEnabled = true;

        // bgmSourceがあり、bgmがOFFの場合
        if (bgmSource != null && !bgmSource.isPlaying)
        {
            // BGMを鳴らす
            bgmSource.Play();
        }
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
