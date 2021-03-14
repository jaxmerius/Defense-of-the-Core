using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthManager : MonoBehaviour
{
    //when you want to access this class from somewhere else, call HealthManager.instance then call the method, for example HealthManager.instance.SubtractHealth(50);
    public static HealthManager instance;
    
    [SerializeField]
    private RectTransform health;

    private float healthWidth;

    public float maxHealth = 1000f;

    private float currentHealth;

    public GameObject gameOver;

    private void Awake()
    {
        instance = this;
        healthWidth = health.rect.width;
        currentHealth = maxHealth;
    }

    //Call this method when you want to subtract health, pass it the amount you want to subtract
    public void SubtractHealth(float h)
    {

        
        if(currentHealth > 0)
        {
            currentHealth -= h;
            float fittedHealth = Fit(currentHealth, 0f, maxHealth, 0f, healthWidth);
            health.sizeDelta = new Vector2(fittedHealth, health.rect.height);
            Debug.Log(currentHealth);

            if(currentHealth <= 0)
            {
                gameOver.SetActive(true);
            }
        }
        //else if (currentHealth <= 0)
        //{
        //    die
        //    gameOver.SetActive(true);
        //}

    }
    
    //Call this method when you want to add health, pass it the amount you want to add
    public void AddHealth(float h)
    {
        currentHealth += h;
        float fittedHealth = Fit(currentHealth, 0f, maxHealth, 0f, healthWidth);
        health.sizeDelta = new Vector2(fittedHealth, health.rect.height);
        Debug.Log(currentHealth);
    }

    public float GetHealth()
    {
        return currentHealth;
    }
    private static float Fit(float oldValue, float oldMin, float oldMax, float newMin, float newMax)
    {
        var oldRange = (oldMax - oldMin);
        var newRange = (newMax - newMin);
        var newValue = (((oldValue - oldMin) * newRange) / oldRange) + newMin;
        return(newValue);
    }
}
