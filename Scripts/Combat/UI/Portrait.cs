using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Portrait : MonoBehaviour
{
    public Image backgroundImage;
    public Image characterImage;
    public Slider healthSlider;
    public TextMeshProUGUI characterName;
    public void SetImage(Sprite sprite)
    {
        characterImage.sprite = sprite;
    }

    public void SetHealthMinMax(int minValue, int maxValue)
    {
        healthSlider.minValue = minValue;
        healthSlider.maxValue = maxValue;
    }

    public void SetHealthValue(int value)
    {
        healthSlider.value = value;
    }

    public void SetName(string name)
    {
        characterName.text = name;
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }

    public void SetAsEnemy()
    {
        backgroundImage.color = Color.red;
    }

    public void SetAsAlly()
    {
        backgroundImage.color = Color.green; // and yellow something, set
    }
}
