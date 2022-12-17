using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointsCountText;
    [SerializeField] private TextMeshProUGUI pointsMultiplayerText;
    [SerializeField] private PointsData pointsData;

    private void Start()
    {
        UpdatePointsCountText();
        UpdatePointsMultiplierText();
        pointsData.onPointsChanged.AddListener(UpdatePointsCountText);
        pointsData.onPointsMultiplierChanged.AddListener(UpdatePointsMultiplierText);
    }

    private void UpdatePointsCountText()
    {
        pointsCountText.text = pointsData.points.ToString();
    }
    
    private void UpdatePointsMultiplierText()
    {
        pointsMultiplayerText.text = pointsData.pointsMultiplier + "x";
    }
}
