using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  int score = 0;
  bool bucketTaken = false;

  // Text object which displays the score
  public TextMeshProUGUI scoreUI;

  void Start()
  {
    scoreUI = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
  }

  // If the bucket is taken, increase score, and set the digger in motion
  public void SetBucketIsTaken()
  {
    bucketTaken = true;
    IncreaseScore();
    Digger digger = GameObject.Find("Digger(Clone)").GetComponent<Digger>();
    digger.Walk(199);
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
}
