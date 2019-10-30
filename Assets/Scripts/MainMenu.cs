using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public GameObject menu;
    public GameObject gameWinMenu;
    public GameObject Boss;
    public GameObject fireballScreen;

    private void Start() {
        if (SceneManager.GetActiveScene().name.Substring(0, 5) == "Stage") {
            menu.SetActive(false);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name.Substring(0, 5) == "Stage") {
            menu.SetActive(!menu.activeSelf);
            if (menu.activeSelf) {
                Time.timeScale = 0;
            } else {
                Time.timeScale = 1;
            }
        }
    }

    public void NewGame() {
        Game.current = new Game();
        Time.timeScale = 1;
        SceneManager.LoadScene("Stage" + Game.current.stage.ToString());
    }

    public void RestartLevel() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Stage" + Game.current.stage.ToString());
    }

    public void NextStage() {
        Game.current.nextStage();
        SceneManager.LoadScene("Stage" + Game.current.stage.ToString());
    }

    public void quitToMainMenu() {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    public void controlsMenu() {
        SceneManager.LoadScene("Controls");
        Time.timeScale = 1;
    }

    public void creditsMenu() {
        SceneManager.LoadScene("Credits");
        Time.timeScale = 1;
    }

    public void openScreen(GameObject screen) {
        Time.timeScale = 0;
        screen.SetActive(true);
    }

    public void closeScreen(GameObject screen) {
        Time.timeScale = 1;
        screen.SetActive(false);
    }

    public void quitGame() {
        Time.timeScale = 1;
        Application.Quit();
    }
}
