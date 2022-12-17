using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMissileSpawnerManager : MonoBehaviour
{
    public static EnemyMissileSpawnerManager Instance = null;
    [SerializeField] private WeightedList<GameObject> targetsList;


    private List<int> _missilesSpawnPattern = new List<int>();

    private int _spawnedMissiles = 0;
    private int _missilesCount = 0;

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
    }

    private void Start()
    {
        targetsList.SetEqualWeightToElements(30);
    }

    public int SpawnMissiles(int level)
    {
        _spawnedMissiles = 0;
        _missilesSpawnPattern.Clear();
        targetsList.SetEqualWeightToElements(30);
        int waves = 2;
        int missilesCount = 0;
        for (int wave = 0; wave < waves; wave++)
        {
            for (int i = 0; i < 2; i++)
            {
                int count = Random.Range(4, 6);
                missilesCount += count;
                _missilesSpawnPattern.Add(count);
            }
            _missilesSpawnPattern.Add(0);
        }

        StartCoroutine(nameof(SpawnMissilesCoroutine), level);
        _missilesCount = missilesCount;
        return missilesCount;
    }

   private  IEnumerator SpawnMissilesCoroutine(int level)
    {
        for (int i = 0; i < _missilesSpawnPattern.Count - 1; i++)
        {
            int spawn = _missilesSpawnPattern[i];
            if (spawn == 0)
            {
                yield return new WaitForSeconds(5f - 0.2f * (level-1));
                continue;
            }

            _spawnedMissiles += spawn;
            for (int missileCount = 0; missileCount < spawn; missileCount++)
            {
                SpawnEnemyMissile(level);
            }

            yield return new WaitForSeconds(3.5f);
        }
    }
    
    private void SpawnEnemyMissile(int level)
    {
        WeightedListElement<GameObject> element = targetsList.GetRandomElement();
        element.weight -= 10;
        GameObject target = element.value;
        int startX = Random.Range(-6, 6);
        Missile missile = ObjectPoolManager.Instance.GetElementByName("enemyMissile").GetComponent<Missile>();
        missile.transform.position = new Vector2(startX, 5.6f);
        missile.gameObject.SetActive(true);
        missile.Fire(target, level);
    }
    

    public void StopSpawning()
    {
        StopAllCoroutines();
    }

    public int GetToSpawnMissilesCount()
    {
        return _missilesCount - _spawnedMissiles;
    }
    
}
