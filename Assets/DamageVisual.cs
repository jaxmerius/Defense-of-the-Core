using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVisual : MonoBehaviour
{
    public float showTime;
    private float time;

    void Update() 
    {
        //when the visual is activated it, show it for the amount set as showTime
        if(gameObject.activeSelf)
        {
            time += Time.deltaTime;
            if(time >= showTime)
            {
                time = 0;
                gameObject.SetActive(false);
            }
        }
    }
}
