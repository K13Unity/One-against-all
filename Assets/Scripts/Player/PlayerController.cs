using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _comboPoints;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private AudioSource _jumpSound;
    [SerializeField] private AudioSource _attackSound;
    [SerializeField] private AudioSource _takeDamageSound;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Shuriken _shurikenPrefab;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _shurikenCreationPoint;
    [SerializeField] private Transform _bombCreationPoint;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Bomb _bombPrefab;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _attackRange = 0.5f;

    private int _pointsForHit;
    private int _pointForDamage = 1;
    private int _attackDamage = 1;
    private int _currentHealth = 10;
    private float _timeBetweenAttack;
    private float _startTimeBetweenAttack = 0.07f;
    private float _radiusGroundCheck = 0.1f;
    private float _jumpForce = 65.0f;
    private bool _isThrowBomb = false;
    public bool _isGrounded;


    private void Start()
    {
        _healthText.text = _currentHealth.ToString();
        _comboPoints.text = "Hit - " + _pointsForHit.ToString();
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
        else _timeBetweenAttack -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.W) && _isGrounded)
        {
            _rigidbody.velocity = Vector2.up * _jumpForce;
            PlaySoundJump();
            _isThrowBomb = false;
           
        }
    }
    private void LaunchShuriken(Vector2 direction)
    {
        
        Shuriken shuriken = Instantiate(_shurikenPrefab);
        if (direction == Vector2.left) shuriken.transform.localScale = new Vector3(-1, 1, 1);
        shuriken.transform.position = _shurikenCreationPoint.position;
        shuriken.Init(_attackDamage, direction);
    }

    private void ThrowBomb(Vector2 directoin)
    {
        Bomb bomb = Instantiate(_bombPrefab);
        bomb.transform.position = _bombCreationPoint.position;
        bomb.Init(directoin);
    }

    private async void AttackAnimation()
    {
        _attackSound.Play();
        await UniTask.Delay(100);
        _animator.SetTrigger("Attack");
    }

    private void OnAttackEnemy()
    {
        _timeBetweenAttack = _startTimeBetweenAttack;

        Collider2D enemy = Physics2D.OverlapCircle(_attackPoint.position, _attackRange, _enemyLayer);
        if (enemy != null) enemy.GetComponent<Enemy>().TakeDamage(_attackDamage, _pointForDamage);
    }

    public void TakeDamage(int damage)
    {
        _takeDamageSound.Play();
        _currentHealth -= damage;
        _healthText.text = _currentHealth.ToString();
        if (_currentHealth <= 0) Die();
    }

    private void Die()
    {
        GameController.Instance.GameOver();
        Destroy(gameObject);
    }

    public void AddComboPoints(int pointsForCombo)
    {
        _pointsForHit += pointsForCombo;
        _comboPoints.text = "Hit " + _pointsForHit.ToString();
    }

    private async void PlaySoundJump()
    {
        await UniTask.Delay(700);
        _jumpSound.Play();
    }
}
