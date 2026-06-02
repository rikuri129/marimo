using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("操作ボタンの親オブジェクト")] public GameObject mobileController;
    [Header("操作ボタンの表示/非表示トグル")] public Toggle toggle;

    private bool hideController = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hideController = PlayerPrefs.GetInt("hideController", 0) == 1;
        mobileController.SetActive(!hideController);
        toggle.SetIsOnWithoutNotify(hideController);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void isCheck(bool isOn)
    {
        hideController = isOn;
        PlayerPrefs.SetInt("hideController", isOn ? 1 : 0);
        mobileController.SetActive(!isOn);
    }
}
