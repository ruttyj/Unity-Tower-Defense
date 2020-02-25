using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class GameOverController : MonoBehaviour
{

    private bool mGamePaused = false;
    private GameObject mGameOverScreen;
    private string mGameOverScreenTag;
    [SerializeField] bool mEnabled = true;

    void Start()
    {
        mGameOverScreenTag = "GameOverScreen";
        mGameOverScreen = GameObject.FindGameObjectWithTag(mGameOverScreenTag);
        Unpause();
    }

    public void GameOver()
    {
        if (!IsPaused())
            Pause();
    }

    public bool IsPaused()
    {
        return mGamePaused;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        mGameOverScreen.SetActive(true);
        mGamePaused = true;
    }

    public void Unpause()
    {
        mGameOverScreen.SetActive(false);
        Time.timeScale = 1;
        mGamePaused = false;
    }
}