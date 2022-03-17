using TMPro;
using UnityEngine;

public class ScrollingText : MonoBehaviour
{
  // The color gradient used in the text
  public Gradient gradient;
  TMP_Text textComponent;
  public float scrollY = 0.5f;
  // The distance between two moves
  public float moveDistance;
  // The current distamce
  float distance;
  // Distance to travel
  float maxDistance;
  // The text width
  float width;
  // The starting position of the scrolling text
  float startPositionX;
  float startPositionY;
  // The text component rect transform
  RectTransform rectTransform;


  void Start()
  {

    // Get text size
    textComponent = GetComponent<TMP_Text>();
    width = textComponent.preferredWidth;
    float height = textComponent.preferredHeight;

    // Get anchor position
    rectTransform = GetComponent<RectTransform>();
    startPositionX = rectTransform.anchoredPosition.x;
    startPositionY = rectTransform.anchoredPosition.y;

    maxDistance = startPositionX + width;

    Texture2D texture = new Texture2D((int) width, (int) height, TextureFormat.ARGB32, false);

    // Create the texture with the gradient
    for (int y = 0; y < height; y++)
    {
      Color col = gradient.Evaluate((float)y / (float)height);

      for (int x = 0; x < width; x++)
        texture.SetPixel(x, y, col);
    }

    texture.Apply();

    // Set the texture for the text
    textComponent.fontMaterial.SetTexture(ShaderUtilities.ID_FaceTex, texture);
  }

  void Update()
  {
    if (distance < maxDistance)
      distance += 4f;
    else
      distance = 0;

    rectTransform.anchoredPosition = new Vector2(startPositionX - distance, startPositionY);
  }
}
