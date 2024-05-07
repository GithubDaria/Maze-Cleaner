using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.Networking;
using TMPro;

public class LoadLevelData : MonoBehaviour
{
    public int width;
    public int height;

    [System.Serializable]
    public class ArrayData
    {
        public int[][] array;
    }


    [SerializeField] private string folderPath;

    [SerializeField] private MapCreation mapcreat;

    [SerializeField] private TextMeshProUGUI TestingSaving;
    // private int[] AllCurrentGameLevels = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 ,1, 1};
    //private int[] AllCurrentGameLevels = { 5, 5, 5, 5, 10, 5, 5, 5, 15, 15, 15, 10 };
    private int[] AllCurrentGameLevels = { 2, 2, 2, 2, 2, 3, 5, 5, 15, 15, 15, 10 };
    private List<int> NoDuplicate = new();
    private int CurrentGameLevelSizeCount = 0;
    [SerializeField] private string[] folderPaths; // Set this in the Unity Editor to an array of the paths of your folders
    public int levelsPerFolder = 5; // Set the number of levels to load per folder

    void Start()
    {
    /*    // Load the JSON file from the "Resources" folder
        TextAsset file = Resources.Load("MyFolder/hello") as TextAsset;

        if (file != null)
        {
            string jsonContent = file.text;

            int[,] array = JsonConvert.DeserializeObject<int[,]>(jsonContent);
            if (array != null)
            {
                // Access the "message" field and print its value
                //Debug.Log("Message: " + array.message);
                TestingSaving.text = array[0, 1].ToString();
            }
            else
            {
               // Debug.LogError("Failed to parse JSON content");
                TestingSaving.text = "Failed to parse JSON content";
            }
        }
        else
        {
            Debug.LogError("Failed to load hello.json");
            TestingSaving.text = "Failed to load hello.json";

        }*/
        GenerateLevelsForGame();
    }
    private void GenerateLevelsForGame()
    {
        if (folderPaths == null || folderPaths.Length == 0)
        {
            Debug.LogError("Folder paths are not set.");
            return;
        }

        // Creating a list of 2D arrays
        List<int[,]> listOfArrays = new List<int[,]>();

        // Iterate over each folder path
        foreach (string folderPath in folderPaths)
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                Debug.LogWarning("Empty folder path found.");
                continue;
            }

            // Get all JSON files in the folder
            //string[] files = Directory.GetFiles(folderPath, "*.json");

            // Shuffle the array of files
            //ShuffleArray(files);

            // Iterate over each JSON file, up to the specified number of levels per folder
            int maxrandomnumber = 100;
            NoDuplicate.Clear();
            for (int i = 0; i < AllCurrentGameLevels[CurrentGameLevelSizeCount]; i++)
            {
                
                System.Random rand = new System.Random((int)System.DateTime.Now.Ticks);
               
                // Generate a random number between 0 and maxRandomNumber
                int randomNumber = rand.Next(0, maxrandomnumber);
                if (!NoDuplicate.Contains(randomNumber))
                {
                    TextAsset file = Resources.Load(folderPath + "/" + randomNumber) as TextAsset;
                    Debug.Log(folderPath + "/" + randomNumber);
                    if (file == null)
                    {
                        maxrandomnumber = randomNumber;
                        i--;
                        Debug.Log("Number too big " + randomNumber + " in " + folderPath);
                    }
                    else
                    {
                        string jsonContent = file.text;
                        int[,] array = JsonConvert.DeserializeObject<int[,]>(jsonContent);
                        listOfArrays.Add(array);
                        NoDuplicate.Add(randomNumber);

                    }
                }
                else
                {
                    i--;
                }
                
            }
            CurrentGameLevelSizeCount++;
        }
        Debug.Log("Number of arrays loaded: " + listOfArrays.Count);

        mapcreat.SetArraysMaps(AllCurrentGameLevels, listOfArrays);
        /*mapcreat.CreateGameMap(rows, cols, AllCurrentGameLevels, listOfArrays);*/
    }
    /*private void GenerateLevelsForGame()
    {
        if (folderPaths == null || folderPaths.Length == 0)
        {
            Debug.LogError("Folder paths are not set.");
            return;
        }

        // Creating a list of 2D arrays
        List<int[,]> listOfArrays = new List<int[,]>();

        // Iterate over each folder path
        foreach (string folderPath in folderPaths)
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                Debug.LogWarning("Empty folder path found.");
                continue;
            }

            // Combine persistent data path with folder path to get full directory path
            string fullFolderPath = Path.Combine(Application.persistentDataPath, folderPath);
            Debug.Log("Searching in folder: " + fullFolderPath);

            // Check if the directory exists
            if (!Directory.Exists(fullFolderPath))
            {
                Debug.LogWarning("Directory does not exist: " + fullFolderPath);
                continue;
            }

            // Get all JSON files in the folder
            string[] files = Directory.GetFiles(fullFolderPath, "*.json");
            Debug.Log("Number of JSON files found: " + files.Length);

            // Shuffle the array of files
            ShuffleArray(files);

            // Iterate over each JSON file, up to the specified number of levels per folder
            for (int i = 0; i < Mathf.Min(files.Length, AllCurrentGameLevels[CurrentGameLevelSizeCount]); i++)
            {
                string file = files[i];
                Debug.Log("Loading JSON file: " + file);

                // Read JSON file content
                string jsonContent = File.ReadAllText(file);

                // Parse JSON into 2D array
                int[,] array = JsonConvert.DeserializeObject<int[,]>(jsonContent);

                // Add the 2D array to the list
                listOfArrays.Add(array);
            }
            CurrentGameLevelSizeCount++;
        }
        Debug.Log("Number of arrays loaded: " + listOfArrays.Count);

        // Assuming mapcreat is an instance of your map creation class
        mapcreat.SetArraysMaps(AllCurrentGameLevels, listOfArrays);
    }*/
    /* private void GenerateLevelsForGame()
     {
         if (folderPaths == null || folderPaths.Length == 0)
         {
             Debug.LogError("Folder paths are not set.");
             return;
         }

         // Creating a list of 2D arrays
         List<int[,]> listOfArrays = new List<int[,]>();

         // Iterate over each folder path
         for (int folderIndex = 0; folderIndex < folderPaths.Length; folderIndex++)
         {
             string folderPath = folderPaths[folderIndex];
             if (string.IsNullOrEmpty(folderPath))
             {
                 Debug.LogWarning("Empty folder path found.");
                 continue;
             }

             // Combine persistent data path with folder path to get full directory path
             string fullFolderPath = Path.Combine(Application.persistentDataPath, folderPath);
             Debug.Log("Searching in folder: " + fullFolderPath);

             // Check if the directory exists
             if (!Directory.Exists(fullFolderPath))
             {
                 Debug.LogWarning("Directory does not exist: " + fullFolderPath);
                 continue;
             }

             // Get all JSON files in the folder
             string[] files = Directory.GetFiles(fullFolderPath, "*.json");
             Debug.Log("Number of JSON files found: " + files.Length);

             // Shuffle the array of files
             ShuffleArray(files);

             int levelsToLoad = AllCurrentGameLevels[folderIndex];
             int levelsLoaded = 0;

             // Iterate over each JSON file, up to the specified number of levels per folder
             for (int i = 0; i < files.Length && levelsLoaded < levelsToLoad; i++)
             {
                 string file = files[i];
                 Debug.Log("Loading JSON file: " + file);

                 // Load JSON file using UnityWebRequest
                 UnityWebRequest request = UnityWebRequest.Get("file://" + file);
                 request.SendWebRequest();

                 while (!request.isDone) { }

                 if (request.result == UnityWebRequest.Result.Success)
                 {
                     string jsonContent = request.downloadHandler.text;

                     // Parse JSON into 2D array
                     int[,] array = JsonConvert.DeserializeObject<int[,]>(jsonContent);

                     // Add the 2D array to the list
                     listOfArrays.Add(array);

                     levelsLoaded++;
                 }
                 else
                 {
                     Debug.LogError("Failed to load JSON file: " + file + ", error: " + request.error);
                 }
             }
         }

         Debug.Log("Number of arrays loaded: " + listOfArrays.Count);

         // Assuming mapcreat is an instance of your map creation class
         mapcreat.SetArraysMaps(AllCurrentGameLevels, listOfArrays);
     }*/

    void ProcessFiles()
    {
        if (string.IsNullOrEmpty(folderPath))
        {
            Debug.LogError("Folder path is not set.");
            return;
        }

        string[] files = Directory.GetFiles(folderPath, "*.json"); // Get all JSON files in the folder

        foreach (string file in files)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);

            // Read JSON file content
            string jsonContent = File.ReadAllText(file);

            // Parse JSON into 2D array
            int[,] array = JsonConvert.DeserializeObject<int[,]>(jsonContent);

            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i, j] == 1)
                    {
                        array[i, j] = 2; // Replace 1s with 2s
                    }
                }
            }

            // Convert modified array back to JSON
            string modifiedJson = JsonConvert.SerializeObject(array);

            // Write modified JSON back to file
            File.WriteAllText(Path.Combine(folderPath, $"{fileName}.json"), modifiedJson);

            // Rename the file (optional)
            File.Move(file, Path.Combine(folderPath, $"{fileName}.json"));

            Debug.Log($"Processed file: {fileName}");
        }
    }
    private void ShuffleArray<T>(T[] array)
    {
        System.Random rng = new System.Random();
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = array[k];
            array[k] = array[n];
            array[n] = value;
        }
    }
    void ParseJson(string json)
    {
        LevelData data = JsonUtility.FromJson<LevelData>(json);
        width = data.width;
        height = data.height;

        // Now you can use 'width' and 'height' in your game
        Debug.Log("Width: " + width + ", Height: " + height);
    }

    void CreateJsonFile()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "LevelsData.json");

        LevelData data = new LevelData();
        data.width = 3;
        data.height = 3;
       // data.myArray = my;

    string json = JsonUtility.ToJson(data);

        // Write the JSON data to a file
        File.WriteAllText(path, json);

        Debug.Log("File created: dimensions.json");
    }

    public void SaveArrayToJson()
    {
      /*  // Sample array data


        // Serialize array to JSON
        string json = JsonConvert.SerializeObject(myArray);

        // Save JSON string to PlayerPrefs
        string filePath = Application.streamingAssetsPath + "/savedArray.json";
        File.WriteAllText(filePath, json);
        Debug.Log("2D Array saved to file: " + filePath);*/
    }
    private string Serialize2DArrayToJson(int[,] array)
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);

        // Create a 2D array of strings to hold the converted values
        string[,] stringArray = new string[rows, cols];

        // Convert int values to strings
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                stringArray[i, j] = array[i, j].ToString();
            }
        }

        // Serialize the string 2D array into JSON using JsonUtility
        return JsonUtility.ToJson(stringArray);
    }
    void LoadArrayFromFile()
    {
        string filePath = Application.streamingAssetsPath + "/savedArray.json";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            int[,] loadedArray = JsonConvert.DeserializeObject<int[,]>(json);
            Debug.Log(ArrayToString(loadedArray));
            // Use loadedArray as needed
            Debug.Log("2D Array loaded from file: " + filePath);
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
    }
    string ArrayToString(int[,] array)
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
}



