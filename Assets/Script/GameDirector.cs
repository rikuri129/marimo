using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    #region //インスペクターで設定
    [Header("フェード")] public FadeDirector Fade;

    #endregion

    #region //プライベート変数
    private bool firstPush = false;     //連打対策するための変数
    private bool GoNextScene = false;   //次のシーンに行ったかどうか
    #endregion

    /// <summary>
    /// フェードアウトを開始
    /// </summary>
    public void PushStart()
    {
        //次のシーンへ移行する処理を追加する(1回のみ)
        if(firstPush == false)
        {
            firstPush = true;

            Fade.StartFadeOut();        //ボタンが押されたらフェードアウトを開始
        }
    }

    public void Update()
    {
        if (!GoNextScene && Fade.IsFadeOutComplete())
        {
            SceneManager.LoadScene("SelectStageScene");
            GoNextScene = true;
        }
    }
}
