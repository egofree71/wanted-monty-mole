using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This class is used to change the size of a crusher's tile
/// </summary>
public class CrusherTile : MonoBehaviour
{

  // The tile number in the crusher
  public int tileNumber;
  public enum crusherType { Left, Middle, Right };
  public crusherType Type;
  // Reference to the graphics sprite
  SpriteRenderer spriteRenderer;
  // The gameobject Crusher manages the Crusher's size
  private MovingObject crusher;
  // The gameObject which stores the sprites
  private CrusherSprites crusherSprites;
  // The sprites used for the crusher animation
  private Sprite[,] sprites = new Sprite[225, 7];

  // Use this for initialization
  void Start()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
    // Get object's script
    crusher = GameObject.Find("Crusher").GetComponent<MovingObject>();
    crusherSprites = GameObject.Find("CrusherSprites").GetComponent<CrusherSprites>();

    // According to the crusher type, use the corresponding sprites
    if (Type == crusherType.Left)
      sprites = crusherSprites.leftSprites;
    else
    if (Type == crusherType.Middle)
      sprites = crusherSprites.middleSprites;
    else
    if (Type == crusherType.Right)
      sprites = crusherSprites.rightSprites;
  }

  // Update is called once per frame
  void LateUpdate()
  {
    // Update the sprite for the given size
    spriteRenderer.sprite = sprites[(int)crusher.currentSize / 4, tileNumber - 1];
  }
}
