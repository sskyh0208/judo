using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour
{
    public void LoadTo(string sceneName)
    {
        // SoundManager.instance.PlayBGM(sceneName);
        FadeIOManager.instance.FadeOutToIn( () => Load(sceneName));
    }

    private void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
