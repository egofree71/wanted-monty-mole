using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazelnut : MonoBehaviour
{
  // The maximum distance
  int maxDistance = 55;
  float yStart;
  float moveStep = 2.0f;
  // The current distance
  int currentDistance = 0;
  // The game object which manages the squirrel
  GameObject squirrel;

  void Start()
  {
    // Store the vertical position
    yStart = transform.position.y;
    squirrel = GameObject.Find("Squirrel(Clone)");
  }

  // Update is called once per frame
  void Update()
  {
    // If the hazelnut has not reached the maximum distance, go down
    if (currentDistance < maxDistance * moveStep)
    {
      transform.position = new Vector2(transform.position.x, transform.position.y - moveStep);
      currentDistance++;
    }
    // Reset position
    else
    {
      transform.position = new Vector2(squirrel.transform.position.x, yStart);
      currentDistance = 0;
    }
  }
}