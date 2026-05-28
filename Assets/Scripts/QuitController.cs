using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void ExitGame()
    {
        // Этот код сработает в уже скомпилированной игре (.exe, .apk и т.д.)
        Application.Quit();

        // Этот код нужен только для проверки в самом редакторе Unity
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}