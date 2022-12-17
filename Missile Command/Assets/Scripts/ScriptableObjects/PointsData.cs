using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Points data", menuName ="Points data")]
public class PointsData : ScriptableObject
{
    public int points = 0;
    public int pointsMultiplier = 1;
    public UnityEvent onPointsChanged = new UnityEvent();
    public UnityEvent onPointsMultiplierChanged = new UnityEvent();

    private void OnEnable()
    {
        points = 0;
        pointsMultiplier = 1;
    }

    public void AddPoints(int value)
    {
        points += value*pointsMultiplier;
        onPointsChanged.Invoke();
    }

    public void RisePointMultiplier()
    {
        pointsMultiplier++;
        onPointsMultiplierChanged.Invoke();
    }
}
