using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using TMPro;

public class PlayFabManager : MonoBehaviour
{
    private List<PlayerLeaderboardEntry> leaderboardData;
    public static PlayFabManager instance;
    [SerializeField] private GameObject EnterName;
    public TMP_InputField PlayerName;

    public TextMeshProUGUI PlayerNameWelcomeMsg;

    private string currentPlayerPlayFabId;
    public string CurrentPlayerScore { get;  set; }
    public int PlayerPos { get; set; }
    public int Coins { get; set; }

    public void Start()
    {
        Login();
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }
    void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful login/acc create!");
        currentPlayerPlayFabId = result.PlayFabId;
        GetCoinsData();
        LoadLeaderboardAndPlayerScore();
        string name = null;
        if (result.InfoResultPayload.PlayerProfile != null)
        {
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
        }
        if (name == null)
        {
            EnterName.SetActive(true);
        }
    }
    public void GetCoinsData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnCoinstRecieved, OnError);
    }

    private void OnCoinstRecieved(GetUserDataResult result)
    {
        Debug.Log("Recieved coins data");
        if(result.Data != null && result.Data.ContainsKey("Coins"))
        {
            Coins = int.Parse(result.Data["Coins"].Value);
            Debug.Log("Coins  " + Coins);
        }
    }

    private void OnLeaderBoardAndPlayerError(PlayFabError error)
    {
        Debug.LogError("Error: " + error.GenerateErrorReport());
    }

    public void LoadLeaderboardAndPlayerScore()
    {
        // Request to get the leaderboard around the current player
        var leaderboardRequest = new GetLeaderboardRequest
        {
            StatisticName = "CleaningScore", // Name of your leaderboard statistic
            StartPosition = 0, // Start position of the leaderboard entries
            MaxResultsCount = 10 // Number of leaderboard entries to retrieve
        };

        // Request to get the current player's score
        var playerScoreRequest = new GetPlayerStatisticsRequest();

        PlayFabClientAPI.GetLeaderboard(leaderboardRequest, result =>
        {
            Debug.Log("Leaderboard Loaded");
            leaderboardData = result.Leaderboard;
            // Find current player's position in leaderboard
            int currentPlayerPosition = -1;
            for (int i = 0; i < result.Leaderboard.Count; i++)
            {
                if(result.Leaderboard[i].PlayFabId == currentPlayerPlayFabId)
                {
                    currentPlayerPosition = i + 1; // Player's position in leaderboard (1-indexed)
                    break;
                }
            }

            if (currentPlayerPosition != -1)
            {
                Debug.Log("Your position in leaderboard: " + currentPlayerPosition);
                PlayerPos = currentPlayerPosition;
            }
            else
            {
                Debug.LogWarning("Player not found in leaderboard.");
            }
        }, OnLeaderBoardAndPlayerError);

        PlayFabClientAPI.GetPlayerStatistics(playerScoreRequest, result =>
        {
            // Player's statistics are returned in 'result.Statistics'
            Debug.Log("Player's Score Loaded");

            // Find the player's score from the retrieved statistics
            foreach (var stat in result.Statistics)
            {
                if (stat.StatisticName == "CleaningScore") // Check for the correct statistic name
                {
                    Debug.Log("Your Score: " + stat.Value);
                    CurrentPlayerScore = stat.Value.ToString();
                    break;
                }
            }
        }, OnLeaderBoardAndPlayerError);
    }
    void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in/creating acc");
        Debug.Log(error.GenerateErrorReport());
    }

    //Updating LeaderBoard
    public void SendLeaderBoard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "CleaningScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);
        // GetLeaderBoard();
    }
    void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfull leaderboard sent");
    }

    public Dictionary<string, int> GetLeaderboardData()
    {
        Dictionary<string, int> leaderboardDictionary = new Dictionary<string, int>();
        if(leaderboardData != null)
        {
            foreach (var entry in leaderboardData)
            {
                leaderboardDictionary.Add(entry.DisplayName.ToString(), (int)entry.StatValue);
            }
        }
        return leaderboardDictionary;
    }
    //Coins
    public void SaveCoinAmount(int coins)
    {
        Coins += coins;
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Coins", Coins.ToString()}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnCoinsDataSucces, OnError);
    }
    void OnCoinsDataSucces(UpdateUserDataResult result)
    {
        Debug.Log("Succese data coins send");
    }
    //Name Ui
    public void ChangeName()
    {
        EnterName.SetActive(true);
    }
    public void SumbitName()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = PlayerName.text,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdatem, OnError);
    }
    private void OnDisplayNameUpdatem(UpdateUserTitleDisplayNameResult obj)
    {
        EnterName.SetActive(false);
    }
}
