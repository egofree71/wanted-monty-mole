using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
  private Player player;

  // Start is called before the first frame update
  void Start()
  {
    player = GameObject.Find("Player").GetComponent<Player>();
  }

  // If the player enters the shield, he's protected
  private void OnTriggerEnter2D(Collider2D collision)
  {
    player.setProtected(true);
  }

  // If the player exits the shield, he's no more protected
  private void OnTriggerExit2D(Collider2D collision)
  {
    player.setProtected(false);
  }

}
