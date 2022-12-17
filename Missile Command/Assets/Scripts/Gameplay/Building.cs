using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IDestroyable
{
    public void Destroy()
    {
        if (GameManager.Instance.GetDestroyedBuildingsOnLevelCount() >= 3) return;
        GameManager.Instance.OnBuildingDestroyed();
        gameObject.SetActive(false);
    }
}
