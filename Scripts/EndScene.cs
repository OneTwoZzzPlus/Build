using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{

    public TextMeshProUGUI result;
    public int mon = 0;
    public int lib = 0;
    public int soc = 0;

    public void Awake()
    {
        mon = GameData.mon;
        lib = GameData.lib;
        soc = GameData.soc;
    }

    public void Start()
    {
        int total = mon + lib + soc;
        float monPercent = 0f;
        float libPercent = 0f;
        float socPercent = 0f;

        if (total > 0)
        {
            monPercent = (float)mon / total * 100f;
            libPercent = (float)lib / total * 100f;
            socPercent = (float)soc / total * 100f;
        }

        result.text = (
            $"Монархизм (правые) - {monPercent:F2}%\n" +
            $"Либерализм (центристы) - {libPercent:F2}%\n" +
            $"Социализм (левые) - {socPercent:F2}%"
        );
    }

    public void ToMenu()
    {
        SceneManager.LoadScene(GameData.menuScene);
    }

}
