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

  void Awake()
  {
    // Get the monster script
    monster = transform.GetComponent<Monster>();

    // Create a rectangle object
    rectangle = new GameObject("Rectangle");
    SpriteRenderer renderer = rectangle.AddComponent<SpriteRenderer>();
    renderer.sortingOrder = 2;
    renderer.sprite = sprite;
    rectangle.transform.position = new Vector2(monster.XPos * 32, -monster.YPos * 32);
  }

  // Delete the rectangle when the monster is deleted
  void OnDestroy()
  {
    GameObject.DestroyImmediate(rectangle);
  }

  void Update()
  {
    // If we are in the editor, calculate the tile position
    if (!Application.isPlaying)
      monster.getCurrentTilePosition();

    // Get the current tile position and display it
    xPos = monster.XPos;
    yPos = monster.YPos;
    rectangle.transform.position = new Vector2(monster.XPos * 32, -monster.YPos * 32);
  }
}
