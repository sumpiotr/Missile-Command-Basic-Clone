using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Transform = UnityEngine.Transform;

public class Base : MonoBehaviour, IDestroyable
{

    [SerializeField] private Transform tower;
    [SerializeField] private GameObject missileMarkersParent;
    [SerializeField] private TextMeshProUGUI infoText;

    public int maxMissileCapacity = 10;
    public int missileCapacity;

    private List<GameObject> _markers = new List<GameObject>();

    private void Start()
    {
        missileCapacity = maxMissileCapacity;
        foreach (Transform child in missileMarkersParent.transform)
        {
            _markers.Add(child.gameObject);
        }
    }

    public void FireMissile(GameObject target)
    {
        Missile missile = ObjectPoolManager.Instance.GetElementByName("playerMissile").GetComponent<Missile>();
        missile.transform.position = (Vector2)transform.position;
        missile.gameObject.SetActive(true);
        missile.Fire(target);
        
        
        Vector2 dir = target.transform.position - tower.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle -= 90;
        tower.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        _markers[maxMissileCapacity - missileCapacity].SetActive(false);
        missileCapacity--;


        if (missileCapacity == 0)DisableBase();
        else if (missileCapacity <= 3)
        {
            infoText.text = "Low";
        }
    }

    public void RestartBase()
    {
        gameObject.SetActive(true);
        missileMarkersParent.SetActive(true);
        infoText.text = "";
        missileCapacity = maxMissileCapacity;
        foreach(GameObject marker in _markers)
        {
            marker.SetActive(true);
        }
    }

    private void DisableBase()
    {
        gameObject.SetActive(false);
        missileMarkersParent.SetActive(false);
        infoText.text = "Out";
    }


    public void Destroy()
    {
        DisableBase();
        GameManager.Instance.SubstractMissile(missileCapacity, true);
    }
}
