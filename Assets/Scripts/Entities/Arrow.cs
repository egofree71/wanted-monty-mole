using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArrowDirection { Left, Right };

public class Arrow : MonoBehaviour
{
  private Player player;
  // The start position of the arrow
  Vector3 startPosition;
  float moveDistance = 4.0f;
  // The current distance
  int distance = 0;
  // The distance to travel
  int maxDistance = 476;
  // How much damage receives the player
  private float damage = 0.05f;
  public ArrowDirection direction;

  void Start()
  {
    // Store start position
    startPosition = transform.position;
    // Get instance of player
    player = GameObject.Find("Player").GetComponent<Player>();
  }

  // Update is called once per frame
  void Update()
  {
    // If the arrow has reached the maximum distance, reset position
    if (distance >= maxDistance)
    {
      transform.position = startPosition;
      distance = 0;
    }
    else
    // Move the arrow
    {
      if (direction == ArrowDirection.Right)
        transform.position = new Vector2(transform.position.x + moveDistance, transform.position.y + moveDistance);
      else
        transform.position = new Vector2(transform.position.x - moveDistance, transform.position.y + moveDistance);

      distance += (int)moveDistance;
    }
  }

  // Decrease player health when it collides with the arrow
  private void OnTriggerStay2D(Collider2D collision)
  {
    player.decreaseHealth(damage);
  }
}
