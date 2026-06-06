using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    [Header("設定画面")] public GameObject settingPanel;
    [Header("BGMのスライダー")] public Slider bgmSlider;
    [Header("SEのスライダー")] public Slider seSlider;
    [Header("BGMの音量テキスト")] public TMP_Text bgmVolNum;
    [Header("SEの音量テキスト")] public TMP_Text seVolNum;

    private bool isSettingOpen = false;     //連打対策するための変数

    public void Start()
    {
        float bgm = PlayerPrefs.GetFloat("bgmVolume", 1);
        float se = PlayerPrefs.GetFloat("seVolume", 1);

        settingPanel.SetActive(false);
        bgmSlider.value = bgm;
        seSlider.value = se;
        bgmVolNum.text = (bgm * 100).ToString("0");
        seVolNum.text = (se * 100).ToString("0");
    }

    /// <summary>
    /// 設定ボタンを押した際の処理
    /// </summary>
    public void pushSetting()
    {
        if (isSettingOpen) return;
        openSettingPanel();
    }

    /// <summary>
    /// closeボタンを押した際の処理
    /// </summary>
    public void pushClose()
    {
        if (!isSettingOpen) return;
        closeSettingPanel();
    }

    /// <summary>
    /// 設定画面を開く
    /// </summary>
    public void openSettingPanel()
    {
        isSettingOpen = true;
        settingPanel.SetActive(true);
    }

    /// <summary>
    /// 設定画面を閉じる
    /// </summary>
    public void closeSettingPanel()
    {
        isSettingOpen = false;
        settingPanel.SetActive(false);
    }

    /// <summary>
    /// 設定画面が開いているかどうかを調べる
    /// </summary>
    /// <returns></returns>
    public bool isSettingPanelOpen()
    {
        return isSettingOpen;
    }

    public void setBGMVolNum(float volume)
    {
        AudioManager.instance.setBGMVolume(volume);
        Debug.Log("てきすとをへんこうします");
        bgmVolNum.text = (volume * 100).ToString("0");
    }

    public void setSEVolNum(float volume)
    {
        AudioManager.instance.setSEVolume(volume);
        seVolNum.text = (volume * 100).ToString("0");
    }
}
