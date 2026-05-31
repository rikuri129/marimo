using UnityEngine;

public class SettingManager : MonoBehaviour
{
    [Header("設定画面")] public GameObject settingPanel;

    private bool isSettingOpen = false;     //連打対策するための変数

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        settingPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

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
}
