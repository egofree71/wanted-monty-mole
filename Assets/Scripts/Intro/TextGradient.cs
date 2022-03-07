using System.IO;
using TMPro;
using UnityEngine;

/// <summary>
///  Class used to set a gradient texture on a text
/// </summary>
[ExecuteInEditMode]
public class TextGradient : MonoBehaviour
{
  // The color gradient used in the text
  public Gradient gradient;

  void OnValidate()
  {
    // Update texture in the editor
    setGradient();
  }

  void Start()
  {
    // Update texture at runtime
    setGradient();
  }

  void setGradient()
  {
    // Get text size
    TMP_Text textComponent = GetComponent<TMP_Text>();
    textComponent.ForceMeshUpdate();
    Vector2 size = textComponent.GetRenderedValues(false);
    int width = (int)size.x;
    int height = (int)size.y;

    Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);

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
}
