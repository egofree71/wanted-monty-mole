using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///  This class is used to modify the manage the player's movement
/// </summary>
public class Player : MonoBehaviour
{
  // Is the player protected by a shield ?
  bool isProtected;

  // The maximal health for the player
  private float maxHealth = 100f;
  // How much damage receives the player
  private float damage = 0.05f;
  private float bigDamage = 0.7f;

  [Header("Player position in the map according to tiles")]
  public int xPos;
  public int yPos;

  // When the user changes the properties in the editor, change the player's position
  void OnValidate()
  {
    transform.position = new Vector2(xPos * 32 - 12, -yPos * 32);
  }

  // player health
  private float health;
  // The object which manages the health bar
  public HealthBar healthBar;
  // The gameObject which stores tiles maps
  private TilesMap tilesMap;
  // The gameobject Bridge manages the bridge's size
  private MovingObject bridge;
  // The gameobject Bridge manages the crusher's size
  private MovingObject crusher;
  // The gameobject LevelManager manages the levels
  private LevelManager levelManager;
  // Animator component for the player
  private Animator playerAnim;
  private enum horizontalDirection { Left, Right, None };
  private enum verticalDirection { Up, Down, None };
  private enum state { Idle, Jumping, ClimbingRightSlope, ClimbingLeftSlope };
  public enum tileType { ElectricGround = 108, ConveyorUp = 94, ConveyorDown = 93, Acid = 95, TrapLeft = 91, TrapRight = 92 }

  // The current directions of the player
  horizontalDirection playerHorizontalDirection;
  verticalDirection playerVerticalDirection;

  horizontalDirection jumpDirection;
  // The current state of the player
  state playerState;

  bool moving = false;
  // The current move's step
  short moveStep;
  short jumpStep = 0;

  // The box collider around the player
  BoxCollider2D boxCollider;
  bool leftArrow;
  bool rightArrow;
  bool upArrow;
  bool downArrow;
  // Is the player falling ?
  bool falling;
  bool animatePlayer;

  // Use this for initialization
  void Awake()
  {
    // Set frame rate to 60
    QualitySettings.vSyncCount = 0;
    Application.targetFrameRate = 60;

    playerAnim = (Animator)GetComponent(typeof(Animator));
    boxCollider = GetComponent<BoxCollider2D>();
    // Get objects scripts
    healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
    tilesMap = GameObject.Find("TilesMap").GetComponent<TilesMap>();
    bridge = GameObject.Find("Bridge").GetComponent<MovingObject>();
    crusher = GameObject.Find("Crusher").GetComponent<MovingObject>();
    levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    playerState = state.Idle;

    // Set the health at the beginning
    health = 55;
    healthBar.setHealth(health);
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKey(KeyCode.Escape))
    {
      Application.Quit();
    }

    // Test if the player is hurt by a background tile
    testPlayerIsHurt();

    playerAnim.SetInteger("xMove", 0);
    playerAnim.SetBool("yMove", false);
    playerAnim.speed = 0.0f;

    // If the player is not moving
    if (moving == false)
    {
      leftArrow = false;
      rightArrow = false;
      upArrow = false;
      downArrow = false;
      falling = false;
      animatePlayer = true;
      playerHorizontalDirection = horizontalDirection.None;
      playerVerticalDirection = verticalDirection.None;

      switch (playerState)
      {
        case state.Idle:
          if (playerIsFalling())
          {
            falling = true;
            animatePlayer = false;
            playerVerticalDirection = verticalDirection.Down;
          }
          else
          {
            // Save the key pressed
            if (Input.GetKey(KeyCode.LeftArrow))
              leftArrow = true;
            else if (Input.GetKey(KeyCode.RightArrow))
              rightArrow = true;
            else if (Input.GetKey(KeyCode.UpArrow))
              upArrow = true;
            else if (Input.GetKey(KeyCode.DownArrow))
              downArrow = true;

            // If the player wants to go up
            if (upArrow && isThereaLadderAbove())
              playerVerticalDirection = verticalDirection.Up;

            // If the player wants to go down
            if (downArrow && isThereaLadderBelow())
              playerVerticalDirection = verticalDirection.Down;

            // If the player is on a bucket conveyor going up
            if (tilesMap.tiles[yPos, xPos] == (int)tileType.ConveyorUp)
              playerVerticalDirection = verticalDirection.Up;

            // If the player is on a bucket conveyor going down
            if (tilesMap.tiles[yPos, xPos] == (int)tileType.ConveyorDown)
              playerVerticalDirection = verticalDirection.Down;

            // If the player wants to go left
            if (leftArrow)
            {
              // If there is slope to the left, first go up
              if (isThereaSlopeToTheLeft())
              {
                playerVerticalDirection = verticalDirection.Up;
                playerState = state.ClimbingLeftSlope;
                animatePlayer = false;
              }

              // If there is not a solid tile to the left, go right
              if (!isThereaSolidLeft())
              {
                playerAnim.SetInteger("xMove", -1);
                playerHorizontalDirection = horizontalDirection.Left;
                playerVerticalDirection = verticalDirection.None;
              }
            }

            // If the player wants to go right
            if (rightArrow)
            {
              // Stop the player if he wants to exit the map
              if (xPos > 254)
                return;

              // If there is slope to the right, first go up
              if (isThereaSlopeToTheRight())
              {
                playerVerticalDirection = verticalDirection.Up;
                playerState = state.ClimbingRightSlope;
                animatePlayer = false;
              }

              // If there is not a solid tile to the right, go right
              if (!isThereaSolidRight())
              {
                playerAnim.SetInteger("xMove", 1);
                playerHorizontalDirection = horizontalDirection.Right;
                playerVerticalDirection = verticalDirection.None;
              }
            }

            if (Input.GetKey(KeyCode.Space))
            {
              // Start a jump
              jumpDirection = horizontalDirection.None;
              playerVerticalDirection = verticalDirection.Up;
              playerState = state.Jumping;
              jumpStep = 1;

              if (rightArrow)
                jumpDirection = horizontalDirection.Right;

              if (leftArrow)
                jumpDirection = horizontalDirection.Left;

              // If the player is going up, don't animate the player
              if (jumpDirection == horizontalDirection.None)
                animatePlayer = false;

              // Test if the jump has to continue
              testJumpStartCollision();
            }

          }
          break;

        case state.Jumping:

          // Stop the player if he wants to exit the map
          if (xPos > 254)
          {
            stopJump();
            return;
          }

          // If the player is going up, don't animate the player
          if (jumpDirection == horizontalDirection.None)
            animatePlayer = false;

          playerHorizontalDirection = jumpDirection;
          jumpStep++;

          if (jumpStep <= 2)
          {
            playerVerticalDirection = verticalDirection.Up;
            // Test if the jump can continue
            testJumpStartCollision();
          }
          else if (jumpStep <= 3)
          {
            // Test if the jump can continue
            testJumpMiddleCollision();
          }
          else if (jumpStep <= 5)
          {
            playerVerticalDirection = verticalDirection.Down;
            // Test if the jump can continue
            testJumpEndCollision();
          }
          break;

        case state.ClimbingLeftSlope:
          playerHorizontalDirection = horizontalDirection.Left;
          break;

        case state.ClimbingRightSlope:
          playerHorizontalDirection = horizontalDirection.Right;
          break;
      }

      switch (playerHorizontalDirection)
      {
        case horizontalDirection.Left:
          playerAnim.SetInteger("xMove", -1);
          xPos--;
          break;

        case horizontalDirection.Right:
          playerAnim.SetInteger("xMove", 1);
          xPos++;
          break;
      }

      switch (playerVerticalDirection)
      {
        case verticalDirection.Up:
          // If the player is not on a slope
          if (playerState != state.ClimbingRightSlope && playerState != state.ClimbingLeftSlope && playerState != state.Jumping)
            playerAnim.SetBool("yMove", true);
          yPos--;
          break;

        case verticalDirection.Down:
          if (falling == false && playerState != state.Jumping)
            playerAnim.SetBool("yMove", true);
          yPos++;
          break;
      }

      if (playerHorizontalDirection != horizontalDirection.None ||
          playerVerticalDirection != verticalDirection.None)
      {
        moveStep = 8;
        moving = true;
      }

    }

    // If the player is doing a move sequence
    if (moving)
    {
      moveStep--;

      if (animatePlayer)
        playerAnim.speed = 1.0f;

      switch (playerHorizontalDirection)
      {
        case horizontalDirection.Left:
          // Move the player to the left
          transform.position = new Vector2(transform.position.x - 4.0f, transform.position.y);
          break;

        case horizontalDirection.Right:
          // Move the player to the right
          transform.position = new Vector2(transform.position.x + 4.0f, transform.position.y);
          break;
      }

      switch (playerVerticalDirection)
      {
        case verticalDirection.Up:
          // Move the player up
          transform.position = new Vector2(transform.position.x, transform.position.y + 4.0f);
          break;

        case verticalDirection.Down:
          // Move the player down
          transform.position = new Vector2(transform.position.x, transform.position.y - 4.0f);
          break;
      }

      // If the process is over
      if (moveStep == 0)
      {
        moving = false;
        moveStep = 8;

        // If the player was on a slope
        if (playerVerticalDirection == verticalDirection.None && (playerState == state.ClimbingRightSlope || playerState == state.ClimbingLeftSlope))
          playerState = state.Idle;

        if (jumpStep == 5)
          stopJump();

        if (playerHorizontalDirection != horizontalDirection.None)
          checkLevers();
      }
    }

  }

  void stopJump()
  {
    playerState = state.Idle;
    playerHorizontalDirection = horizontalDirection.None;
    playerVerticalDirection = verticalDirection.None;
    jumpStep = 0;
  }

  // Test if the player is colliding something when he starts to jump
  void testJumpStartCollision()
  {
    // If there is a solid tile above
    if (tilesMap.tiles[yPos - 2, xPos] < 90)
      stopJump();

    if (jumpDirection == horizontalDirection.Left)
    {
      // If there are solid tiles to the upper left
      if (tilesMap.tiles[yPos - 2, xPos - 1] < 90 || tilesMap.tiles[yPos - 1, xPos - 1] < 90)
        stopJump();
    }

    if (jumpDirection == horizontalDirection.Right)
    {
      // If there are solid tiles to the upper right
      if (tilesMap.tiles[yPos - 2, xPos + 1] < 90 || tilesMap.tiles[yPos - 1, xPos + 1] < 90)
        stopJump();
    }

  }

  // Test if the player is colliding something when he is in the middle of the jump
  void testJumpMiddleCollision()
  {
    // If the player has an non empty tile below
    if (tilesMap.tiles[yPos + 1, xPos] != 256)
    {
      stopJump();
    }
    else
    {
      int tileUnderHead = tilesMap.tiles[yPos - 1, xPos];

      // If the player's head is on a rope or ladder
      if (tileUnderHead == 100 || tileUnderHead == 122 || tileUnderHead == 101
       || tileUnderHead == 129)
        stopJump();
    }

    switch (jumpDirection)
    {
      case horizontalDirection.None:
        stopJump();
        break;

      case horizontalDirection.Right:
        // If there are solid tiles to the right
        if (tilesMap.tiles[yPos - 1, xPos + 1] < 90 || tilesMap.tiles[yPos, xPos + 1] < 90)
          stopJump();
        break;

      case horizontalDirection.Left:
        // If there are solid tiles to the left
        if (tilesMap.tiles[yPos - 1, xPos - 1] < 90 || tilesMap.tiles[yPos, xPos - 1] < 90)
          stopJump();
        break;
    }
  }

  // Test if the player is colliding something when he is in the end of the jump
  void testJumpEndCollision()
  {
    if (jumpDirection == horizontalDirection.Left)
    {
      // If there are solid tiles to the bottom left
      if (tilesMap.tiles[yPos, xPos - 1] < 90 || tilesMap.tiles[yPos + 1, xPos - 1] < 90)
        stopJump();
    }

    if (jumpDirection == horizontalDirection.Right)
    {
      // If there are solid tiles to the bottom right
      if (tilesMap.tiles[yPos, xPos + 1] < 90 || tilesMap.tiles[yPos + 1, xPos + 1] < 90)
        stopJump();
    }

    // If the player has an non empty tile below
    if (tilesMap.tiles[yPos + 1, xPos] != 256)
      stopJump();
  }


  // Test if the player is falling
  bool playerIsFalling()
  {
    int tileUnderHead = tilesMap.tiles[yPos - 1, xPos];
    int tileBelow = tilesMap.tiles[yPos + 1, xPos];

    // If the tile below the player is a crusher tile
    if (tileBelow >= 167 && tileBelow <= 187)
      return true;

    // If the player's head is under a 'left' crusher tile, test if there is a collision
    if (tileUnderHead >= 167 && tileUnderHead <= 173)
    {
      if (crusher.currentSize <= (173 - tileUnderHead + 1) * 32)
        return true;
    }

    // If the player's head is under a 'middle' crusher tile, test if there is a collision
    if (tileUnderHead >= 174 && tileUnderHead <= 180)
    {
      if (crusher.currentSize <= (180 - tileUnderHead + 1) * 32)
        return true;
    }

    // If the player's head is under a 'right' crusher tile, test if there is a collision
    if (tileUnderHead >= 181 && tileUnderHead <= 187)
    {
      if (crusher.currentSize <= (187 - tileUnderHead + 1) * 32)
        return true;
    }

    // If the player is above a tile of a bridge going right
    if (tileBelow >= 133 && tileBelow <= 139)
    {
      // If there is currently no bridge under the player
      if (bridge.currentSize <= (tileBelow - 133) * 32 + 32)
        return true;
      else
        return false;
    }

    // If the player is above a tile of a bridge going right
    if (tileBelow >= 140 && tileBelow <= 146)
    {
      // If there is currently no bridge under the player
      if (bridge.currentSize <= (146 - tileBelow) * 32 + 32)
        return true;
      else
        return false;
    }

    // If there is nothing below and the head is not on a ladder, the player is falling
    if (tileBelow == 256 && !new List<int> { 100, 101, 122, 129 }.Contains(tilesMap.tiles[yPos - 1, xPos]))
      return true;

    return false;
  }


  // Test if the player is hurt
  void testPlayerIsHurt()
  {
    // If the player is falling
    if (falling)
    {
      decreaseHealth(damage);
      return;
    }

    int tileBelow = tilesMap.tiles[yPos + 1, xPos];
    int tileUnderLeg = tilesMap.tiles[yPos, xPos];
    int tileUnderHead = tilesMap.tiles[yPos - 1, xPos];

    List<int> hurtingTiles = new List<int> { (int)tileType.Acid, (int)tileType.TrapLeft, (int)tileType.TrapRight };

    // If the player crosses acid or traps
    if (hurtingTiles.Contains(tileBelow) || hurtingTiles.Contains(tileUnderLeg) || hurtingTiles.Contains(tileUnderHead))
    {
      decreaseHealth(damage);
      return;
    }

    // If the player is above an electric ground
    if (tileBelow == (int)tileType.ElectricGround)
      decreaseHealth(bigDamage);
  }

  // Reset health and update health bar
  public void resetHealth()
  {
    health = maxHealth;
    healthBar.setHealth(health);
  }

  // Decrease health and update health bar
  public void decreaseHealth(float damage)
  {
    // If the player is inside a shield, no damage
    if (isProtected)
      return;

    health -= damage;
    healthBar.setHealth(health);
  }

  // Test is there a slope tile left to the player
  bool isThereaSlopeToTheLeft()
  {
    if (new List<int> { 76, 77, 80 }.Contains(tilesMap.tiles[yPos, xPos - 1]))
      return true;
    else
      return false;
  }

  // Test is there a slope tile right to the player
  bool isThereaSlopeToTheRight()
  {
    if (new List<int> { 75, 77, 79 }.Contains(tilesMap.tiles[yPos, xPos + 1]))
      return true;
    else
      return false;
  }

  // Test is there a ladder below the player
  bool isThereaLadderBelow()
  {
    if (new List<int> { 100, 101, 122, 129 }.Contains(tilesMap.tiles[yPos - 1, xPos]) ||
            new List<int> { 97, 99, 100, 105 }.Contains(tilesMap.tiles[yPos + 1, xPos]))
      return true;
    else
      return false;
  }

  // Test is there a ladder above the player
  bool isThereaLadderAbove()
  {
    if (new List<int> { 122, 129 }.Contains(tilesMap.tiles[yPos - 2, xPos]) ||
            new List<int> { 100, 101, 105 }.Contains(tilesMap.tiles[yPos - 1, xPos]) ||
            new List<int> { 100, 105 }.Contains(tilesMap.tiles[yPos + 1, xPos]))
      return true;
    else
      return false;
  }

  // Test is there a solid tile right to the player
  bool isThereaSolidRight()
  {
    if (tilesMap.tiles[yPos, xPos + 1] < 90 || tilesMap.tiles[yPos - 1, xPos + 1] < 90)
      return true;
    else
      return false;
  }

  // Test is there a solid tile left to the player
  bool isThereaSolidLeft()
  {
    if (tilesMap.tiles[yPos, xPos - 1] < 90 || tilesMap.tiles[yPos - 1, xPos - 1] < 90)
      return true;
    else
      return false;
  }

  // Test is there a solid tile below the player
  bool isCollidingaSolidDown()
  {
    if (tilesMap.tiles[yPos + 1, xPos] <= 107)
      return true;
    else
      return false;
  }

  // Check if the player is crossing a lever
  void checkLevers()
  {
    int level = levelManager.level;

    // Get the level index in the json
    List<LevelData> levelList = levelManager.levels.list;
    int index = levelList.FindIndex(a => a.number == level);

    int xLeverPosition = levelList[index].lever.x;
    int yLeverPosition = levelList[index].lever.y;

    // If the player is crossing a lever, increase level
    if (xPos == xLeverPosition && yPos == yLeverPosition)
    {
      // Get current lever (two game objects) for current level
      GameObject[] levers = GameObject.FindGameObjectsWithTag("lever_" + level);

      // Switch the lever
      foreach (GameObject lever in levers)
        lever.GetComponent<LeverTile>().activate();

      levelManager.goNextLevel();
    }
  }

  // Set if the player is protected by a shield
  public void setProtected(bool isProtected)
  {
    this.isProtected = isProtected;
  }
}



