using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private AudioSource _bombSound;

    void Start()
    {
        _bombSound.Play();
        Destroy(gameObject, 0.5f);
    }
}
   
