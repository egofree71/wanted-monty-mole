using UnityEngine;

public class Letter : MonoBehaviour
{
  // Bottom left's Position of the screen
  Vector2 bottomLeftPosition;
  // The distance between two moves
  float moveDistance = 4.0f;

  // Letter's size
  Vector3 sizeLetter;
  int letterWidth;

  void Start()
  {
    bottomLeftPosition = Camera.main.ScreenToWorldPoint(Vector2.zero);

    // Get letter size
    sizeLetter = GetComponent<SpriteRenderer>().bounds.size;
    letterWidth = (int)sizeLetter.x;
  }

  void Update()
  {
    // If the letter has not reached the left border, move it
    if (transform.position.x > bottomLeftPosition.x - letterWidth)
      transform.position = new Vector2(transform.position.x - moveDistance, transform.position.y);
    else
      Destroy(gameObject);
  }
}
