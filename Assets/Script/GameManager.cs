using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region //インスペクターで設定
    public static GameManager instance = null;                   //この変数はGameManagerスクリプトのインスタンスが入る空箱
    [Header("スコア")] public int score;                         //スコア
    [Header("現在のステージ")] public int stageNum;              //今いるステージ
    [Header("現在の復帰位置")] public int continueNum;           //復帰する場所
    [Header("現在の残機")] public int heartNum;                  //残機
    [Header("デフォルトの残機数")] public int defaultHeartNum;   //デフォルトの残機数
    [Header("ステージの時間制限")] public float defaultTime;     //ステージの制限時間
    //[Header("オーディオ管理スクリプト")] public AudioManager audioManager;
    [HideInInspector] public bool isGameOver;                    //ゲームオーバーかどうか
    [HideInInspector] public bool isStageClear;                  //ゲームクリアかどうか
    #endregion

    private AudioSource audioSource = null;

    private void Awake()
    {
        //instan 変数が空箱なら
        if (instance == null)
        {
            instance = this;                    //このインスタンスのアドレスを入れる
            heartNum = defaultHeartNum;
            DontDestroyOnLoad(this.gameObject);
        }

        //すでにインスタンスが存在する場合
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        //インスタンスを取得する
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 残機数を１増やす
    /// </summary>
    public void AddHeartNum()
    {
        //上限より小さければ
        if(heartNum < 99)
        {
            ++heartNum;
        }
    }

    /// <summary>
    /// 残機を一つ減らす
    /// </summary>
    public void SubHeartNum()
    {
        //残機数が０より大きければ、残機数を１減らす
        if (heartNum > 0)
        {
            --heartNum;
        }

        //そうでなければ、ゲームオーバーフラグを立てる
        else
        {
            isGameOver = true;

        }
    }

    /// <summary>
    /// ゲームをリトライする
    /// </summary>
    public void RetryGame()
    {
        //それぞれの設定をリセットする
        isGameOver = false;
        heartNum = defaultHeartNum;
        score = 0;
        continueNum = 0;
    }

    /// <summary>
    /// 音(SE)を鳴らす
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySE(AudioClip clip)
    {
        if(audioSource != null)
        {
            audioSource.PlayOneShot(clip, AudioManager.instance.getSEVolume());
        }
        else
        {
            Debug.Log("SEが設定されていません");
        }
    }
}
