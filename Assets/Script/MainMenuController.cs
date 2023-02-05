using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public Transform mainMenuBtn;

    int in_game;

    enum DIFFICULTY {
        EASY = 0,
        NORMAL = 1,
        HARD = 2
    }

    public void startGame() {
        //PanelManager.instance.loadCustomDifficulty();
        SceneManager.LoadScene("MayBeFinal_Andrew");
        //PanelManager.instance.loadPanel("\nPlease choose your difficulty:", delegate {startNewGame((int)DIFFICULTY.EASY);}, delegate {startNewGame((int)DIFFICULTY.NORMAL);}, "Easy", "Normal", addButtons);
    }

    public void resumeGame() {
        SceneManager.LoadScene("Game");
    }

    void startNewGame(int difficulty) {
        PlayerPrefs.SetInt("difficulty", difficulty);
        PlayerPrefs.SetInt("in_game", 0);
        PlayerPrefs.SetString("historicalMoves", "");

        SceneManager.LoadScene("Game");
    }

    void addButtons() {
        GameObject button_panel = PanelManager.instance.panel.Find("ButtonsPanel").gameObject;
        Transform button = Resources.Load<Transform>("Prefebs/Button");
        button.localScale = new Vector3(1, 1, 0);
        Transform difficulty_button = Instantiate(button, button_panel.transform);
        difficulty_button.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Hard";
        button_panel.GetComponent<HorizontalLayoutGroup>().padding.left = 50;
        button_panel.GetComponent<HorizontalLayoutGroup>().padding.right = 50;
        button_panel.GetComponent<HorizontalLayoutGroup>().spacing = 50;
        difficulty_button.GetComponent<Button>().onClick.AddListener(delegate {startNewGame((int)DIFFICULTY.HARD);});

        //add close button
        Transform close_button_rss = Resources.Load<Transform>("Prefebs/CloseButton");
        Transform close_button = Instantiate(close_button_rss, button_panel.transform.parent.Find("Header"));
        close_button.GetComponent<Button>().onClick.AddListener(delegate {PanelManager.instance.closePanel();});
    }

    void Start() {
        in_game = PlayerPrefs.GetInt("in_game");

        PanelManager.instance.setPanelSize(0.6f, 0.6f);
        PanelManager.instance.setPanelPosition(new Vector3(0,0,1));
    }

}
