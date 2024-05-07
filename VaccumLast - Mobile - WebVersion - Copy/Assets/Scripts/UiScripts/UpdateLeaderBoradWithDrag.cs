using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpdateLeaderBoradWithDrag : MonoBehaviour, IDragHandler
{
    [SerializeField] private LeaderBoardShow LeaderBoardShow;
    public float dragStartPos;
    public float NeedDrag;
    void Start()
    {
        dragStartPos = LeaderBoardShow.transform.localPosition.y;
        NeedDrag = dragStartPos - 600f;
    }
    public void OnDrag(PointerEventData eventData)
    {
        // Check if the user is dragging downwards
        if (LeaderBoardShow.transform.localPosition.y <= NeedDrag)
        {
            // Implement your functionality here
            Debug.Log("Dragging downwards. Trigger refresh or function.");
            LeaderBoardShow.UpdateLeaderBoard();
            // Call your function or update data here
        }
    }
}
