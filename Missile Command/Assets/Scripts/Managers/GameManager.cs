using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    private const int bonusBuildingPointsLim = 10000;

    [SerializeField] private List<Base> bases;
    [SerializeField] private List<Building> buildings;
    [SerializeField] private BonusPointsCounter bonusPointsCounter;
    [SerializeField] private PointsData pointsData;

    private int _playerMissilesCount = 0;
    private int _enemyMissilesCount = 0;
    private int _buildingsCount = 0;
    private int _destroyedBuildings = 0;
    
    private bool _pause = false;

    private int _currentLevel = 1;

    private int _bonusBuildingPointsTier = 1;

    private int _bonusBuildingsCount = 0;



    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
    }

    private void Start()
    {
        _playerMissilesCount = 10 * bases.Count;
        _buildingsCount = buildings.Count;
        _enemyMissilesCount = EnemyMissileSpawnerManager.Instance.SpawnMissiles(1);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_pause) return;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (mousePosition.y < -3.9) return;
            
            GameObject crosshair = ObjectPoolManager.Instance.GetElementByName("crosshairs");
            crosshair.transform.position = (Vector2)mousePosition;
            crosshair.SetActive(true);

            Base nearest = GetNearestBase(mousePosition);
            if(nearest != null)nearest.FireMissile(crosshair);
        }
    }

    public void StartNextLevel()
    {
        _playerMissilesCount = 10 * bases.Count;
        _destroyedBuildings = 0;
        foreach (Base bBase in bases)
        {
            bBase.RestartBase();
        }

        if(pointsData.points/bonusBuildingPointsLim >= _bonusBuildingPointsTier)
        {
            _bonusBuildingsCount+= (pointsData.points / bonusBuildingPointsLim) - (_bonusBuildingPointsTier-1);
            _bonusBuildingPointsTier += pointsData.points / bonusBuildingPointsLim;
        }

        int buildingsToFix = _bonusBuildingsCount;
        for(int i = 0; i < buildingsToFix; i++)
        {
            if (!FixBuilding()) return;
            _bonusBuildingsCount--;
        }

        _enemyMissilesCount = EnemyMissileSpawnerManager.Instance.SpawnMissiles(++_currentLevel);
        if ((_currentLevel - 1) % 2 == 0 && pointsData.pointsMultiplier < 6) pointsData.RisePointMultiplier();
        _pause = false;
    }

    private Base GetNearestBase(Vector2 position)
    {
        Base nearest = bases.Find(x => x.gameObject.activeSelf);
        if (nearest == null) return null;
        float nearestDistance = Vector2.Distance(position, nearest.transform.position);
        foreach (Base bBase in bases)
        {
            if (!bBase.isActiveAndEnabled) continue;
            float newDistance = Vector2.Distance(position, bBase.transform.position);
            if ( newDistance < nearestDistance)
            {
                nearestDistance = newDistance;
                nearest = bBase;
            }
            else if (newDistance == nearestDistance)
            {
                nearest = nearest.missileCapacity >= bBase.missileCapacity ? nearest : bBase;
            }
        }

        return nearest;
    }

    public void SubstractMissile(int count, bool player)
    {
        if (player)
        {
            _playerMissilesCount -= count;

            if (_playerMissilesCount == 0)
            {
                _pause = true;
                EnemyMissileSpawnerManager.Instance.StopSpawning();
                _enemyMissilesCount -= EnemyMissileSpawnerManager.Instance.GetToSpawnMissilesCount();
                Time.timeScale = 4;
            }
        }
        else
        {
            _enemyMissilesCount -= count;
            if (_enemyMissilesCount == 0)
            {
                Time.timeScale = 1;
                _pause = true;
                bonusPointsCounter.CountBonusPoints(_playerMissilesCount, _buildingsCount);
            }
        }
       
    }


    #region Buildings

    private bool FixBuilding() 
    {
        Building destroyedBuilding = buildings.Find(x => !x.gameObject.activeSelf);
        if (destroyedBuilding == null) return false;
        destroyedBuilding.gameObject.SetActive(true);
        _destroyedBuildings--;
        _buildingsCount++;
        return true;
    }


    public void OnBuildingDestroyed()
    {
        _buildingsCount--;
        _destroyedBuildings++;
        if (_buildingsCount == 0)
        {
            SceneManager.LoadScene("EndScreen");
        }
    }


    public int GetDestroyedBuildingsOnLevelCount()
    {
        return _destroyedBuildings;
    }
    #endregion


}
