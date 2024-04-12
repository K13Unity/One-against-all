using Cysharp.Threading.Tasks;
using UnityEngine;



public class Enemy : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] Transform _attackPoint;
    [SerializeField] float _moveSpeed = 5.0f;
    [SerializeField] LayerMask _playerLayer;
    [SerializeField] float _punchDistance = 15.0f;
    [SerializeField] float _attackRange = 0.5f;

    private PlayerController _player;

    private int _attackDamage = 1;
    private int _health = 3;
    private int _currentHealth;
    private float _distanceToPlayer;
    private float _moveDirection;
    private float _pushAwayDistance = 30f;
    private bool _canMove = true;




    protected void Start()
    {
        _currentHealth = _health;
    }

    public void Init(PlayerController player)
    {
        _player = player;
        DecideMovementDirection();
    }

    private void DecideMovementDirection()
    {
        float direction = Mathf.Sign(_player.transform.position.x - transform.position.x);
        _moveDirection = direction;
        LookAtThePlayer();
    }

    private void Update()
    {
        if (!_canMove)
        {
            return;
        }

        _distanceToPlayer = Vector2.Distance(transform.position, _player.transform.position);

        if (_distanceToPlayer > _pushAwayDistance)
        {
            UpdateDirectionTowardsPlayer();
        }

        if (_distanceToPlayer >= _punchDistance)
        {
            transform.Translate(new Vector2(_moveDirection, 0) * _moveSpeed * Time.deltaTime);
        }
        else
        {
            AnimationAttackPlayer();
        }
    }

    private  void AnimationAttackPlayer()
    {
        _animator.SetTrigger("Attack");
    }

    private void AttackPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(_attackPoint.position, _attackRange, _playerLayer);
        if (player != null)
        {
            player.GetComponent<PlayerController>().TakeDamage(_attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }

    private void UpdateDirectionTowardsPlayer()
    {
        _moveDirection = Mathf.Sign(_player.transform.position.x - transform.position.x);
        LookAtThePlayer(); 
    }

    private void LookAtThePlayer()
    {
        transform.localScale = new Vector3(_moveDirection, 1, 1);
    }

    private async void PushAway()
    {
        if (transform.localPosition.x < 1)
        {
            transform.Translate(Vector2.left * 4f);
        }
        else
        {
            transform.Translate(Vector2.right * 4f);
        }
        await UniTask.Delay(1000);
        _canMove = true;
    }

    public void TakeDamage(int damage, int pointForCombo)
    {
        _currentHealth -= damage;
        _player.Init(pointForCombo);
        _canMove = false;
        PushAway();
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        Destroy(gameObject);
    }
}

