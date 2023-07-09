using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideAnimationEvent : MonoBehaviour
{
    GameObject tempGO;
    public Animator anim;

    public void GuideLevel01Start()
    {
        AudioManager.instance.Play("Guide01"); 

    }

    public void GuideLevel01End()
    {
        tempGO = GameObject.Find("blink");
        tempGO.SetActive(false);
        gameObject.SetActive(false);
        tempGO = null;
    }

    

}
