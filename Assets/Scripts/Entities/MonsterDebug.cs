using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This class is used to display the tile position of a monster with a rectangle
/// </summary>
[ExecuteInEditMode]
public class MonsterDebug : MonoBehaviour
{
  // The rectangle image
  public Sprite sprite;
  // The game object with the rectangle sprite
  GameObject rectangle;
  // The monster script
  Monster monster;
  public int xPos;
  public int yPos;
  public bool snapToTile;

  void Awake()
  {
    // Get the monster script
    monster = transform.GetComponent<Monster>();
    monster.getSize();

    // Create a rectangle object
    rectangle = new GameObject("Rectangle");
    SpriteRenderer renderer = rectangle.AddComponent<SpriteRenderer>();
    renderer.sortingOrder = 2;
    renderer.sprite = sprite;
    rectangle.transform.position = new Vector2(monster.XPos * 32, -monster.YPos * 32);
  }


  // If the snapToTile option is activated, move the monster to the center of the tile
  void OnValidate()
  {
    if (!snapToTile)
      return;


    int xTile = monster.XPos * 32;
    int yTile = -monster.YPos * 32;

    // Get tile center
    int xCenter = xTile + Global.tileSize / 2;
    int yCenter = yTile - Global.tileSize / 2;

    int yOffset = monster.Height - Global.tileSize;
    int xMonster = xCenter - monster.Width / 2;
    int yMonster = yCenter - yOffset + monster.Height / 2;

    transform.position = new Vector2(xMonster, yMonster);
    snapToTile = false;
  }

  // Delete the rectangle when the monster is deleted
  void OnDestroy()
  {
    GameObject.DestroyImmediate(rectangle);
  }

  void Update()
  {
    if (snapToTile)
      return;

    // If we are in the editor, calculate the tile position
    if (!Application.isPlaying)
      monster.getCurrentTilePosition();

    // Get the current tile position and display it
    xPos = monster.XPos;
    yPos = monster.YPos;
    rectangle.transform.position = new Vector2(monster.XPos * 32, -monster.YPos * 32);
  }
}
