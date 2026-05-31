using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [Header("続ける/設定/やめるボタンの親オブジェクト")] public GameObject pauesMene;
    [Header("SettingManager")] public SettingManager settingManager;

    private bool isPaused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauesMene.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingManager.isSettingPanelOpen())
                settingManager.closeSettingPanel();

            else if(!isPaused)
                openPauseMenu();
         
            else
                closePauseMenu();
        }
    }

    /// <summary>
    /// ポーズ画面を開く
    /// </summary>
    public void openPauseMenu()
    {
        if (isPaused) return;

        isPaused = true;
        pauesMene.SetActive(true);
        Time.timeScale = 0;
        
    }

    /// <summary>
    /// ポーズ画面を閉じる
    /// </summary>
    public void closePauseMenu() 
    {
        if (!isPaused) return ;

        isPaused = false;
        pauesMene.SetActive(false); 
        Time.timeScale = 1;
    }
}
