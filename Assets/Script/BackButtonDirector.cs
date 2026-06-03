using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonDirector : MonoBehaviour
{
    #region
    private bool firstPush =false;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (firstPush == false)
            {
                backPush();

            }
        }
    }

    public void backPush()
    {
        if(firstPush == false)
        {
            firstPush = true;
            SceneManager.LoadScene("TitleScene");
        }
        //firstPush = false;
    }
}
