using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  int score = 0;
  bool bucketTaken = false;

  // Text object which displays the score
  public TextMeshProUGUI scoreUI;
  // The objects used for 'game over'
  public GameObject playerTopLeft;
  public GameObject playerTopRight;
  public GameObject playerBottomLeft;
  public GameObject playerBottomRight;


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

  // Display the game over sequence
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

    yield return new WaitForSeconds(1f);

    // Reload scene
    Scene scene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(scene.name);
  }
}
