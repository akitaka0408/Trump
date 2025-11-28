using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance { get; private set; }
    public AudioSource bgmSource;

    // BGMの有効状態を外部から取得可能にする
    public bool BGMEnabled => bgmEnabled;

    private bool bgmEnabled = true; // BGMが有効かどうか

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

    // BGMを再生
    public void PlayBGM()
    {
        bgmEnabled = true;

        if (bgmSource != null && !bgmSource.isPlaying)
        {
            bgmSource.Play();
        }
    }

    // BGMを停止
    public void StopBGM()
    {
        bgmEnabled = false;

        if (bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }
    }
}
