using Ink.Parsed;
using Ink.Runtime;
using ModestTree;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dialogues : MonoBehaviour
{
    [Header("Файл сюжета")]
    private Ink.Runtime.Story story;
    public TextAsset inkJson;

    [Header("Переменные сюжета")]
    [SerializeField] private int mon = 0;
    [SerializeField] private int lib = 0;
    [SerializeField] private int soc = 0;

    [Header("Управление")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject choicePanel;
    private List<ButtonAction> choiceButtons = new();
    [SerializeField] private Button menuButton;
    [SerializeField] private Button infoButton;

    [Header("Текстовое отображение")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject titlePanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TextMeshProUGUI infoText;

    [Header("Визуальное отображение")]
    [SerializeField] private Image background;
    [SerializeField] private Image skinLeft;
    [SerializeField] private Image skinRight;
    [SerializeField] private Image study;

    public bool WaitChoice { get; private set; } // Определяет, нужен ли выбор сейчас

    private void Awake()
    {
        story = new Ink.Runtime.Story(inkJson.text);

        menuButton.onClick.AddListener(MenuButtonAction);
        infoButton.onClick.AddListener(InfoButtonAction);

        foreach (ButtonAction child in choicePanel.GetComponentsInChildren<ButtonAction>())
        {
            choiceButtons.Add(child);
        }
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            choiceButtons[i].GetComponent<ButtonAction>().Construct(i, inputReader);
        }

        skinLeft.color = Color.white;
        skinRight.color = Color.white;
        background.color = Color.white;
    }

    private void Start()
    {
        StartDialogue();
    }

    // ИНИЦИАЛИЗАЦИЯ СЮЖЕТА

    private void StartDialogue()
    {
        WaitChoice = false;
        if (PlayerPrefs.HasKey("InkSaveState")) LoadingStart();
        else FirstStart();
    }

    private void FirstStart()
    {
        study.gameObject.SetActive(true);
    }

    private void LoadingStart()
    {
        story.state.LoadJson(PlayerPrefs.GetString("InkSaveState"));
        ShowStory();
    }

    private void FinishDialogue()
    {
        WaitChoice = false;
        ResetProgress();
        SceneManager.LoadScene(GameData.endScene);
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetString("InkSaveState", story.state.ToJson());
        PlayerPrefs.Save();
    }

    private void ResetProgress()
    {
        PlayerPrefs.DeleteKey("InkSaveState");
        PlayerPrefs.Save();
    }
    private void UpdateGameData()
    {
        mon = (int)story.variablesState["mon"];
        lib = (int)story.variablesState["lib"];
        soc = (int)story.variablesState["soc"];
        GameData.mon = mon;
        GameData.lib = lib;
        GameData.soc = soc;
    }

    // ВНЕШНИЕ ВЫЗОВЫ

    public void MenuButtonAction()
    {
        SaveProgress();
        SceneManager.LoadScene(GameData.menuScene);
    }

    public void InfoButtonAction()
    {
        infoPanel.SetActive(!infoPanel.activeInHierarchy);
    }
    
    public void ChoiceButtonAction(int choiceIndex)
    {
        story.ChooseChoiceIndex(choiceIndex);
        WaitChoice = false;
        ContinueStory();
    }

    public void ContinueStory()
    {
        if (WaitChoice) return;
        study.gameObject.SetActive(false); 

        if (story.canContinue)
        {
            story.Continue();
            ShowStory();
        }
        else
        {
            FinishDialogue();
        }
    }

    // ОТОБРАЖЕНИЕ

    private void ShowStory()
    {
        UpdateGameData();
        ShowTitle();
        ShowText();
        ShowImage("background", background);
        ShowImage("skinL", skinLeft);
        ShowImage("skinR", skinRight);
        ShowChoiceButtons();
    }

    private void ShowText()
    {
        dialogueText.text = story.currentText;
        nameText.text = (string)story.variablesState["character"];

        infoPanel.GetComponentInChildren<TextMeshProUGUI>().text = ((string)story.variablesState["info"]).Replace('\\', '\n');
    }

    private void ShowImage(string ink_name, Image image)
    {
        string res = (string)story.variablesState[ink_name];
        if (res == "") image.gameObject.SetActive(false);
        else
        {
            Sprite sprite = Resources.Load<Sprite>(res);
            image.sprite = sprite;
            if (sprite is null) Debug.Log("Unknow " + ink_name + " = " + res);
            image.gameObject.SetActive(true);
        }
    }

    private void ShowChoiceButtons()
    {
        if (story.currentChoices.IsEmpty())
        {
            WaitChoice = false;
            choicePanel.SetActive(false);
        }
        else
        {
            WaitChoice = true;
            List<Ink.Runtime.Choice> currentChoices = story.currentChoices;
            for (int i = 0; i < choiceButtons.Count; i++)
            {
                ButtonAction choice = choiceButtons[i];
                if (i < currentChoices.Count)
                {
                    choice.gameObject.SetActive(true);
                    TextMeshProUGUI choiceText = choice.GetComponentInChildren<TextMeshProUGUI>();
                    choiceText.text = currentChoices[i].text;
                }
                else
                {
                    choice.gameObject.SetActive(false);
                }
            }
            choicePanel.SetActive(true);
        }   
    }

    private void ShowTitle()
    {
        if (story.currentText.StartsWith("TITLE"))
        {
            titlePanel.GetComponentInChildren<TextMeshProUGUI>().text = story.currentText.Substring(6).Replace('\\', '\n');
            titlePanel.SetActive(true);
        }
        else
        {
            titlePanel.SetActive(false);
        }
    }




}
