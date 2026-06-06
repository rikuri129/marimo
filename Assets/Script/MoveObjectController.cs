using UnityEngine;

public class MoveObjectController : MonoBehaviour
{
    #region //インスペクターで設定
    [Header("移動経路")] public GameObject[] movePoint;
    [Header("速さ")] public float speed = 1.0f;
    #endregion

    private Rigidbody2D rb = null;
    private int nowPoint = 0;
    private Vector2 oldPostion = Vector2.zero;
    private Vector2 myVelocity = Vector2.zero;
    private bool returnPoint = false;           //折り返し地点かどうか

    void Start()
    {
        //インスタンスを取得
        rb = GetComponent<Rigidbody2D>();
        oldPostion = rb.position;

        //動く床を移動経路の0番目の位置に設定
        if(movePoint != null && movePoint.Length > 0 && rb != null)
        {
            rb.position = movePoint[nowPoint].transform.position;
        }
        else
        {
            Debug.Log("設定が足りていません");
        }
    }


    void FixedUpdate()
    {
        if(movePoint != null && movePoint.Length > 0 && rb != null)
        {
            //通常進行
            if(!returnPoint)
            {
                normalMove();
            }

            //折り返し
            else
            {
                returnMove();
            }
        }

        myVelocity = (rb.position - oldPostion) / Time.deltaTime;   //速度を求める
        oldPostion = rb.position;                                   //前の位置を記録
    }

    /// <summary>
    /// 外部から速度を得られるようにする
    /// </summary>
    /// <returns></returns>
    public Vector2 GetVelocity()
    {
        return myVelocity;
    }

    public void normalMove()
    {
        int nextPoint = nowPoint + 1;

        //目標ポイントとの誤差がわずかになるまで移動する
        if (Vector2.Distance(transform.position, movePoint[nextPoint].transform.position) > 0.1f)
        {
            //speed * Time.deltaTime は道のりを求めている
            Vector2 toVector = Vector2.MoveTowards(transform.position, movePoint[nextPoint].transform.position, speed * Time.deltaTime);

            //次のポイントへ移動
            rb.MovePosition(toVector);
        }

        //誤差が残りわずかになったら床を次のポイントへ移動させる
        else
        {
            rb.MovePosition(movePoint[nextPoint].transform.position);
            ++nowPoint;                                                 //現在いるポイントを更新する

            //現在地が配列の最後だった場合
            if (nowPoint + 1 >= movePoint.Length)
            {
                returnPoint = true;                                     //折り返し地点のフラグを立てる
            }
        }
    }

    public void returnMove()
    {
        int nextPoint = nowPoint - 1;

        //目標ポイントとの誤差がわずかになるまで移動する
        if (Vector2.Distance(transform.position, movePoint[nextPoint].transform.position) > 0.1f)
        {
            //speed * Time.deltaTime は道のりを求めている(道のり = 速さ × 時間)
            Vector2 toVector = Vector2.MoveTowards(transform.position, movePoint[nextPoint].transform.position, speed * Time.deltaTime);

            //次のポイントへ移動
            rb.MovePosition(toVector);
        }

        //誤差が残りわずかになったら床を次のポイントへ移動させる
        else
        {
            rb.MovePosition(movePoint[nextPoint].transform.position);
            --nowPoint;                                                 //現在いるポイントを更新する

            //現在地が配列の最初だった場合
            if (nowPoint <= 0)
            {
                returnPoint = false;                                     //折り返し地点のフラグを立てる
            }
        }
    }
}
