using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public enum LeverType { Right, Left };

/// <summary>
///  This class is used to manage levels
/// </summary>
public class LevelManager : MonoBehaviour
{
  // Current level
  public int level;
  // Contains the levels objects
  public Levels levels;

  public void goNextLevel()
  {
    LoadLevelObjects();
    level++;
  }

  // Load objects for current level
  void LoadLevelObjects()
  {
    // Get the level index in the json
    List<LevelData> levelList = levels.list;
    int index = levelList.FindIndex(a => a.number == level);

    // Container which contains all objects
    GameObject.Destroy(GameObject.Find("Objects"));
    GameObject objects = new GameObject("Objects");
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
      string type = objectData.type;

      // Add the prefab for the current object to the map
      UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Characters/" + type + ".prefab", typeof(GameObject));
      GameObject childObject = GameObject.Instantiate(prefab, new Vector2(xPosition, yPosition), Quaternion.identity) as GameObject;
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
