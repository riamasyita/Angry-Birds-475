using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlBird : Bird
{
    public float explodeRadius = 2.00f;
    public float force = 250f;
    public LayerMask LayerToHit;

    public GameObject explode;

    public override void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
        Explode();
    }

    void Explode()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, explodeRadius, LayerToHit);

        foreach (Collider2D obj in objects)
        {
            Vector2 arah = obj.transform.position - transform.position;

            obj.GetComponent<Rigidbody2D>().AddForce(arah * force);
            if (obj.tag == "Enemy")
            {
                Enemy enemy = obj.GetComponent<Enemy>();
                enemy.setHit();
            }
        }
        GameObject explodebird = Instantiate(explode, transform.position, Quaternion.identity);
        Destroy(explodebird, 5);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explodeRadius);
    }
}
