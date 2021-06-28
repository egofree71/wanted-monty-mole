using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This class is used to generate and store the sprites used for the crusher animation
/// </summary>
public class CrusherSprites : MonoBehaviour
{

  // The different textures used for generating the sprites
  public Texture2D textureLeft;
  public Texture2D textureMiddle;
  public Texture2D textureRight;

  const int maxSize = 192;
  const int tilesNumber = 7;

  // Sprites for the left, middle and right part of the crusher
  public Sprite[,] leftSprites = new Sprite[maxSize / 4 + 1, tilesNumber];
  public Sprite[,] middleSprites = new Sprite[maxSize / 4 + 1, tilesNumber];
  public Sprite[,] rightSprites = new Sprite[maxSize / 4 + 1, tilesNumber];

  // Use this for initialization
  void Start()
  {
    int tileSize = 32;

    // Generate the sprites for the left part of the crusher
    for (int tileNumber = 1; tileNumber <= tilesNumber; tileNumber++)
      for (int size = 0; size <= maxSize; size += 4)
        leftSprites[size / 4, tileNumber - 1] = Sprite.Create(textureLeft, new Rect(0.0f, -size + ((tileNumber - 1) * tileSize), tileSize, tileSize), new Vector2(0f, 0.0f), 1.0f);

    // Generate the sprites for the middle part of the crusher
    for (int tileNumber = 1; tileNumber <= tilesNumber; tileNumber++)
      for (int size = 0; size <= maxSize; size += 4)
        middleSprites[size / 4, tileNumber - 1] = Sprite.Create(textureMiddle, new Rect(0.0f, -size + ((tileNumber - 1) * tileSize), tileSize, tileSize), new Vector2(0f, 0.0f), 1.0f);

    // Generate the sprites for the right part of the crusher
    for (int tileNumber = 1; tileNumber <= tilesNumber; tileNumber++)
      for (int size = 0; size <= maxSize; size += 4)
        rightSprites[size / 4, tileNumber - 1] = Sprite.Create(textureRight, new Rect(0.0f, -size + ((tileNumber - 1) * tileSize), tileSize, tileSize), new Vector2(0f, 0.0f), 1.0f);

  }

}
