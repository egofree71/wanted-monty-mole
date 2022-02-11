using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{

  // The object used to display the logo
  public GameObject logo;

  void Start()
  {
    StartCoroutine(MoveLogo());
  }

  // Move logo and mole
  IEnumerator MoveLogo()
  {
    // Get logo width
    Vector3 sizeLogo = logo.GetComponent<SpriteRenderer>().bounds.size;
    int logoWidth = (int)sizeLogo.x;

    // Move logo to the center
    while (logo.transform.position.x + logoWidth/2> 0)
    {
      logo.transform.Translate(Vector3.left);
      yield return null;
    }
    
  }

  // Update is called once per frame
  void Update()
  {
    // Quit the application when the escape key is pressed
    if (Input.GetKey(KeyCode.Escape))
      Application.Quit();

    // Start playing if the player press the space key
    if (Input.GetKeyDown(KeyCode.Space))
      SceneManager.LoadScene("Main");  
  }
}
