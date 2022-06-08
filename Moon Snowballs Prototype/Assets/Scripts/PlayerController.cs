using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerMod
{
    Player,
    AI
};

public class PlayerController : MonoBehaviour
{
    public PlayerMod mod = PlayerMod.Player;

    public LayerMask layer;
    public LayerMask AILayer;

    public GameObject dropParticlePredab;
    public GameObject trail;
    public GameObject shellPrefab;
    public GameObject visualShell;
    public Transform hand;
    public Transform shellSpawnPoint;
    public Transform head;
    public HealthController healthController;

    public float maxTimeForFly;
    public float maxSpeedForSpawnParticle;
    public float maxHandRotationSpeedForTrail;
    public float force;
    public float jumpRotatoinForce;
    public float rotatoinForce;
    public float rayDistance;
    public float maxDelay;
    public float handRotationSpeed;
    public float shellForce;
    public float shootDelay;
    public float flyForce;
    [Header("AI options")]
    public float handUpDelay;
    public float lookDistance;

    GameObject target;

    float curreFlyTime;
    float curretHandRotationSpeed;
    float curretTime;
    float curretShootDelayTime;
    float curretHandUpDelay;
    bool startHandRot;
    bool isGround;
    new Rigidbody2D rigidbody2D;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Head_0");
    }


    private void FixedUpdate()
    {
        if (healthController.destroy)
        {
            startHandRot = false;
        }
        else
        {

            if (startHandRot)
            {
                StartHandRot();
            }
            else
            {
                StopHandRot();
            }

            if (!visualShell.activeSelf)
            {
                curretShootDelayTime += Time.fixedDeltaTime;
                if (curretShootDelayTime >= shootDelay)
                {
                    visualShell.SetActive(true);
                    curretShootDelayTime = 0;
                }
            }

            if (mod == PlayerMod.AI)
            {
                if (!PlayManager.restartGame)
                    AI();
                else
                {
                    startHandRot = false;
                }
            }

            Equilibrium();
        }
    }

    void Equilibrium()
    {
        Physics2D.queriesHitTriggers = false;
        if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.right), rayDistance, layer))
        {
            curretTime += Time.fixedDeltaTime;
            if (curretTime >= maxDelay)
            {
                rigidbody2D.AddTorque(rotatoinForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
            }
        }
        else if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.left), rayDistance, layer))
        {
            curretTime += Time.fixedDeltaTime;
            if (curretTime >= maxDelay)
            {
                rigidbody2D.AddTorque(-rotatoinForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
            }
        }
        else
        {
            curretTime = 0;
        }
    }

    public void Jump()
    {
        curretTime = 0;
        if (isGround)
        {
            rigidbody2D.AddForce(transform.up * force, ForceMode2D.Impulse);

            rigidbody2D.AddTorque(jumpRotatoinForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
    }

    void StartHandRot()
    {
        curretHandRotationSpeed += handRotationSpeed * Time.fixedDeltaTime;
        hand.Rotate(Vector3.forward * curretHandRotationSpeed * Time.fixedDeltaTime);
        if(curretHandRotationSpeed * Time.fixedDeltaTime >= maxHandRotationSpeedForTrail)
        {
            trail.SetActive(true);
        }
        curreFlyTime += Time.fixedDeltaTime;
        if (curreFlyTime >= maxTimeForFly)
        {
                rigidbody2D.AddForce(transform.up * flyForce, ForceMode2D.Force);
        }
    }

    void StopHandRot()
    {
        trail.SetActive(false);
        curreFlyTime = 0;
        if (hand.localRotation.z < -0.02f | hand.localRotation.z > 0.02f)
        {
            curretHandRotationSpeed -= handRotationSpeed * Time.fixedDeltaTime;
            hand.Rotate(Vector3.forward * curretHandRotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            curretHandRotationSpeed = 0;
            hand.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void ShootDown()
    {
        curretHandRotationSpeed = 0;
        startHandRot = true;
    }

    public void ShootUp()
    {
        Shoot();
        curretHandRotationSpeed = 0;
        startHandRot = false;
    }

    void Shoot()
    {
        if (!visualShell.activeSelf) return;
        visualShell.SetActive(false);
        GameObject go = Instantiate<GameObject>(shellPrefab, shellSpawnPoint.position, shellSpawnPoint.rotation);
        go.gameObject.GetComponent<Rigidbody2D>().AddForce(-go.transform.up * shellForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            foreach (ContactPoint2D item in collision.contacts)
            {
                Vector2 hitPoint = item.point;
                if (rigidbody2D.velocity.magnitude >= maxSpeedForSpawnParticle)
                {
                    Instantiate(dropParticlePredab, hitPoint, Quaternion.identity);
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            isGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isGround = false;
        }
    }

    private void AI()
    {
        if(Random.Range(0, 100) == 1)
        {
            Jump();
            return;
        }
        if(curretHandUpDelay < handUpDelay)
            curretHandUpDelay += Time.fixedDeltaTime;

        Vector2 direction = target.transform.position - head.position;
        if (Physics2D.Raycast(head.position, direction, lookDistance, AILayer))
        {
            if (curretHandUpDelay >= handUpDelay)
            {
                if (!Physics2D.Raycast(hand.position, hand.TransformDirection(Vector3.down), 100, AILayer))
                {
                    /*
                    RaycastHit2D hit2D = Physics2D.Raycast(hand.position, hand.TransformDirection(Vector3.down), 100, AILayer);
                    if(hit2D.collider.name != "Head_0")
                    {
                        ShootDown();
                    }
                    else
                    {
                        ShootUp();
                    }
                    */
                    startHandRot = true;
                }
                else
                {
                    RaycastHit2D hit2D = Physics2D.Raycast(hand.position, hand.TransformDirection(Vector3.down), 100, AILayer);
                    if (hit2D.collider.name == "Head_0")
                    {
                        Shoot();
                        startHandRot = false;
                        curretHandRotationSpeed = 0;
                        curretHandUpDelay = 0;
                    }
                }
            }
        }
        else
        {
            startHandRot = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * rayDistance);
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * rayDistance);
        Gizmos.DrawRay(hand.position, hand.TransformDirection(Vector3.down) * 10);

        Vector2 direction = GameObject.Find("Head_0").transform.position - head.position;
        Gizmos.DrawRay(head.position, direction * lookDistance);
    }
}
