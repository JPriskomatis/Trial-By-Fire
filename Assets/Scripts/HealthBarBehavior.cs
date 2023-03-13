using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehavior : MonoBehaviour
{

    public Slider slider;
    public Color low;
    public Color high;

    public Vector3 Offset;



    public void SetHealth(int curHealth, int maxHealth)
    {
        slider.gameObject.SetActive(curHealth < maxHealth);
        slider.value = curHealth;
        slider.maxValue = maxHealth;

        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, slider.normalizedValue);
    }


    //If enemy is not being damaged to for a long period of time then their HP bar is hidden again.
    public void DisableHealth()
    {
        slider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);

        
        
    }
}
