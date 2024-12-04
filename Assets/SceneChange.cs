using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public float ChangeTime;
    public string sceneName;

    void Update()
    {
        ChangeTime-= Time.deltaTime;
        if (ChangeTime <=0)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
