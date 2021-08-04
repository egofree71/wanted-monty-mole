using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazelnut : MonoBehaviour
{
  // How much damage receives the player when he collides with the hazelnut
  private float damage = 0.05f;
  // The maximum distance
  int maxDistance = 220;
  float yStart;
  float moveStep = 2.0f;
  // The current distance
  int currentDistance = 0;
  // The game object which manages the squirrel
  GameObject squirrel;
  private Player player;

  void Start()
  {
    // Store the vertical position
    yStart = transform.position.y;
    squirrel = GameObject.Find("Squirrel(Clone)");
    player = GameObject.Find("Player").GetComponent<Player>();
  }

  // Update is called once per frame
  void Update()
  {
    // If the hazelnut has not reached the maximum distance, go down
    if (currentDistance < maxDistance)
    {
      transform.position = new Vector2(transform.position.x, transform.position.y - moveStep);
      currentDistance += (int) moveStep;
    }
    // Reset position
    else
    {
      transform.position = new Vector2(squirrel.transform.position.x, yStart);
      currentDistance = 0;
    }
  }

  // Decrease player health when it collides with the hazelnut
  private void OnTriggerStay2D(Collider2D collision)
  {
    player.decreaseHealth(damage);
  }
}
