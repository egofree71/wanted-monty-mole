using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas : MonoBehaviour
{
  // How much damage receives the player when he collides with the gas
  private float damage = 0.05f;
  // Is gas entity moving to the right ?
  bool isMovingRight = true;
  // The maximum distance
  int maxDistance = 200;
  float moveDistance = 2.0f;
  // The current distance
  int currentDistance = 0;
  private Player player;

  private void Start()
  {
    player = GameObject.Find("Player").GetComponent<Player>();
  }

  void Update()
  {
    if (isMovingRight)
    {
      // If the gas has not reached the maximum distance, move to the right
      if (currentDistance < maxDistance)
      {
        transform.position = new Vector2(transform.position.x + moveDistance, transform.position.y);
        currentDistance += (int)moveDistance;
      }
      else
      {
        isMovingRight = !isMovingRight;
      }
    }
    else
    {
      // If the gas has not reached the minimum distance, move to the left
      if (currentDistance > 0)
      {
        transform.position = new Vector2(transform.position.x - moveDistance, transform.position.y);
        currentDistance -= (int)moveDistance;
      }
      else
      {
        isMovingRight = !isMovingRight;
      }
    }
  }

  // Decrease player health when it collides with the gas
  private void OnTriggerStay2D(Collider2D collision)
  {
    player.decreaseHealth(damage);
  }
}
