using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MazeGenerator : MonoBehaviour
{
  Maze maze;
  // Prefabs used to display the maze
  public Transform verticalCell;
  public Transform turnLeftCell;
  public Transform horizontalCell;
  public Transform turnRightCell;
  GameObject mazeObject;

  // Cell's types
  const int vertical = 97;
  const int turnLeft = 126;
  const int horizontal = 226;
  const int turnRight = 236;

  // Use this for initialization
  void Start()
  {
    // Initialize a new maze
    maze = new Maze(9, 40);
  }

  // Update is called once per frame
  void Update()
  {
    if (maze.generationIsNotFinished())
    {
      maze.generate(5);
      displayMaze(maze);
    }
    else
    {
      // Store the maze for the next scene into a static class
      Global.mazeTiles = maze.Cells;
      SceneManager.LoadScene("Main");
    }

  }

  public void displayMaze(Maze maze)
  {
    int[,] cells = maze.Cells;
    int rowsNumber = cells.GetLength(0);
    int columnsNumber = cells.GetLength(1);

    Transform cell;
    // Destroy previous maze
    Destroy(mazeObject);
    mazeObject = new GameObject("Maze");

    // Scan each row
    for (int rowMaze = 0; rowMaze < rowsNumber; rowMaze++)
    {
      // Scan each column
      for (int columnMaze = 0; columnMaze < columnsNumber; columnMaze++)
      {
        // Display a prefab on the screen depending on the cell type
        switch (cells[rowMaze, columnMaze])
        {
          case vertical:
            cell = Instantiate(verticalCell, new Vector2(columnMaze, -rowMaze), Quaternion.identity);
            cell.parent = mazeObject.transform;
            break;
          case turnLeft:
            cell = Instantiate(turnLeftCell, new Vector2(columnMaze, -rowMaze), Quaternion.identity);
            cell.parent = mazeObject.transform;
            break;
          case horizontal:
            cell = Instantiate(horizontalCell, new Vector2(columnMaze, -rowMaze), Quaternion.identity);
            cell.parent = mazeObject.transform;
            break;
          case turnRight:
            cell = Instantiate(turnRightCell, new Vector2(columnMaze, -rowMaze), Quaternion.identity);
            cell.parent = mazeObject.transform;
            break;
        }

      }
    }
  }
}
