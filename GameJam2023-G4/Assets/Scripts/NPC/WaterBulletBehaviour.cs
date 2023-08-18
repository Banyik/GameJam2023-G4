using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBulletBehaviour : MonoBehaviour
{
    bool isShooting = false;
    Vector3 direction;
    Player.Behaviour playerBehaviour;
    public GameObject particleSystem;
    public void Shoot(Vector3 from, Vector3 to)
    {
        playerBehaviour = GameObject.Find("Player").GetComponent<Player.Behaviour>();
        direction = CalculateDirection(from, to);
        isShooting = true;
    }

    Vector2 CalculateDirection(Vector3 from, Vector3 to)
    {
        return (to -  from).normalized;
    }

    private void FixedUpdate()
    {
        if (isShooting)
        {
            particleSystem.transform.up = transform.position - direction;

            transform.position += direction * 10 * Time.deltaTime;
            if(Vector3.Distance(gameObject.transform.position, playerBehaviour.gameObject.transform.position) < 0.3f)
            {
                if (!playerBehaviour.avoidStun)
                {
                    playerBehaviour.ChangeState(Player.State.StunnedStart);
                }
                else
                {
                    playerBehaviour.avoidStun = false;
                }
                Destroy(gameObject);
            }
            else if(Vector3.Distance(gameObject.transform.position, playerBehaviour.gameObject.transform.position) > 20f)
            {
                Destroy(gameObject);
            }
        }
    }
}
