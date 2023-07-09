using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideAnimationEvent : MonoBehaviour
{
    GameObject tempGO;

    public void GuideLevel01Start()
    {
        tempGO = GameObject.Find("DigBlock");
        tempGO.SetActive(false);

    }

    public void GuideLevel01End()
    {
        tempGO.SetActive(true);
        tempGO = null;
        gameObject.SetActive(false);
    }
}
