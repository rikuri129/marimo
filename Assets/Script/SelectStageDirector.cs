using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectStageDirector : MonoBehaviour
{
    [Header("フェード")] public FadeDirector Fade;

    public GameObject mainCamera;
    public int selectStage;

    private static int selectedStageNum;
    private bool isLoading = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(Fade == null)
        {
            print("エラーです。");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Camera cam = mainCamera.GetComponent<Camera>();

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Collider2D col = Physics2D.OverlapPoint(mousePos);

        //マウスで選択されたステージへ飛ぶようにする(その際、フェードアウトをしながら移動する)
        if(Input.GetMouseButtonUp(0))
        {
            if (col == this.GetComponent<Collider2D>())
            {
                selectedStageNum = selectStage;
                print(selectedStageNum);
                GameManager.instance.stageNum = selectedStageNum;
                Fade.StartFadeOut();
            }
        }
        
        if (!isLoading && Fade.IsFadeOutComplete())
        {
            isLoading = true;
            Debug.Log("Fade Complete");
            Debug.Log("Load : Stage" + selectedStageNum);
            SceneManager.LoadScene("Stage" + selectedStageNum);
        }
        

    }
}
