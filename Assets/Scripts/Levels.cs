using System.Collections.Generic;

[System.Serializable]
public class Levels
{
  public List<LevelData> list;
}

[System.Serializable]
public class LevelData
{
  public int number;
  public Lever lever;
}

[System.Serializable]
public class Lever
{
  public int x;
  public int y;
  public string type;
}
