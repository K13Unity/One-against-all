using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] Shuriken _shurikenPrefab;
    [SerializeField] Bomb _bombPrefab;
    [SerializeField] Animator _animator;
    [SerializeField] Transform _shurikenCreationPoint;
    [SerializeField] Transform _bombCreationPoint;
    [SerializeField] Transform _attackPoint;
    [SerializeField] Transform _groundCheck;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] LayerMask _enemyLayer;
    [SerializeField] float _attackRange = 0.5f;
    [SerializeField] TextMeshProUGUI _comboPoints;
    [SerializeField] TextMeshProUGUI _healthText;

    private int _pointsForCombo = 10;
    private int _pointForDamage = 10;
    private int _attackDamage = 1;
    private int _currentHealth = 10;
    private float _timeBetweenAttack;
    private float _startTimeBetweenAttack = 0.07f;
    private float _radiusGroundCheck = 0.1f;
    private float _jumpForce = 60.0f;
    public bool _isGrounded;
    private bool _isThrowBomb = false;


    private void Start()
    {
        _healthText.text = "X-" + _currentHealth.ToString();
        _comboPoints.text = "Points - " + _pointsForCombo.ToString();
    }
    private void Update()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _radiusGroundCheck, _groundLayer);

        if (_timeBetweenAttack <= 0)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
                if(_isGrounded) AttackAnimation();
                else LaunchShuriken(Vector2.left);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                if(_isGrounded) AttackAnimation();
                else LaunchShuriken(Vector2.right);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (!_isGrounded && !_isThrowBomb)
                {
                    ThrowBomb(Vector2.down);
                    _isThrowBomb = true;
                }
                else return;
            }
        }
        else
        {
            _timeBetweenAttack -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.W) && _isGrounded)
        {
            _rigidbody.velocity = Vector2.up * _jumpForce;
            _isThrowBomb = false;
        }
    }
    private void LaunchShuriken(Vector2 direction)
    {
        
        Shuriken shuriken = Instantiate(_shurikenPrefab);
        if (direction == Vector2.left)
        {
            shuriken.transform.localScale = new Vector3(-1, 1, 1);
        }
        shuriken.transform.position = _shurikenCreationPoint.position;
        shuriken.Init(_attackDamage, direction);
    }
    private void ThrowBomb(Vector2 directoin)
    {
        Bomb bomb = Instantiate(_bombPrefab);
        bomb.transform.position = _bombCreationPoint.position;
        bomb.Init(directoin);
    }

    private void AttackAnimation()
    {
        _animator.SetTrigger("Attack");
    }

    private void OnAttackEnemy()
    {
        _timeBetweenAttack = _startTimeBetweenAttack;

        Collider2D enemy = Physics2D.OverlapCircle(_attackPoint.position, _attackRange, _enemyLayer);
        if (enemy != null)
        {
            enemy.GetComponent<Enemy>().TakeDamage(_attackDamage, _pointForDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _healthText.text = "X-" + _currentHealth.ToString();
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    internal void Init(int pointsForCombo)
    {
        _pointsForCombo += pointsForCombo;
        _comboPoints.text = "Points: " + _pointsForCombo.ToString();
    }
}
