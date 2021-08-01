using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This class is used to move randomly a monster
/// </summary>
public class Monster : MonoBehaviour
{
  // Direction types
  const int Right = 0;
  const int Left = 1;
  const int Down = 2;
  const int Up = 3;

  // Counter used to change direction
  int counterChangeDirection = 0;
  // The maximum value for the counter
  const int maxCounter = 12;
  // The current direction
  int direction;
  // The distance between two moves
  float moveDistance = 2.0f;
  // The current move's step
  int moveStep = 1;
  // The number of moves to travel a tile
  int maxMoveStep;

  void Start()
  {
    // Get a direction and number of steps for the next direction change
    direction = Random.Range(0, 4);
    counterChangeDirection = Random.Range(0, maxCounter);
    // Calculate the number of moves we need to travel a tile
    maxMoveStep = Global.tileSize / (int)moveDistance;
  }

  void Update()
  {
    // Move  monster with the current direction
    switch (direction)
    {
      case Left:
        transform.position = new Vector2(transform.position.x - moveDistance, transform.position.y);
        break;

      case Right:
        transform.position = new Vector2(transform.position.x + moveDistance, transform.position.y);
        break;

      case Up:
        transform.position = new Vector2(transform.position.x, transform.position.y + moveDistance);
        break;

      case Down:
        transform.position = new Vector2(transform.position.x, transform.position.y - moveDistance);
        break;
    }

    // If the monster has traveled a tile distance
    if (moveStep == maxMoveStep)
    {
      moveStep = 1;
      counterChangeDirection++

      // If it is time to change direction
      if (counterChangeDirection == maxCounter)
      {
        // Reset counter and get a new direction
        counterChangeDirection = Random.Range(0, maxCounter);
        direction = Random.Range(0, 4);
      }
    }

    moveStep++;
  }
}
