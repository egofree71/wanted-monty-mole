using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  int score = 0;
  // Text object which displays the score
  public TextMeshProUGUI scoreUI;

  void Start()
  {
    scoreUI = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
  }

  // Increase score by one and diplay it
  internal void IncreaseScore()
  {
    score++;
    scoreUI.text = $"{score}";
  }
}
