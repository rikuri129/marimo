using UnityEngine;
using UnityEngine.UI;

public class FadeDirector : MonoBehaviour
{
    #region //インスペクターで設定
    [Header("最初からフェードインが完了しているかどうか")] public bool FirstFadeInComp;
    #endregion

    #region //プライベート変数  
    private Image img = null;
    private float FrameCount = 0.0f;            //アクティブになってから何フレーム経ったかカウントする
    private float timer = 0.0f;
    private bool FadeIn = false;                //フェードインしたかどうかを判定
    private bool FadeOut = false;               //フェードアウトしたかどうかを判定
    private bool CompFadeIn = false;            //フェードインが完了したかどうかを判定
    private bool CompFadeOut = false;           //フェードアウトが完了したかどうかを判定
    #endregion

    void Start()
    {
        //インスタンスを取得
        img = GetComponent<Image>();

        //FirstFadeInComp が true ならフェードイン完了の処理をする
        if(FirstFadeInComp)
        {
            FadeInComplete();
        }

        //そうでなければ、フェードインを開始
        else
        {
            StartFadeIn();
        }
    }

    void Update()
    {
        //2フレーム(シーン開始時は秒数よりフレームごとに処理したほうが良い)より大きければ
        if(FrameCount > 2)
        {
            if (FadeIn)
            {
                FadeInUpdate();
            }

            else if(FadeOut)
            {
                FadeOuntUpdate();
            }
        }

        ++FrameCount;       //フレームを加算する
    }

    /// <summary>
    /// フェードイン中の処理
    /// </summary>
    public void StartFadeIn()
    {
        if(FadeIn || FadeOut)
        {
            return;
        }

        FadeIn = true;          
        CompFadeIn = false;
        img.color = new Color(1, 1, 1, 1);
        img.fillAmount = 1;
        timer = 0.0f;
        img.raycastTarget = true;
    }

    /// <summary>
    /// フェードインが完了したかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsFadeInComplete()
    {
        return CompFadeIn;
    }

    /// <summary>
    /// フェードアウト中の処理
    /// </summary>
    public  void StartFadeOut()
    {
        //フェード中なら何もしない
        if (FadeIn || FadeOut)
        {
            return;
        }

        FadeOut = true;
        CompFadeOut = false;
        img.color = new Color(1, 1, 1, 0);
        img.fillAmount = 0;
        timer = 0.0f;
        img.raycastTarget = true;
    }

    /// <summary>
    /// フェードアウトが完了したかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsFadeOutComplete()
    {
        return CompFadeOut;
    }

    /// <summary>
    /// 2フレームごとにフェードインしていく
    /// </summary>
    private void FadeInUpdate()
    {
        //フェード中
        if (timer < 1.0f)                                    //１秒でフェードするようにする
        {
            img.color = new Color(1, 1, 1, 1 - timer);     // (赤, 緑, 青, 透明度) となっている(透明度は, 0に近いほど透明になる)
            img.fillAmount = 1 - timer;
        }

        //フェード完了
        else
        {
            FadeInComplete();
        }

        timer += Time.unscaledDeltaTime;                    //タイマーを加算する
    }

    /// <summary>
    /// フェードイン完了後の処理をする
    /// </summary>
    private void FadeInComplete()
    {
        img.color = new Color(1, 1, 1, 0);      //終わったら値をしっかり指定してあげる
        img.fillAmount = 0;
        img.raycastTarget = false;              //アタッチされている UGUI の当たり判定をオフにする

        timer = 0.0f;                           //それぞれのフラグをセットする
        FadeIn = false;
        CompFadeIn = true;
    }

    /// <summary>
    /// 2フレームごとにフェードアウトしていく
    /// </summary>
    private void FadeOuntUpdate()
    {
        //フェードアウト中
        if (timer < 1.0f)                                    //１秒でフェードするようにする
        {
            img.color = new Color(1, 1, 1, timer);     // (赤, 緑, 青, 透明度) となっている(透明度は, 0に近いほど透明になる)
            img.fillAmount = timer;
        }

        //フェードアウト完了
        else
        {
            FadeOuntComplete();
        }

        timer += Time.unscaledDeltaTime;                    //タイマーを加算する
    }

    /// <summary>
    /// フェードアウト完了後の処理をする
    /// </summary>
    private void FadeOuntComplete()
    {
        img.color = new Color(1, 1, 1, 1);      //終わったら値をしっかり指定してあげる
        img.fillAmount = 1;
        img.raycastTarget = false;              //アタッチされている UGUI の当たり判定をオフにする

        timer = 0.0f;                           //それぞれのフラグをセットする
        FadeOut = false;
        CompFadeOut = true;
    }
}
