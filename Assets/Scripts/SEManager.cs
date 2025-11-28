using UnityEngine;

public class SEManager : MonoBehaviour
{
    public static SEManager Instance { get; private set; }

    public AudioSource seSource;   // SE用AudioSource
    public AudioClip clickSE;      // ボタンクリック音など

    // SEの有効状態を外部から取得可能にする
    public bool SEEnabled => seEnabled;

    private bool seEnabled = true; // SEが有効かどうか

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたいでも生存
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // SE有効化
    public void EnableSE()
    {
        seEnabled = true;
    }

    // SE無効化
    public void DisableSE()
    {
        seEnabled = false;
    }

    // 任意のSEを再生
    public void PlaySE(AudioClip clip)
    {
        if (seEnabled && seSource != null && clip != null)
        {
            seSource.PlayOneShot(clip);
        }
    }

    // クリック音専用関数
    public void PlayClickSE()
    {
        PlaySE(clickSE);
    }
}
