using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    // オブジェクト定義
    public GameObject darkOverlay;   // 暗転処理用のパネル
    public GameObject optionPanel;   // オプション表示パネル
    public GameObject recordPanel;   // 戦績表示パネル
    public GameObject achievePanel;  // 称号表示パネル

    // トグル処理用の定義
    [SerializeField] private RectTransform _bgmHandle;
    [SerializeField] private Toggle _bgmToggle;
    [SerializeField] private Image _bgmBackgroundImage;
    [SerializeField] private Color _bgmBackgroundOnColor, _bgmBackgroundOffColor;

    [SerializeField] private RectTransform _seHandle;
    [SerializeField] private Toggle _seToggle;
    [SerializeField] private Image _seBackgroundImage;
    [SerializeField] private Color _seBackgroundOnColor, _seBackgroundOffColor;

    // 開始時に実行される初期化
    void Start()
    {
        // SE トグルの初期化
        if (_seToggle != null && SEManager.Instance != null)
        {
            _seToggle.isOn = SEManager.Instance.SEEnabled;
            _seHandle.anchoredPosition *= -1.0f;
            _seBackgroundImage.color = _seToggle.isOn ? _seBackgroundOnColor : _seBackgroundOffColor;

            if (_seToggle.isOn) SEManager.Instance.EnableSE();
            else SEManager.Instance.DisableSE();
        }
        
        // BGM トグルの初期化
        if (_bgmToggle != null && BGMManager.Instance != null)
        {
            _bgmToggle.isOn = BGMManager.Instance.BGMEnabled;
            _seHandle.anchoredPosition *= -1.0f;
            _bgmBackgroundImage.color = _bgmToggle.isOn ? _bgmBackgroundOnColor : _bgmBackgroundOffColor;

            if (_bgmToggle.isOn) BGMManager.Instance.PlayBGM();
            else BGMManager.Instance.StopBGM();
        }
    }

    // Optionボタン
    public void OnOptionButton()
    {
        darkOverlay.SetActive(true);
        optionPanel.SetActive(true);
        SEManager.Instance?.PlayClickSE();
    }

    public void OnOptionCloseButton()
    {
        darkOverlay.SetActive(false);
        optionPanel.SetActive(false);
        SEManager.Instance?.PlayClickSE();
    }

    // BGMトグル
    public void ToggleChangedBGM()
    {
        _bgmHandle.anchoredPosition *= -1.0f;

        if (_bgmToggle.isOn)
        {
            _bgmBackgroundImage.color = _bgmBackgroundOnColor;
            BGMManager.Instance?.PlayBGM();
        }
        else
        {
            _bgmBackgroundImage.color = _bgmBackgroundOffColor;
            BGMManager.Instance?.StopBGM();
        }

        SEManager.Instance?.PlayClickSE();
    }

    // SEトグル
    public void ToggleChangedSE()
    {
        _seHandle.anchoredPosition *= -1.0f;

        if (_seToggle.isOn)
        {
            _seBackgroundImage.color = _seBackgroundOnColor;
            SEManager.Instance?.EnableSE();
        }
        else
        {
            _seBackgroundImage.color = _seBackgroundOffColor;
            SEManager.Instance?.DisableSE();
        }

        SEManager.Instance?.PlayClickSE();
    }

    // Recordボタン
    public void OnRecordButton()
    {
        darkOverlay.SetActive(true);
        recordPanel.SetActive(true);
        SEManager.Instance?.PlayClickSE();
    }

    // Recordボタン内のcloseボタン
    public void OnRecordCloseButton()
    {
        darkOverlay.SetActive(false);
        recordPanel.SetActive(false);
        SEManager.Instance?.PlayClickSE();
    }

    // Achieveボタン
    public void OnAchieveButton()
    {
        darkOverlay.SetActive(true);
        achievePanel.SetActive(true);
        SEManager.Instance?.PlayClickSE();
    }

    // Achiveパネル内のcloseボタン
    public void OnAchieveCloseButton()
    {
        darkOverlay.SetActive(false);
        achievePanel.SetActive(false);
        SEManager.Instance?.PlayClickSE();
    }

    // BlackJackボタン
    public void OnBlackJackButton()
    {
        SEManager.Instance?.PlayClickSE();
        SceneManager.LoadScene("BlackjackMenuScene");
    }

    // OldMaidボタン
    public void OnOldMaidButton()
    {
        SEManager.Instance?.PlayClickSE();
        Debug.Log("OldMaid Button Pressed.");
    }
}
