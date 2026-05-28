using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void SceneChange(string name)
    {
        SceneManager.LoadScene(name);
        Time.timeScale = 1.0f;

    }
    public void SceneChangeWithSound(string name, string musicType)
    {
        SceneManager.LoadScene(name);
        Time.timeScale = 1.0f;

        ChangeMusicAfterLoad(musicType);
    }
    public void ChangeMusicAfterLoad(string musicType)
    {

        if (SoundManager.Instance != null)
        {
            if (musicType == "Menu")
                SoundManager.Instance.PlayMenuMusic();
            else if (musicType == "Game")
                SoundManager.Instance.PlayGameMusic();
            else if (musicType == "Stop")
                SoundManager.Instance.StopMusic();
        }
    }

    public void LoadMenuWithMusic()
    {
        SceneChangeWithSound("Menu", "Menu");
    }


    public void LoadGameWithMusic()
    {
        SceneChangeWithSound("Game", "Game");
    }
}
