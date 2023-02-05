using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PanelManager : MonoBehaviour
{
    public static PanelManager instance = null;

    public Boolean hasPopUp = false;

    public Transform panel;

    float panel_width = 1f;

    float panel_height = 1f;

    // TODO: need to refactor!
    enum DIFFICULTY {
        EASY,
        NORMAL,
        HARD
    }

    Vector3 panel_position = Vector3.zero;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }

    public void setPanelSize(float width, float height) {
        panel_width = width;
        panel_height = height;
    }

    public void setPanelPosition(Vector3 position) {
        panel_position = position;
    }


    // TODO: need to refactor it!
    public void loadPanel(string message, Action firstBtnCallback, Action secondBtnCallback, string firstBtnText = "Confirm", string secondBtnText = "Cancel", Action panelCallback = null) {
        if (instance.hasPopUp) return;
        instance.hasPopUp = true;
        Transform prefebs_panel = Resources.Load<Transform>("Prefebs/PopUpPanel");
        prefebs_panel.localScale = new Vector3(panel_width, panel_height, 0);
        instance.panel = Instantiate(prefebs_panel, GameObject.FindGameObjectWithTag("MainCanvas").transform);
        if (panel_position != Vector3.zero) {
            instance.panel.position = panel_position;
        }
        
        GameObject firstBtn = instance.panel.Find("ButtonsPanel/Confirm").gameObject;
        firstBtn.GetComponent<Button>().onClick.AddListener(delegate {firstBtnCallback();});
        GameObject firstBtnTextMessage = instance.panel.Find("ButtonsPanel/Confirm/Text").gameObject;
        firstBtnTextMessage.GetComponent<TextMeshProUGUI>().text = firstBtnText;
        GameObject secondBtn = instance.panel.Find("ButtonsPanel/Cancel").gameObject;
        secondBtn.GetComponent<Button>().onClick.AddListener(delegate {secondBtnCallback(); });
        GameObject secondBtnTextMessage = instance.panel.Find("ButtonsPanel/Cancel/Text").gameObject;
        secondBtnTextMessage.GetComponent<TextMeshProUGUI>().text = secondBtnText;
        GameObject PopUpMessage = instance.panel.Find("Message").gameObject;
        PopUpMessage.GetComponent<Text>().text = message;

        if (panelCallback != null) {
            panelCallback();
        }
    }

    public void closePanel() {
        if (instance.panel != null)
            Destroy(instance.panel.gameObject);
        instance.hasPopUp = false;
    }

    public void loadHelp(Boolean isInGame = false) {
        if (instance.hasPopUp) return;
        instance.hasPopUp = true;
        Transform prefebs_panel = Resources.Load<Transform>("Prefebs/HowToPlayPanel");
        prefebs_panel.localScale = new Vector3(0.7f, 0.7f, 0);
        instance.panel = Instantiate(prefebs_panel, GameObject.FindGameObjectWithTag("MainCanvas").transform);
        instance.panel.position = new Vector3(0f,0f,0);
        if (isInGame) {
            instance.panel.localPosition = new Vector3(0f,0f,0);
            instance.panel.localScale = new Vector3(1.2f, 1.2f, 0);
        }

        Transform closeButton = instance.panel.Find("CloseButton").transform;
        closeButton.GetComponent<Button>().onClick.AddListener(delegate {instance.closePanel();});
    }

    public void loadWin(Boolean isInGame = false) {
        if (instance.hasPopUp) return;
        instance.hasPopUp = true;
        Transform prefebs_panel = Resources.Load<Transform>("Prefebs/WinPanel");
        instance.panel = Instantiate(prefebs_panel, GameObject.FindGameObjectWithTag("MainCanvas").transform);

        // Play clap music
        int sfx = PlayerPrefs.GetInt("settings.sfx");
        if (sfx == 1) {
            instance.panel.GetComponent<AudioSource>().Play();
        }
        
        instance.panel.position = new Vector3(0f,0f,0);
        instance.panel.localPosition = new Vector3(0f,0f,0);
        instance.panel.localScale = new Vector3(1.2f, 1.2f, 0);
        Transform homeButton = instance.panel.Find("ButtonsPanel/HomeButton").transform;
        homeButton.GetComponent<Button>().onClick.AddListener(delegate {instance.closePanel();SceneManager.LoadScene("MainMenu");});

        Transform nextButton = instance.panel.Find("ButtonsPanel/NextButton").transform;
        nextButton.GetComponent<Button>().onClick.AddListener(delegate {instance.closePanel();TimeTracker.instance.StopAndResetTimer();PlayerPrefs.SetInt("in_game", 0);PlayerPrefs.SetString("historicalMoves", "");SceneManager.LoadScene("Game");});

        // calculate the best time
        int difficulty_level = PlayerPrefs.GetInt("difficulty");
        string difficulty = ((DIFFICULTY)difficulty_level).ToString();
        string strVal = PlayerPrefs.GetString ( "BestTime"+difficulty );
        long ticks = long.MaxValue;
        if (strVal != "") {
            long.TryParse ( strVal , out ticks );
        }
        
        
        Transform difficulty_text = instance.panel.Find("difficulty_text").transform;
        difficulty_text.GetComponent<Text>().text="Difficulty: "+ difficulty;

        TimeSpan best_time = new TimeSpan ( ticks );
        Transform best_time_text = instance.panel.Find("best_time_text").transform;
        if (TimeSpan.Compare(best_time, TimeTracker.instance.timeCounter) > 0) {
            PlayerPrefs.SetString ( "BestTime"+difficulty, TimeTracker.instance.timeCounter.Ticks.ToString( ) );
            best_time_text.GetComponent<Text>().text="Best Time: "+string.Format ( "{0:D2}:{1:D2}:{2:D2}" , TimeTracker.instance.timeCounter.Hours , TimeTracker.instance.timeCounter.Minutes , TimeTracker.instance.timeCounter.Seconds );
        } else {
            best_time_text.GetComponent<Text>().text="Best Time: "+string.Format ( "{0:D2}:{1:D2}:{2:D2}" , best_time.Hours , best_time.Minutes , best_time.Seconds );
        }

        Transform time_text = instance.panel.Find("time_text").transform;
        time_text.GetComponent<Text>().text="Time: "+string.Format ( "{0:D2}:{1:D2}:{2:D2}" , TimeTracker.instance.timeCounter.Hours , TimeTracker.instance.timeCounter.Minutes , TimeTracker.instance.timeCounter.Seconds );
    }


    public void loadLose(Boolean isInGame = false) {
        if (instance.hasPopUp) return;
        instance.hasPopUp = true;
        Transform prefebs_panel = Resources.Load<Transform>("Prefebs/LosePanel");
        instance.panel = Instantiate(prefebs_panel, GameObject.FindGameObjectWithTag("MainCanvas").transform);
        instance.panel.position = new Vector3(0f,0f,0);
        instance.panel.localPosition = new Vector3(0f,0f,0);
        instance.panel.localScale = new Vector3(1.2f, 1.2f, 0);
        Transform homeButton = instance.panel.Find("ButtonsPanel/HomeButton").transform;
        homeButton.GetComponent<Button>().onClick.AddListener(delegate {instance.closePanel();SceneManager.LoadScene("MainMenu");});

        Transform nextButton = instance.panel.Find("ButtonsPanel/NextButton").transform;
        nextButton.GetComponent<Button>().onClick.AddListener(delegate {instance.closePanel();TimeTracker.instance.StopAndResetTimer();PlayerPrefs.SetInt("in_game", 0);PlayerPrefs.SetString("historicalMoves", "");SceneManager.LoadScene("Game");});

        // calculate the best time
        int difficulty_level = PlayerPrefs.GetInt("difficulty");
        string difficulty = ((DIFFICULTY)difficulty_level).ToString();
        string strVal = PlayerPrefs.GetString ( "BestTime"+difficulty );
        long ticks = long.MaxValue;
        if (strVal != "") {
            long.TryParse ( strVal , out ticks );
        }
        
        Transform difficulty_text = instance.panel.Find("difficulty_text").transform;
        difficulty_text.GetComponent<Text>().text="Difficulty: "+ difficulty;

        Transform time_text = instance.panel.Find("time_text").transform;
        time_text.GetComponent<Text>().text="Time: "+string.Format ( "{0:D2}:{1:D2}:{2:D2}" , TimeTracker.instance.timeCounter.Hours , TimeTracker.instance.timeCounter.Minutes , TimeTracker.instance.timeCounter.Seconds );
    }

    public void loadSettings(Boolean isInGame = false) {
        if (instance.hasPopUp) return;
        instance.hasPopUp = true;
        Transform prefebs_panel = Resources.Load<Transform>("Prefebs/SettingsPanel");
        prefebs_panel.localScale = new Vector3(0.7f, 0.7f, 0);
        instance.panel = Instantiate(prefebs_panel, GameObject.FindGameObjectWithTag("MainCanvas").transform);
        instance.panel.position = new Vector3(0f,0f,0);
        if (isInGame) {
            instance.panel.localPosition = new Vector3(0f,0f,0);
            instance.panel.localScale = new Vector3(1.2f, 1.2f, 0);
        }
        int music = PlayerPrefs.GetInt("settings.music");
        if (music == 0) {
            Transform text = instance.panel.Find("Button_BGM/Text").transform;
            text.GetComponent<Text>().text = "OFF";
        }
        int sfx = PlayerPrefs.GetInt("settings.sfx");
        if (sfx == 0) {
            Transform text = instance.panel.Find("Button_SFX/Text").transform;
            text.GetComponent<Text>().text = "OFF";
        }

        Transform closeButton = instance.panel.Find("CloseButton").transform;
        Transform closeButton2 = instance.panel.Find("ButtonsPanel/CloseButton").transform;
        closeButton.GetComponent<Button>().onClick.AddListener(delegate {instance.closePanel();});
        closeButton2.GetComponent<Button>().onClick.AddListener(delegate {instance.closePanel();});
    }

    public void loadCustomDifficulty(Boolean isInGame = false) {
        if (instance.hasPopUp) return;
        instance.hasPopUp = true;
        Transform prefebs_panel = Resources.Load<Transform>("Prefebs/DifficultySelectionPanel");
        prefebs_panel.localScale = new Vector3(0.7f, 0.7f, 0);
        instance.panel = Instantiate(prefebs_panel, GameObject.FindGameObjectWithTag("MainCanvas").transform);
        instance.panel.position = new Vector3(0f,0f,0);
        if (isInGame) {
            instance.panel.localPosition = new Vector3(0f,0f,0);
            instance.panel.localScale = new Vector3(1.2f, 1.2f, 0);
        }
        
        Transform closeButton = instance.panel.Find("CloseButton").transform;
        closeButton.GetComponent<Button>().onClick.AddListener(delegate {instance.closePanel();});

        Transform easyButton = instance.panel.Find("EasyButton").transform;
        easyButton.GetComponent<Button>().onClick.AddListener(delegate {startGame((int)DIFFICULTY.EASY);});

        Transform normalButton = instance.panel.Find("NormalButton").transform;
        normalButton.GetComponent<Button>().onClick.AddListener(delegate {startGame((int)DIFFICULTY.NORMAL);});

        Transform hardButton = instance.panel.Find("HardButton").transform;
        hardButton.GetComponent<Button>().onClick.AddListener(delegate {startGame((int)DIFFICULTY.HARD);});
    }

    void startGame(int difficulty) {
        PlayerPrefs.SetInt("difficulty", difficulty);
        PlayerPrefs.SetInt("in_game", 0);
        PlayerPrefs.SetString("historicalMoves", "");

        SceneManager.LoadScene("Game");
    }
}
