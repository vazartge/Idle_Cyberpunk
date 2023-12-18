using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopStarUI : MonoBehaviour
{
    public GameObject ActiveStar;

    public void ActivateStar()
    {
        ActiveStar.SetActive(true);
    }

    public void DeactivateStar()
    {
        ActiveStar.SetActive(false);
    }
}
