using Cysharp.Threading.Tasks;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private Explosion _explosionPrefab;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _radiusDamage = 0.5f;
    [SerializeField] private float _throwingSpeed = 5.0f;

    private int _pointsForCombo = 1;
    private Vector2 _direction;
    private int _damage = 3;

    public void Init(Vector2 direction)
    {
        _direction = direction;
    }

    private void Update()
    {
        transform.Translate(_direction * _throwingSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Ground ground = other.GetComponent<Ground>();
        if (ground != null)
        {
            BombExplosion();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            DestroyBomb();
        }
    }

    private void BombExplosion()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _radiusDamage);
        foreach (Collider2D col in colliders)
        {
            Enemy nearbyEnemy = col.GetComponent<Enemy>();
            if (nearbyEnemy != null)
            {
                nearbyEnemy.TakeDamage(_damage, _pointsForCombo);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_attackPoint.position, _radiusDamage);
    }
    
    private void DestroyBomb()
    {
        Destroy(gameObject);
    }
}
