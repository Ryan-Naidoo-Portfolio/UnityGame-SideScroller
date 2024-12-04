using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public GameObject bullet;
    private GameObject player;
    public Transform bulletPos;
    private float timer;
    private EvilWizardBoss evilWizardBoss;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        evilWizardBoss = GetComponent<EvilWizardBoss>(); // Get reference to the EvilWizardBoss component
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the boss is alive before proceeding
        if (!evilWizardBoss.IsAlive)
        {
            return;
        }

        float distance = Vector2.Distance(transform.position, player.transform.position);
        Debug.Log(distance);

        if (distance < 10)
        {
            timer += Time.deltaTime;

            if (timer > 2)
            {
                timer = 0;
                shoot();
            }
        }
    }

    void shoot()
    {
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }
}
