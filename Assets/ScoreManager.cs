using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour
{
    //when you want to access this class from somewhere else, call ScoreManager.instance then call the method, for example ScoreManager.instance.AddScore(50);
    public static ScoreManager instance;

    private float score;
    
    [SerializeField]
    private Text text;

    public AudioSource deathSound;
    public AudioSource explosionSound;
    
    private void Awake()
    {
        instance = this;
        score = 0f;

        text.text = "Score: " + score.ToString();
    }

    //Call this method when you want to add score, pass it the amount you want to add
    public void AddScore(float s)
    {
        if (s == 20)
        {
            explosionSound.Play();
        }
        else
        {
            deathSound.pitch = Random.Range(0.9f, 1.1f);
            deathSound.Play();
        }
        score += s;
        text.text = "Score: " + score.ToString();
    }

    //call this method when you want to subtract score, pass it the amount you want to subtract
    public void SubtractScore(float s)
    {
        score -= s;
        text.text = "Score: " + score.ToString();
    }
    public float GetScore()
    {
        return score;
    }
    
}
