using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System;
using UnityEngine;

public class EnemyAssassin : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] Shuriken _shurikenPrefab;
    [SerializeField] Transform _attackPoint;
    [SerializeField] LayerMask _playerLayer;
    [SerializeField] float _moveSpeed = 5.0f;

    private PlayerController _player;
    private Transform _targetPoint;
    private int _attackDamage = 1;
    private int _health = 3;
    private int _currentHealth;
    private int _pointsForCombo = 1;

    public event Action OnEnemyDeath;


    protected void Start()
    {
        _currentHealth = _health;

    }

    public void Init(Transform targetPoint, PlayerController player)
    {
        _player = player;
        _targetPoint = targetPoint;
        if (targetPoint.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }


    private void Update()
    {
        if (_targetPoint != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, _targetPoint.position, _moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, _targetPoint.position) < 0.1f)
            {
                transform.position = _targetPoint.position;
                _targetPoint = null;
                AnimationIdle();
            }
        }
    }

    private async void AnimationIdle()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("Idle");
            await UniTask.Delay(3000);
            AnimationAttackPlayer();
        }
    }

    private void AnimationAttackPlayer()
    {
        if (_animator != null) _animator.SetTrigger("Attack");
    }

    private void AttackPlayer()
    {
        Vector2 directionToPlayer = (_player.transform.position - transform.position).normalized;
        Shuriken shuriken = Instantiate(_shurikenPrefab);
        shuriken.transform.position = _attackPoint.position;
        shuriken.Init(_attackDamage, directionToPlayer);
        if (directionToPlayer.x < 0) shuriken.transform.localScale = new Vector3(-1, 1, 1);
        else shuriken.transform.localScale = new Vector3(1, 1, 1);
        AnimationIdle();
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _player.AddComboPoints(_pointsForCombo);
        if (_currentHealth <= 0)
        {
            OnEnemyDeath?.Invoke();
            OnEnemyDeath -= OnEnemyDeath;
            Die();
        }
    }

    protected void Die()
    {
        GameController.Instance.AddScore();
        Destroy(gameObject);
    }
}   
