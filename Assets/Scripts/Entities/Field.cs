using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
  // How much damage receives the player
  private float damage = 0.05f;
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
  // The maximum distance
  int maxDistance;
  // The current direction
  bool isDirectionDown = true;
  // The current distance
  int currentDistance = 0;
  private Player player;
  // Is the field active (not empty)
  bool isActive = true;


  void Start()
  {
    // Get the Player script
    player = GameObject.Find("Player").GetComponent<Player>();
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

    Sprite newSprite = Sprite.Create(texture, sprite.rect, new Vector2(0f, 0.0f), 1.0f);
    newSprite.texture.filterMode = FilterMode.Point;
    // Start with an empty sprite
    spriteRenderer.sprite = newSprite;

  }

  void changeDirection()
  {
    isDirectionDown = !isDirectionDown;
    animationCounter = 50;
    isActive = false;
  }

  // Reset the counter used to set animation speed
  void resetAnimationCounter()
  {
    if (isActive == false)
      isActive = true;

    animationCounter = 3;
  }

  void Update()
  {
    animationCounter--;

    if (animationCounter > 0)
      return;
    else
      resetAnimationCounter();

    // Start with an empty background
    System.Array.Copy(clearPixels, destPixels, destPixels.Length);

    if (isDirectionDown)
    {
      // If we have not reached the maximum distance, increase the distance
      if (currentDistance < maxDistance)
        currentDistance += width * 3;
      else
        isDirectionDown = !isDirectionDown;
    }
    else
    {
      // If we have not reached the minimum distance, decrease the distance
      if (currentDistance > 0)
        currentDistance -= width * 3;
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

  // If the player collides with an active field, decrease player health
  private void OnTriggerStay2D(Collider2D collision)
  {
    if (isActive)
      player.decreaseHealth(damage);
  }
}
