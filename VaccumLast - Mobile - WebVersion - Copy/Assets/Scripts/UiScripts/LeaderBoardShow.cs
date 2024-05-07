using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardShow : MonoBehaviour
{
    private Dictionary<string, int> scoreDictionary = new Dictionary<string, int>();
    [SerializeField] private GameObject SpecialLeaderBoardPrefab;
    [SerializeField] private GameObject LeaderBoardPrefab;
    [SerializeField] private TextMeshProUGUI CurrentPlayerPosition;
    [SerializeField] private TextMeshProUGUI LoadingText;
    [SerializeField] private Animator FadeAnim;


    [SerializeField] private PlayFabManager playFabManager;

    private int Score;

    [SerializeField] private Button CloseButton;
    [SerializeField] private Button OpenBUtton;


    private int LeaderBoardCount;

    [SerializeField] public List<Sprite> MedalSprites;
    [SerializeField] private bool ReLoadNeeded = false;
    [SerializeField] private TextMeshProUGUI CurrentPlayScoreText;
    [SerializeField] private TextMeshProUGUI LoadingTextAnim;
    private string currentplascore;
    /* [SerializeField] private Sprite FirstPlace;
     [SerializeField] private Sprite SecondPlace;
     [SerializeField] private Sprite ThirdPlace;*/
    private Transform NeededScale;


/*    void Start()
    {
        UpdateLeaderBoard();
    }*/
    public void UpdateLeaderBoard()
    {
        if(LeaderBoardCount >1)
        {
            playFabManager.LoadLeaderboardAndPlayerScore();
        }
        LeaderBoardCount = 1;
        scoreDictionary.Clear();
        Dictionary<string, int> leaderboardDictionary = playFabManager.GetLeaderboardData();
        if (leaderboardDictionary.Count != 0)
        {
            KillAllKids();
            foreach (var player in leaderboardDictionary)
            {
                scoreDictionary.Add(player.Key, player.Value);
                GenerateLeaderBoardLine(player.Key, player.Value);
            }
            CurrentPlayScoreText.text = playFabManager.CurrentPlayerScore;
            StopCoroutine(nameof(TypeWriter));
            LoadingTextAnim.gameObject.SetActive(false);
            CurrentPlayerPosition.text = playFabManager.PlayerPos.ToString();
        }
        else
        {
            LoadingTextAnim.gameObject.SetActive(true);
            StartCoroutine(TypeWriter("Loading...", .5f));
        }
    }
    private void KillAllKids()
    {
        for (int i = this.transform.childCount - 1; i >= 2; i--)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }
    }
    public IEnumerator TypeWriter(string text, float waitTime)
    {
        string textdots = "";
        for (int i = 0; i < 3; i++)
        {
            Debug.Log("Started");
            textdots += ".";
            LoadingTextAnim.text = text + textdots;

            yield return new WaitForSeconds(waitTime);
        }
        UpdateLeaderBoard();
    }
    private void GenerateLeaderBoardLine(string name, int score)
    {
        if (LeaderBoardCount <= 3)
        {
            GameObject leaderboardline = Instantiate(SpecialLeaderBoardPrefab);
            leaderboardline.transform.SetParent(this.transform);
            leaderboardline.transform.localScale = SpecialLeaderBoardPrefab.transform.localScale;
            leaderboardline.SetActive(true);
            //leaderboardline.transform.parent = this.transform;
            LeaderBoardElementsClass leaderboardlineClass = leaderboardline.GetComponent<LeaderBoardElementsClass>();
            leaderboardlineClass.MedalType.GetComponent<Image>().sprite = MedalSprites[LeaderBoardCount - 1];
            leaderboardlineClass.PlayerName.text = name;
            leaderboardlineClass.PlayerScore.text = score.ToString();
        }
        else
        {
            GameObject leaderboardline = Instantiate(LeaderBoardPrefab);
            leaderboardline.transform.SetParent(this.transform);
            leaderboardline.transform.localScale = LeaderBoardPrefab.transform.localScale;
            leaderboardline.SetActive(true);
            //leaderboardline.transform.parent = this.transform;
            LeaderBoardElementsClass leaderboardlineClass = leaderboardline.GetComponent<LeaderBoardElementsClass>();
            leaderboardlineClass.PlayersPosition.text = LeaderBoardCount.ToString();
            leaderboardlineClass.PlayerName.text = name;
            leaderboardlineClass.PlayerScore.text = score.ToString();
        }
        LeaderBoardCount++;

    }

}
