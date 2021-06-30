using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  int score = 0;
  // Text object which display the score
  public Text scoreUI;

  void Start()
  {
    scoreUI = GameObject.Find("Score").GetComponent<Text>();
  }

  // Increase score by one and diplay it
  internal void IncreaseScore()
  {
    score++;
    scoreUI.text = $"{score}";
  }
}
