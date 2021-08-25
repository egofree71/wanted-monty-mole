using System.Collections.Generic;

/// <summary>
///  This class is used to deserializa a json into classes
/// </summary>
[System.Serializable]
public class Levels
{
  public StartData start;
  public EndData end;
  public List<LevelData> list;
}

[System.Serializable]
public class StartData
{
  public List<ObjectData> objects;
}

[System.Serializable]
public class EndData
{
  public int x;
  public int y;
}

[System.Serializable]
public class LevelData
{
  public int number;
  public ObjectData lever;
  public List<ObjectData> objects;
}

[System.Serializable]
public class ObjectData
{
  public int x;
  public int y;
  public string type;
  public float cycle_offset;
}