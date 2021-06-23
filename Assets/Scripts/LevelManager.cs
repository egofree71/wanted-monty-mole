using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum LeverType { Right, Left };

/// <summary>
///  This class is used to manage levels
/// </summary>
public class LevelManager : MonoBehaviour
{

  // Current level
  public int level = 0;
  // Contains the levels objects
  public Levels levels;

  public void LoadJson()
  {
    //read and parse the json
    try
    {
      string jsonString = Resources.Load<TextAsset>("Levels").ToString();
      levels = JsonUtility.FromJson<Levels>(jsonString);
    }
    catch (IOException e)
    {
      Debug.Log(e.Message + "\n Cannot open file.");
      return;
    }
  }

  void Start()
  {
    LoadJson();
  }
}
