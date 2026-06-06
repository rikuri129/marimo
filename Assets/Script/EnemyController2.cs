//using Unity.Android.Gradle.Manifest;
using UnityEngine;
using System.Collections;

public class EnemyController2 : MonoBehaviour
{
    #region //インスペクターで設定
    [Header("加算するスコア")] public int myScore;                   //敵を倒したときに加算するスコアの値
    [Header("画面外でも行動するか")] public bool nonVisible = false; //画面外でも動くか決める
    [Header("移動経路")] public GameObject[] movePoint;
    [Header("速さ")] public float speed = 1.0f;
    [Header("重力")] public float gravity;                           //重力変数
    #endregion

    #region //プライベート変数
    private Rigidbody2D rb = null;                                   //Rigidbody2Dのインスタンスにアクセスする変数
    private SpriteRenderer sr = null;
    private BoundDirector bd = null;
    private EnemyController en = null;
    private MoveObjectController moc = null;
    private CapsuleCollider2D capcol = null;
    private Animator anim = null;
    private Transform t = null;                                     //Enemyについてる子オブジェクトにアクセスるための変数
    private string deadAreaTag = "DeadArea";
    private Vector3 defaultScal;
    private int nowPoint = 0;
    private Vector2 oldPostion = Vector2.zero;
    private bool isDead = false;
    private bool returnPoint = false;           //折り返し地点かどうか
    #endregion

    void Start()
    {
        //それぞれのインスタンスを取得
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        bd = GetComponent<BoundDirector>();
        en = GetComponent<EnemyController>();
        moc = GetComponent<MoveObjectController>();
        capcol = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        t = transform.Find("ColliderObject");
        defaultScal = transform.localScale;
        oldPostion = rb.position;

        //敵を移動経路の0番目の位置に設定
        if (movePoint != null && movePoint.Length > 0 && rb != null)
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
        if(bd.PlayerStepOn)
        {
            isStepOn("Enemy_updown_Dead");
        }

        else if(!bd.PlayerStepOn)
        {
            if (movePoint != null && movePoint.Length > 0 && rb != null)
            {
                //通常進行
                if (!returnPoint)
                {
                    normalMove();
                }

                //折り返し
                else
                {
                    returnMove();
                }
            }
            oldPostion = rb.position;                                   //前の位置を記録
            
        }

    }

    public void normalMove()
    {
        int nextPoint = nowPoint + 1;

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

    public void isStepOn(string animationPlay)
    {
        if (!isDead)
        {
            anim.Play(animationPlay);
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = new Vector2(0, -gravity);
            isDead = true;

            //ColliderObjectの子オブジェクトをすべて非アクティブにする
            if (t != null)
            {
                t.gameObject.SetActive(false);
            }

            if (GameManager.instance != null)
            {
                GameManager.instance.score += 10;   //プレーヤのスコアを加算する
            }

            StartCoroutine(disableEnemy()); //3秒後に非アクティブ化
        }

        else
        {
            transform.Rotate(0, 0, 5);
        }
    }

    /// <summary>
    /// オブジェクトを非アクティブにする
    /// </summary>
    /// <returns></returns>
    private IEnumerator disableEnemy()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }

    public bool DeadJudge()
    {
        return isDead;
    }
}
