using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    public static GameMgr instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameMgr>();
            }

            return m_instance;
        }
    }
    private static GameMgr m_instance;
    void Start()
    {
        UIManager.instance.score = 0;
        UIManager.instance.UpdateScoreText(0);

    }


}
