using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This class is used to move randomly a monster
/// </summary>
public class Monster : MonoBehaviour
{
  // How much damage receives the player when he collides with a monster
  private float damage = 0.05f;
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
  float moveDistance = 4.0f;
  // The current move's step inside a tile
  int moveStep = 0;
  // The number of tiles to travel
  int tilesToTravel;
  int tilesTraveled;
  // The number of moves to travel a tile
  int maxMoveStep;
  // Position in the map according to tiles
  private int xPos;
  private int yPos;
  // Component size
  private int width;
  private int height;

  // The gameObject which stores tiles maps
  private TilesMap tilesMap;
  private Player player;

  public int Width
  {
    get { return width; }
  }
  public int Height
  {
    get { return height; }
  }

  public int XPos
  {
    get { return xPos; }
  }
  public int YPos
  {
    get { return yPos; }
  }

  void Start()
  {
    getSize();
    // Get a direction and number of tiles to travel
    direction = Random.Range(0, 4);
    tilesToTravel = Random.Range(0, maxTiles);
    // Calculate the number of moves we need to travel a tile
    maxMoveStep = Global.tileSize / (int)moveDistance;
    tilesMap = GameObject.Find("TilesMap").GetComponent<TilesMap>();
    player = GameObject.Find("Player").GetComponent<Player>();
    // Get the current tile position
    getCurrentTilePosition();
    // Test if there is a solid tile for the current direction
    testCollision();
  }

  // Get component size
  public void getSize()
  {
    Vector3 size = GetComponent<SpriteRenderer>().bounds.size;
    width = (int)size.x;
    height = (int)size.y;
  }

  // Get the position in the map according to tiles
  public void getCurrentTilePosition()
  {
    // Get current position
    int x = (int)transform.position.x;
    int y = (int)transform.position.y;

    // Calculate center
    int yOffset = height - Global.tileSize;
    xPos = x + width / 2;
    yPos = y + yOffset - height / 2;

    xPos = xPos / 32;
    yPos = -(yPos / 32);
  }

  // Test if there is a solid tile for the current direction
  void testCollision()
  {
    if (direction == Left && tilesMap.tiles[yPos, xPos - 1] < 91)
      direction = None;

    if (direction == Right && tilesMap.tiles[yPos, xPos + 1] < 91)
      direction = None;

    if (direction == Up && tilesMap.tiles[yPos - 1, xPos] < 91)
      direction = None;

    if (direction == Down && tilesMap.tiles[yPos + 1, xPos] < 91)
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
      moveStep = 0;

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

  // Decrease player health when it collides with a monster
  private void OnTriggerStay2D(Collider2D collision)
  {
    player.decreaseHealth(damage);
  }

}
