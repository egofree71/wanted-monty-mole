using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coal : MonoBehaviour
{
  // If the player collides with the object
  private void OnTriggerEnter2D(Collider2D collision)
  {
    Destroy(gameObject);
  }

  // Update is called once per frame
  void Update()
    {
        
    }
}
