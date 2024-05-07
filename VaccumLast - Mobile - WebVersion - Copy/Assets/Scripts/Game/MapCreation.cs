using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreation : MonoBehaviour
{
    [Header("Input values that i current type by myself")]
    private int SizeRows;
    private int SizeCols;

    [SerializeField] private int ItemsAmount = 0;
    /*private int row = 4;
    private int col = 4;*/
   [SerializeField] private int[,] MapArray = {
    { 0, 0, 1, 1, 0, 5, 1 },
    { 0, 1, 1, 1, 1, 0, 1 },
    { 0, 0, 0, 0, 0, 0, 1 },
    { 0, 1, 1, 1, 1, 1, 1 },
    { 0, 0, 0, 0, 0, 0, 0 }
};
    private int[,] SecondMap =
    {
    { 5, 0, 0, 0, 0 },
    { 0, 2, 0, 0, 0 },
    { 0, 0, 0, 0, 2 },
    { 2, 2, 0, 0, 0 },
    { 0, 0, 0, 0, 0 }
};
    List<int[,]> ListOfArrays = new List<int[,]>();
    int[] AllCurrentGameLevels = new int[12];
    private int GameCount = 0;
    public int GameCounterForReplay = 1;
    private float TimeUsed;
    private int Score = 0;
    private float MaxTimeGiven;

    private int CoinsCollected = 0;

    private int TheAddReplayUsed = 0; 

    [SerializeField] private int ReplayCounter = 0;
    // In game objects 
    [Header("Game elements")]
    [SerializeField] private GameObject Grid;
    [SerializeField] private GameObject GridFloor;
    [SerializeField] private GameObject Wall;
    [SerializeField] private GameObject[] Walls;
    [SerializeField] private GameObject Dirt;
    [SerializeField] private GameObject Roomba;
    [SerializeField] private GameObject FloorPrefab;
    [SerializeField] private GameObject Coin;

    //UI
    [Header("Ui elements")]
    [SerializeField] private GameObject GameOverScreen;

    [Header("Scripts")]
    [SerializeField] private GameObject GameManager;
    [SerializeField] private Board board;
    [SerializeField] private Timer timer;
    [SerializeField] private CameraPositioning cameraPositioning;
    [SerializeField] private ScreenManag screenManag;
    [SerializeField] private PlayFabManager playFabManager;
    [SerializeField] private GameOverUi GameoverUI;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private Coin coinscript;





    private List<GameObject> ListOfDirt = new List<GameObject>();
    [SerializeField]
    private List<GameObject> ListOfFurniture1by1 = new List<GameObject>();
    [SerializeField]
    private List<GameObject> ListOfFurniture2by1 = new List<GameObject>();
    [SerializeField]
    private List<GameObject> ListOfFurniture2by2 = new List<GameObject>();

    private List<Quaternion> allowedRotations = new List<Quaternion>();

    // [SerializeField] private GameObject Roomba;
    private int RowRoombaStartPos;

 

    private int ColRoombaStartPos;

    private float wallHeight = 0.04570944f;
    public enum TypeOfFutniture
    {
        Pos1by1,
        Pos2by1,
        Pos2by2
    }
    private TypeOfFutniture PosiblePositions;
    /*private void Start()
    {
        int rows = MapArray.GetLength(0);
        int cols = MapArray.GetLength(1);
        CreateGameMap(rows, cols);
    }*/
    private void Start()
    {
        coinscript.CoinWasCollected.AddListener(CoinWasCollectedEvent);
    }
    public void SetArraysMaps(int[] allCurrentGameLevels, List<int[,]> listOfArrays)
    {
        MaxTimeGiven = timer.maxTimeGiven;
        AllCurrentGameLevels = allCurrentGameLevels;
        ListOfArrays = listOfArrays;
        MapArray = (int[,])listOfArrays[0].Clone();
        int rows = MapArray.GetLength(0);
        int cols = MapArray.GetLength(1);
        CreateGameMap(rows, cols);

    }
    public void CreateGameMap(int rows, int cols)
    {
        SizeRows = rows;
        SizeCols = cols;


        float InitialScaleX = GridFloor.transform.localScale.x;
        float InitialScaleZ = GridFloor.transform.localScale.z;
        float NewScaleX = InitialScaleX *  SizeRows;
        float NewScaleZ = InitialScaleZ * SizeCols;
        Vector3 newScale = new Vector3(NewScaleX, 1, NewScaleZ);
        GridFloor.transform.localScale = newScale;
        float NewZPos = 0.5f * (SizeCols - 1) + GridFloor.transform.position.z - 0.037f;
        float NewXPos = 0.5f * (SizeRows - 1) + GridFloor.transform.position.x - 0.037f;
        GridFloor.transform.position = new Vector3(NewXPos, 0, NewZPos);

        cameraPositioning.AdjestTheCamera(rows, cols);
        SpawnDirtPlanes();
        SpawnFurniture();
        SpawnRoomba();
        board.NewLevel();
        Roomba.SetActive(true);
        screenManag.ChangeLevelUi(GameCount + 1);
        screenManag.ChangeScoreUI(Score);
        GetPositionForCount();
        timer.StartTimer(((SizeCols + SizeRows) / 2) + 3);
    }
    private void SpawnDirtPlanes()
    {
        float spacing = 0.97f;
        float marginside = 0.5f;
        float margindown = 0.53f;

        Vector3 gridBottomLeftCorner = Grid.transform.position - new Vector3(Grid.transform.localScale.x * 0.5f - margindown, 0f, Grid.transform.localScale.z * 0.5f - marginside);

        float offsetX = ( SizeRows - 1) * 0.5f * 0.978f; // Offset for horizontal alignment
        float offsetZ = (SizeCols - 1) * 0.5f * spacing; // Offset for vertical alignment

        for (int i = 0; i <  SizeRows ; i++) // Vertical tiles
        {
            for (int j = 0; j < SizeCols; j++) // Horizontal tiles
            {
                Vector3 spawnPosition = gridBottomLeftCorner + new Vector3(i * 0.97f - offsetX, 0.0001f, j * spacing - offsetZ); // Calculate spawn position

                GameObject dirt = Instantiate(Dirt, spawnPosition, Quaternion.identity);
                // Set parent only if needed, remove this line if not necessary
                dirt.name = i + "," + j;
                dirt.transform.parent = GridFloor.transform; // Set the parent to the script's transform or another suitable parent
                ListOfDirt.Add(dirt);
            }
        }
       /* foreach(GameObject dirtob in ListOfDirt)
        {
            dirtob.transform.SetParent(null);
        }*/
    }
    private void SpawnFurniture()
    {
        GameObject CurrentFurniture;
        ItemsAmount = 0;
        for (int i = 0; i < SizeRows; i++)
        {
            for (int j = 0; j < SizeCols; j++)
            {
                if (MapArray[i, j] == 2)
                {
                    PosiblePositions = TypeOfFutniture.Pos1by1;
                    if (j + 1 < SizeCols && MapArray[i, j + 1] == 2)
                    {
                        if (i + 1 < SizeRows && (MapArray[i + 1, j] == 2))
                        {
                            if (MapArray[i + 1, j + 1] == 2)
                            {
                                PosiblePositions = TypeOfFutniture.Pos2by2;
                            }
                        }
                        else
                        {
                            PosiblePositions = TypeOfFutniture.Pos2by1;
                        }
                    }
                    else if (i + 1 < SizeRows && (MapArray[i + 1, j] == 2))
                    {
                        PosiblePositions = TypeOfFutniture.Pos2by1;
                    }
                    if (PosiblePositions == TypeOfFutniture.Pos1by1)
                    {
                        CurrentFurniture = GetRandomFurniture(ListOfFurniture1by1);
                        GameObject dirt = ListOfDirt[i * SizeCols + j];

                        allowedRotations.Clear();
                        //testig below
                        if (i != 0)
                        {
                            allowedRotations.Add(Quaternion.Euler(0f, 90f, 0f));

                        }
                        if (i != SizeRows - 1)
                        {
                            allowedRotations.Add(Quaternion.Euler(0f, 180f, 0f));
                        }
                        if (j != 0)
                        {
                            allowedRotations.Add(Quaternion.identity);

                        }
                        if (j != SizeCols - 1)
                        {
                            allowedRotations.Add(Quaternion.Euler(0f, 270f, 0f));

                        }
                        int randomIndex = UnityEngine.Random.Range(0, allowedRotations.Count);
                        Quaternion rotation = allowedRotations[randomIndex];
                        GameObject furnit = Instantiate(CurrentFurniture, dirt.transform.position, rotation);
                        furnit.transform.parent = GridFloor.transform;
                        MapArray[i, j] = 1;


                    }
                    else if (PosiblePositions == TypeOfFutniture.Pos2by1)
                    {
                        // Quaternion.Euler(0f, 90f, 0f);
                        CurrentFurniture = GetRandomFurniture(ListOfFurniture2by1);
                        GameObject dirt = ListOfDirt[i * SizeCols + j];
                        if (j + 1 < SizeCols && MapArray[i, j + 1] == 2)
                        {
                            GameObject furnit = Instantiate(CurrentFurniture, dirt.transform.position, Quaternion.Euler(0f, 180f, 0f));
                            furnit.transform.parent = GridFloor.transform;
                            MapArray[i, j] = 1;
                            if (furnit.GetComponent<Furniture>().GiveTypeOfFurnip() == Furniture.TypeOfFutniture.Pos2by1)
                            {
                                MapArray[i, j + 1] = 1;
                            }

                        }
                        else if (i + 1 < SizeRows && MapArray[i + 1, j] == 2)
                        {
                            GameObject furnit = Instantiate(CurrentFurniture, dirt.transform.position, Quaternion.Euler(0f, 270f, 0f));
                            furnit.transform.parent = GridFloor.transform;
                            MapArray[i, j] = 1;
                            if (furnit.GetComponent<Furniture>().GiveTypeOfFurnip() == Furniture.TypeOfFutniture.Pos2by1)
                            {
                                MapArray[i + 1, j] = 1;

                            }

                        }
                        //GameObject furnit = Instantiate(CurrentFurniture, new Vector3(i + .592, Quaternion.identity);


                    }
                    else if (PosiblePositions == TypeOfFutniture.Pos2by2)
                    {
                        CurrentFurniture = GetRandomFurniture(ListOfFurniture2by2);
                        GameObject dirt = ListOfDirt[i * SizeCols + j];
                        GameObject furnit = Instantiate(CurrentFurniture, dirt.transform.position, Quaternion.Euler(0f, 270f, 0f));
                       // furnit.transform.parent = GridFloor.transform;


                        MapArray[i, j] = 1;
                        if (furnit.GetComponent<Furniture>().GiveTypeOfFurnip() == Furniture.TypeOfFutniture.Pos2by1)
                        {
                            bool IsVertical = UnityEngine.Random.Range(0, 2) == 0;
                            if (IsVertical)
                            {
                                furnit.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
                                MapArray[i + 1, j] = 1;
                            }
                            else
                            {
                                furnit.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                                MapArray[i, j + 1] = 1;
                            }

                        }
                        else if (furnit.GetComponent<Furniture>().GiveTypeOfFurnip() == Furniture.TypeOfFutniture.Pos2by2)
                        {
                            MapArray[i, j + 1] = 1;
                            MapArray[i + 1, j] = 1;
                            MapArray[i + 1, j + 1] = 1;

                        }
                        furnit.transform.parent = GridFloor.transform;

                    }
                }
                else if (MapArray[i, j] == 5)
                {
                    RowRoombaStartPos = i;
                    ColRoombaStartPos = j;
                }
                else if (MapArray[i, j] == 0)
                {
                    ItemsAmount++;
                }
            }

        }
    }
    private GameObject GetRandomFurniture(List<GameObject> listOfFurniture)
    {
        int randomIndex = UnityEngine.Random.Range(0, listOfFurniture.Count);
        return listOfFurniture[randomIndex];
    }

    public string ArrayToString(int[,] array)
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);

        string arrayString = "";

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                arrayString += array[i, j].ToString() + " ";
            }
            arrayString += "\n";
        }

        return arrayString;
    }
    private void SpawnRoomba()
    {
        Quaternion rotation;
        if (RowRoombaStartPos-1 >= 0 && MapArray[RowRoombaStartPos -1, ColRoombaStartPos] == 0)
        {
             rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else if (ColRoombaStartPos - 1 >= 0 && MapArray[RowRoombaStartPos, ColRoombaStartPos-1] == 0)
        {
             rotation = Quaternion.Euler(0f, 0f, 0f);

        }
        else if (RowRoombaStartPos + 1 != SizeRows && MapArray[RowRoombaStartPos + 1, ColRoombaStartPos] == 0)
        {
            rotation = Quaternion.Euler(0f, 180f, 0f);

        }
        else
        {
            rotation = Quaternion.Euler(0f, 270f, 0f);
        }
        Roomba.transform.rotation = rotation;
        Roomba.transform.position = new Vector3(RowRoombaStartPos, 0.2f, ColRoombaStartPos);

    }
    private void GetPositionForCount()
    { 
        if(GameCount >= 20)
        //if(GameCount >= 20)
        {
            int randomNumber = UnityEngine.Random.Range(0, 100);
            if (randomNumber < 10)
            {
                for (int i = 0; i < SizeRows + SizeCols; i++) // Vertical tiles
                {
                    int randrow = UnityEngine.Random.Range(0, SizeRows);
                    int randcol = UnityEngine.Random.Range(0, SizeCols);

                    if (IsCointPossble(randrow, randcol))
                    {
                        //SpawnCoin(randrow, randcol);
                    }
                }
            }
            return;
        }
    }
    private bool IsCointPossble(int row, int col)
    {
        if(MapArray[row, col] !=1 && MapArray[row, col] != 5)
        {
            return true;
        }
        return false;
    }
    private void SpawnCoin(int row, int col)
    {
        Coin.SetActive(true);
        Coin.transform.position = new Vector3(row * 1.0f, Coin.transform.position.y, col * 1.0f);

    }
    private void CoinWasCollectedEvent()
    {
        //coinscript.PlayCoinCollected();
        CoinsCollected++;
        Coin.SetActive(false);

    }
    public void ReplayMap()
    {
        Destroy(GridFloor);
        MapArray = (int[,])ListOfArrays[GameCount].Clone();
        // Debug.Log(ArrayToString(MapArray));
        /*ReplayCounter++;*/
        Debug.Log("Replay countr = " + ReplayCounter);
         GridFloor = Instantiate(FloorPrefab);
        Grid = GridFloor.GetComponent<Floor>().Grid;
        Walls = GridFloor.GetComponent<Floor>().Walls;
        ListOfDirt.Clear();
        CreateGameMap(SizeRows, SizeCols);
        TheAddReplayUsed++;
        if(TheAddReplayUsed >= 2)
        {
            screenManag.ExtraReplayUsed();
        }
    }
    public void StartANewMap(int movesMade)
    {
        soundManager.PlayLevelDoneSound();
        Destroy(GridFloor);
        if (GameCount + 1 != ListOfArrays.Count)
        {

            Score = (int)CountScore(movesMade);
            CountFinalScore();
            MapArray = GetNextMap();
            Debug.Log(ArrayToString(MapArray));
            GridFloor = Instantiate(FloorPrefab);
            Grid = GridFloor.GetComponent<Floor>().Grid;
            Walls = GridFloor.GetComponent<Floor>().Walls;
            ListOfDirt.Clear();
            GameCounterForReplay++;
            if (GameCounterForReplay == 10)
            {
                TheAddReplayUsed = 1;
                screenManag.RestoreReplay();
                GameCounterForReplay = 0;
            }
            int rows = MapArray.GetLength(0);
            int cols = MapArray.GetLength(1);
            CreateGameMap(rows, cols);
        }
        else
        {
            CompleteAllLevels();
        }
      
    }
    private void CompleteAllLevels()
    {
        timer.StopTimer();
        Debug.Log("Score =" + Score);
        playFabManager.SendLeaderBoard(Score);
        GameOverScreen.SetActive(true);
        GameoverUI.SetGameOverScreen(true, Score);
        soundManager.PlayWinSound();
    }
    public void GameOver()
    {
        
        Debug.Log("Score =" + Score);
        board.enabled = false;
        playFabManager.SaveCoinAmount(CoinsCollected);
        playFabManager.SendLeaderBoard(Score);
        GameOverScreen.SetActive(true);
        screenManag.SetGameOverScreen(false, Score);
        soundManager.PlayGameOverSound();

    }
    public void NoTime()
    {
        screenManag.CanSafeReplay(TheAddReplayUsed);
    }
    private float CountScore(int movesMade)
    {
        return Score + SizeCols * SizeRows;
    }
    private float CountFinalScore()
    {
        return Score + (timer.maxTimeGiven - timer.elapsedTime) * 5;
    }
    private int[,] GetNextMap()
    {
        GameCount++;
        return (int[,])ListOfArrays[GameCount].Clone();
    }
    public int GiveRows()
    {
        return SizeRows;
    }
    public int GiveCollums()
    {
        return SizeCols;
    }
    public int GiveCurrentPosRows()
    {
        return RowRoombaStartPos;
    }
    public int GiveCurrentPosCollums()
    {
        return ColRoombaStartPos;
    }
    public int[,] GiveMapArray()
    {
        return MapArray;
    }
    public int GiveItemsAmount()
    {
        return ItemsAmount+1;
    }
}
