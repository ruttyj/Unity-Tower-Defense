using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class PauseController : MonoBehaviour
{

    private bool mGamePaused = false;
    private GameObject mPauseScreen;
    private string mPauseScreenTag;
    [SerializeField] bool mEnabled = true;

    void Start()
    {
        mPauseScreenTag = "PauseScreen";
        mPauseScreen = GameObject.FindGameObjectWithTag(mPauseScreenTag);
        Unpause();
    }

    void Update()
    {
        if (mEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (IsPaused())
                    Unpause();
                else
                    Pause();
            }
        }
    }

    public bool IsPaused()
    {
        return mGamePaused;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        mPauseScreen.SetActive(true);
        mGamePaused = true;
    }

    public void Unpause()
    {
        mPauseScreen.SetActive(false);
        Time.timeScale = 1;
        mGamePaused = false;
    }
}