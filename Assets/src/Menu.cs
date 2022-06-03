using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
public class Menu : MonoBehaviour
{
    public int GAME_SCENE_INDEX = 1;

    public void load_game()
    {
        SceneManager.LoadScene(GAME_SCENE_INDEX);
    }
}
