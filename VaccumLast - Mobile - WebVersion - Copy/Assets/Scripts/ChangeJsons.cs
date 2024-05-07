using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class ChangeJsons : MonoBehaviour
{
    // Directory path containing the JSON files
    public string directoryPath = "Assets/4x4";
    public int CountSames = 0;
    // Directory path containing the JSON files

    private string filePrefix = "";
    public bool FineSame;
    public int NumberToRemove;
    private int[] AllCurrentGameLevels = { 119, 121, 23, 91, 78, 63, 39, 88, 99, 71, 65, 62, 99 };


    public void RemoveAndRenameFiles(int fileNumberToRemove)
    {
        string filePathToRemove = Path.Combine(directoryPath, $"{filePrefix}{fileNumberToRemove}.json");
        Debug.Log(filePathToRemove);
        if (File.Exists(filePathToRemove))
        {
            // Remove the file with the specified name
            Debug.Log(filePathToRemove);

            File.Delete(filePathToRemove);

            // Rename remaining files
            for (int i = fileNumberToRemove + 1; ; i++)
            {
                string filePath = Path.Combine(directoryPath, $"{filePrefix}{i}.json");
                if (File.Exists(filePath))
                {
                    string newFilePath = Path.Combine(directoryPath, $"{filePrefix}{(i - 1)}.json");
                    File.Move(filePath, newFilePath);
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            Debug.LogWarning($"File {filePathToRemove} not found.");
        }
    }
    private void Start()
    {
        if (FineSame)
        {
            string[] jsonFiles = Directory.GetFiles(directoryPath, "*.json");

            // Deserialize each JSON file and compare its arrays with every other file
            for (int i = 0; i < jsonFiles.Length; i++)
            {
                for (int j = i + 1; j < jsonFiles.Length; j++)
                {
                    CompareJsonFiles(jsonFiles[i], jsonFiles[j]);

                }
                Debug.Log(CountSames + " sames");
                CountSames = 0;
            }
        }
        else
        {
                RemoveAndRenameFiles(121);
        }

    }

    private void CompareJsonFiles(string filePath1, string filePath2)
    {
        // Read JSON data from files
        string json1 = File.ReadAllText(filePath1);
        string json2 = File.ReadAllText(filePath2);

        // Deserialize JSON data into 2D arrays
        int[,] array1 = JsonConvert.DeserializeObject<int[,]>(json1);
        int[,] array2 = JsonConvert.DeserializeObject<int[,]>(json2);

        // Compare arrays from both files
        bool areEqual = AreArraysEqual(array1, array2);
        if (areEqual)
        {
            CountSames++;
        }
        Debug.Log($"Comparison between {filePath1} and {filePath2}: Arrays are equal: {areEqual}");
    }

    private bool AreArraysEqual(int[,] array1, int[,] array2)
    {
        // Check if arrays have the same dimensions
        if (array1.GetLength(0) != array2.GetLength(0) || array1.GetLength(1) != array2.GetLength(1))
        {
            return false;
        }

        for (int i = 0; i < array1.GetLength(0); i++)
        {
            for (int j = 0; j < array1.GetLength(1); j++)
            {
                // Check if array values are different, considering special cases
                if (array1[i, j] != array2[i, j] && !(array1[i, j] == 5 && array2[i, j] == 0) && !(array1[i, j] == 0 && array2[i, j] == 5))
                {
                    return false;
                }
            }
        }

        return true;
    }
}
