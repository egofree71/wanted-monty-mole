using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This class is used to activate a lever
/// </summary>
public class LeverTile : MonoBehaviour
{

  // The sprite when the lever is activated
  public Sprite activated;
  // Reference to the graphics sprite
  SpriteRenderer spriteRenderer;

  // Start is called before the first frame update
  void Start()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
  }

  // Display the activated sprite
  public void activate()
  {
    spriteRenderer.sprite = activated;
  }

}
