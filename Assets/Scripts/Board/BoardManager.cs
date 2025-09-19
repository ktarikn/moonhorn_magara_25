using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    public GameObject board;
    private bool isActive;
    public Controller controller;

    public Action currentAction = null; 
    public string ButtonName = null; 

    private void Awake()
    {
        isActive = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            InteractWithBoard();
        }
    }

    void InteractWithBoard()
    {
        board.SetActive(!isActive);
        isActive = !isActive;
    }

    public void ChangeButtonAction()
    {
        switch (ButtonName)
        {
            case "A": 
                controller.keyAAction = currentAction;
                break;
            case "W":
                controller.keyWAction = currentAction;
                break;
            case "S":
                controller.keySAction = currentAction;
                break;
            case "D":
                controller.keyDAction = currentAction;
                break;
        }
        ButtonName = "";
        currentAction = null;
    }

    public void choseCurrentAction(string Action)
    {
        switch(Action)
        {
            case "RollRight":
                currentAction = controller.RollRight;
                break;
            case "RollLeft":
                currentAction = controller.RollLeft;
                break;
        }
        if (ButtonName != "")
        {
            Debug.Log("test");
            ChangeButtonAction();
        }
    }

    public void choseButtonName(string _ButtonName)
    {
        ButtonName = _ButtonName;
        if (currentAction != null)
        {
            ChangeButtonAction();
        }
    }
}
