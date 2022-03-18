using TMPro;
using UnityEngine;

public class ScrollingText : MonoBehaviour
{
  // List of gradientsused for the text
  public Gradient[] gradient;
  // The index of the gradient currently used
  int gradientIndex;
  int gradientNumbers;
  // Textures for color gradients
  Texture2D[] textures;

  TMP_Text textComponent;
  public float scrollY;
  // The distance between two moves
  public float moveDistance;
  // The current distamce
  float distance;
  // Distance to travel
  float maxDistance;
  // Text size
  float width;
  float height;
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
    height = textComponent.preferredHeight;

    // Get anchor position
    rectTransform = GetComponent<RectTransform>();
    startPositionX = rectTransform.anchoredPosition.x;
    startPositionY = rectTransform.anchoredPosition.y;

    maxDistance = startPositionX + width;

    gradientNumbers = gradient.Length;

    // Create all textures and set the first texture
    textures = new Texture2D[gradientNumbers];
    createTextures();
    textComponent.fontMaterial.SetTexture(ShaderUtilities.ID_FaceTex, textures[0]);
  }

  // Set the next gradient texture
  public void updateTexture()
  {
    if (gradientIndex < gradientNumbers - 1)
      gradientIndex++;
    else
      gradientIndex = 0;

    // Set the texture for the text
    textComponent.fontMaterial.SetTexture(ShaderUtilities.ID_FaceTex, textures[gradientIndex]);
  }

  // Create textures for all gradients
  public void createTextures()
  {
    // Process each gradient
    for (int i = 0; i < gradientNumbers; i++)
    {
      Texture2D texture = new Texture2D((int)width, (int)height, TextureFormat.ARGB32, false);

      // Create the texture for the current gradient gradient
      for (int y = 0; y < height; y++)
      {
        Color col = gradient[i].Evaluate((float)y / (float)height);

        for (int x = 0; x < width; x++)
          texture.SetPixel(x, y, col);
      }

      texture.Apply();

      textures[i] = texture;
    }

  }

  void Update()
  {
    // Increase the distance till we have not reached the end
    if (distance < maxDistance)
      distance += moveDistance;
    else
      distance = 0;

    // Scroll text
    rectTransform.anchoredPosition = new Vector2(startPositionX - distance, startPositionY);

    // Shift color gradient
    float offsetY = Mathf.Repeat(Time.time * scrollY, 1);
    textComponent.fontMaterial.SetTextureOffset(ShaderUtilities.ID_FaceTex, new Vector2(0, offsetY));
  }
}
