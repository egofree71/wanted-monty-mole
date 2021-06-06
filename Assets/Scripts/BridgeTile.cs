using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This class is used to change the size of a bridge's tile
/// </summary>
public class BridgeTile : MonoBehaviour
{
  // Reference to the graphics sprite
  SpriteRenderer spriteRenderer;
  // The tile's number in the bridge
  public int tileNumber;
  // The position of the tile in the bridge
  float currentPosition;
  float tileSize;
  // The gameobject Bridge manages the bridge's size
  private MovingObject bridge;

  // Use this for initialization
  void Start()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
    tileSize = spriteRenderer.size.x;
    currentPosition = tileNumber * tileSize;
    // Get object's script
    bridge = GameObject.Find("Bridge").GetComponent<MovingObject>();
  }

  // Update is called once per frame
  void Update()
  {
    // If the size of the bridge is inside the tile's position, change the tile's size
    if (bridge.currentSize <= currentPosition && bridge.currentSize >= currentPosition - tileSize)
      spriteRenderer.size = new Vector2(bridge.currentSize - (tileSize * (tileNumber - 1)), tileSize);

  }
}
