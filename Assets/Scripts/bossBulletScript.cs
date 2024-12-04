using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossBulletScript : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    public float force = 5f;
    private float bulletTimer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 180);
    }

    // Update is called once per frame
    void Update()
    {
        bulletTimer += Time.deltaTime;
        if (bulletTimer > 10)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Damageable>().Health -= 10;
            Destroy(gameObject);
        }
    }
}
