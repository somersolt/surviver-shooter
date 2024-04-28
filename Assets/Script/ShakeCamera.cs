using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShakeCamera : MonoBehaviour
{
    public float shakeDuration = 0.1f;
    public float shakeAmount = 0.05f;
    public float decreaseFactor = 1.0f;

    public CinemachineVirtualCamera v1;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = v1.transform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            v1.transform.localPosition = originalPosition + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            v1.transform.localPosition = originalPosition;
        }
    }

    public void Shake()
    {
        shakeDuration = 0.1f;
    }

    public void BossFight()
    {
        GameMgr.instance.cutSceneBoss.SetActive(false);
        GameMgr.instance.cutScenePlayer.SetActive(false);
        GameMgr.instance.boss.SetActive(true);
        GameMgr.instance.player.SetActive(true);
    }

}
