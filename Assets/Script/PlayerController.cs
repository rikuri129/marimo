using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region //インスペクターで設定する
    [Header("移動速度")] public float speed;                         //スピード調整変数(具体的な数字はインスペクターで設定)
    [Header("重力")] public float gravity;                           //重力変数
    [Header("踏みつけ判定の高さの割合")] public float StepOnRate;    //踏みつけ判定の高さ割合
    [Header("接地判定")] public GroundCheckDirector ground;          //接地判定スクリプトを参照する変数
    [Header("頭をぶつけた判定")] public GroundCheckDirector head;    //頭をぶつけた判定
    [Header("ダッシュの加減速")] public AnimationCurve DashCurve;    //ダッシュの加減速
    [Header("ジャンプするときに鳴らすSE")] public AudioClip jumpSE;
    [Header("敵にあたったときに鳴らすSE")] public AudioClip damageSE;
    [Header("小ジャンプ時間")] public float minijumptime;
    [Header("中ジャンプ時間")] public float centjumptime;
    [Header("大ジャンプ時間")] public float maxjumptime;
    [Header("小ジャンプ")] public float miniforce;
    [Header("中ジャンプ")] public float centforce;
    [Header("大ジャンプ")] public float maxforce;
    [Header("ジャンプ時間")] public float JumpTimeLimit;
    [Header("ジャンプ速度")] public float JumpSpeed;
    [Header("Rボタン")] public RLButtonDirector RButton;
    [Header("Lボタン")] public RLButtonDirector LButton;
    [Header("ジャンプボタン")] public RLButtonDirector JumpButton;
    #endregion

    #region //プライベート変数
    private Animator anim = null;                   //Animator のインスタンスを取得
    private Rigidbody2D rb = null;                  //Rigidbody2D のインスタンスを取得
    private CapsuleCollider2D capcol = null;        //CapsuleCollider2D のインスタンスを取得
    private SpriteRenderer sr = null;               //SpriteRenderer のインスタンスを取得
    private MoveObjectController moveObj = null;    //MoveObjectController のインスタンスを取得
    private Vector3 defaultScal;                    //プレーヤの大きさを得る
    private bool isGround = false;                  //設置判定の状態を入れておく変数
    private bool isHead = false;                    //頭をぶつけた判定をする
    private bool isEnemyGround = false;
    private bool isJump = false;                    //ジャンプ判定をする変数
    private bool isOtherJump = false;               //別のジャンプ判定(踏みつけたとき)
    private bool isRun = false;                     //走っているかを判定
    private bool isDown = false;                    //ダウンしているかを判定
    private bool isContinue = false;                //コンティニューしたかどうか
    private bool isBlink = false;
    private bool nonDownAnim = false;               //ダウンアニメーションを再生するかどうか
    private bool isClearMotion = false;             //クリアアニメーションを再生するかどうか
    private bool isCharging = false;
    private float chargTime = 0.0f;
    private float continueTime = 0.0f;              //コンティニューしてからの経過時間
    private float blinkTime = 0.0f;                 //プレーヤを点滅させるための時間
    private float DashTime = 0.0f;                  //ダッシュ時間を測る
    private float BeforeKey = 0.0f;                 //前回のキー入力を記録
    private string EnemyTag = "Enemy";              //Enemy タグ
    private string deadAreaTag = "DeadArea";        //DeadArea タグ
    private string hitAreaTag = "HitArea";          //HitArea タグ
    private string moveFloorTag = "MoveFloor";      //MoveFloor タグ
    private string fallFloorTag = "FallFloor";      //FallFloor タグ
    #endregion

    void Start()
    {
        Application.targetFrameRate = 60;           //フレームレートを固定する

        //それぞれのインスタンスを取得
        anim = GetComponent<Animator>();           
        rb = GetComponent<Rigidbody2D>();          
        capcol = GetComponent<CapsuleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        defaultScal = transform.localScale;
    }

    void FixedUpdate()
    {
        //ダウン中/ゲームオーバー中/クリアでないときのみ実行する
        if (!isDown && !GameManager.instance.isGameOver && !GameManager.instance.isStageClear)
        {
            //設置判定を得る
            isGround = ground.IsGround();
            isEnemyGround = ground.IsEnemyGround();
            isHead = head.IsGround();

            //着地したらジャンプ状態をfalseにする
            if ((isGround || isEnemyGround) && rb.linearVelocity.y <= 0)
            {
                isOtherJump = false;
                isJump = false;
            }

            float xspeed = GetXSpeed();         //x方向の速度をゲットする
            float yspeed = GetYSpeed(); ; //y方向の速度をゲットする7

            //アニメーションを適用
            SetAnimation();

            //動く床にいるとき動く床の速度を取得する
            Vector2 addVelocity = Vector2.zero;
            if(moveObj != null)
            {
                addVelocity = moveObj.GetVelocity();
            }

            rb.linearVelocity = new Vector2(xspeed + addVelocity.x, yspeed);      //プレーヤの速度を適用する(動く床の速度も足す. いないときは0のまま)

        }
        else
        {
            if(!isClearMotion && GameManager.instance.isStageClear)
            {
                anim.Play("Stand");                         //クリアアニメーションを再生する(まだ用意できていないためスタンドアニメーション)
                isClearMotion = true;                       //クリアアニメーションフラグを立てる
            }

            rb.linearVelocity = new Vector2(0, -gravity);
        }

    }

    private void Update()
    {
        //コンティニューフラグがオンのとき
        if(isContinue)
        {
            //blinkTime が 0.2 より大きいとき、全ての状態をリセットする
            if(blinkTime > 0.2f)
            {
                sr.enabled = true;      
                blinkTime = 0.0f;       
            }

            //0.1 より大きいとき
            else if(blinkTime > 0.1f)
            {
                sr.enabled = false;     //プレーヤを消す
            }

            //それ以外のとき
            else
            {
                sr.enabled = true;      //プレーヤを表示
            }

            //1秒経ったら点滅を終わる
            if(continueTime > 1.0f)
            {
                isBlink = false;
                isContinue = false;
                blinkTime = 0.0f;
                continueTime = 0.0f;
                sr.enabled = true;
            }

            //そうでなければそれぞれの秒数を加算
            else
            {
                isBlink = true;
                blinkTime += Time.deltaTime;
                continueTime += Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// コンティニュー待機状態か
    /// </summary>
    /// <returns></returns>
    public bool IsContinueWaiting()
    {
        if(GameManager.instance.isGameOver)
        {
            return false;
        }

        else
        {
            return isDownAnimComplete() || nonDownAnim;
        }
    }

    /// <summary>
    /// プレーヤをコンティニューさせる
    /// </summary>
    public void ContinuePlayer()
    {
        //ダウン状態から復帰するためにそれぞれのフラグをリセットする
        isDown = false;
        anim.Play("Stand");
        isJump = false;
        isOtherJump = false;
        isRun = false;
        nonDownAnim = false;

        //コンティニューフラグを立てる
        isContinue = true;
    }

    /// <summary>
    /// y成分で必要な計算をし、速度を返す
    /// </summary>
    /// <returns>y軸の速さ</returns>
    private float GetYSpeed()
    {
        CapsuleCollider2D collision = null;
        collision = gameObject.GetComponent<CapsuleCollider2D>();
        
        //チャージ開始時
        if (isGround && (Input.GetKeyDown(KeyCode.Space) || JumpButton.IsPush()))
        {
                isCharging = true;
                chargTime = 0f;
        }
        //チャージ中
        if(isCharging)
        {
            //ボタンを押している最中の処理
            if ((Input.GetKey(KeyCode.Space) || JumpButton.IsHold()))
            {
                chargTime += Time.deltaTime;

                //チャージ時間が大ジャンプ判定の時間以上になったら自動でジャンプ
                if (chargTime >= maxjumptime)
                {
                    GameManager.instance.PlaySE(jumpSE);
                    rb.AddForce(Vector2.up * maxforce, ForceMode2D.Impulse);
                    isJump = true;
                    isCharging = false;
                    chargTime = 0f;
                }

            }
            //ボタンを離したときの処理
            else if ((Input.GetKeyUp(KeyCode.Space) || JumpButton.IsNotPush()))
            {
                GameManager.instance.PlaySE(jumpSE);

                if (chargTime < minijumptime)
                {
                    rb.AddForce(Vector2.up * miniforce, ForceMode2D.Impulse);
                }
                else if (chargTime < centjumptime)
                {
                    rb.AddForce(Vector2.up * centforce, ForceMode2D.Impulse);
                }
                else
                {
                    rb.AddForce(Vector2.up * maxforce, ForceMode2D.Impulse);
                }
                isJump = true;
                isCharging = false;
                chargTime = 0f;
            }
        }

        return rb.linearVelocity.y;
    }

    /// <summary>
    /// x成分で必要な計算をし、速度を返す
    /// </summary>
    /// <returns>x軸の速さ</returns>
    private float GetXSpeed()
    {
        float HorizontalKey = Input.GetAxis("Horizontal");      //Input Manager の Horizontal にあるキーを参照する
        float xspeed = 0.0f;

        //右方向入力がされたとき
        if (HorizontalKey > 0 || RButton.IsPush())
        {
            transform.localScale = new Vector3(defaultScal.x, defaultScal.y, defaultScal.z);        //右向きのときは身体を右に向ける
            isRun = true;
            xspeed = speed;                                                                         //右入力なら正の速度
            DashTime += Time.deltaTime;                                                             //ダッシュ中は時間を測る
        }

        //左方向入力がされたとき
        else if (HorizontalKey < 0 || LButton.IsPush())
        {
            transform.localScale = new Vector3(-defaultScal.x, defaultScal.y, defaultScal.z);       //左向きのときは身体を左に向ける
            isRun = true;
            xspeed = -speed;                                                                        //左入力なら負の速度
            DashTime += Time.deltaTime;                                                             //ダッシュ中は時間を測る
        }

        //何入力もないとき
        else
        {
            isRun = false;
            xspeed = 0.0f;                                      //何も入力がされていないなら速度を0にする
            DashTime = 0.0f;                                    //ダッシュを辞めたら時間をリセット
        }

        //キー入力が反転した場合はダッシュ時間をリセット
        if (HorizontalKey > 0 && BeforeKey < 0)
        {
            DashTime = 0.0f;
        }

        else if (HorizontalKey < 0 && BeforeKey > 0)
        {
            DashTime = 0.0f;
        }

        BeforeKey = HorizontalKey;      //前回のキー入力を記録

        //アニメーションカーブを速度に適用
        xspeed *= DashCurve.Evaluate(DashTime);

        return xspeed;
    }

    /// <summary>
    /// アニメーションを設定する
    /// </summary>
    private void SetAnimation()
    {
        anim.SetBool("Jump", isJump || isOtherJump);
        anim.SetBool("Run", isRun);
    }

    /// <summary>
    /// ダウンアニメーションが完了しているかどうか
    /// </summary>
    /// <returns></returns>
    private bool isDownAnimComplete()
    {
        if (isDown && anim != null)
        {
            AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);       //何のアニメーションを再生しているかを取得

            if (currentState.IsName("Down"))
            {
                if (currentState.normalizedTime >= 1.0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 接触判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //それぞれのフラグを用意
        bool isEnemyTag = (collision.collider.tag == EnemyTag);
        bool isMoveFloor = (collision.collider.tag == moveFloorTag);
        bool isFallFloor = (collision.collider.tag == fallFloorTag);

        if (isEnemyTag || isMoveFloor || isFallFloor)
        {
            float StepOnHeight = (capcol.size.y * (StepOnRate / 100f));                     //踏みつけ判定になる高さ
            float JudgePos = transform.position.y - (capcol.size.y / 2f) + StepOnHeight;    //踏みつけ判定のワールド座標

            foreach(ContactPoint2D p in collision.contacts)
            {
                //接触した位置が踏みつけ判定になる位置より下だったらもう一度跳ねる
                if(p.point.y < JudgePos)
                {
                    if (isEnemyTag || isFallFloor)
                    {
                        BoundDirector bd = collision.gameObject.GetComponent<BoundDirector>();

                        if (bd != null)
                        {
                            //敵を踏んだら
                            if (isEnemyTag)
                            {
                                bd.PlayerStepOn = true;
                                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
                                rb.AddForce(Vector2.up * bd.BoundHeight, ForceMode2D.Impulse);
                                isJump = true;
                            }

                            //落ちる床を踏んだら
                            else if(isFallFloor)
                            {
                                bd.PlayerStepOn = true;
                            }
                        }
                        else
                        {
                            Debug.Log("BoundDirectorがアタッチされていません");
                        }
                    }
                    else if(isMoveFloor)
                    {
                        moveObj = collision.gameObject.GetComponent<MoveObjectController>();
                    }
                }

                //そうでなければ RecieveDamage の引数を true にする
                else
                {
                    if (isEnemyTag)
                    {
                        RecieveDamege(true);
                        break;
                    }
                }
            }
        }

        //動く床を踏んだ時のみ MoveObjectController を取得する
        else if (isMoveFloor)
        {
            float StepOnHeight = (capcol.size.y * (StepOnRate / 100f));                     //踏みつけ判定になる高さ
            float JudgePos = transform.position.y - (capcol.size.y / 2f) + StepOnHeight;    //踏みつけ判定のワールド座標
        }
    }

    /// <summary>
    /// 動く床から離れたとき、取得したスクリプト MoveObjectController を離す
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.collider.tag == moveFloorTag)
        {
            moveObj = null;
        }
    }

    /// <summary>
    /// 落下した場合ととげにあたった場合の処理
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //落下したときは RecieveDamage の引数を false(ダウンアニメーションを再生しない)にする
        if (collision.tag == deadAreaTag)
        {
            RecieveDamege(false);
        }

        //とげにあたった場合は RecieveDamage の引数を true(ダウンアニメーションを再生する)にする
        else if (collision.tag == hitAreaTag)
        {
            RecieveDamege(true);
        }
    }
   
    /// <summary>
    /// 敵にあたったときの処理
    /// </summary>
    private void RecieveDamege(bool downAnim)
    {
        //やられている最中やクリア中はダメージを受けないようにする
        if (isDown || GameManager.instance.isStageClear || isBlink)
        {
            return;
        }
        else
        {
            //アニメーションフラグが true ならアニメーションを再生
            if(downAnim)
            {
                GetComponent<ParticleSystem>().Play();      //パーティクルを表示する
                anim.Play("Down");                          //ダウンアニメーションを実行
                GameManager.instance.PlaySE(damageSE);
            }
            else
            {
                nonDownAnim = true;                         //そうでなければ再生しないフラグを立てる
            }
            
            isDown = true;                              //ダウンフラグを立てる
            GameManager.instance.SubHeartNum();         //残機数を減らす
        }
    }

    public bool DownJudge()
    {
        return isDown;
    }
}
