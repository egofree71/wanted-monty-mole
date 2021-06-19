using System.Collections.Generic;
using UnityEngine;

public enum LeverType { Right, Left };

/// <summary>
///  This class is used to manager levels
/// </summary>
public class LevelManager : MonoBehaviour
{

  // Current level
  public int level = 0;

  // Contains the levers position (x,y) in the map with their type
  public int[,] leversPosition =
  {
    // Level 0
    { 42, 12, (int) LeverType.Left },
    // Level 1
    { 40, 22, (int) LeverType.Right },
    // Level 2
    { 41, 29, (int) LeverType.Left },
    // Level 3
    { 39, 35, (int) LeverType.Right },
    // Level 4
    { 47, 34, (int) LeverType.Left },
    // Level 5
    { 137, 46, (int) LeverType.Left },
    // Level 6
    { 55, 12, (int) LeverType.Right }
  };

}
