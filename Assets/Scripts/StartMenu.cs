using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    // オブジェクト定義
    public GameObject darkOverlay;   // 暗転処理用のパネル
    public GameObject optionPanel;   // オプション表示パネル
    public GameObject recordPanel;   // 戦績表示パネル
    public GameObject achievePanel;  // 称号表示パネル

    // 開始時に実行されるメソッド
    void Start()
    {
        Debug.Log("hello world");
    }

    // Optionボタンを押したときに実行されるメソッド
    public void OnOptionButton()
    {
        // 背景を暗くする処理
        darkOverlay.SetActive(true);
        // Panelを表示する処理
        optionPanel.SetActive(true);
    }

    // OptionPanel内のcloseボタンを押したときに実行されるメソッド
    public void OnOptionCloseButton()
    {
        // 背景を明るくする処理
        darkOverlay.SetActive(false);
        // Panelを閉じる処理
        optionPanel.SetActive(false);

    }

    // Recordボタンを押したときの処理
    public void OnRecordButton()
    {
        // 背景を暗くする処理
        darkOverlay.SetActive(true);
        // Panelを表示する処理
        recordPanel.SetActive(true);
    }

    // RecordPanel内のcloseボタンを押したときに実行されるメソッド
    public void OnRecordCloseButton()
    {
        // 背景を明るくする処理
        darkOverlay.SetActive(false);
        // Panelを閉じる処理
        recordPanel.SetActive(false);
    }

    // Recordボタンを押したときの処理
    public void OnAchieveButton()
    {
        // 背景を暗くする処理
        darkOverlay.SetActive(true);
        // Panelを表示する処理
        achievePanel.SetActive(true);
    }

    // RecordPanel内のcloseボタンを押したときに実行されるメソッド
    public void OnAchieveCloseButton()
    {
        // 背景を明るくする処理
        darkOverlay.SetActive(false);
        // Panelを閉じる処理
        achievePanel.SetActive(false);
    }

    // BlackJackボタンを押したときに実行されるメソッド
    public void OnBlackJackButton()
    {
        // BlackjackMenu画面に遷移
        SceneManager.LoadScene("BlackjackMenuScene");
    }

    public void OnOldMaidButton()
    {
        Debug.Log("ボタンが押されました！");

    }

}
