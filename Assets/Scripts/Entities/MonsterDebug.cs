using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This class is used to display the tile position of a monster with a rectangle
/// </summary>
public class MonsterDebug : MonoBehaviour
{
  // The rectangle image
  public Sprite sprite;
  // The game object with the rectangle sprite
  GameObject rectangle;
  // The monster script
  Monster monster;

  void Start()
  {
    // Get the monster script
    monster = transform.GetComponent<Monster>();

    // Create a rectangle object
    rectangle = new GameObject("Rectangle");
    SpriteRenderer renderer = rectangle.AddComponent<SpriteRenderer>();
    renderer.sprite = sprite;
    rectangle.transform.position = new Vector2(monster.XPos * 32, -monster.YPos * 32);
  }

  void Update()
  {
    // Update the rectangle position with the monster tile position
    rectangle.transform.position = new Vector2(monster.XPos * 32, -monster.YPos * 32);
  }
}
