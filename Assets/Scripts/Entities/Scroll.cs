using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
  // If the player collides with the object
  private void OnTriggerEnter2D(Collider2D collision)
  {
    Destroy(gameObject);
  }

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
}
