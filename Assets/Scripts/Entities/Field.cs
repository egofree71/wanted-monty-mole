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
  // Array length
  int length;
  // Sprite size
  int width;
  int height;
  // Counter used to set animation speed
  int animationCounter;
  // Counter used to pause animation when direction is changed
  int pauseCounter;
  // The maximum distance
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
    
    maxDistance = height * width / 2;
    destPixels = new Color32[width * height];
    length = width * height;
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

  void changeDirection()
  {
    isDirectionDown = !isDirectionDown;
    animationCounter = 50;
  }

  void Update()
  {
    animationCounter--;

    if (animationCounter > 0)
      return;
    else
      animationCounter = 2;

    // Start with an empty background
    System.Array.Copy(clearPixels, destPixels, destPixels.Length);

    if (isDirectionDown)
    {
      // If we have not reached the maximum distance, increase the distance
      if (currentDistance < maxDistance)
        currentDistance += width * 2;
      else
        isDirectionDown = !isDirectionDown;
    }
    else
    {
      // If we have not reached the minimum distance, decrease the distance
      if (currentDistance > 0)
        currentDistance -= width * 2;
      else
        changeDirection();
    }

    // Copy the original sprite pixels to the destination
    for (int i = 0; i < currentDistance; i++)
    {
      destPixels[i] = spritePixels[i];
      destPixels[length - i - 1] = spritePixels[i];
    }

    // Apply the new texture
    spriteRenderer.sprite.texture.SetPixels32(destPixels);
    spriteRenderer.sprite.texture.Apply();
  }
}
