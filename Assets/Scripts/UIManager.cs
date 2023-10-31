using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager> //Display UI only
{
    public TMP_Text scoreText;
    public TMP_Text timeText;
    public TMP_Text enemyCountText;

    private void Start()
    {
        UpdateScore(0);         //Defaults to 0 on start
        UpdateTime(0);
        UpdateEnemyCount(0);
    }
    public void UpdateScore(int _score)
    {
        scoreText.text = "Score: " + _score.ToString();
    }

    public void UpdateTime(float _time)
    {
        //Convert the float to a string
        timeText.text = _time.ToString("F2"); //F stands for float + 2 decimals
    }

    public void UpdateEnemyCount(int _count)
    {
        enemyCountText.text = "Enemy Count: " + _count.ToString();
    }
}
