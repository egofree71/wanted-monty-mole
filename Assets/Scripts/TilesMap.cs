using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
///  This class is used to store into an array all the tiles used in the map
/// </summary>
public class TilesMap : MonoBehaviour
{
  int rows = 56;
  int columns = 256;

  // The one dimensional array which contains the binary file
  byte[] binArray;
  public int[,] tiles;

  void Start()
  {
    tiles = new int[rows, columns];

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

    // Save the binary file into the tiles array, which is divided in rows and columns
    for (int row = 0; row < rows; row++)
      for (int column = 0; column < columns; column++)
      {
        tiles[row, column] = binArray[row * columns + column];
        // If it's an empty character, change its number, as characters below 90
        // are considered as solid in collision test, except this one
        if (tiles[row, column] == 32)
          tiles[row, column] = 256;
      }
  }
}
