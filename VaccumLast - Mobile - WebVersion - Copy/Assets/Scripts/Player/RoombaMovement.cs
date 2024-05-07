using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaMovement : MonoBehaviour
{
    private bool IsMoving;
    private Vector3 targetpos;
    [SerializeField] private float speed;

    [SerializeField] private Board board;
    [SerializeField] private int itemTriggerCount=2;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private AudioSource cleanEffect;

    public int ItemTriggerCount
    {
        get { return itemTriggerCount; }
        set { itemTriggerCount = value; }
    }

    public enum Direction
    {
        Waiting,
        Moving,
        Standing,
    }
    [SerializeField] private Direction State;
    public void StartNewLevel()
    {
        State = Direction.Waiting;
    }
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 61;
    }
    void FixedUpdate()
    {
        if (State == Direction.Moving)
        {
            if (!particleSystem.isPlaying)
            {
                particleSystem.Play();
            }
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetpos, step);

            if (Vector3.Distance(transform.position, targetpos) < 0.01f)
            {
                State = Direction.Waiting;
            }
        }
        else
        {
            if (particleSystem.isPlaying)
            {
                particleSystem.Stop();
            }
        }

    }
    public bool GetTheColider(Vector3 PosToMoveTo, int rotatDeegree)
    {

        if (State == Direction.Waiting)
        {
            transform.rotation = Quaternion.Euler(0, rotatDeegree, 0);
            targetpos = PosToMoveTo;
            State = Direction.Moving;
            return true;
        }
        return false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(State != Direction.Standing)
        {
            if (other.gameObject.tag == "Dirt")
            {
                itemTriggerCount--;
                other.gameObject.SetActive(false);
                cleanEffect.Play();
                CheckTheItemCount();
            }
        }
    }
    private void CheckTheItemCount()
    {
        if(itemTriggerCount == 0)
        {
            State = Direction.Standing;
            board.FinishLevel();
        }
    }
}
