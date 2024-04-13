using UnityEngine;

public class Shuriken : MonoBehaviour
{
    [SerializeField] private float _shurikenSpeed = 10.0f;

    private Vector2 _direction;

    private float _shurikenLifeTime = 5.0f;
    private int _shurikenDamage;
    private int _pointForCombo = 1;


    private void Start()
    {
        Destroy(gameObject, _shurikenLifeTime); 
    }

    private void Update()
    {
        if (_direction != Vector2.zero) transform.Translate(_direction * _shurikenSpeed * Time.deltaTime);
    }

    public void Init (int damage, Vector2 direction)
    {
        _shurikenDamage = damage;
        _direction = direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(_shurikenDamage);
            Destroy(gameObject);
        }

        EnemyAssassin enemyAssassin = collision.GetComponent<EnemyAssassin>();
        if (enemyAssassin != null)
        {
            enemyAssassin.TakeDamage(_shurikenDamage);
            Destroy(gameObject);
        }

        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(_shurikenDamage, _pointForCombo);
            Destroy(gameObject);
        }
    }
}
