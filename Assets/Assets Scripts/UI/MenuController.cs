using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    #region MAIN MENU variables.
    Canvas mainCanvas;
    Transform buttonPositionParent, mainButtonsParent;

    Transform[] buttonPositions = new Transform[7]; // Seven potential positions.
    Button[] mainButtons = new Button[4];

    int firstButtonPositionIndex, buttonIndex;
    int downwardCount = 0; // This will keep track of how far the menu options have been shifted upward. (Due to the user pressing DOWN at the main menu.)
    #endregion

    private enum MenuType // This system's subject to change, right now I'd like to experiment with keeping all menu-related functions within this script.
    {
        MAIN, NEWGAME, LOADGAME, OPTIONS, PAUSE
    }
    private MenuType activeMenu = MenuType.MAIN; // May have to make this public in future, we'll see.   

    void Awake()
    {
        #region MAIN MENU Initialisation Operations.
        #region Get access to required game object parents.
        mainCanvas = GameObject.Find("canvas_main").GetComponent<Canvas>(); // Get access to the main menu's canvas.
        buttonPositionParent = mainCanvas.transform.Find("button_positions_main").GetComponent<Transform>(); // Get access to the parent of the main menu's potential button positions.
        mainButtonsParent = mainCanvas.transform.Find("buttons_main").GetComponent<Transform>(); // Get access to the parent of the main menu's buttons.
        #endregion
        #region Gain access to each of the main menu's potential button positions.
        for (int i = 0; i < buttonPositions.Length; i++)
        {   // We'll have 7 potential button positions on our main menu. (Index ranges from 0 to 6.)
            buttonPositions[i] = buttonPositionParent.transform.GetChild(i);
        }
        #endregion
        #region Now get a reference to each button on the main menu.       
        for (int i = 0; i < mainButtons.Length; i++)
        {   // We'll have four Main menu buttons, corresponding to their index as follows: 0 - New Game, 1 - Load Game, 2 - Options, 3 - Quit
            mainButtons[i] = mainButtonsParent.transform.GetChild(i).GetComponent<Button>();
        }
        #endregion

        // Get the difference between the number of potential button positions (7) and the number of buttons (4)...
        // ...In order to determine the position index of the first button (New Game).
        firstButtonPositionIndex = buttonPositions.Length - mainButtons.Length; // Should be 3, since we've got 7 potential positions and 4 buttons.

        #region Set the starting position of each button.       
        buttonIndex = 0; // Start at 0 so we can set the position of button 0 (New Game) first.

        for (int i = firstButtonPositionIndex; i < buttonPositions.Length; i++) // Start our iteration at the position we determined the first button would appear at. (Position 3, the first button position index).
        {
            mainButtons[buttonIndex].transform.position = buttonPositions[i].transform.position; // Set each of our buttons to their appropriate starting positions.
            buttonIndex++; // Increase the index so we can set the position of buttons 1, 2 and 3 in the next three loops.
        }
        #endregion
        #endregion
    }

    void Start()
    {
    }

    void Update()
    {
        switch (activeMenu)
        {
            case MenuType.MAIN:
                if (Input.GetKeyUp(KeyCode.DownArrow)) NavigateDownMain();
                if (Input.GetKeyUp(KeyCode.UpArrow)) NavigateUpMain();
                break;
        }
    }

    #region MAIN MENU METHODS & COROUTINES.
    private void NavigateDownMain()
    {
        if (downwardCount < firstButtonPositionIndex) // The menu buttons can only shift as far as the difference in number between button positions and the buttons themselves.
        {   // Since the difference is 3, we have three extra positions to move to, and no more. We use "downwardCount" to determine how far we have moved.
            buttonIndex = 0;
            for (int i = 0; i < buttonPositions.Length; i++) // Iterate through each potential position.
            {
                if (i >= firstButtonPositionIndex - downwardCount && buttonIndex < mainButtons.Length) // Once we've reached the first position that holds a button... 
                {   // ...and assuming we haven't yet set a new position for each button...                                   
                    StartCoroutine(ShiftButtonPosition(buttonIndex, buttonPositions[i - 1].transform.position, 0.2f)); // Shift this button to the position directly above its current position.
                    buttonIndex++; // Increase the button index so that we can shift the positions of buttons 1, 2 and 3 in the next three iterations.
                }
            }
            downwardCount++; // Increase the count so that we know how far "down" the player is in the main menu.
        }
    }
    private void NavigateUpMain()
    {
        if (downwardCount > 0) // If this count is at 0 it means the user is at the "top" of the menu, and therefore can move no further upwards.
        {
            buttonIndex = mainButtons.Length - 1; // Start our button index at 3, "Quit Game", the button at the bottom of the list.
            for (int i = buttonPositions.Length - 1; i >= 0; i--) // Iterate through each position, starting from the bottom (position 6) and moving upwards.
            {
                // What follows is the same as what is occuring in the "NavigateDownMain" method, the only difference is that we are moving through the positions in reverse...
                // ...Shifting each button to the position directly below it, instead of above.
                if (i <= (buttonPositions.Length - 1) - downwardCount && buttonIndex >= 0)
                {
                    StartCoroutine(ShiftButtonPosition(buttonIndex, buttonPositions[i + 1].transform.position, 0.2f));
                    buttonIndex--;
                }
            }
            downwardCount--; // Decrease the count so that we know how far "up" the player is in the main menu.
        }
    }

    private IEnumerator ShiftButtonPosition(int buttonIndex, Vector3 newPosition, float transitionDuration)
    {   // https://answers.unity.com/questions/63060/vector3lerp-works-outside-of-update.html Coroutine derived from top answer here.
        float startTime = Time.time; // Get the time this coroutine started.

        Vector3 currentPosition = mainButtons[buttonIndex].transform.position; // Determine which button must be moved according to the index parameter.

        while (Time.time < startTime + transitionDuration) // While the transition duration hasn't passed...
        {
            // ...Move the menu button to its new position, lerping is used to achieve a "smoother" effect.
            mainButtons[buttonIndex].transform.position = Vector3.Lerp(currentPosition, newPosition, (Time.time - startTime) / transitionDuration);
            yield return null;
        }

        mainButtons[buttonIndex].transform.position = newPosition; // Ensure the button is at the exact position it should be by the end.
    }
    #endregion
}