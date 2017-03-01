﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuLogic : MenuLogic {

    public GameObject Options;
    public GameObject Settings;
    public GameObject KeyBinds;
    public GameObject Score;
    private GameObject WinScreen;
    private GameObject LoseScreen;
    private bool _win;
    private bool gameEnd;

    public bool win
    {
        get { return _win; }
        set { _win = value; }
    }
    
    new void toMainMenu()
    {
        Settings.SetActive(false);
        Options.SetActive(!Options.activeSelf);
        Score.SetActive(!Score.activeSelf);
        if (!Options.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    new void toSettings()
    {
        Options.SetActive(false);
        Settings.SetActive(true);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)&&!Settings.activeSelf&&!gameEnd)
        {
            toMainMenu();
        }
    }
    void toEnd()
    {
        if (win)
        {
            WinScreen.SetActive(true);
        }else {
            LoseScreen.SetActive(true);
        }

            Options.SetActive(false);
            Settings.SetActive(false);
           
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        
    }

    void Start()
    {
        gameEnd = false;
        _win = false;
    }
}
