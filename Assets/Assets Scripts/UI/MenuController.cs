using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    #region MAIN MENU variables.
    public float ButtonMotionCooldown = 0.1f;
    public float highlightPositionMoveSpeed = 0.2f;
    public float sliderAdjustmentPrecision = 0.05f;

    Canvas mainCanvas;
    GameObject mainPanel, filePanel, optionsPanel, quitPanel;
    List<GameObject> mainMenuPanels = new List<GameObject>();
    Transform buttonPositionParent, mainButtonsParent, highlightedPosition;

    Transform[] buttonPositions = new Transform[7]; // Seven potential positions.
    Button[] mainButtons = new Button[4];
    Button[] quitButtons = new Button[2];
    GameObject[] optionsSettings = new GameObject[5];
    GameObject highlightedObject;

    int firstButtonPositionIndex, buttonIndex, downwardCountMain, downwardCountOptions;
    bool buttonsInMotion;
    #endregion

    private enum MenuType // This system's subject to change, right now I'd like to experiment with keeping all menu-related functions within this script.
    {
        MAIN, FILE, OPTIONS, QUIT, PAUSE
    }
    private MenuType activeMenu;

    void Awake()
    {
        MainPanelInitialisation();
        OptionsPanelInitialisation();
        QuitPanelInitialisation();

        SetAsActiveMenu("Main");
    }

    void Start()
    {
        //Debug.Log(optionsSettings[4].name);
    }

    void Update()
    {
        switch (activeMenu)
        {
            case MenuType.MAIN:
                if (Input.GetKeyUp(KeyCode.DownArrow)) NavigateDownMain();
                if (Input.GetKeyUp(KeyCode.UpArrow)) NavigateUpMain();
                CheckButtonSelection();
                break;

            case MenuType.OPTIONS:
                NavigateOptions();
                CheckButtonSelection();
                break;

            case MenuType.QUIT:
                NavigateQuit();
                CheckButtonSelection();
                break;
        }
    }

    #region COMMON MENU METHODS & COROUTINES.
    private void CheckButtonSelection()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            if(highlightedObject.GetComponent<Button>() != null) highlightedObject.GetComponent<Button>().onClick.Invoke();
            else if (highlightedObject.GetComponent<Toggle>() != null) highlightedObject.GetComponent<Toggle>().isOn = !highlightedObject.GetComponent<Toggle>().isOn;
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (highlightedObject.GetComponent<Slider>() != null) highlightedObject.GetComponent<Slider>().value += sliderAdjustmentPrecision;
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (highlightedObject.GetComponent<Slider>() != null) highlightedObject.GetComponent<Slider>().value -= sliderAdjustmentPrecision;
        }
    }

    public void SetAsActiveMenu(string menuName)
    {
        bool menuValid = true;

        if (menuName.ToUpper() == "MAIN") activeMenu = MenuType.MAIN;
        else if (menuName.ToUpper() == "FILE") activeMenu = MenuType.FILE;
        else if (menuName.ToUpper() == "OPTIONS") activeMenu = MenuType.OPTIONS;
        else if (menuName.ToUpper() == "QUIT") activeMenu = MenuType.QUIT;
        else
        {
            Debug.Log(string.Format("The '{0}' menu does not exist. Please check the on-click event for this button.", menuName));
            menuValid = false;
        }
        if (menuValid)
        {
            StartCoroutine(TransferHighlightPosition(activeMenu, 0.2f));
            UpdateMenuPanels();
        }
    }

    private void UpdateMenuPanels()
    {
        foreach (GameObject menuPanel in mainMenuPanels)
        {
            if (menuPanel.name.ToUpper().Contains(activeMenu.ToString())) menuPanel.SetActive(true);
            else menuPanel.SetActive(false);
        }
    }

    private IEnumerator TransferHighlightPosition(MenuType activatedMenu, float transitionDuration)
    {   // https://answers.unity.com/questions/63060/vector3lerp-works-outside-of-update.html Coroutine derived from top answer here.

        Vector3 newHighlightPosition;

        switch (activatedMenu)
        {
            default: // If we are returning to the main menu.                
                newHighlightPosition = buttonPositions[firstButtonPositionIndex].transform.position; // Move the highlight back to its main menu position. 

                for (int i = 0; i <= mainButtons.Length; i++)
                {
                    if (mainButtons[i].transform.position == newHighlightPosition)
                    {
                        highlightedObject = mainButtons[i].gameObject;
                        break;
                    }
                }
                break;

            case MenuType.OPTIONS: // If we're moving to the options menu.                
                newHighlightPosition = optionsSettings[0].transform.position;
                highlightedObject = optionsSettings[0].gameObject;
                break;

            case MenuType.QUIT: // If we're moving to the quit prompt.                
                newHighlightPosition = quitButtons[0].transform.position;
                highlightedObject = quitButtons[0].gameObject;
                break;
        }

        float startTime = Time.time; // Get the time this coroutine started.

        while (Time.time < startTime + transitionDuration) // While the transition duration hasn't passed...
        {
            // ...Move the menu button to its new position, lerping is used to achieve a "smoother" effect.
            highlightedPosition.position = Vector3.Lerp(highlightedPosition.transform.position, newHighlightPosition, (Time.time - startTime) / transitionDuration);
            yield return null;
        }
        highlightedPosition.position = newHighlightPosition; // Ensure the button is at the exact position it should be by the end.
    }

    private IEnumerator ShiftHighlightPosition(Vector3 highlightDestinationPosition, float transitionDuration)
    {
        float startTime = Time.time; // Get the time this coroutine started.

        while (Time.time < startTime + transitionDuration) // While the transition duration hasn't passed...
        {
            // ...Move the menu button to its new position, lerping is used to achieve a "smoother" effect.
            highlightedPosition.position = Vector3.Lerp(highlightedPosition.transform.position, highlightDestinationPosition, (Time.time - startTime) / transitionDuration);
            yield return null;
        }
        highlightedPosition.position = highlightDestinationPosition; // Ensure the button is at the exact position it should be by the end.
    }
    #endregion

    #region MAIN MENU METHODS & COROUTINES.
    private void NavigateDownMain()
    {
        if (downwardCountMain < firstButtonPositionIndex && !buttonsInMotion) // The menu buttons can only shift as far as the difference in number between button positions and the buttons themselves.            
        {
            buttonsInMotion = true;
            // Since the difference is 3, we have three extra positions to move to, and no more. We use "downwardCount" to determine how far we have moved.
            buttonIndex = 0;
            for (int i = 0; i < buttonPositions.Length; i++) // Iterate through each potential position.
            {
                if (i >= firstButtonPositionIndex - downwardCountMain && buttonIndex < mainButtons.Length) // Once we've reached the first position that holds a button... 
                {   // ...and assuming we haven't yet set a new position for each button...                                   
                    StartCoroutine(ShiftButtonPosition(activeMenu, buttonIndex, i - 1, highlightPositionMoveSpeed)); // ...Shift this button to the position directly above its current position.
                    buttonIndex++; // Increase the button index so that we can shift the positions of buttons 1, 2 and 3 in the next three iterations.
                }
            }
            StartCoroutine("FalsifyButtonMotionBool");
            downwardCountMain++; // Increase the count so that we know how far "down" the player is in the main menu.
        }
    }
    private void NavigateUpMain()
    {
        if (downwardCountMain > 0 && !buttonsInMotion) // If this count is at 0 it means the user is at the "top" of the menu, and therefore can move no further upwards.
        {
            buttonsInMotion = true;
            buttonIndex = mainButtons.Length - 1; // Start our button index at 3, "Quit Game", the button at the bottom of the list.
            for (int i = buttonPositions.Length - 1; i >= 0; i--) // Iterate through each position, starting from the bottom (position 6) and moving upwards.
            {
                // What follows is the same as what is occuring in the "NavigateDownMain" method, the only difference is that we are moving through the positions in reverse...
                // ...Shifting each button to the position directly below it, instead of above.
                if (i <= (buttonPositions.Length - 1) - downwardCountMain && buttonIndex >= 0)
                {
                    StartCoroutine(ShiftButtonPosition(activeMenu, buttonIndex, i + 1, highlightPositionMoveSpeed));
                    buttonIndex--;
                }
            }
            StartCoroutine("FalsifyButtonMotionBool");
            downwardCountMain--; // Decrease the count so that we know how far "up" the player is in the main menu.
        }
    }

    private IEnumerator ShiftButtonPosition(MenuType currentMenu, int buttonIndex, int positionIndex, float transitionDuration) // May look into making this more of a universal coroutine. (Not just for main menu.)             
    {   // https://answers.unity.com/questions/63060/vector3lerp-works-outside-of-update.html Coroutine derived from top answer here.

        Button buttonToMove;
        Vector3 currentPosition, newPosition;

        switch (currentMenu)
        {
            default: // If we are currently in the main menu.
                buttonToMove = mainButtons[buttonIndex];
                currentPosition = buttonToMove.transform.position; // Determine which button must be moved according to the index parameter.
                newPosition = buttonPositions[positionIndex].transform.position; // Determine which position the button must be moved to.
                break;
        }

        float startTime = Time.time; // Get the time this coroutine started.

        while (Time.time < startTime + transitionDuration) // While the transition duration hasn't passed...
        {
            // ...Move the menu button to its new position, lerping is used to achieve a "smoother" effect.
            buttonToMove.transform.position = Vector3.Lerp(currentPosition, newPosition, (Time.time - startTime) / transitionDuration);
            yield return null;
        }

        if (positionIndex == firstButtonPositionIndex) highlightedObject = buttonToMove.gameObject; // Confirmed to work.

        buttonToMove.transform.position = newPosition; // Ensure the button is at the exact position it should be by the end.
    }

    private IEnumerator FalsifyButtonMotionBool()
    {
        yield return new WaitForSeconds(ButtonMotionCooldown);
        buttonsInMotion = false;
    }
    #endregion   

    #region OPTIONS MENU METHODS & COROUTINES.
    private void NavigateOptions()
    {
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            downwardCountOptions++;

            if(downwardCountOptions >= optionsSettings.Length) downwardCountOptions = 0;

            highlightedObject = optionsSettings[downwardCountOptions];
            StartCoroutine(ShiftHighlightPosition(highlightedObject.transform.position, highlightPositionMoveSpeed));
        }

        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            downwardCountOptions--;

            if (downwardCountOptions < 0) downwardCountOptions = optionsSettings.Length - 1;

            highlightedObject = optionsSettings[downwardCountOptions];
            StartCoroutine(ShiftHighlightPosition(highlightedObject.transform.position, highlightPositionMoveSpeed));
        }       
    }   
    #endregion

    #region QUIT MENU METHODS & COROUTINES.
    private void NavigateQuit()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            if (highlightedObject == quitButtons[0]) highlightedObject = quitButtons[1].gameObject;
            else highlightedObject = quitButtons[0].gameObject;

            StartCoroutine(ShiftHighlightPosition(highlightedObject.transform.position, highlightPositionMoveSpeed));
        }

        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (highlightedObject == quitButtons[1]) highlightedObject = quitButtons[0].gameObject;
            else highlightedObject = quitButtons[1].gameObject;

            StartCoroutine(ShiftHighlightPosition(highlightedObject.transform.position, highlightPositionMoveSpeed));
        }
    }

    public void QuitGame()
    {
        Debug.Log("Game Quit.");
        Application.Quit();
    }
    #endregion

    #region INITIALISATION METHODS
    private void MainPanelInitialisation() 
    {       
        #region Get access to required game object parents.
        mainCanvas = GameObject.Find("canvas_main").GetComponent<Canvas>(); // Get access to the main menu's canvas.
        highlightedPosition = mainCanvas.transform.Find("position_highlight"); // Find our menu option highlighter.

        #region Get access to each of the main menu's panels.
        mainPanel = mainCanvas.transform.Find("pnl0_main").gameObject;
        filePanel = mainCanvas.transform.Find("pnl1_file").gameObject;
        optionsPanel = mainCanvas.transform.Find("pnl2_options").gameObject;
        quitPanel = mainCanvas.transform.Find("pnl3_quit").gameObject;

        mainMenuPanels.Add(mainPanel);
        mainMenuPanels.Add(filePanel);
        mainMenuPanels.Add(optionsPanel);
        mainMenuPanels.Add(quitPanel);
        #endregion

        buttonPositionParent = mainPanel.transform.Find("0_button_positions_main"); // Get access to main panel's potential button positions.
        mainButtonsParent = mainPanel.transform.Find("1_buttons_main"); // Get access to the main panel's buttons.
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
    }

    private void OptionsPanelInitialisation() 
    {   // Get access to each setting in the options menu.
        optionsSettings[0] = optionsPanel.transform.Find("1_settings_options/0_stg_tgl_fullscreen").gameObject;
        optionsSettings[1] = optionsPanel.transform.Find("1_settings_options/1_stg_sdr_volume_master").gameObject;
        optionsSettings[2] = optionsPanel.transform.Find("1_settings_options/2_stg_sdr_volume_bgm").gameObject;
        optionsSettings[3] = optionsPanel.transform.Find("1_settings_options/3_stg_sdr_volume_sfx").gameObject;
        optionsSettings[4] = optionsPanel.transform.Find("1_settings_options/4_btn_return").gameObject;      
    }

    private void QuitPanelInitialisation()
    {   // Get access to options "yes" and "no" in the quit prompt.       
        quitButtons[0] = quitPanel.transform.Find("1_buttons_quit/btn_0_no").GetComponent<Button>();
        quitButtons[1] = quitPanel.transform.Find("1_buttons_quit/btn_1_yes").GetComponent<Button>();        
    }
    #endregion
}