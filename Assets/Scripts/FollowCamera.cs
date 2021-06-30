using UnityEngine;
using System.Collections;
public class FollowCamera : MonoBehaviour
{
  // Reference to the player's transform.
  public Transform player;

  void LateUpdate()
  {
    // Follow player
    this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 100, -10);
  }
}