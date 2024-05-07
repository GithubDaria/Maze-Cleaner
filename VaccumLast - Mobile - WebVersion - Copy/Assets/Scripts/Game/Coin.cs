using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    public UnityEvent CoinWasCollected;
    public Animator CoinAnimations;

    private void OnTriggerEnter(Collider other)
    {
        CoinWasCollected.Invoke();
    }
    public void PlayCoinCollected()
    {
        CoinAnimations.SetTrigger("CoinCollected");
    }
}
