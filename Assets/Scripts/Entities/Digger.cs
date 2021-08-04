using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digger : MonoBehaviour
{
  // How much damage receives the player when he collides with the digger
  private float damage = 0.05f;
  // Is the digger walking
  bool walking = false;
  float moveDistance = 2.0f;
  // The distance to walk
  int distance = 0;
  // The current distance
  int currentDistance = 0;
  // Animator component for the digger
  private Animator diggerAnim;
  private Player player;

  public void Start()
  {
    player = GameObject.Find("Player").GetComponent<Player>();
    diggerAnim = (Animator)GetComponent(typeof(Animator));
    // Stop animation
    diggerAnim.speed = 0.0f;
  }

  // Walk for a distance
  public void Walk(int distance)
  {
    walking = true;
    this.distance = distance;
    diggerAnim.speed = 0.5f;
  }

  // Update is called once per frame
  void Update()
  {
    // Move the digger to the right
    if (walking)
    {
      transform.position = new Vector2(transform.position.x + moveDistance, transform.position.y);
      currentDistance += (int) moveDistance;

      // If digger has reached the distance, stop walking
      if (currentDistance >= distance)
      {
        walking = false;
        diggerAnim.speed = 0.0f;
      }
    }
  }

  // Decrease player health when it collides with the digger
  private void OnTriggerStay2D(Collider2D collision)
  {
    player.decreaseHealth(damage);
  }
}
