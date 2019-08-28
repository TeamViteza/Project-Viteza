using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;
using FMODUnity;
public class MenuController : MonoBehaviour
{
    #region MAIN MENU variables.
    public float uiElementMotionCooldown = 0.1f;
    public float highlightPositionMoveSpeed = 0.2f;
    public float panelPositionMoveSpeed = 0.2f;
    public float sliderAdjustmentPrecision = 0.05f;

    Canvas mainCanvas;
    Resolution[] resolutions;
    GameObject mainPanel, filePanel, infoPanel, optionsPanel, quitPanel;
    Vector3[] panelPositions = new Vector3[10]; // This array will contain both active and inactive panel positions. (0-4 for active positions, 5-9 for inactive positions)
    List<GameObject> mainMenuPanels = new List<GameObject>();
    Transform buttonPositionParent, mainButtonsParent, saveFilePositionParent, saveFileParent, highlightedPosition;

    Transform[] buttonPositions = new Transform[7]; // Seven potential positions.
    Button[] mainButtons = new Button[4];
    Button btnReturnFile, btnReturnInfo;
    Button[] quitButtons = new Button[2];
    GameObject[] saveFilePositions = new GameObject[17];
    GameObject[] saveFiles = new GameObject[9];
    GameObject[] optionsSettings = new GameObject[7];
    GameObject highlightedObject, highlightedSaveFile, fileCursor;
    SaveFile fileToLoad;
    Image highlightImage;
    Dropdown qualityDropdown, resolutionDropdown;

    int panelPositionDifference, firstButtonPositionIndex, firstFilePositionIndex, buttonIndex, fileIndex, downwardCountMain, rightCountFile, downwardCountOptions;
    bool uiElementsInMotion, highlightPositionTransferred, axisInUse;
    #endregion

    private enum MenuType // This system's subject to change, right now I'd like to experiment with keeping all menu-related functions within this script.
    {
        MAIN, FILE, INFO, OPTIONS, QUIT, PAUSE
    }
    private MenuType activeMenu;

    //EventInstance playNavSound;
    //EventInstance playSelectSound;
    //EventInstance playNegativeSound;

    void Awake()
    {      
        resolutions = Screen.resolutions;
        //playNavSound = RuntimeManager.CreateInstance("event:/Master/SFX/UISFX/Nav");
        //playSelectSound = RuntimeManager.CreateInstance("event:/Master/SFX/UISFX/Pos");
        //playNegativeSound = RuntimeManager.CreateInstance("event:/Master/SFX/UISFX/Neg");

        MainPanelInitialisation();
        FilePanelInitialisation();
        InfoPanelInitialisation();
        OptionsPanelInitialisation();
        QuitPanelInitialisation();       

        SetAsActiveMenu("Main");       
    }

    void Start()
    {        
    }

    void Update()
    {
        //Debug.Log("UI Elements in motion: " + uiElementsInMotion);
        if (!highlightPositionTransferred) StartCoroutine(TransferHighlightPosition(activeMenu, highlightPositionMoveSpeed)); // Move the highlighter if the active menu has changed.

        CheckButtonSelection();
        CheckDirectionalAxisUse(); // I'll keep an eye on where this is placed in the Update().          

        switch (activeMenu)
        {
            case MenuType.MAIN:
                NavigateMain();
                break;

            case MenuType.FILE:
                NavigateFile();
                break;

            case MenuType.OPTIONS:
                NavigateOptions(GetAxisAsButtonDown("D-PadV"));
                HandleSliderAdjustment();
                break;

            case MenuType.QUIT:
                NavigateQuit();
                break;
        }
    }

    #region COMMON MENU METHODS & COROUTINES.
    private void CheckButtonSelection()
    {
        if (Input.GetButtonUp("BtnA") && !uiElementsInMotion)
        {
            if (highlightedObject.GetComponent<Button>() != null) highlightedObject.GetComponent<Button>().onClick.Invoke();
            else if (highlightedObject.GetComponent<Dropdown>() != null) highlightedObject.GetComponent<Dropdown>().Show();
            else if (highlightedObject.GetComponent<Toggle>() != null) highlightedObject.GetComponent<Toggle>().isOn = !highlightedObject.GetComponent<Toggle>().isOn;

            if (highlightedObject.name.Contains("return"))
            {
                //playNegativeSound.start();
            }
            else
            {
                //playSelectSound.start();
            }
        }
    }
    public void SetAsActiveMenu(string menuName)
    {
        bool menuValid = true;

        if (menuName.ToUpper() == "MAIN") activeMenu = MenuType.MAIN;
        else if (menuName.ToUpper() == "FILE") activeMenu = MenuType.FILE;
        else if (menuName.ToUpper() == "INFO") activeMenu = MenuType.INFO;
        else if (menuName.ToUpper() == "OPTIONS") activeMenu = MenuType.OPTIONS;
        else if (menuName.ToUpper() == "QUIT") activeMenu = MenuType.QUIT;
        else
        {
            Debug.Log(string.Format("The '{0}' menu does not exist. Please check the on-click event for this button.", menuName));
            menuValid = false;
        }
        if (menuValid)
        {
            highlightedPosition.gameObject.SetActive(false);
            UpdateMenuPanels();
        }
    }
    private void UpdateMenuPanels()
    {
        int panelIndex = 0;

        foreach (GameObject menuPanel in mainMenuPanels)
        {
            if (menuPanel.name.ToUpper().Contains(activeMenu.ToString()))
            {
                menuPanel.SetActive(true);
                StartCoroutine(ShiftUiElementPosition(menuPanel.transform, panelPositions[panelIndex], panelPositionMoveSpeed, true));
            }
            else
            {
                StartCoroutine(ShiftUiElementPosition(menuPanel.transform, panelPositions[panelIndex + panelPositionDifference], panelPositionMoveSpeed, true));
                menuPanel.SetActive(false);
            }
            panelIndex++;
        }
    }
    private void CheckDirectionalAxisUse()
    {
        if (Input.GetAxisRaw("D-PadV") == 0 && Input.GetAxisRaw("D-PadH") == 0) axisInUse = false;
    }
    private void NavigateTwoButtonMenu(GameObject button1, GameObject button2) // Applicable to the file and quit menu.
    {
        //playNavSound.start();
        if (highlightedObject.name == button1.name) highlightedObject = button2;
        else highlightedObject = button1;

        StartCoroutine(ShiftUiElementPosition(highlightedPosition, highlightedObject.transform.position, highlightPositionMoveSpeed, false));
    }
    private string GetAxisAsButtonDown(string axisNameIn)
    {
        float axisValue = Input.GetAxisRaw(axisNameIn);
        if (axisValue != 0 && !axisInUse)
        {
            axisInUse = true;
            return axisValue.ToString();
        }
        else return "0";
    }

    private IEnumerator TransferHighlightPosition(MenuType activatedMenu, float transitionDuration)
    {   // https://answers.unity.com/questions/63060/vector3lerp-works-outside-of-update.html Coroutine derived from top answer here.       
        highlightPositionTransferred = true;
        uiElementsInMotion = true;
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

            case MenuType.FILE: // If we're moving to the file menu.                
                newHighlightPosition = btnReturnFile.transform.position;
                highlightedObject = btnReturnFile.gameObject;
                break;

            case MenuType.INFO: // If we're moving to the Info menu.                
                newHighlightPosition = btnReturnInfo.transform.position;
                highlightedObject = btnReturnInfo.gameObject;
                break;

            case MenuType.OPTIONS: // If we're moving to the options menu.                
                newHighlightPosition = optionsSettings[0].transform.position;
                highlightedObject = optionsSettings[0].gameObject;
                downwardCountOptions = 0;
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
        uiElementsInMotion = false;
        highlightedPosition.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        if (!highlightImage.enabled) highlightImage.enabled = true;
    } // Gotta look into refactoring these coroutines.
    private IEnumerator ShiftUiElementPosition(Transform uiElement, Vector3 uiElementDestinationPosition, float transitionDuration, bool elementIsPanel)
    {
        uiElementsInMotion = true;
        float startTime = Time.time; // Get the time this coroutine started.

        while (Time.time < startTime + transitionDuration) // While the transition duration hasn't passed...
        {
            // ...Move the menu button to its new position, lerping is used to achieve a "smoother" effect.
            uiElement.position = Vector3.Lerp(uiElement.position, uiElementDestinationPosition, (Time.time - startTime) / transitionDuration);
            yield return null;
        }
        uiElement.position = uiElementDestinationPosition; // Ensure the button is at the exact position it should be by the end.
        if (elementIsPanel) highlightPositionTransferred = false;
        uiElementsInMotion = false;
    }
    private IEnumerator ShiftButtonPosition(MenuType currentMenu, int buttonIndex, int positionIndex, float transitionDuration)
    {   // https://answers.unity.com/questions/63060/vector3lerp-works-outside-of-update.html Coroutine derived from top answer here.
        uiElementsInMotion = true; // Added for testing purposes.
        GameObject objectToMove;
        Vector3 currentPosition, newPosition;

        switch (currentMenu)
        {
            default: // If we are currently in the main menu.
                objectToMove = mainButtons[buttonIndex].gameObject;
                currentPosition = objectToMove.transform.position; // Determine which button must be moved according to the index parameter.
                newPosition = buttonPositions[positionIndex].transform.position; // Determine which position the button must be moved to.
                break;

            case MenuType.FILE: // If we are currently in the file menu.
                objectToMove = saveFiles[fileIndex];
                currentPosition = objectToMove.transform.position; // Determine which object must be moved according to the index parameter.
                newPosition = saveFilePositions[positionIndex].transform.position; // Determine which position the object must be moved to.
                break;
        }

        float startTime = Time.time; // Get the time this coroutine started.

        while (Time.time < startTime + transitionDuration) // While the transition duration hasn't passed...
        {
            // ...Move the menu button to its new position, lerping is used to achieve a "smoother" effect.
            objectToMove.transform.position = Vector3.Lerp(currentPosition, newPosition, (Time.time - startTime) / transitionDuration);
            yield return null;
        }

        if (currentMenu == MenuType.MAIN && positionIndex == firstButtonPositionIndex) highlightedObject = objectToMove.gameObject; // Confirmed to work.        

        objectToMove.transform.position = newPosition; // Ensure the object is at the exact position it should be by the end.
        uiElementsInMotion = false;
    }

    #endregion

    #region MAIN MENU METHODS & COROUTINES.
    private void NavigateMain()
    {
        switch (GetAxisAsButtonDown("D-PadV"))
        {
            case "-1":
                //playNavSound.start();
                NavigateDownMain();
                break;
            case "1":
                //playNavSound.start();
                NavigateUpMain();
                break;
        }
    }
    private void NavigateDownMain()
    {
        if (downwardCountMain < firstButtonPositionIndex) // The menu buttons can only shift as far as the difference in number between button positions and the buttons themselves.            
        {
            // Since the difference is 3, we have three extra positions to move to, and no more. We use "downwardCountMain" to determine how far we have moved.
            buttonIndex = 0;
            for (int i = 0; i < buttonPositions.Length; i++) // Iterate through each potential position.
            {
                if (i >= firstButtonPositionIndex - downwardCountMain && buttonIndex < mainButtons.Length) // Once we've reached the first position that holds a button... 
                {   // ...and assuming we haven't yet set a new position for each button...                                   
                    StartCoroutine(ShiftButtonPosition(activeMenu, buttonIndex, i - 1, highlightPositionMoveSpeed)); // ...Shift this button to the position directly above its current position.
                    buttonIndex++; // Increase the button index so that we can shift the positions of buttons 1, 2 and 3 in the next three iterations.
                }
            }
            downwardCountMain++; // Increase the count so that we know how far "down" the player is in the main menu.
        }
    }
    private void NavigateUpMain()
    {
        if (downwardCountMain > 0) // If this count is at 0 it means the user is at the "top" of the menu, and therefore can move no further upwards.
        {
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
            downwardCountMain--; // Decrease the count so that we know how far "up" the player is in the main menu.
        }
    }
    #endregion

    #region FILE MENU METHODS & COROUTINES
    private void NavigateFile()
    {
        if (GetAxisAsButtonDown("D-PadV") != "0") NavigateTwoButtonMenu(btnReturnFile.gameObject, fileCursor);

        if (highlightedObject == fileCursor)
        {
            switch (GetAxisAsButtonDown("D-PadH"))
            {
                case "-1":
                    NavigateLeftFile();
                    break;
                case "1":
                    NavigateRightFile();
                    break;
            }
        }
    }
    private void NavigateLeftFile()
    {
        if (rightCountFile < firstFilePositionIndex && !uiElementsInMotion)
        {
            //playNavSound.start();
            fileIndex = 0;
            for (int i = 0; i < saveFilePositions.Length; i++)
            {
                if (i >= firstFilePositionIndex + rightCountFile && fileIndex < saveFiles.Length)
                {
                    StartCoroutine(ShiftButtonPosition(activeMenu, fileIndex, i + 1, highlightPositionMoveSpeed));
                    HighlightFile(i, fileIndex - 1);
                    fileIndex++;
                }
            }
            rightCountFile++;
        }
    }
    private void NavigateRightFile()
    {
        if (rightCountFile > -firstFilePositionIndex && !uiElementsInMotion)
        {
            //playNavSound.start();
            fileIndex = 0;
            for (int i = 0; i < saveFilePositions.Length; i++)
            {
                if (i >= firstFilePositionIndex + rightCountFile && fileIndex < saveFiles.Length)
                {
                    StartCoroutine(ShiftButtonPosition(activeMenu, fileIndex, i - 1, highlightPositionMoveSpeed));
                    fileIndex++;
                    HighlightFile(i, fileIndex);
                }
            }
            rightCountFile--;
        }
    }
    private void HighlightFile(int positionIndex, int fileIndex)
    {
        if (positionIndex == firstFilePositionIndex * 2) highlightedSaveFile = saveFiles[fileIndex]; // Ensure that whichever file lands in the center is designated the highlighted file.
    }
    public void LoadSaveFile()
    {
        fileToLoad = highlightedSaveFile.GetComponent<SaveFile>();

        if (fileToLoad != null) fileToLoad.LoadData();
    }
    #endregion

    #region OPTIONS MENU METHODS & COROUTINES.
    private void NavigateOptions(string axisValue)
    {
        if (axisValue == "0" || qualityDropdown.transform.Find("Dropdown List") != null || resolutionDropdown.transform.Find("Dropdown List") != null) return;

        //playNavSound.start();

        if (axisValue == "-1") // Down
        {
            downwardCountOptions++;
            if (downwardCountOptions >= optionsSettings.Length) downwardCountOptions = 0;
        }
        else if (axisValue == "1") // Up
        {
            downwardCountOptions--;
            if (downwardCountOptions < 0) downwardCountOptions = optionsSettings.Length - 1;
        }

        highlightedObject = optionsSettings[downwardCountOptions];
        StartCoroutine(ShiftUiElementPosition(highlightedPosition, highlightedObject.transform.position, highlightPositionMoveSpeed, false));
    }
    private void HandleSliderAdjustment()
    {
        if (highlightedObject.GetComponent<Slider>() != null)
        {
            Slider sliderToAdjust = highlightedObject.GetComponent<Slider>();
            switch (GetAxisAsButtonDown("D-PadH"))
            {
                case "-1": // Left
                    sliderToAdjust.value -= sliderAdjustmentPrecision;
                    break;
                case "1": // Right
                    sliderToAdjust.value += sliderAdjustmentPrecision;
                    break;
            }
        }
    }
    private void SetResolutionOptions()
    {
        resolutionDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();
        int currentResIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string resOption = resolutions[i].width + " x " + resolutions[i].height;
            resolutionOptions.Add(resOption);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
    }
    public void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetFullScreen(bool fullScreenEnabled)
    {
        Screen.fullScreen = fullScreenEnabled;
    }
    #endregion

    #region QUIT MENU METHODS & COROUTINES.
    private void NavigateQuit()
    {
        if (GetAxisAsButtonDown("D-PadV") != "0") NavigateTwoButtonMenu(quitButtons[1].gameObject, quitButtons[0].gameObject);
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
        highlightImage = highlightedPosition.GetComponent<Image>();

        #region Get access to each of the main menu's panels, as well as the active and inactive positions of each panel.
        mainPanel = mainCanvas.transform.Find("pnl0_main").gameObject;
        filePanel = mainCanvas.transform.Find("pnl1_file").gameObject;
        infoPanel = mainCanvas.transform.Find("pnl2_info").gameObject;
        optionsPanel = mainCanvas.transform.Find("pnl3_options").gameObject;
        quitPanel = mainCanvas.transform.Find("pnl4_quit").gameObject;

        mainMenuPanels.Add(mainPanel);
        mainMenuPanels.Add(filePanel);
        mainMenuPanels.Add(infoPanel);
        mainMenuPanels.Add(optionsPanel);
        mainMenuPanels.Add(quitPanel);

        panelPositionDifference = panelPositions.Length / 2; // Should be 5. Each active position has a corresponding inactive position 5 elements away from it. 
        // For example: the main panel's active position is position 0, its inactive position is position 5.

        for (int i = 0; i < mainMenuPanels.Count; i++) // Get our panel positions.
        {
            panelPositions[i] = mainMenuPanels.ElementAt(i).transform.position; // Get this panel's designated active position.
            panelPositions[i + panelPositionDifference] = mainCanvas.transform.Find(string.Format("inactive_pos_pnl_{0}", i)).position; // Get this panel's designated inactive position.
        }
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
    private void FilePanelInitialisation()
    {
        fileCursor = filePanel.transform.Find("3_file_cursor").gameObject; // Get access to the file cursor.       
        btnReturnFile = filePanel.transform.Find("4_btn_return").GetComponent<Button>(); // Get the file panel's return button.       

        saveFilePositionParent = filePanel.transform.Find("0_slot_positions"); // Get access to the file panel's potential file positions.
        saveFileParent = filePanel.transform.Find("1_slots"); // Get access to the file panel's files.

        #region Gain access to each of the file menu's potential file positions.
        for (int i = 0; i < saveFilePositions.Length; i++)
        {   // We'll have 17 potential file positions on our file menu. (Index ranges from 0 to 16.)
            saveFilePositions[i] = saveFilePositionParent.transform.GetChild(i).gameObject;
        }
        #endregion
        #region Now get a reference to each file on the file menu.       
        for (int i = 0; i < saveFiles.Length; i++)
        {   // We'll have 9 save slots/files.
            saveFiles[i] = saveFileParent.transform.GetChild(i).gameObject;
        }
        #endregion

        firstFilePositionIndex = (saveFilePositions.Length - saveFiles.Length) / 2; // Should be 4.

        #region Set the starting position of each save file.       
        fileIndex = 0;

        for (int i = firstFilePositionIndex; i < saveFilePositions.Length - firstFilePositionIndex; i++) // Start our iteration at the position we determined the first file would appear at. (Position 4, the first file position index).
        {
            saveFiles[fileIndex].transform.position = saveFilePositions[i].transform.position; // Set each of our buttons to their appropriate starting positions.
            HighlightFile(i, fileIndex);
            fileIndex++; // Increase the index so we can set the position of buttons 1, 2 and 3 in the next three loops.
        }
        #endregion       
    }
    private void InfoPanelInitialisation()
    {
        btnReturnInfo = infoPanel.transform.Find("3_btn_return_info").GetComponent<Button>();
    }
    private void OptionsPanelInitialisation()
    {   // Get access to each setting in the options menu.       
        // I want to use GetChild(index) with a for loop here, but Unity won't consider my objects' transforms in the right order despite me ordering them correctly. 
        // So let's explicitly call out each child object instead.
        optionsSettings[0] = optionsPanel.transform.Find("1_settings_options/0_stg_drpdwn_quality").gameObject;
        optionsSettings[1] = optionsPanel.transform.Find("1_settings_options/1_stg_drpdwn_resolution").gameObject;
        optionsSettings[2] = optionsPanel.transform.Find("1_settings_options/2_stg_tgl_fullscreen").gameObject;
        optionsSettings[3] = optionsPanel.transform.Find("1_settings_options/3_stg_sdr_volume_master").gameObject;
        optionsSettings[4] = optionsPanel.transform.Find("1_settings_options/4_stg_sdr_volume_bgm").gameObject;
        optionsSettings[5] = optionsPanel.transform.Find("1_settings_options/5_stg_sdr_volume_sfx").gameObject;
        optionsSettings[6] = optionsPanel.transform.Find("1_settings_options/6_btn_return_options").gameObject;

        qualityDropdown = optionsSettings[0].GetComponent<Dropdown>();
        resolutionDropdown = optionsSettings[1].GetComponent<Dropdown>();

        SetResolutionOptions();
    }
    private void QuitPanelInitialisation()
    {   // Get access to options "yes" and "no" in the quit prompt.       
        quitButtons[0] = quitPanel.transform.Find("1_buttons_quit/btn_0_no").GetComponent<Button>();
        quitButtons[1] = quitPanel.transform.Find("1_buttons_quit/btn_1_yes").GetComponent<Button>();
    }
    #endregion
}