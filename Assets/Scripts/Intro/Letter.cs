using UnityEngine;

public class Letter : MonoBehaviour
{
  // Bottom left's position of the screen
  Vector2 bottomLeftPosition;
  // The distance between two moves
  public float moveDistance;
  // The color gradient used to fade text
  public Gradient colorGradient;

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
    {
      transform.position = new Vector2(transform.position.x - moveDistance, transform.position.y);
      // Set the color for the current position
      Vector3 viewPortPosition = Camera.main.WorldToViewportPoint(transform.position);
      GetComponent<SpriteRenderer>().color = colorGradient.Evaluate(viewPortPosition.x);
    }
    else
    {
      Destroy(gameObject);
    }
  }
}
