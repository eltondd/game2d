using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game {
    public static Game current;

    public int stage;
    public int playerHP;
	
    public Game() {
        stage = 1;
        playerHP = 5;
        Time.timeScale = 1;
    }

    public void nextStage() {
        stage++;
    }
}
