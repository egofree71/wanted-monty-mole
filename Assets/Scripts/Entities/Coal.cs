using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coal : MonoBehaviour
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
    // destroy the object and increase score
    Destroy(gameObject);
    gameManager.IncreaseScore();
  }


}
