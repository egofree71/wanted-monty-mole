using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPlayerMovement : MonoBehaviour
{
  // Animator component for the player
  private Animator playerAnim;
  PlayerMovement characterMovement;

  // Use this for initialization
  void Awake()
  {
    playerAnim = (Animator)GetComponent(typeof(Animator));
    // Get object's script
    characterMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
  }

  // Update is called once per frame
  void Update()
  {
    bool leftArrow = Input.GetKey(KeyCode.A);
    bool rightArrow = Input.GetKey(KeyCode.D);
    bool upArrow = Input.GetKey(KeyCode.W);
    bool downArrow = Input.GetKey(KeyCode.S);

    if (leftArrow | rightArrow | upArrow | downArrow)
      playerAnim.speed = 2.0f;

    if (leftArrow)
    {
      playerAnim.SetInteger("xMove", -1);
      transform.position = new Vector2(transform.position.x - 32.0f, transform.position.y);
      characterMovement.xPos--;
    }

    if (rightArrow)
    {
      playerAnim.SetInteger("xMove", 1);
      transform.position = new Vector2(transform.position.x + 32.0f, transform.position.y);
      characterMovement.xPos++;
    }

    if (upArrow)
    {
      playerAnim.SetInteger("xMove", 0);
      playerAnim.SetBool("yMove", true);
      transform.position = new Vector2(transform.position.x, transform.position.y + 32.0f);
      characterMovement.yPos--;
    }

    if (downArrow)
    {
      playerAnim.SetInteger("xMove", 0);
      playerAnim.SetBool("yMove", true);
      transform.position = new Vector2(transform.position.x, transform.position.y - 32.0f);
      characterMovement.yPos++;
    }
  }


}



