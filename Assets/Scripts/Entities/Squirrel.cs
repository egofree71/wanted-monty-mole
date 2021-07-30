using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squirrel : MonoBehaviour
{
  // Is squirrel moving to the right ?
  bool isMovingRight = true;
  // The maximum distance
  int maxDistance = 100;
  float moveStep = 2.0f;
  // The current distance
  int currentDistance = 0;

  void Update()
  {  
    if (isMovingRight)
    {
      // If the squirrel has not reached the maximum distance, move to the right
      if (currentDistance < maxDistance * moveStep)
      {
        transform.position = new Vector2(transform.position.x + moveStep, transform.position.y);
        currentDistance++;
      }
      else
      {
        ChangeDirection();
      }
    }
    else
    {
      // If the squirrel has not reached the minimum distance, move to the left
      if (currentDistance > 0)
      {
        transform.position = new Vector2(transform.position.x - moveStep, transform.position.y);
        currentDistance--;
      }
      else
      {
        ChangeDirection();
      }
    }
  }

  private void ChangeDirection()
  {
    isMovingRight = !isMovingRight;
    // Flip image
    gameObject.transform.Rotate(0, 180, 0);
  }
}
