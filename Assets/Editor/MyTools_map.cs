using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
///  This class is used to import the binary map into the main scene.
///  Before running the script, in the editor some tiles should be deleted manually, 
///  otherwise the editor will not detect the scene has been modified and it will not save modifications
/// </summary>
public partial class MyTools
{
  private const int pixelsPerTile = 32;

  // Load the maze scence
  [MenuItem("MyTools/Play")]
  public static void RunMainScene()
  {
    EditorSceneManager.OpenScene("Assets/Scenes/Maze.unity");
    EditorApplication.isPlaying = true;
  }

  // Import the map into the main scene
  [MenuItem("MyTools/Import map")]
  public static void ImportMap()
  {
    EditorSceneManager.OpenScene("Assets/Scenes/Main.unity");

    int rows = 56;
    int columns = 256;
    // The list of tiles used in the map
    UnityEngine.Object[] tiles = new UnityEngine.Object[254];
    // The one dimensional array which contains the binary file
    byte[] binArray;
    // The two dimensional array which stores the tiles containted in the binary file
    int[,] tilesArray = new int[rows, columns];
    // The gameobject LevelManager manages the levels
    LevelManager levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    levelManager.LoadJson();
    // List of level data
    List<LevelData> levelList = levelManager.levels.list;
    // The number of the current tile
    int tileNumber;
    // Used to place special tiles to a different column or row
    int rowOffset, columnOffset;

    //read the binary file which contains the map
    try
    {
      binArray = Resources.Load<TextAsset>("Map").bytes;
    }
    catch (IOException e)
    {
      Debug.Log(e.Message + "\n Cannot open file.");
      return;
    }

    // Store tiles into memory
    for (int i = 0; i < 254; i++)
    {
      UnityEngine.Object tile = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Environment/background_" + i + ".prefab", typeof(GameObject));
      if (tile != null)
        tiles[i] = tile;
    }

    // The root object (folder) which contains all background objects
    GameObject.DestroyImmediate(GameObject.Find("Backgrounds"));
    GameObject backgrounds = new GameObject("Backgrounds");

    // Contains all tiles which must be processed
    int[] validTiles = {27, 34, 35, 36, 37, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79,
                      80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99,
                      100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115,
                      116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131,
                      132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147,
                      148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163,
                      164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179,
                      180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 193, 200, 207, 225, 228, 229,
                      230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245,
                      246, 247, 248, 249, 250, 251, 252, 253, 254};

    // Save the binary file into the tiles array, which is divided in rows and columns
    for (int row = 0; row < rows; row++)
      for (int column = 0; column < columns; column++)
        tilesArray[row, column] = binArray[row * columns + column];

    // Process each tile of the map
    for (int row = 0; row < rows; row++)
    {
      for (int column = 0; column < columns; column++)
      {
        // Save the current value for the current row and column
        tileNumber = tilesArray[row, column];

        // If it's a valid tile, process it
        if (Array.IndexOf(validTiles, tileNumber) >= 0)
        {
          rowOffset = 0;
          columnOffset = 0;

          // If it's a tile of bridge 'going left', the sprite is flipped vertically and horizontally,
          // and the tile has to be placed to the next column and row
          if (tileNumber >= 140 && tileNumber <= 146)
          {
            rowOffset = 1;
            columnOffset = 1;
          }

          // Create a prefab with the offset stored in the binary file and add it to the scene
          GameObject prefab = instantiatePrefab(tiles, tileNumber, row - rowOffset, column + columnOffset, backgrounds);

          // If we are dealing with a maze tile, tag it
          if (column >= 58 && column <= 137 && row >= 16 && row <= 42)
            prefab.tag = "maze";

          // If we are dealing with a lever, tag the prefab
          foreach (LevelData level in levelList)
          {
            int xLeverPosition = level.lever.x;
            int yLeverPosition = level.lever.y;
            string leverType = level.lever.type;

            int xOffset;

            // Tag also the lever prefab nearby
            if (leverType == "right")
              xOffset = 1;
            else
              xOffset = -1;

            if ((xLeverPosition == column && yLeverPosition == row) ||
                (xLeverPosition + xOffset == column && yLeverPosition == row))
              prefab.tag = "lever_" + level.number;
          }
        }
        else
        {
          // If it's not a space character
          if (tileNumber != 32)
            Debug.Log("Invalid tile value:" + tileNumber);
        }
      }

    }

    addTilesAroundMap(tiles, backgrounds, tilesArray);

  }

  // Add decorating tiles arround the map
  private static void addTilesAroundMap(UnityEngine.Object[] tiles, GameObject backgrounds, int[,] tilesArray)
  {
    int rows = 56;
    int columns = 256;
    // Number of rows to add
    int rowsToAdd = 10;

    // Add 'earth' tiles above and below
    for (int row = -rowsToAdd; row < 0; row++)
    {
      for (int column = 0; column < columns; column++)
      {
        instantiatePrefab(tiles, 89, row, column, backgrounds);
        instantiatePrefab(tiles, 89, row + rows + rowsToAdd, column, backgrounds);
      }
    }

    // Number of columns to add
    int columnsToAdd = 14;

    // Add 'earth' tiles to the left and right
    for (int row = -rowsToAdd; row < rows + rowsToAdd; row++)
    {
      for (int column = -columnsToAdd; column < 0; column++)
      {
        instantiatePrefab(tiles, 89, row, column, backgrounds);
        instantiatePrefab(tiles, 89, row, column + columns + columnsToAdd + 2, backgrounds);
      }
    }

    int tileToAdd;

    // Take the first two columns and add them to the right of the map
    for (int row = -rowsToAdd; row < rows + rowsToAdd; row++)
    {
      for (int column = columns; column < columns + 2; column++)
      {
        // If we are not inside the map use an 'earth' tile
        if (row <= 1 || row >= rows - 1)
          tileToAdd = 89;
        else
          tileToAdd = tilesArray[row + 1, column - columns];

        instantiatePrefab(tiles, tileToAdd, row, column, backgrounds);
      }
    }
  }

  // Instantiate a prefab with for a given row and column
  private static GameObject instantiatePrefab(UnityEngine.Object[] tiles, int tileNumber, int row, int column, GameObject backgrounds)
  {
    GameObject childObject = GameObject.Instantiate(tiles[tileNumber], new Vector2(column * pixelsPerTile, -row * pixelsPerTile), Quaternion.identity) as GameObject;

    // Add the prefab into the Backgrounds object
    childObject.transform.parent = backgrounds.transform;

    return childObject;
  }

}