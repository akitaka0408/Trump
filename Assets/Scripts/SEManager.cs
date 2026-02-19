using UnityEngine;
using UnityEngine.Audio;

// SEの操作
public class SEManager : MonoBehaviour
{
    // フィールド定義
    private static SEManager instance;  // シングルトン管理用
    public AudioSource seSource;        // SE用AudioSource
    public AudioClip clickSE;           // ボタンクリック音
    public AudioClip cardSE;            // カードをめくる、引く音
    public AudioClip drawSE;            // 引き分け字の音
    public AudioClip winSE;             // 勝利時の音
    public AudioClip loseSE;            // 敗北時の音
    public AudioClip blackjackSE;       // ブラックジャック時の音
    public AudioClip retireSE;          // リタイア時の音
    public AudioClip gameOverSE;        // ゲームオーバー時の音
    public AudioMixer mixer;            // オーディオミキサー

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

    // シングルトン参照用
    public static SEManager Instance
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


    // SEの音量設定
    public void SetVolume(float value)
    {
        if (value <= 0.0001f)
        {
            mixer.SetFloat("SEVolume", -80f);
        }
        else
        {
            mixer.SetFloat("SEVolume", Mathf.Log10(value) * 20);
        }
    }

    // SEを鳴らす
    public void PlaySE(AudioClip clip)
    {
        seSource.PlayOneShot(clip);
    }

    // クリック音
    public void PlayClickSE()
    {
        PlaySE(clickSE);
    }

    // カードをめくる、引く音
    public void PlayCardSE()
    {
        PlaySE(cardSE);
    }

    // 引き分け時の音
    public void PlayDrawSE()
    {
        PlaySE(drawSE);
    }

    // 勝利時の音
    public void PlayWinSE()
    {
        PlaySE(winSE);
    }

    // 敗北時の音
    public void PlayLoseSE()
    {
        PlaySE(loseSE);
    }

    // ブラックジャック時の音
    public void PlayBlackjackSE()
    {
        PlaySE(blackjackSE);
    }

    // リタイア時の音
    public void PlayRetireSE()
    {
        PlaySE(retireSE);
    }

    // ゲームオーバー時の音
    public void PlayGameOverSE()
    {
        PlaySE(gameOverSE);
    }
    
}
