using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _force = 10;
    [SerializeField] private float _lifeTime = 3;

    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * _force);
    }

    private void FixedUpdate()
    {
        if (_lifeTime > 0)
        {
            _lifeTime -= 0.02f;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
