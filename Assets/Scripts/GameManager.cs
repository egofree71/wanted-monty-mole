using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class GameManager : MonoBehaviour
{
  int score = 0;
  bool bucketTaken = false;

  // Text object which displays the score
  public TextMeshProUGUI scoreUI;
  // The objects used for the 'game over' sequence
  public GameObject playerTopLeft;
  public GameObject playerTopRight;
  public GameObject playerBottomLeft;
  public GameObject playerBottomRight;
  public GameObject graveBottom;
  public GameObject graveTop;

  private void Start()
  {
    // Set frame rate to 50
    QualitySettings.vSyncCount = 0;
    Application.targetFrameRate = 50;
  }


  void Update()
  {
    // Quit the application when the escape key is pressed
    if (Input.GetKey(KeyCode.Escape))
      Application.Quit();
  }

  // If the bucket is taken, increase score, and set the digger in motion
  public void SetBucketIsTaken()
  {
    bucketTaken = true;
    IncreaseScore();
    Digger digger = GameObject.Find("Digger(Clone)").GetComponent<Digger>();
    digger.Walk(796);
  }

  // Increase score by one and diplay it
  internal void IncreaseScore()
  {
    score++;
    // Display the score with three digits
    int firstDigit = score % 10;
    int secondDigit = score / 10 % 10;
    int thirdDigit = score / 100 % 10;
    scoreUI.SetText("<sprite={0}><sprite={1}><sprite={2}>", thirdDigit, secondDigit, firstDigit);
  }


  // Start the game over process
  internal void TriggerGameOver()
  {
    // Desactivate the player
    GameObject player = GameObject.Find("Player");
    player.SetActive(false);
    Camera.main.backgroundColor = Color.red;

    // Delete entities of the current level
    GameObject objects = GameObject.Find("Objects");
    GameObject.Destroy(GameObject.Find("Objects"));

    Animator[] animatorsInTheScene = FindObjectsOfType(typeof(Animator)) as Animator[];

    // Stop all animated tiles
    foreach (Animator animatorItem in animatorsInTheScene)
      animatorItem.enabled = false;

    CrusherTile[] crusherTiles = FindObjectsOfType(typeof(CrusherTile)) as CrusherTile[];

    // Stop crushers animation
    foreach (CrusherTile crusherTile in crusherTiles)
      crusherTile.enabled = false;

    BridgeTile[] bridgeTiles = FindObjectsOfType(typeof(BridgeTile)) as BridgeTile[];

    // Stop bridges animation
    foreach (BridgeTile bridgeTile in bridgeTiles)
      bridgeTile.enabled = false;

    StartCoroutine(ProcessGameOver(player));
  }

  // Display the game over sequence with a coroutine
  IEnumerator ProcessGameOver(GameObject player)
  {
    // Get the current position of the player
    int xPosition = (int)player.transform.position.x;
    int yPosition = (int)player.transform.position.y;

    // Get width and height for the left part of the dead player
    Vector3 sizePlayerTopLeft = playerTopLeft.GetComponent<SpriteRenderer>().bounds.size;
    int width = (int)sizePlayerTopLeft.x;
    Vector3 sizePlayerBottomLeft = playerBottomLeft.GetComponent<SpriteRenderer>().bounds.size;
    int height = (int)sizePlayerBottomLeft.y;

    // Display dead player
    GameObject newPlayerTopLeft = GameObject.Instantiate(playerTopLeft, new Vector2(xPosition, yPosition + height), Quaternion.identity);
    GameObject newPlayerTopRight = GameObject.Instantiate(playerTopRight, new Vector2(xPosition + width, yPosition + height), Quaternion.identity);
    GameObject newPlayerBottomLeft = GameObject.Instantiate(playerBottomLeft, new Vector2(xPosition, yPosition), Quaternion.identity);
    GameObject newPlayerBottomRight = GameObject.Instantiate(playerBottomRight, new Vector2(xPosition + width, yPosition), Quaternion.identity);

    yield return null;

    // The distance to travel
    float maxDistance = 280f;
    // The current distance
    float distance = 0;
    // The distance between two moves
    float moveDistance = 4.0f;

    // Move the different parts of the dead player
    while (distance < maxDistance)
    {
      newPlayerTopLeft.transform.position = new Vector2(newPlayerTopLeft.transform.position.x - moveDistance, newPlayerTopLeft.transform.position.y + moveDistance);
      newPlayerTopRight.transform.position = new Vector2(newPlayerTopRight.transform.position.x + moveDistance, newPlayerTopRight.transform.position.y + moveDistance);
      newPlayerBottomLeft.transform.position = new Vector2(newPlayerBottomLeft.transform.position.x - moveDistance, newPlayerBottomLeft.transform.position.y - moveDistance);
      newPlayerBottomRight.transform.position = new Vector2(newPlayerBottomRight.transform.position.x + moveDistance, newPlayerBottomRight.transform.position.y - moveDistance);

      distance += moveDistance;
      // slow down move according to distance
      yield return new WaitForSeconds(distance / 6000);
    }

    // Delete dead player
    Destroy(newPlayerTopLeft);
    Destroy(newPlayerTopRight);
    Destroy(newPlayerBottomLeft);
    Destroy(newPlayerBottomRight);

    // Height of the black rectangle displayed in the hud
    int blackRectHeight = 180;

    // Get screen size
    PixelPerfectCamera camera = GameObject.Find("Main Camera").GetComponent<PixelPerfectCamera>();
    int screenHeight = camera.refResolutionY - blackRectHeight;

    // Get height of the bottom grave
    Vector3 sizeGraveBottom = graveBottom.GetComponent<SpriteRenderer>().bounds.size;
    int bottomGraveHeight = (int)sizeGraveBottom.y;

    distance = 0;
    maxDistance = screenHeight / 2 + bottomGraveHeight;
    moveDistance = 10.0f;

    // Display the bottom grave
    GameObject newGraveBottom = GameObject.Instantiate(graveBottom, new Vector2(xPosition, yPosition - maxDistance), Quaternion.identity);

    yield return null;

    // Move the bottom grave to the center
    while (distance < maxDistance)
    {
      newGraveBottom.transform.position = new Vector2(newGraveBottom.transform.position.x, newGraveBottom.transform.position.y + moveDistance);
      distance += moveDistance;
      yield return null;
    }

    // Get height of the top grave
    Vector3 sizeGraveTop = graveBottom.GetComponent<SpriteRenderer>().bounds.size;
    int topGraveHeight = (int)sizeGraveTop.y;

    distance = 0;
    maxDistance = screenHeight / 2 - topGraveHeight;
    moveDistance = 6.0f;

    // Display the top grave
    GameObject newGraveTop = GameObject.Instantiate(graveTop, new Vector2(xPosition, yPosition + maxDistance + topGraveHeight + 10), Quaternion.identity);

    yield return null;

    // Move the top grave to the center
    while (distance < maxDistance)
    {
      newGraveTop.transform.position = new Vector2(newGraveTop.transform.position.x, newGraveTop.transform.position.y - moveDistance);
      distance += moveDistance;

      // slow down move according to distance
      float delay = maxDistance - distance;

      if (delay > 0)
        delay = delay / 12000;

      yield return new WaitForSeconds(delay);
    }

    // Reload scene
    SceneManager.LoadScene("Intro");
  }
}
