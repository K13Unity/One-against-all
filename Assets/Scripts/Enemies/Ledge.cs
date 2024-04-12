using UnityEngine;

public class Ledge : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] Transform _targetPoint;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] EnemyAssassin _enemyAssassinPrefab;

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
