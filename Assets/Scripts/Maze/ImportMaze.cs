using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportMaze : MonoBehaviour
{

  // Prefabs used to display the maze
  public Transform verticalWall;
  public Transform horizontalWall;
  public Transform intersectionWall;
  public Transform ladder;

  // Use this for initialization
  void Start()
  {
    // Cell's types of the schematic maze
    const int vertical = 97;
    const int turnLeft = 126;
    const int horizontal = 226;
    const int turnRight = 236;

    const int bVerticalWall = 24;
    const int bHorizontalWall = 23;
    const int bIntersectionWall = 22;

    const int pixelsPerTile = 32;

    // If the maze scene has not been loaded
    if (Global.mazeTiles == null)
      return;

    // Delete previous maze
    GameObject[] mazeTiles = GameObject.FindGameObjectsWithTag("maze");

    foreach (GameObject mazetile in mazeTiles)
      Destroy(mazetile);

    // Get the generated maze of the previous scene
    int[,] cells = Global.mazeTiles;
    // Store the maze's walls. Used in the second step to create ladders
    int[,] graphicalMaze;

    int rows = 9;
    int columns = 40;
    int destinationRows = rows * 3;
    int destinationColumns = columns * 2 + 1;

    // Skip first cell (Maze's exit)
    cells[0, 0] = 0;
    graphicalMaze = new int[destinationRows, destinationColumns];

    // Container which contains all maze graphical cells
    GameObject maze = new GameObject("Maze");
    maze.transform.position = new Vector2(58 * pixelsPerTile, -16 * pixelsPerTile);

    // Create the walls of the graphical maze and store also the result into an array
    for (int row = 0; row < rows; row++)
    {
      for (int column = 0; column < columns; column++)
      {
        int cellType = cells[row, column];

        int destinationColumn = column * 2;
        int destinationRow = row * 3;

        switch (cellType)
        {
          case vertical:
            Transform cellVertical = Instantiate(intersectionWall, maze.transform);
            cellVertical.transform.localPosition = new Vector2(destinationColumn * pixelsPerTile, -destinationRow * pixelsPerTile);
            graphicalMaze[destinationRow, destinationColumn] = bIntersectionWall;

            Transform cellVertical2 = Instantiate(verticalWall, maze.transform);
            cellVertical2.transform.localPosition = new Vector2(destinationColumn * pixelsPerTile, (-destinationRow - 1) * pixelsPerTile);
            graphicalMaze[destinationRow + 1, destinationColumn] = bVerticalWall;

            Transform cellVertical3 = Instantiate(verticalWall, maze.transform);
            cellVertical3.transform.localPosition = new Vector2(destinationColumn * pixelsPerTile, (-destinationRow - 2) * pixelsPerTile);
            graphicalMaze[destinationRow + 2, destinationColumn] = bVerticalWall;
            break;

          case turnLeft:
            Transform cellTurnLeft = Instantiate(horizontalWall, maze.transform);
            cellTurnLeft.transform.localPosition = new Vector2(destinationColumn * pixelsPerTile, -destinationRow * pixelsPerTile);
            graphicalMaze[destinationRow, destinationColumn] = bHorizontalWall;
            break;

          case horizontal:
            Transform cellHorizontal = Instantiate(horizontalWall, maze.transform);
            cellHorizontal.transform.localPosition = new Vector2(destinationColumn * pixelsPerTile, -destinationRow * pixelsPerTile);
            graphicalMaze[destinationRow, destinationColumn] = bHorizontalWall;

            Transform cellHorizontal2 = Instantiate(horizontalWall, maze.transform);
            cellHorizontal2.transform.localPosition = new Vector2((destinationColumn + 1) * pixelsPerTile, -destinationRow * pixelsPerTile);
            graphicalMaze[destinationRow, destinationColumn + 1] = bHorizontalWall;
            break;

          case turnRight:
            Transform cellTurnRight = Instantiate(intersectionWall, maze.transform);
            cellTurnRight.transform.localPosition = new Vector2(destinationColumn * pixelsPerTile, -destinationRow * pixelsPerTile);
            graphicalMaze[destinationRow, destinationColumn] = bIntersectionWall;

            Transform cellTurnRight2 = Instantiate(horizontalWall, maze.transform);
            cellTurnRight2.transform.localPosition = new Vector2((destinationColumn + 1) * pixelsPerTile, -destinationRow * pixelsPerTile);
            graphicalMaze[destinationRow, destinationColumn + 1] = bHorizontalWall;

            Transform cellTurnRight3 = Instantiate(verticalWall, maze.transform);
            cellTurnRight3.transform.localPosition = new Vector2(destinationColumn * pixelsPerTile, (-destinationRow - 1) * pixelsPerTile);
            graphicalMaze[destinationRow + 1, destinationColumn] = bVerticalWall;

            Transform cellTurnRight4 = Instantiate(verticalWall, maze.transform);
            cellTurnRight4.transform.localPosition = new Vector2(destinationColumn * pixelsPerTile, (-destinationRow - 2) * pixelsPerTile);
            graphicalMaze[destinationRow + 2, destinationColumn] = bVerticalWall;
            break;

        }
      }
    }

    // Add maze's exit
    Transform cellTurnRightExit = Instantiate(intersectionWall, maze.transform);
    cellTurnRightExit.transform.localPosition = new Vector2(0, 0);
    graphicalMaze[0, 0] = bIntersectionWall;

    Transform cellTurnRightExit2 = Instantiate(horizontalWall, maze.transform);
    cellTurnRightExit2.transform.localPosition = new Vector2(1 * pixelsPerTile, 0);
    graphicalMaze[0, 1] = bHorizontalWall;

    int verticalSpaces = 0;

    // Add ladders
    for (int column = 0; column < destinationColumns - 1; column++)
    {
      for (int row = destinationRows - 1; row > 0; row--)
      {
        // If the cell is empty, increase the number of spaces
        if (graphicalMaze[row, column] == 0)
          verticalSpaces++;
        else
          verticalSpaces = 0;

        if (verticalSpaces >= 3)
        {
          Transform aLadder = Instantiate(ladder, maze.transform);
          aLadder.transform.localPosition = new Vector2(column * pixelsPerTile, -row * pixelsPerTile);
          graphicalMaze[row, column] = 122;
        }

      }

      verticalSpaces = 0;
    }

    Global.graphicalMazeTiles = graphicalMaze;
  }

}
