using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

  public Image image;

  public void setHealth(float value)
  {
    image.fillAmount = value / 100;
  }
}
