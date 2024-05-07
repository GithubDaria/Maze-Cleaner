using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDots : MonoBehaviour
{
    public enum StateOfGoal
    {
        Present,
        Taken
    }
    private StateOfGoal currentState = StateOfGoal.Present;

    [SerializeField] private Material TakenGoal;
    public StateOfGoal CurrentState
    {
        get { return currentState; }
        private set { currentState = value; }
    }

    public void ChangeMatOfObject()
    {
        if (currentState != StateOfGoal.Taken)
        {
           // this.GetComponent<Renderer>().material = TakenGoal;
            currentState = StateOfGoal.Taken;
        }

    }
    
}
