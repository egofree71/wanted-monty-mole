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
  public int level;
  // Contains the entities properties
  public Levels levels;
  // List of prefabs
  public GameObject[] entities;

  public void goNextLevel()
  {
    LoadLevelObjects();
    level++;
  }

  // Load objects for current level
  void LoadLevelObjects()
  {
    if (level == 0)
      return;

    // Get the level index in the json
    List<LevelData> levelList = levels.list;
    int index = levelList.FindIndex(a => a.number == level);

    // Container which contains all objects
    GameObject objects = new GameObject("Objects");

    GameObject.Destroy(GameObject.Find("Objects"));

    InstantiatePrefabs(levelList[index].objects, objects);
  }
  // Load objects at the beginning
  void LoadFirstObjects()
  {
    // Get the level index in the json
    List<ObjectData> firstObjects = levels.start.objects;

    GameObject objects = new GameObject("Objects");
    InstantiatePrefabs(firstObjects, objects);

  }

  // Add all the prefabs for the current level
  private void InstantiatePrefabs(List<ObjectData> objects, GameObject parent)
  {
    // For each object in the current level
    foreach (ObjectData objectData in objects)
    {
      int xPosition = objectData.x;
      int yPosition = objectData.y;
      float animationOffset = objectData.cycle_offset;
      string type = objectData.type;

      GameObject prefab = null;

      // Select the prefab for the current entity
      foreach (GameObject gameObject in entities)
        if (gameObject.name.ToLower() == type)
        {
          prefab = gameObject;
          break;
        }

      // Add the prefab to the map
      GameObject childObject = GameObject.Instantiate(prefab, new Vector2(xPosition, yPosition), Quaternion.identity) as GameObject;

      // Set the animation offset
      if (animationOffset != 0)
      {
        Animator animator = childObject.GetComponent<Animator>();
        animator.SetFloat("offset", animationOffset);
      }
      
      childObject.transform.parent = parent.transform;
    }
  }

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
    LoadFirstObjects();
  }
}
