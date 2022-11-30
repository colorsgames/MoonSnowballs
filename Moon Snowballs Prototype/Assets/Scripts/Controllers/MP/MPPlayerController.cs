using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MPPlayerController : NetworkBehaviour
{
    [SerializeField] private PlayerMod mod = PlayerMod.Player;
    [SerializeField] private ClientsInfo clientInfo;

    [SerializeField] private LayerMask layer;
    [SerializeField] private LayerMask AILayer;

    [SerializeField] private GameObject dropParticlePrefab;
    [SerializeField] private GameObject trail;
    [SerializeField] private GameObject visualShell;
    [SerializeField] private Transform hand;
    [SerializeField] private Transform shellSpawnPoint;
    [SerializeField] private Transform head;
    [SerializeField] private HealthController healthController;

    [SerializeField] private float maxTimeForFly;
    [SerializeField] private float maxSpeedForSpawnParticle;
    [SerializeField] private float maxHandRotationSpeedForTrail;
    [SerializeField] private float force;
    [SerializeField] private float jumpRotatoinForce;
    [SerializeField] private float rotatoinForce;
    [SerializeField] private float rayDistance;
    [SerializeField] private float maxDelay;
    [SerializeField] private float handRotationSpeed;
    [SerializeField] private float shellForce;
    [SerializeField] private float shootDelay;
    [SerializeField] private float flyForce;
    [Header("AI options")]
    [SerializeField] private float handUpDelay;
    [SerializeField] private float lookDistance;
    [SerializeField] private float attackLookDistance;

    PlayerTeam playerTeam;

    GameObject target;
    GameObject shellPrefab;

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
        CameraController.instance.AddPlayer(transform);
        playerTeam = GetComponent<PlayerTeam>();


        if (isLocalPlayer)
        {
            if (isServer) playerTeam.team = Team.Blue;
            else playerTeam.team = Team.Red;
        }
        else
        {
            if (isServer) playerTeam.team = Team.Red;
            else playerTeam.team = Team.Blue;
        }

        playerTeam.SetSprites();

        shellPrefab = playerTeam.shellPref;

        rigidbody2D = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Head_0");

        if (playerTeam.team == Team.Red)
        {
            jumpRotatoinForce *= -1;
        }
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
                if (!GameManager.restartGame)
                    AI();
                else
                {
                    startHandRot = false;
                }
            }

            Equilibrium();
        }
    }

    private void Update()
    {
        if (mod == PlayerMod.AI) return;
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.A)) Jump();
            if (Input.GetKeyDown(KeyCode.D)) ShootDown();
            if (Input.GetKeyUp(KeyCode.D)) ShootUp();
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
        if (curretHandRotationSpeed * Time.fixedDeltaTime >= maxHandRotationSpeedForTrail)
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
        if (isServer)
            SpawnBullet();
        else
            CmdSpanwBullet();
    }

    [Server]
    public void SpawnBullet()
    {
        GameObject go = Instantiate<GameObject>(shellPrefab, shellSpawnPoint.position, shellSpawnPoint.rotation);
        NetworkServer.Spawn(go);
        go.gameObject.GetComponent<Rigidbody2D>().AddForce(-go.transform.up * shellForce, ForceMode2D.Impulse);
    }

    [Command]
    public void CmdSpanwBullet()
    {
        SpawnBullet();
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
                    Instantiate(dropParticlePrefab, hitPoint, Quaternion.identity);
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
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
        if (Random.Range(0, 100) == 1)
        {
            Jump();
            return;
        }
        if (curretHandUpDelay < handUpDelay)
            curretHandUpDelay += Time.fixedDeltaTime;

        Vector2 direction = target.transform.position - head.position;
        direction.Normalize();
        if (Physics2D.Raycast(head.position, direction, lookDistance, AILayer))
        {
            if (curretHandUpDelay >= handUpDelay)
            {
                if (!Physics2D.Raycast(hand.position, hand.TransformDirection(Vector3.down), attackLookDistance, AILayer))
                {
                    startHandRot = true;
                }
                else
                {
                    RaycastHit2D hit2D = Physics2D.Raycast(hand.position, hand.TransformDirection(Vector3.down), attackLookDistance, AILayer);
                    if (hit2D.collider.GetComponent<HealthController>())
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

    /*private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * rayDistance);
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * rayDistance);
        Gizmos.DrawRay(hand.position, hand.TransformDirection(Vector3.down) * attackLookDistance);

        Vector2 direction = GameObject.Find("Head_0").transform.position - head.position;
        Gizmos.DrawRay(head.position, direction.normalized * lookDistance);
    }*/

}
