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

    // Add rows made of squares
    for (int column = 0; column < 40; column++)
    {
      int positionX;

      // Add first row
      positionX = (column * pixelsPerTile) + startPositionX;
      Vector2 position = new Vector2(positionX, 300f);
      instantiateSquare(square, 0, column, position, container);

      // Add second row
      position = new Vector2(positionX, 300f - 23 * pixelsPerTile);
      instantiateSquare(square, 25, column, position, container);
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