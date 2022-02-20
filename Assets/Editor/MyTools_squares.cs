using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public partial class MyTools
{
  // Add squares to intro scene
  [MenuItem("MyTools/Add squares")]
  public static void AddSquares()
  {
    EditorSceneManager.OpenScene("Assets/Scenes/Intro.unity");

    // Add a container which contains all squares objects
    GameObject.DestroyImmediate(GameObject.Find("Squares"));
    GameObject container = new GameObject("Squares");

    // Load the square prefab
    UnityEngine.Object square = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Square.prefab", typeof(GameObject));

    // Get the left position of the camera
    Vector2 bottomLeftPosition = Camera.main.ScreenToWorldPoint(Vector2.zero);
    int startPositionX = (int)bottomLeftPosition.x;

    // Use logo's vertical position for first row
    GameObject logo = GameObject.Find("Logo");
    float logoHeight = logo.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
    float startPositionY = logo.transform.position.y + logoHeight;

    const int secondRowPosition = 3;
    const int thirdRowPosition = 21;

    // Add rows made of squares
    for (int column = 0; column <= 39; column++)
    {
      int positionX;

      // Add first row
      positionX = column * pixelsPerTile + startPositionX;
      Vector2 position = new Vector2(positionX, startPositionY);
      instantiateSquare(square, 0, column, position, container);

      // Add second row
      position = new Vector2(positionX, startPositionY - thirdRowPosition * pixelsPerTile);
      instantiateSquare(square, thirdRowPosition, column, position, container);

      // Add third row
      if (column > 12 && column <= 26)
      { 
        position = new Vector2(positionX, startPositionY - secondRowPosition * pixelsPerTile);
        instantiateSquare(square, 3, column, position, container);
      }
    }

    const int secondColumnPosition = 13;
    const int thirdColumnPosition = 26;
    const int fourthColumnPosition = 39;
    int numberOfRows = thirdRowPosition - 1;

    // Add columns
    for (int row = 1; row <= numberOfRows; row++)
    {
      // Add first column
      Vector2 position = new Vector2(startPositionX, startPositionY - row * pixelsPerTile);
      instantiateSquare(square, row, 0, position, container);

      // Add second and third row
      if (row > 0 && row <= 2)
      {
        position = new Vector2(startPositionX + secondColumnPosition * pixelsPerTile, startPositionY - row * pixelsPerTile);
        instantiateSquare(square, row, secondColumnPosition, position, container);

        position = new Vector2(startPositionX + thirdColumnPosition * pixelsPerTile, startPositionY - row * pixelsPerTile);
        instantiateSquare(square, row, thirdColumnPosition, position, container);
      }

      // Add fourth column
      position = new Vector2(startPositionX + fourthColumnPosition * pixelsPerTile, startPositionY - row * pixelsPerTile);
      instantiateSquare(square, row, fourthColumnPosition, position, container);
    }
  }

  // Instantiate a square object for a given row and column
  private static void instantiateSquare(UnityEngine.Object square, int row, int column, Vector2 position, GameObject container)
  {
    // Create a new square object
    GameObject newSquare = GameObject.Instantiate(square, position, Quaternion.identity) as GameObject;
    // Set the color
    newSquare.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
    // Set row and column
    Square squareScript = newSquare.GetComponent<Square>();
    squareScript.row = row;
    squareScript.column = column;

    // Add the prefab into the squares container
    newSquare.transform.parent = container.transform;
  }
}