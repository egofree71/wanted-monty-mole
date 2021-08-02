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
  const int None = -1;
  // The maximum tiles to travel
  const int maxTiles = 13;
  // The current direction
  int direction;
  // The distance between two moves
  float moveDistance = 2.0f;
  // The current move's step inside a tile
  int moveStep = 1;
  // The number of tiles to travel
  int tilesToTravel;
  int tilesTraveled;
  // The number of moves to travel a tile
  int maxMoveStep;
  // Position in the map according to tiles
  private int xPos;
  private int yPos;
  // The gameObject which stores tiles maps
  private TilesMap tilesMap;

  void Start()
  {
    // Get a direction and number of tiles to travel
    direction = Random.Range(0, 4);
    tilesToTravel = Random.Range(0, maxTiles);
    // Calculate the number of moves we need to travel a tile
    maxMoveStep = Global.tileSize / (int)moveDistance;
    tilesMap = GameObject.Find("TilesMap").GetComponent<TilesMap>();
    getCurrentTilePosition();
    testCollision();
  }

  // Get the position in the map according to tiles
  void getCurrentTilePosition()
  {
    // Get component size
    Vector3 size = GetComponent<SpriteRenderer>().bounds.size;
    int width = (int)size.x;
    int height = (int)size.y;

    // Calculate tile position
    int x = (int)transform.position.x;
    int y = (int)transform.position.y;

    xPos = (x + width / 3) / 32;
    yPos = -y / 32;
  }

  // Test is there a solid tile right to the monster
  bool isThereaSolidRight()
  {
    if (tilesMap.tiles[yPos, xPos + 1] < 91)
      return true;
    else
      return false;
  }

  // Test is there a solid tile left to the monster
  bool isThereaSolidLeft()
  {
    if (tilesMap.tiles[yPos, xPos - 1] < 91)
      return true;
    else
      return false;
  }

  // Test is there a solid tile above to the monster
  bool isThereaSolidAbove()
  {
    if (tilesMap.tiles[yPos - 1, xPos] < 91)
      return true;
    else
      return false;
  }

  // Test is there a solid tile below the monster
  bool isThereaSolidDown()
  {
    if (tilesMap.tiles[yPos + 1, xPos] < 91)
      return true;
    else
      return false;
  }

  // Test if there is solid tile around the monster
  void testCollision()
  {
    if (direction == Left && isThereaSolidLeft())
      direction = None;

    if (direction == Right && isThereaSolidRight())
      direction = None;

    if (direction == Up && isThereaSolidAbove())
      direction = None;

    if (direction == Down && isThereaSolidDown())
      direction = None;
  }

  void Update()
  {
    // If the monster has reached the destination
    if (tilesTraveled == tilesToTravel)
    {
      // Reset counter and get a new direction
      tilesTraveled = 0;
      tilesToTravel = Random.Range(0, maxTiles);
      direction = Random.Range(0, 4);
      testCollision();
    }
    else
    {
      moveStep++;

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

        // Update position in the map
        if (direction == Left)
          xPos--;

        if (direction == Right)
          xPos++;

        if (direction == Up)
          yPos--;

        if (direction == Down)
          yPos++;

        tilesTraveled++;
        testCollision();
      }
    }
  }
}
