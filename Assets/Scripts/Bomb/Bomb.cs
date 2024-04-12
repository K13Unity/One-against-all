using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] LayerMask _enemyLayer;
    [SerializeField] Transform _attackPoint;
    [SerializeField] float _radiusDamage = 0.5f;
    [SerializeField] float _throwingSpeed = 5.0f;

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
            Destroy(gameObject);
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
}
