using UnityEngine;

public class Ledge : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private EnemyAssassin _enemyAssassinPrefab;

    public bool IsEmpty { get; private set; } = true;

    public void SetEnemy(EnemyAssassin enemy)
    {
        enemy.transform.position = _spawnPoint.position;
        enemy.Init(_targetPoint, _playerController);
        enemy.OnEnemyDeath += OnEnemyDeath;
        IsEmpty = false;
    }

    public void OnEnemyDeath()
    {
        IsEmpty = true;
    }
}