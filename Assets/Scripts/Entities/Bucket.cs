using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour
{
  // The gameobject GameManager manages the game
  private GameManager gameManager;

  void Start()
  {
    gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
  }

  // If the player collides with the object
  private void OnTriggerEnter2D(Collider2D collision)
  {
    Destroy(gameObject);
    gameManager.SetBucketIsTaken();
  }
}
