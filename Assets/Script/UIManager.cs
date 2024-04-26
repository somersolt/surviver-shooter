using System.Collections;
using UnityEngine.SceneManagement; 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }
    private static UIManager m_instance;
    public GameObject gameoverUI;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public int score = 0;


    public void UpdateScoreText(int newScore)
    {
        score += newScore;
        scoreText.text = "Score : " + score;
    }
    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Main");
    }

}
