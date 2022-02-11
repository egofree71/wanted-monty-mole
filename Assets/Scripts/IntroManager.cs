using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{

  // The object used to display the logo
  public GameObject logo;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    // Start playing if the player press the space key
    if (Input.GetKeyDown(KeyCode.Space))
      SceneManager.LoadScene("Main");
    
  }
}
