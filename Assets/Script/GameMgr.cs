using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameMgr : MonoBehaviour
{
    public bool cutScene = false;
    public bool onboss = false;
    public PlayableDirector pd;
    public GameObject cutSceneBoss;
    public GameObject cutScenePlayer;
    public GameObject player;
    public GameObject boss;
    public GameObject fade;


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

    private void Update()
    {
        if (!cutScene && UIManager.instance.score >= 2000)
        {
            Destroy(GameObject.FindGameObjectWithTag("GunEffect").gameObject);
            onboss = true;
            cutScene = true;
            cutSceneBoss.SetActive(true);
            cutScenePlayer.SetActive(true);
            var enemys = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in enemys)
            {
                enemy.SetActive(false);
            }
            player.SetActive(false);
            pd.Play();
        }
    }
}
