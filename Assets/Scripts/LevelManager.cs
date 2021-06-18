using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
  private enum LeverType { Right, Left,  };
  // Current level
  public int level = 0;

  // Contains the levers position (x,y) in the map with their type
  public int[,] leversPosition =
  {
    { 42, 12, (int) LeverType.Right },
    { 40, 22, (int) LeverType.Left },
    { 41, 29, (int) LeverType.Right },
    { 39, 35, (int) LeverType.Left },
    { 47, 34, (int) LeverType.Left },
    { 137, 46, (int) LeverType.Right },
    { 55, 12, (int) LeverType.Right }
  };

  // Start is called before the first frame update
  void Start()
  {

  }

}
