using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class EnemyAssassin : MonoBehaviour
{
    [SerializeField] private AudioSource _takeDamageSound;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private Animator _animator;
    [SerializeField] private Shuriken _shurikenPrefab;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _moveSpeed = 5.0f;

    private PlayerController _player;
    private Transform _movingTargetPoint;
    private int _attackDamage = 1;
    private int _maxHealth = 3;
    private int _currentHealth;
    private int _pointsForCombo = 1;

    public event Action OnEnemyDeath;


    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void Init(Transform targetPoint, PlayerController player)
    {
        _player = player;
        _movingTargetPoint = targetPoint;
        if (targetPoint.position.x < transform.position.x) transform.localScale = new Vector3(-1, 1, 1);
    }


    private void Update()
    {
        if (_movingTargetPoint != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, _movingTargetPoint.position, _moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, _movingTargetPoint.position) < 0.1f)
            {
                transform.position = _movingTargetPoint.position;
                _movingTargetPoint = null;
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
        AnimationIdle();
    }

    public void TakeDamage(int damage)
    {
        _takeDamageSound.Play();
        _currentHealth -= damage;
        _player.AddComboPoints(_pointsForCombo);
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnEnemyDeath?.Invoke();
        GameController.Instance.AddScore();
        Destroy(gameObject);
    }
}   
