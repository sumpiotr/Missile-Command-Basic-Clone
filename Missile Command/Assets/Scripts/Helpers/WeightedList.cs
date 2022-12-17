using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeightedList<T>
{
    [SerializeField]private List<WeightedListElement<T>> elements = new List<WeightedListElement<T>>();


    public WeightedListElement<T> GetRandomElement() 
    {
        int randomNumber = Random.Range(0, (int)GetTotalWeight());

        float sum = 0;

        foreach(WeightedListElement<T> element in elements)
        {
            sum += element.weight;
            if(randomNumber <= sum)
            {
                return element;
            }
        }
        return null;
    }

    public T GetRandomValue()
    {
        int randomNumber = Random.Range(0, (int)GetTotalWeight());

        float sum = 0;

        foreach (WeightedListElement<T> element in elements)
        {
            sum += element.weight;
            if (randomNumber <= sum)
            {
                return element.value;
            }
        }
        return default(T);
    }

    private float GetTotalWeight() 
    {
        float weight = 0;
        foreach(WeightedListElement<T> element in elements) 
        {
            weight += element.weight;
        }
        return weight;
    }

    public void SetEqualWeightToElements(float weight) 
    {
        foreach(WeightedListElement<T> element in elements)
        {
            element.weight = weight;
        }
    }
}


[System.Serializable]
public class WeightedListElement<T>
{
    public T value;
    public float weight;
}
