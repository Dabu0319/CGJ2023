using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;



public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;
    private void Awake()
    {
        instance = this;
    }
    
    public Animator transition;
    //public float transitionTime = 1f;
    //public float transitionTime;
    
    public float fadeInTime = 1f;
    public float fadeOutTime = 1f;
    
    
    
    public GameObject nextLevelButton;
    //public float nextLevelButtonDelay = 0f;
    
    
    void Start()
    {
        transition.speed = 1/fadeInTime;
        StartCoroutine(ChangeAnimationSpeed());
        //set the transitionTime to the length of the animation
        //transitionTime = transition.runtimeAnimatorController.animationClips[0].length;

        //set the animationclop's end time to the transitionTime
        //https://answers.unity.com/questions/1002400/how-do-i-set-the-time-of-an-animation-playing-in-t.html
        //transition.runtimeAnimatorController.animationClips[0]. = transitionTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            LoadNextLevel();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            LoadPreviousLevel();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }
    
    public void LoadPreviousLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
    }
    
    IEnumerator LoadLevel(int levelIndex)
    {
        
        transition.SetTrigger("StartTransition");
        yield return new WaitForSeconds(fadeOutTime);
        SceneManager.LoadScene(levelIndex);
    }
    
    //在fadeIn后将animation的speed设置为1/fadeOutTime，从而实现fadeIn和fadeOut的时间不同
    IEnumerator ChangeAnimationSpeed()
    {
        
        yield return new WaitForSeconds(fadeInTime);
        transition.speed = 1/fadeOutTime;
    }
    
    public void NextLevelButton(float delay)
    {
        nextLevelButton.GetComponentInChildren<DOTweenAnimation>().delay = delay;
        nextLevelButton.SetActive(true);
    }
    
    public void RestartLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }
}
