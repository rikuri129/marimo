using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectStageManager : MonoBehaviour
{
    [Header("フェード")] public FadeDirector Fade;
    [Header("カメラ")]public Camera mainCamera;

    private int selectedStageNum;
    private bool isLoding =false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Fade == null)
        {
            print("エラーです。");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D col = Physics2D.OverlapPoint(mousePos);

        //マウスで選択されたステージへ飛ぶようにする(その際、フェードアウトをしながら移動する)
        if (Input.GetMouseButtonUp(0))
        {
            if (col != null)
            {
                StageTriggerManager trigger = col.GetComponent<StageTriggerManager>();
                
                if(col != null)
                {
                    selectedStageNum = trigger.stageNum;
                    GameManager.instance.stageNum = selectedStageNum;
                    Fade.StartFadeOut();
                }
            }
        }

        if (Fade.IsFadeOutComplete() && !isLoding)
        {
            isLoding = true;
            SceneManager.LoadScene("Stage" + selectedStageNum);
        }


    }
}
