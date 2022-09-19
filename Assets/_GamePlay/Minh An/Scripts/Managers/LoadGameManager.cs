using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameManager : GenerticSingleton<LoadGameManager>
{
    public void ReloadGame()
    {
        StartCoroutine(LoadLevel(0));
    }
    public void LoadLevelGame(int levelIndex) {
        StartCoroutine(LoadLevel(levelIndex));
    }
    IEnumerator LoadLevel(int levelIndex) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);
        while (!operation.isDone)
        {
            yield return null;
        }
        EnventManager.TriggerEvent(EventName.ChangeSkinOnPlayer.ToString());
    }
}
