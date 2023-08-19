using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBulletBehaviour : MonoBehaviour
{
    bool isShooting = false;
    Vector3 direction;
    Player.PlayerNPCHandler handler;
    public GameObject particleSys;
    public void Shoot(Vector3 from, Vector3 to)
    {
        handler = GameObject.Find("ScriptHandler").GetComponent<Player.PlayerNPCHandler>();
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
            particleSys.transform.up = transform.position - direction;

            transform.position += direction * 10 * Time.deltaTime;
            if(Vector3.Distance(gameObject.transform.position, handler.GetPlayerPosition()) < 0.3f)
            {
                if (!handler.IsPlayerAvoidingStun(true))
                {
                    handler.StunPlayer();
                }
                Destroy(gameObject);
            }
            else if(Vector3.Distance(gameObject.transform.position, handler.GetPlayerPosition()) > 20f)
            {
                Destroy(gameObject);
            }
        }
    }
}
