using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
  // The player's game object
  private Player player;

  void Start()
  {
    player = GameObject.Find("Player").GetComponent<Player>();
  }

  // If the player collides with the object
  private void OnTriggerEnter2D(Collider2D collision)
  {
    Destroy(gameObject);
    player.resetHealth();
  }
}
