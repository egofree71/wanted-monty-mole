using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///  This class is used to manage the intro
/// </summary>
public class IntroManager : MonoBehaviour
{
  // The objects used to display logo and mole
  public GameObject logo;
  public GameObject mole;

  // Store the sprite renderers of squares into an array
  private SpriteRenderer[,] squares = new SpriteRenderer[22, 40];
  // The current row and column for the black rectangle
  int column;
  int row;
  int previousColumn = -1;
  int previousRow;
  // Colors used for animation
  List<Color> colors = new List<Color>() {Color.blue, Color.green, Color.magenta, Color.red, Color.black };
  // The current color
  Color color;
  // The index of the current color
  int colorIndex;
  // The step in the color animation
  int colorStep;

  // A single letter of the message
  public GameObject letter;
  // The message to display at the bottom
  [TextArea(3, 10)]
  public string message;
  // The message splitted in a char array
  char[] messageLetters;
  // Sprites used to display letters
  public Sprite[] letters;
  // The letter index of the message
  int letterIndex = 0;
  // Used to know when create a new letter
  int scrollCounter = 0;

  // Horizontal start positions of moving objects;
  float logoStartPositionX;
  float moleStartPositionX;
  float letterStartPositionX;

  // Letter's width
  Vector3 sizeLetter;
  int letterWidth;

  // Logo's width
  Vector3 sizeLogo;
  int logoWidth;
  // Top right's position of the screen
  Vector2 topRightPosition;
  
  // The distance between two moves
  float moveDistance = 4.0f;
  // Number of steps needed to move a character entirely
  int numberOfSteps;

  void Start()
  {
    // Load squares into an array
    LoadSquares();

    // Set frame rate to 50
    QualitySettings.vSyncCount = 0;
    Application.targetFrameRate = 50;

    // Store the original position of the logo
    logoStartPositionX = logo.transform.position.x;
    moleStartPositionX = mole.transform.position.x;

    // Get logo width
    sizeLogo = logo.GetComponent<SpriteRenderer>().bounds.size;
    logoWidth = (int)sizeLogo.x;

    messageLetters = message.ToCharArray();

    // Get letter size
    sizeLetter = letter.GetComponent<SpriteRenderer>().bounds.size;
    letterWidth = (int)sizeLetter.x;

    // Calculate number of steps
    int moveDistance = (int) letter.GetComponent<Letter>().moveDistance;
    numberOfSteps = letterWidth / moveDistance;

    // Calculate the start position of the letter
    topRightPosition = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    letterStartPositionX = topRightPosition.x + letterWidth;

    InvokeRepeating("ShiftColors", 0, 0.05f);
    StartCoroutine(MoveLogo());
  }

  // Load squares into an array
  private void LoadSquares()
  {
    // Get all squares objects in the Squares container
    GameObject squaresContainer = GameObject.Find("Squares");

    // Store the sprite renderers of squares into a multi-dimensional array
    foreach (Transform square in squaresContainer.transform)
    {
      Square squareScript = square.GetComponent<Square>();
      int row = squareScript.row;
      int column = squareScript.column;
      squares[row, column] = square.gameObject.GetComponent<SpriteRenderer>();
    }

  }

  // Process color animation for squares
  private void ShiftColors()
  {
    // Change color of the current square
    squares[row, column].color = Color.black;

    // If it's time to change color
    if (row == 0 && column == 1)
    {
      color = colors[colorIndex];
      colorIndex++;

      // Reset color index if we have reached the last color
      if (colorIndex == colors.Count)
        colorIndex = 0;
    }

    // Process first row
    if (column < 39 && row == 0)
    {
      if (previousColumn != -1)
        squares[previousRow, previousColumn].color = color;
      previousColumn = column;
      previousRow = row;
      column++;
      return;
    }

    // Process second column
    if (column == 39 && row < 21)
    {
      squares[previousRow, previousColumn].color = color;
      previousColumn = column;
      previousRow = row;
      row++;
      return;
    }

    // Process second row
    if (column > 0 && row == 21)
    {
      squares[previousRow, previousColumn].color = color;
      previousColumn = column;
      previousRow = row;
      column--;
      return;
    }

    // Process first column
    if (column == 0 && row > 0)
    {
      squares[previousRow, previousColumn].color = color;
      previousColumn = column;
      previousRow = row;
      row--;
      return;
    }
  }

  // Move logo and mole
  IEnumerator MoveLogo()
  {
    while (true)
    {
      yield return new WaitForSeconds(2);

      // The distance to travel
      float maxDistance = logoStartPositionX + logoWidth / 2;
      // The current distance
      float distance = 0;

      // Move logo and mole to the center
      while (distance < maxDistance)
      {
        logo.transform.position = new Vector2(logo.transform.position.x - moveDistance, logo.transform.position.y);
        mole.transform.position = new Vector2(mole.transform.position.x - moveDistance, mole.transform.position.y);
        distance += moveDistance;
        yield return null;
      }

      yield return new WaitForSeconds(3);

      maxDistance = logoStartPositionX + logoWidth;
      distance = 0;

      // Move logo and mole to the left
      while (distance < maxDistance)
      {
        logo.transform.position = new Vector2(logo.transform.position.x - moveDistance, logo.transform.position.y);
        mole.transform.position = new Vector2(mole.transform.position.x - moveDistance, mole.transform.position.y);
        distance += moveDistance;
        yield return null;
      }

      // Reset horizontal positions
      logo.transform.position = new Vector2(logoStartPositionX, logo.transform.position.y);
      mole.transform.position = new Vector2(moleStartPositionX, mole.transform.position.y);
      yield return null;
    }
  }

  // Update is called once per frame
  void Update()
  {
    DisplayMessage();

    // Quit the application when the escape key is pressed
    if (Input.GetKey(KeyCode.Escape))
      Application.Quit();

    // Start playing if the player press the space key
    if (Input.GetKeyDown(KeyCode.Space))
      SceneManager.LoadScene("Main");
  }

  // Display the message at the screen's bottom
  void DisplayMessage()
  {
    scrollCounter++;

    // If it's time to display a new letter of the message
    if (scrollCounter >= numberOfSteps)
    {
      // Get the letter to display
      int currentChar = messageLetters[letterIndex] - 65;

      // Skip the space character
      if (currentChar >= 0)
      { 
        // Display a new letter and set its sprite
        GameObject newLetter = Instantiate(letter, new Vector2(letterStartPositionX, -472f), Quaternion.identity);
        newLetter.GetComponent<SpriteRenderer>().sprite = letters[currentChar];
      }

      scrollCounter = 0;

      // If we have not yet reached the end, go the next letter
      if (letterIndex < message.Length - 1)
        letterIndex++;
      else
        letterIndex = 0;

    }
  }
}
