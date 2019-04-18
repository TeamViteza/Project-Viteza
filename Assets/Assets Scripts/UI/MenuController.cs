using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    Canvas mainCanvas;
    Transform buttonPositionParent, mainButtonsParent;
    Transform[] buttonPositions = new Transform[7];
    Button[] mainButtons = new Button[4];

    int firstButtonPositionIndex, mainButtonCounter;
    int downCounter = 0;

    void Awake()
    {
        mainCanvas = GameObject.Find("canvas_main").GetComponent<Canvas>(); // Get access to the main menu's canvas.
        buttonPositionParent = mainCanvas.transform.Find("button_positions_main").GetComponent<Transform>(); // Get access to the parent of the main menu's potential button positions.
        mainButtonsParent = mainCanvas.transform.Find("buttons_main").GetComponent<Transform>(); // Get access to the parent of the main menu's buttons.

        #region Gain access to each of the main menu's potential button positions.
        for (int i = 0; i < buttonPositions.Length; i++)
        {
            buttonPositions[i] = buttonPositionParent.transform.GetChild(i);
        }
        #endregion

        #region Now get a reference to each button on the main menu.       
        for (int i = 0; i < mainButtons.Length; i++)
        {
            mainButtons[i] = mainButtonsParent.transform.GetChild(i).GetComponent<Button>();
        }
        #endregion

        firstButtonPositionIndex = buttonPositions.Length - mainButtons.Length; // Should be 3.

        #region Set the starting position of each button.       
        int buttonCounter = 0;

        for (int i = firstButtonPositionIndex; i < buttonPositions.Length; i++)
        {
            mainButtons[buttonCounter].transform.position = buttonPositions[i].transform.position; // Set each of our buttons to their appropriate starting positions.
            buttonCounter++;
        }
        #endregion
    }

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.DownArrow)) NavigateDownMain();

        if (Input.GetKeyUp(KeyCode.UpArrow)) NavigateUpMain();
    }

    private void NavigateDownMain()
    {
        firstButtonPositionIndex = buttonPositions.Length - mainButtons.Length; // Should be 3.

        if (downCounter < firstButtonPositionIndex)
        {
            mainButtonCounter = 0;
            for (int i = 0; i < buttonPositions.Length; i++)
            {
                if (i >= firstButtonPositionIndex - downCounter && mainButtonCounter < mainButtons.Length)
                {
                    mainButtons[mainButtonCounter].transform.position = buttonPositions[i - 1].transform.position; // This'll soon be replaced with a lerp function.
                    mainButtonCounter++;
                }
            }
            downCounter++;
            Debug.Log("Down Counter: " + downCounter);
        }
    }

    private void NavigateUpMain()
    {
        firstButtonPositionIndex = buttonPositions.Length - mainButtons.Length; // Should be 3.

        if (downCounter > 0)
        {
            mainButtonCounter = mainButtons.Length - 1;
            for (int i = buttonPositions.Length - 1; i >= 0; i--)
            {
                if (i <= (buttonPositions.Length - 1) - downCounter && mainButtonCounter >= 0)
                {
                    Debug.Log("Button Index: " + mainButtonCounter + " position(i + 1): " + (i + 1));
                    mainButtons[mainButtonCounter].transform.position = buttonPositions[i + 1].transform.position; // This'll soon be replaced with a lerp function.
                    mainButtonCounter--;
                }
            }
            downCounter--;
        }
    }
}