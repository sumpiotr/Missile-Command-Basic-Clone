using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BonusPointsCounter : MonoBehaviour
{

    [SerializeField] private GameObject ui;
    
    [SerializeField] private TextMeshProUGUI missilesPointsText;
    [SerializeField] private Transform missilesImagesParent;
    
    [SerializeField] private TextMeshProUGUI buildingsPointsText;
    [SerializeField] private Transform buildingsImagesParent;

    [SerializeField] private PointsData pointsData;

    private List<GameObject> _buildingsImages = new List<GameObject>();
    private List<GameObject> _missilesImages = new List<GameObject>();

    private void Start()
    {
        foreach(Transform child in missilesImagesParent)
        {
            _missilesImages.Add(child.gameObject);
        }
        foreach(Transform child in buildingsImagesParent)
        {
            _buildingsImages.Add(child.gameObject);
        }
    }

    public void CountBonusPoints(int missilesCount, int buildingsCount)
    {
        foreach (GameObject image in _missilesImages)
        {
            image.SetActive(false);
        }
        foreach (GameObject image in _buildingsImages)
        {
            image.SetActive(false);
        }

        missilesPointsText.text = "0";
        buildingsPointsText.text = "0";
        ui.SetActive(true);
        StartCoroutine(CountBonusPointsCoroutine(missilesCount, buildingsCount));
    }

    private IEnumerator CountBonusPointsCoroutine(int missilesCount, int buildingsCount)
    {
        for (int i = 0; i < missilesCount; i++)
        {
            _missilesImages[i].SetActive(true);
            missilesPointsText.text = ((i + 1) * 5 * pointsData.pointsMultiplier).ToString();
            yield return new WaitForSeconds(0.15f);
        }
        
        for (int i = 0; i < buildingsCount; i++)
        {
            _buildingsImages[i].SetActive(true);
            buildingsPointsText.text = ((i + 1) * 100 * pointsData.pointsMultiplier).ToString();
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(1.5f);
        pointsData.AddPoints((missilesCount * 5 * pointsData.pointsMultiplier) + (buildingsCount * 100 * pointsData.pointsMultiplier));
        ui.SetActive(false);
        GameManager.Instance.StartNextLevel();
    }
    
    
}
