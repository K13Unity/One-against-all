using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private EnemyAssassin _enemyAssassinPrefab;
    [SerializeField] private List<Transform> _spawnPoint;
    [SerializeField] private List<Ledge> _ledge;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private float _lastCreationTime = 2f;


    private float _intervalTime = 0;

    void Update()
    {
        ReverseTimeFlow();

    }

    void ReverseTimeFlow()
    {
        _intervalTime += Time.deltaTime;
        if (_intervalTime >= _lastCreationTime)
        {
            CreateEnemy();
            _intervalTime = 0;
        }
    }


    private void CreateEnemy()
    {
        var randomSpawn =  Random.Range(0, _spawnPoint.Count);
        Enemy enemy = Instantiate(_enemyPrefab, _spawnPoint[randomSpawn].position, Quaternion.identity);
        enemy.Init(_playerController);
        
        var emptyLedge = _ledge.Find(ledge => ledge.IsEmpty);
        if (emptyLedge != null)
        {
            var enemyAssassin = Instantiate(_enemyAssassinPrefab);
            emptyLedge.SetEnemy(enemyAssassin);
        }
    }
}
