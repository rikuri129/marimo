using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("続ける/設定/やめるボタンの親オブジェクト")] public GameObject pauesMene;
    [Header("SettingManager")] public SettingManager settingManager;
    [Header("FadeDirector")] public FadeDirector fadeDirector;

    private bool isPaused = false;
    private bool pushQuite = false;

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

        //ステージの状態をリセット&タイトル画面に戻る
        if (pushQuite && fadeDirector.IsFadeOutComplete()) 
        {
            Time.timeScale = 1;
            GameManager.instance.RetryGame();
            SceneManager.LoadScene("TitleScene");
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

    public void quiteGame()
    {
        if(!pushQuite)
        {
            pushQuite = true;
            fadeDirector.StartFadeOut();
        }
    }
}
