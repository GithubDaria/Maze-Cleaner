using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenManag : MonoBehaviour
{
    public GameObject ReplayButtonOn;
    public GameObject ReplayButtonOff;
    public GameObject NoMoreBonusReplayButtonOff;
    public GameObject WatchAdsScreen;

    private bool AddWasUsed;

    [SerializeField] private TextMeshProUGUI Score;
    [SerializeField] private TextMeshProUGUI Level;

    [SerializeField] private RewardedAds rewardedAds;

    [SerializeField] private Timer timer;
    [SerializeField] private TextMeshProUGUI TittleResultText;
    [SerializeField] private TextMeshProUGUI ScoreText;

    [Header("No time")]
    [SerializeField] private GameObject NotimeFreeReplay;
    [SerializeField] private GameObject NotimeAddReplay;
    [SerializeField] private GameObject NoTimeNoReplay;

    [SerializeField] private Button NoMoreReplaceBackClose;

    [Header("Animation")]
    [SerializeField] private Animator ReplayAppers;


    public void SetGameOverScreen(bool iswin, int score)
    {
        if (iswin)
        {
            TittleResultText.text = "Finished";
        }
        else
        {
            TittleResultText.text = "Game Over";
        }
        ScoreText.text = score.ToString();
    }
    public void CanSafeReplay(int replaycount)
    {
        if(replaycount == 2)
        {
            NoTimeNoReplay.SetActive(true);
            NoMoreReplaceBackClose.enabled = false;
        }
        else if (replaycount == 1 && !AddWasUsed)
        {
            NotimeAddReplay.SetActive(true);
        }
        else
        {
            NotimeFreeReplay.SetActive(true);
        }
    }
    public void StartGameAgain()
    {

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex);
    }
    public void ExitToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void GetFreeReplay()
    {
        timer.PauseTimer();
        AdsManager.Instance.rewardedAds.ShowRewardedAd();
    }
    public void ApplyAddReward()
    {
        Debug.Log("Apply reward");
        timer.ResumeTimer();
        ReplayButtonOn.transform.localScale = new Vector3(0, 0, 0);
        ReplayButtonOn.SetActive(true);
        ReplayAppers.SetTrigger("AppearingReplay");

        ReplayButtonOff.SetActive(false);
        WatchAdsScreen.SetActive(false);
    }
    public void RestoreReplay()
    {
        ReplayButtonOn.transform.localScale = new Vector3(0,0,0);
        ReplayButtonOn.SetActive(true);
        ReplayAppers.SetTrigger("AppearingReplay");

        ReplayButtonOff.SetActive(false);
        NoMoreBonusReplayButtonOff.SetActive(false);
    }
    public void ExtraReplayUsed()
    {
        NoMoreBonusReplayButtonOff.SetActive(true);
        MakeReplaySmall();
       // ReplayButtonOn.SetActive(false);
        ReplayAppers.SetTrigger("makesmall");

        ReplayButtonOff.SetActive(false);

    }
    public void ChangeScoreUI(int score)
    {
        Score.text = "Score " + score.ToString();

    }
    public void ChangeLevelUi(int level)
    {
        Level.text = "Level " + level.ToString();

    }
    public void MakeReplaySmall()
    {
        ReplayAppers.SetTrigger("makesmall");

    }

}
