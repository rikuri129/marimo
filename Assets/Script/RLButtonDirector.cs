using UnityEngine;

public class RLButtonDirector : MonoBehaviour
{
    private bool isPush = false;
    private bool isDown = false;
    private bool isUp = false;      

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Input.GetKeyDownのようなイメージ
    /// </summary>
    public void OnButtonDown()
    {
        isPush = true;
        isDown = true;
    }

    /// <summary>
    /// Input.GetKeyUpのようなイメージ
    /// </summary>
    public void OnButtonUp()
    {
        isPush= false;
        isUp = true;
    }

    /// <summary>
    /// Input.GetKeyのようなイメージ
    /// </summary>
    /// <returns></returns>
    public bool IsPush()
    {
        bool temp = isDown;
        isDown = false;
        return temp;
    }

    /// <summary>
    /// ボタンを離したか判定する用
    /// </summary>
    /// <returns></returns>
    public bool IsNotPush()
    {
        bool temp = isUp;
        isUp = false;
        return temp;
    }

    /// <summary>
    /// ボタンを押しているか判定するよう
    /// </summary>
    /// <returns></returns>
    public bool IsHold()
    {
        return isPush;
    }
}
