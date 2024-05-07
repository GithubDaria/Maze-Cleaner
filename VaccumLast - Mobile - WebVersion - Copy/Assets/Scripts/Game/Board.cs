using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private int rows;
    private int columns; 

    public List<GameObject> DotsArray = new List<GameObject>();
    private int[,] ObjectsArray;

    private float xOffset = 1.0f;
    private float yOffset = 1.0f;

    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;
    public float minSwipeDistance = 20f;

    private int CurrentPosX;
    private int CurrentPosY;

    private int MovesMade = 0;


    public GameObject prefabObject;

    public int ItemsAmount;

    [SerializeField] private Material WallMaterial;
    [SerializeField] private Material ItemsMaterial;
    [SerializeField] private Material TakenItemsMaterial;
    [SerializeField] private Transform PlayerObject;
    [SerializeField] private RoombaMovement PlayerScript;
    [SerializeField] private MapCreation MapCreationScript;

    private enum MoveDirection
    {
        Up,
        Down,
        Left,
        Right,
        None
    }
    private MoveDirection SecondMove = MoveDirection.None;
    //[SerializeField] private Transform PlayerObject;
    public void NewLevel()
    {
        ObjectsArray = MapCreationScript.GiveMapArray();
        Debug.Log(MapCreationScript.ArrayToString(ObjectsArray));
        rows = MapCreationScript.GiveRows();
        columns = MapCreationScript.GiveCollums();
        CurrentPosX = MapCreationScript.GiveCurrentPosCollums();
        CurrentPosY = MapCreationScript.GiveCurrentPosRows();
        ItemsAmount = MapCreationScript.GiveItemsAmount();
        PlayerScript.ItemTriggerCount = ItemsAmount;
        PlayerScript.StartNewLevel();
        Debug.Log(PlayerScript.ItemTriggerCount + "= items");

        //PlayerObject.position = new Vector3((CurrentPosY + 0.5f) * 1, PlayerObject.position.y, (CurrentPosX + 0.5f )* 1);
        //CreateTheMap();

    }
    void Update()
    {
       
        if(SecondMove != MoveDirection.None)
        {
            if(SecondMove == MoveDirection.Up)
            {
                MoveUp();
            }
            else if (SecondMove == MoveDirection.Down)
            {
                MoveDown();
            }
            else if (SecondMove == MoveDirection.Right)
            {
                MoveRight();
            }
            else if (SecondMove == MoveDirection.Left)
            {
                MoveLeft();
            }
        }
        else
        {
            DetectSwipe();
            if (Input.GetKeyDown(KeyCode.W))
            {
                MoveUp();

            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                MoveDown();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                MoveRight();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                MoveLeft();
            }
        }
    }
    private void MoveUp()
    {
        for (int i = CurrentPosY - 1; i >= 0; i--)
        {
            if (ObjectsArray[i, CurrentPosX] == 0)
            {
                //ItemsAmount--;
                ObjectsArray[i, CurrentPosX] = -1;
            }
            else if (ObjectsArray[i, CurrentPosX] == 1)
            {
                if (CurrentPosY != i + 1)
                {

                    if (PlayerScript.GetTheColider(new Vector3((i + 1) * xOffset, PlayerObject.position.y, (CurrentPosX) * yOffset), 90))
                    {

                        CurrentPosY = i + 1;
                        MovesMade++;
                        SecondMove = MoveDirection.None;
                    }
                    else
                    {
                        SecondMove = MoveDirection.Up;
                    }
                }
                //CheckCountGoal();
                break;
            }
            if (i - 1 < 0)
            {
                if (CurrentPosY != i)
                {

                    if (PlayerScript.GetTheColider(new Vector3((i) * xOffset, PlayerObject.position.y, (CurrentPosX) * yOffset), 90))
                    {

                        CurrentPosY = i;
                        MovesMade++;
                        SecondMove = MoveDirection.None;
                    }
                    else
                    {
                        SecondMove = MoveDirection.Up;
                    }
                }
                //CheckCountGoal();
                break;
            }
        }
    }
    private void MoveDown()
    {
        for (int i = CurrentPosY + 1; i < rows; i++)
        {
            if (ObjectsArray[i, CurrentPosX] == 0)
            {
                ////ItemsAmount--;
                ObjectsArray[i, CurrentPosX] = -1;
            }
            else if (ObjectsArray[i, CurrentPosX] == 1)
            {
                if (CurrentPosY != i - 1)
                {

                    if (PlayerScript.GetTheColider(new Vector3((i - 1) * xOffset, PlayerObject.position.y, (CurrentPosX) * yOffset), 270))
                    {

                        CurrentPosY = i - 1;
                        MovesMade++;
                        SecondMove = MoveDirection.None;
                    }
                    else
                    {
                        SecondMove = MoveDirection.Down;
                    }
                }
                //CheckCountGoal();

                break;
            }
            if (i + 1 == rows)
            {
                if (CurrentPosY != i)
                {
                    if (PlayerScript.GetTheColider(new Vector3((i) * xOffset, PlayerObject.position.y, (CurrentPosX) * yOffset), 270))
                    {
                        CurrentPosY = i;
                        MovesMade++;
                        SecondMove = MoveDirection.None;
                    }
                    else
                    {
                        SecondMove = MoveDirection.Down;
                    }
                }
                //CheckCountGoal();

                break;
            }

        }
    }
    private void MoveRight()
    {
        // Add your logic here for what happens when D is pressed
        for (int i = CurrentPosX + 1; i < columns; i++)
        {
            if (ObjectsArray[CurrentPosY, i] == 0)
            {
                //ItemsAmount--;
                ObjectsArray[CurrentPosY, i] = -1;
            }
            else if (ObjectsArray[CurrentPosY, i] == 1)
            {
                if (CurrentPosX != i + 1)
                {
                    if (PlayerScript.GetTheColider(new Vector3((CurrentPosY) * xOffset, PlayerObject.position.y, (i - 1) * yOffset), 180))
                    {
                        CurrentPosX = i - 1;
                        MovesMade++;
                        SecondMove = MoveDirection.None;
                    }
                    else
                    {
                        SecondMove = MoveDirection.Right;
                    }
                }
                //CheckCountGoal();

                break;
            }

            if (i + 1 == columns)
            {
                if (CurrentPosX != i)
                {
                    if (PlayerScript.GetTheColider(new Vector3((CurrentPosY) * xOffset, PlayerObject.position.y, (i) * yOffset), 180))
                    {
                        CurrentPosX = i;
                        MovesMade++;
                        SecondMove = MoveDirection.None;
                    }
                    else
                    {
                        SecondMove = MoveDirection.Right;
                    }
                }
                //CheckCountGoal();

                break;
            }
        }
    }
    private void MoveLeft()
    {
        for (int i = CurrentPosX - 1; i >= 0; i--)
        {
            if (ObjectsArray[CurrentPosY, i] == 0)
            {
                //ItemsAmount--;
                ObjectsArray[CurrentPosY, i] = -1;
            }
            else if (ObjectsArray[CurrentPosY, i] == 1)
            {
                if (CurrentPosX != i - 1)
                {
                    if (PlayerScript.GetTheColider(new Vector3((CurrentPosY) * xOffset, PlayerObject.position.y, (i + 1) * yOffset), 0))
                    {
                        CurrentPosX = i + 1;
                        MovesMade++;
                        SecondMove = MoveDirection.None;
                    }
                    else
                    {
                        SecondMove = MoveDirection.Left;
                    }
                }
                //CheckCountGoal();

                break;
            }
            if (i - 1 < 0)
            {
                if (CurrentPosX != i)
                {
                    if (PlayerScript.GetTheColider(new Vector3((CurrentPosY) * xOffset, PlayerObject.position.y, (i) * yOffset), 0))
                    {
                        CurrentPosX = i;
                        MovesMade++;
                        SecondMove = MoveDirection.None;
                    }
                    else
                    {
                        SecondMove = MoveDirection.Left;
                    }
                }
                //CheckCountGoal();

                break;
            }
        }
    }
    void DetectSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                fingerDownPosition = touch.position;
                fingerUpPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                fingerUpPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                fingerUpPosition = touch.position;
                DetectDirection();
            }
        }
    }

    void DetectDirection()
    {
        Vector2 swipeDelta = fingerUpPosition - fingerDownPosition;

        if (swipeDelta.magnitude < minSwipeDistance)
            return;

        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            if (swipeDelta.x > 0)
                {
                MoveRight();  
                }
                else
            {
                MoveLeft();
            }
        }
        else
        {
            if (swipeDelta.y > 0)
                {
                MoveUp();
                }
            else
            {
                MoveDown();
            }
        }
    }
    public void FinishLevel()
    {
            Debug.Log("End");
            MapCreationScript.StartANewMap(MovesMade);
            NewLevel();
    }

}
