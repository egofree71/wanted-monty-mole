using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This class is used to activate a lever
/// </summary>
public class LeverTile : MonoBehaviour
{

  public Sprite activated;
  // Reference to the graphics sprite
  SpriteRenderer spriteRenderer;

  // Start is called before the first frame update
  void Start()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
  }

  public void activate()
  {
    spriteRenderer.sprite = activated;
  }

}
