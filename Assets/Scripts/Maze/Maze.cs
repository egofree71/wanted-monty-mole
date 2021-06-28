using System.Collections;
using System.Collections.Generic;

/// <summary>
///  This class is used to create automatically a maze each time the game starts.
///  It uses the 'hunt and kill' algorithm (c.f https://tinyurl.com/44arru8)
/// </summary>
public class Maze
{
  // Directions constants
  const int Down = 0;
  const int Left = 1;
  const int Up = 2;
  const int Right = 3;

  // Cell's types
  const int Vertical = 97;
  const int TurnLeft = 126;
  const int Horizontal = 226;
  const int TurnRight = 236;

  int rows;
  int columns;
  int row;
  int column;
  int numberOfcells;

  // Generate random numbers for directions
  System.Random rnd;

  int direction;
  // First direction tested for the current cell
  int firstDirection;
  int previousColumn;

  int[,] cells;
  int numberOfcellsFilled;

  // Constructor
  public Maze(int rows, int columns)
  {
    this.rows = rows;
    this.columns = columns;

    numberOfcells = rows * columns - 1;
    cells = new int[rows, columns];
    // Place a seed in the center of the maze
    cells[4, 20] = TurnRight;

    rnd = new System.Random();
  }

  public int[,] Cells
  {
    get { return cells; }
  }

  // Generate the maze for a number of steps
  public void generate(int numberOfSteps)
  {
    int step = 0;

    while (step < numberOfSteps && generationIsNotFinished())
    {
      // If the current cell is not empty, find a non empty adjacent cell
      if (cells[row, column] != 0)
      {

        // If we are not testing 'another' direction
        if (direction == firstDirection)
        {
          // Get a new random direction
          direction = rnd.Next(0, 4);
          firstDirection = direction;
        }

        switch (direction)
        {
          case Down:
            // If we are on the bottom edge or if the cell is not empty, test another direction
            if (row == (rows - 1) || cells[row + 1, column] != 0)
            {
              testAnotherDirection();
            }
            else
            {
              row++;
              cells[row, column] = Vertical;
              firstDirection = direction;
              numberOfcellsFilled++;
              step++;
            }
            break;

          case Left:
            // If we are on the left edge or if the cell is not empty, test another direction
            if (previousColumn < 2 || cells[row, column - 1] != 0)
            {
              testAnotherDirection();
            }
            else
            {
              cells[row, column - 1] = TurnRight;

              if (cells[row, column] == TurnRight)
                cells[row, column] = Horizontal;

              if (cells[row, column] == Vertical)
                cells[row, column] = TurnLeft;

              previousColumn = column;
              column--;
              firstDirection = direction;
              numberOfcellsFilled++;
              step++;
            }
            break;

          case Up:
            // If we are on the top edge or if the cell is not empty, test another direction
            if (row < 1 || cells[row - 1, column] != 0)
            {
              testAnotherDirection();
            }
            else
            {
              cells[row - 1, column] = TurnRight;

              if (cells[row, column] == TurnRight)
                cells[row, column] = Vertical;

              if (cells[row, column] == Horizontal)
                cells[row, column] = TurnLeft;

              row--;
              firstDirection = direction;
              numberOfcellsFilled++;
              step++;
            }
            break;

          case Right:
            // If we are on the right edge or if the cell is not empty, test another direction
            if (previousColumn >= (columns - 2) || cells[row, column + 1] != 0)
            {
              testAnotherDirection();
            }
            else
            {
              previousColumn = column;
              column++;
              cells[row, column] = Horizontal;
              firstDirection = direction;
              numberOfcellsFilled++;
              step++;
            }

            break;
        }

      }
      else
      {
        goToNextCell();
      }

    }

  }

  // If not all cells have been filled
  public bool generationIsNotFinished()
  {
    if (numberOfcellsFilled < numberOfcells)
      return true;
    else
      return false;
  }

  // If an adjacent cell for a given direction is not empty,
  // test another adjacent cell with another direction
  private void testAnotherDirection()
  {
    int tmpDirection = 0;

    if (direction == Down) tmpDirection = Left;
    if (direction == Left) tmpDirection = Up;
    if (direction == Up) tmpDirection = Right;
    if (direction == Right) tmpDirection = Down;

    // If the new direction is not the first direction, use it
    if (tmpDirection != firstDirection)
    {
      direction = tmpDirection;
    }
    else
    {
      // Stop testing new directions
      firstDirection = direction;
      goToNextCell();
    }
  }

  // Set the position to the next cell (if we are not on the last column of the row
  // go the right, or go to the first column of the next row
  private void goToNextCell()
  {
    column++;

    // if we are beyond the second last cell start again to the top left
    if (column >= columns - 1 && row == rows - 1)
    {
      row = 0;
      column = 0;
    }

    // If we are on the last column, go the next row
    if (column == columns)
    {
      column = 0;
      row++;
    }

    previousColumn = column;

  }

}
