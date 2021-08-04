using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trolley : MonoBehaviour
{
  float moveDistance = 4.0f;
  // The distance to walk
  int distance = 0;
  // The distance to travel
  int maxDistance = 2492;
  // The horizontal start position
  int xOrigin;
  private Player player;

  void Start()
  {
    // Store start position
    xOrigin = (int) transform.position.x;
    // Get instance of player
    player = GameObject.Find("Player").GetComponent<Player>();
  }

  void Update()
  {
    // If trolley has reached the maximum distance, reset position
    if (distance >= maxDistance)
    {
      transform.position = new Vector2(xOrigin, transform.position.y);
      distance = 0;
    }
    else
    {
      transform.position = new Vector2(transform.position.x + moveDistance, transform.position.y);
      distance += (int) moveDistance;
    }

  }

  // If the player enters the trolley, he's protected
  private void OnTriggerEnter2D(Collider2D collision)
  {
    player.setProtected(true);
  }

  // If the player exits the trolley, he's no more protected
  private void OnTriggerExit2D(Collider2D collision)
  {
    player.setProtected(false);
  }
}
