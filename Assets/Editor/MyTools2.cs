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
    GameObject squares = new GameObject("Squares");

    for (int column = 0; column < 40; column++)
    {
      int positionX;

      positionX = (column * pixelsPerTile) - 650;
      // Create a new square object
      UnityEngine.Object square = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Square.prefab", typeof(GameObject));
      GameObject newSquare = GameObject.Instantiate(square, new Vector2(positionX, 300f), Quaternion.identity) as GameObject;
      // Set the color
      newSquare.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
      Square squareScript = newSquare.GetComponent<Square>();
      squareScript.column = column;

      // Add the prefab into the squares object
      newSquare.transform.parent = squares.transform;
    }
  }

}