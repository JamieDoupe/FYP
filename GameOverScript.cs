using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    // Return player to the start of the game
   public void loadGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}
