using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(GameData.mainScene);
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("InkSaveState");
        PlayerPrefs.Save();
        Debug.Log("�������� �������!");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}