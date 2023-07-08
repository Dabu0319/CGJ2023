using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelName : MonoBehaviour
{
    public Text levelNum;
    private int level;

    public List<Sprite> levelNumImages;

    void Start()
    {
        //get the current level number from sceneManager
        level = SceneManager.GetActiveScene().buildIndex;
        levelNum.text = level.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
