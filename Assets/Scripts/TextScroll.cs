using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextScroll : MonoBehaviour
{
  // The message to display
  public TextMeshProUGUI endMessage;
  int distance = 0;
  // The distance to travel
  int maxDistance = 1280;


  void Start()
  {
    StartCoroutine(displayEndMessage());
  }

  // Display the end message with a vertical scroll
  IEnumerator displayEndMessage()
  {

    while (distance < maxDistance)
    {
      endMessage.transform.Translate(Vector3.up);
      distance++;
      yield return new WaitForSeconds(0.025f);
    }

    yield return new WaitForSeconds(4f);

    SceneManager.LoadScene("Intro");
  }

  // Update is called once per frame
  void Update()
  {
    // Quit the application when the escape key is pressed
    if (Input.GetKey(KeyCode.Escape))
      Application.Quit();

    // Load the inro when the space key is pressed
    if (Input.GetKeyDown(KeyCode.Space))
      SceneManager.LoadScene("Intro");
  }
}
