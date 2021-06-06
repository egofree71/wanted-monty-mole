using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This class is used to modify the global size for all moving objects (bridges and crushers)
/// </summary>
public class MovingObject : MonoBehaviour
{
  public float maxSize;
  public float currentSize;
  // The current direction (1-> increasing, -1->decreasing)
  int direction;
  // Counter used to delay animation
  int counter = 1;

  // Use this for initialization
  void Start()
  {
    if (currentSize == 0)
      direction = -1;
    else
      direction = 1;
  }

  // Update is called once per frame
  void Update()
  {
    counter--;

    if (counter == 0)
      counter = 1;
    else
      return;

    // If the size is zero, change direction
    if (currentSize <= 0)
      direction = -direction;

    // If the size is equals to the maximal's size, change direction
    if (currentSize >= maxSize)
      direction = -direction;

    // Increase/decrease size
    currentSize += 4 * direction;
  }
}
