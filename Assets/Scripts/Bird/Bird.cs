using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bird : MonoBehaviour
{
    public enum BirdState { Idle, Thrown, HitSomething }
    public GameObject parent;
    public Rigidbody2D RigidBody;
    public CircleCollider2D Collider;

    public UnityAction OnBirdDestroyed = delegate { };
    public UnityAction<Bird> OnBirdShot = delegate { };
    public BirdState State { get { return _state; } }

    private BirdState _state;
    private float _minVelocity = 0.05f;
    private bool _flagDestroy = false;

    void Start()
    {
        RigidBody.bodyType = RigidbodyType2D.Kinematic;
        Collider.enabled = false;
        _state = BirdState.Idle;
    }

    void FixedUpdate()
    {
        // jika kondisi saat ini adalah Idle dan kecepatannya berubah menjadi lebih dari 0.5
        if (_state == BirdState.Idle && RigidBody.velocity.sqrMagnitude >= _minVelocity)
        {
            // mengubah state dari burung menjadi Thrown
            _state = BirdState.Thrown;
        }

        //  Jika kondisi burung pada saat ini adalah Thrown, dan kecepatan burung telah berada di bawah batas minimum (0.5)
        if ((_state == BirdState.Thrown || _state == BirdState.HitSomething) && RigidBody.velocity.sqrMagnitude < _minVelocity && !_flagDestroy)
        {
            //Hancurkan gameobject setelah 2 detik
            //jika kecepatannya sudah kurang dari batas minimum
            _flagDestroy = true;
            StartCoroutine(DestroyAfter(2));
        }
    }

    public virtual void OnCollisionEnter2D(Collision2D col)
    {
        _state = BirdState.HitSomething;
    }

    private IEnumerator DestroyAfter(float second)
    {
        yield return new WaitForSeconds(second);
        Destroy(gameObject);
    }

    // Method MoveTo berfungsi untuk menginisiasi posisi dan mengubah parent dari game object burung.
    public void MoveTo(Vector2 target, GameObject parent)
    {
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.position = target;
    }

    // Method Shoot berfungsi untuk melemparkan burung dengan arah, jarak tali yang ditarik, dan kecepatan awal
    public void Shoot(Vector2 velocity, float distance, float speed)
    {
        Collider.enabled = true;
        RigidBody.bodyType = RigidbodyType2D.Dynamic;
        RigidBody.velocity = velocity * speed * distance;
        OnBirdShot(this);
    }

    public virtual void OnTap()
    {
        //It has no skill to be shown off, poor bird
    }

    void OnDestroy()
    {
        if (_state == BirdState.Thrown || _state == BirdState.HitSomething)
            OnBirdDestroyed();
    }

}
