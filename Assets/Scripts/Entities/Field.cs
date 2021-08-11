using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
  SpriteRenderer spriteRenderer;
  // The field sprite
  Sprite sprite;
  // Transparent pixels
  Color32[] clearPixels;
  // The sprite pixels
  Color32[] spritePixels;
  Color32[] destPixels;
  // Sprite size
  int width;
  int height;
  int maxDistance;
  // The current direction
  bool isDirectionDown = true;
  // The current distance
  int currentDistance = 0;

  void Start()
  {
    // Get the sprite renderer
    spriteRenderer = GetComponent<SpriteRenderer>();
    // Store the original sprite
    sprite = spriteRenderer.sprite;
    // Store the sprite pixels
    spritePixels = sprite.texture.GetPixels32();
    // Store the sprite size
    width = sprite.texture.width;
    height = sprite.texture.height;
    
    maxDistance = height / 2;
    destPixels = new Color32[width * height];
    // Create an empty texture
    Texture2D texture = new Texture2D(width, height);

    // Fill the texture with transparent color
    Color fillColor = Color.clear;
    clearPixels = new Color32[width * height];

    for (int i = 0; i < clearPixels.Length; i++)
      clearPixels[i] = fillColor;

    texture.SetPixels32(clearPixels);
    texture.Apply();

    // Start with an empty sprite
    spriteRenderer.sprite = Sprite.Create(texture, sprite.rect, new Vector2(0f, 0.0f), 1.0f);
  }


  void Update()
  {
    // Start with an empty background
    System.Array.Copy(clearPixels, destPixels, destPixels.Length);

    if (isDirectionDown)
    {
      if (currentDistance < maxDistance)
        currentDistance++;
      else
        isDirectionDown = !isDirectionDown;
    }
    else
    {
      if (currentDistance > 0)
        currentDistance--;
      else
        isDirectionDown = !isDirectionDown;
    }

    for (int i = 0; i < width * currentDistance; i++)
    {
      destPixels[i] = spritePixels[i];
    }

    spriteRenderer.sprite.texture.SetPixels32(destPixels);
    spriteRenderer.sprite.texture.Apply();
  }
}
