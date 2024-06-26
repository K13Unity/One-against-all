using Cysharp.Threading.Tasks;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _moveSpeed = 5.0f;
    [SerializeField] private float _punchDistance = 15.0f;
    [SerializeField] private float _attackRange = 0.5f;

    private PlayerController _player;

    private int _attackDamage = 1;
    private int _maxHealth = 3;
    private int _currentHealth;
    private float _distanceToPlayer;
    private float _moveDirection;
    private float _pushAwayDistance = 30f;
    private bool _canMove = true;




    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void Init(PlayerController player)
    {
        _player = player;
        DecideMovementDirection();
    }

    private void DecideMovementDirection()
    {
        _moveDirection = Mathf.Sign(_player.transform.position.x - transform.position.x);
        transform.localScale = new Vector3(_moveDirection, 1, 1);
    }

    private void Update()
    {
        if (!_canMove || _player == null) return;

        _distanceToPlayer = Vector2.Distance(transform.position, _player.transform.position);

        if (_distanceToPlayer > _pushAwayDistance) DecideMovementDirection();
        if (_distanceToPlayer >= _punchDistance) transform.Translate(new Vector2(_moveDirection, 0) * _moveSpeed * Time.deltaTime);
        else AnimationAttackPlayer();
    }

    private  void AnimationAttackPlayer()
    {
        _animator.SetTrigger("Attack");
    }

    private void AttackPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(_attackPoint.position, _attackRange, _playerLayer);
        if (player != null) player.GetComponent<PlayerController>().TakeDamage(_attackDamage);
    }

    private async void PushAway()
    {
        transform.Translate(transform.localPosition.x < 1 ? Vector2.left * 4f : Vector2.right * 4f);
        await UniTask.Delay(1000);
        _canMove = true;
    }

    public void TakeDamage(int damage, int pointForCombo)
    {
        _currentHealth -= damage;
        _player.AddComboPoints(pointForCombo);
        _canMove = false;
        PushAway();
        if (_currentHealth <= 0) Die();
    }

    private void Die()
    {
        GameController.Instance.AddScore();
        Destroy(gameObject);
    }
}

