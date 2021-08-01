using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas : MonoBehaviour
{
  // Is gas entity moving to the right ?
  bool isMovingRight = true;
  // The maximum distance
  int maxDistance = 50;
  float moveDistance = 2.0f;
  // The current distance
  int currentDistance = 0;

  void Update()
  {
    if (isMovingRight)
    {
      // If the gas has not reached the maximum distance, move to the right
      if (currentDistance < maxDistance * moveDistance)
      {
        transform.position = new Vector2(transform.position.x + moveDistance, transform.position.y);
        currentDistance++;
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
        currentDistance--;
      }
      else
      {
        isMovingRight = !isMovingRight;
      }
    }
  }
}
