using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositioning : MonoBehaviour
{
    public Transform target; // Reference to the object you want to keep in the center
    private int MaxNumberPerMap = 5;
    private Camera mainCamera;
    [SerializeField] private Vector3 FirstPosition;
    private float FirstPosZ;
    private float FirstPosY;
    private float FirstPosX;
    private float Spacing = 0.5f;
    void Start()
    {
        FirstPosition = transform.position;
        FirstPosZ = transform.position.z;
        FirstPosY = transform.position.y;
        FirstPosX = transform.position.x;
    }

    public void AdjestTheCamera(int rows, int cols)
    {
        Debug.Log("Fixed");
        if(cols > MaxNumberPerMap)
        {
            Vector3 newposition = FirstPosition;
            Debug.Log("FirstPosition.position.x  " + FirstPosX);
            Debug.Log("cols - " + cols);
            Debug.Log("(rows - MaxNumberPerMap) * Spacing " + ((rows - 5 + cols - MaxNumberPerMap) * 1f));
            newposition.x = FirstPosX + ((rows - 5 + cols - MaxNumberPerMap) * 1f);
            newposition.y = FirstPosY + cols - MaxNumberPerMap + 2f;
            newposition.z = FirstPosZ + ((cols - MaxNumberPerMap) * Spacing) - 0.5f;
                transform.position = newposition;
           
        }
        else
        {
            Vector3 newposition = FirstPosition;
            float FirstPosZ = FirstPosition.z;
            Debug.Log("FirstPosition.position.z  "+ FirstPosZ);
            Debug.Log("cols - " + cols);
            Debug.Log("((MaxNumberPerMap - rows) * Spacing " + (MaxNumberPerMap + 1 - cols) * Spacing);
            if (rows > 5)
            {
                newposition.x = FirstPosX + ((rows - 5) * 1f);
                Debug.Log("FirstPosition.position.x  " + FirstPosX);
                Debug.Log("rows - " + rows);
                Debug.Log("(rows - MaxNumberPerMap) * Spacing " + ((rows - 5) * 0.5f));
            }
            newposition.z = FirstPosZ - ((MaxNumberPerMap + 1 - cols) * Spacing);
            transform.position = newposition;
        }
    }
}
