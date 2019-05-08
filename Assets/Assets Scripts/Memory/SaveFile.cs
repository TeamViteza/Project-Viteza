using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveFile : MonoBehaviour {

    string data; // Only for show at the moment.

	// Nothin' here just yet.

    public void LoadData()
    {
        if (data == null) SceneManager.LoadScene(1); // For now, let's just load a sample gameplay scene. In the future, if there is no data, the game will start from the beginning.
    }
}