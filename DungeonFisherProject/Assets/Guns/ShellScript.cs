using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellScript : MonoBehaviour
{
    [Header("For Standart Sell (type = 0)")]
    public int speed;
    public int Type;
    public int Damage;
    public GameObject[] Monsters;
    public bool SprayCan;
    private float TimeToSprayDestroy;
    public GameObject Boss;
    [Header("For Granade (type = 1)")]
    public float SetTime;
    private float TimeS;
    public GameObject Boom;
    public float BoomRadus;
    [Header("For Energi (type = 2)")]
    public float DistanceToTarget;
    public GameObject TargetMonster;
    public Sprite[] SpritesEnergi;
    private float TimeToNextLight;
    public Vector2[] LevelEndMaxPixels;
    private int SetLevel;
    private Vector2 currentDirection = new Vector3(0, 1, 0);
    // type 3 - crossbow bolt And arrow
    private bool Active = true;
    private GameObject AttachedObject;
    private Vector2 RelativePosition;
    private bool Flip;
    private float TimeToDestroi = 2;
    [Header("For LaserGun (type = 4)")]
    public GameObject Laser;
    public Transform LaserGun;
    public float LaserFactor;
    public bool Diminish;
    void Start()
    {
        if (SprayCan == true) { TimeToSprayDestroy = Random.Range(5f,8f); }
        if (Type == 1)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * -speed);
        }
        if (Type == 2) { TimeToNextLight = 0.1f; gameObject.GetComponent<SpriteRenderer>().sprite = SpritesEnergi[(Random.Range(1, 5) * (SetLevel + 1)) - 1]; }
    }
    void Update()
    {
        if (Type == 4)
        {
            Laser.transform.rotation = gameObject.transform.rotation;
            Laser.transform.position = Vector2.Lerp(gameObject.transform.position, LaserGun.position, 0.5f);
            Laser.transform.localScale = new Vector3(Vector2.Distance(gameObject.transform.position, LaserGun.transform.position) * LaserFactor, 1, 1);
        }
    }
    void FixedUpdate()
    {
        if (Type == 0)
        {
            if(SprayCan == true )
            {
                gameObject.GetComponent<Rigidbody2D>().drag += Time.deltaTime * 200;
                TimeToSprayDestroy -= Time.deltaTime;
                if (TimeToSprayDestroy <= 0)
                {
                    if (gameObject.GetComponent<SpriteRenderer>().color.a > 0.01f)
                    {
                        gameObject.GetComponent<SpriteRenderer>().color = new Color(gameObject.GetComponent<SpriteRenderer>().color.r, gameObject.GetComponent<SpriteRenderer>().color.g, gameObject.GetComponent<SpriteRenderer>().color.b, gameObject.GetComponent<SpriteRenderer>().color.a - Time.deltaTime);
                    }
                    else { Destroy(gameObject); }
                }
            }

            gameObject.GetComponent<Rigidbody2D>().velocity = transform.right * -speed;
        }
        if (Type == 4)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = transform.right * -speed;
            

        }
        if (Type == 1)
        {
            if (TimeS < SetTime) { TimeS += Time.deltaTime; }
            else { Destroy(gameObject); }
            if (TimeS > SetTime - 0.25f && Boom.activeSelf == false)
            {
                Boom.SetActive(true);
                GameObject Player = GameObject.FindWithTag("Player");
                if (BoomRadus > Vector2.Distance(transform.position, Player.transform.position))
                {
                    Vector2 PointReclining = new Vector2(-(gameObject.transform.position.x - Player.transform.position.x), -(gameObject.transform.position.y - Player.transform.position.y));
                    Player.GetComponent<Rigidbody2D>().AddForce(PointReclining * 1000f * (BoomRadus - Vector2.Distance(transform.position,Player.transform.position))  );
                    Player.GetComponent<RangerScript>().ShakeCameraAmplitude = (BoomRadus - Vector2.Distance(transform.position, Player.transform.position)) * 10;
                    Player.GetComponent<RangerScript>().SpeedShakeCam = (BoomRadus - Vector2.Distance(transform.position, Player.transform.position)) * 10;
                    Player.GetComponent<RangerScript>().CameraShakeTime = 0.4f;
                    if (Vector2.Distance(transform.position,Player.transform.position) <= BoomRadus / 1.5f) { Player.GetComponent<RangerScript>().HP -= 1; }
                }
                for (int i = 0; i < Monsters.Length; i++)
                {
                    if (Monsters[i] != null && BoomRadus > Vector2.Distance(transform.position, Monsters[i].transform.position))
                    {
                        Vector2 PointReclining = new Vector2(-(gameObject.transform.position.x - Monsters[i].transform.position.x), -(gameObject.transform.position.y - Monsters[i].transform.position.y));
                        Monsters[i].GetComponent<Rigidbody2D>().AddForce(PointReclining * 2000f * (BoomRadus - Vector2.Distance(transform.position, Monsters[i].transform.position)));
                        Monsters[i].GetComponent<ZombieFish>().HP -= Damage;
                    }
                }
                GameObject[] Shells = GameObject.FindGameObjectsWithTag("Shell");
                for (int i = 0; i < Shells.Length; i++)
                {
                    if (BoomRadus > Vector2.Distance(transform.position, Shells[i].transform.position))
                    {
                        Vector2 PointReclining = new Vector2(-(gameObject.transform.position.x - Shells[i].transform.position.x), -(gameObject.transform.position.y - Shells[i].transform.position.y));
                        Shells[i].GetComponent<Rigidbody2D>().AddForce(PointReclining * 2000f * (BoomRadus - Vector2.Distance(transform.position, Shells[i].transform.position)));
                    }
                }
                GameObject[] MonsterShells = GameObject.FindGameObjectsWithTag("MonsterShell");
                for (int i = 0; i < MonsterShells.Length; i++)
                {
                    if (BoomRadus > Vector2.Distance(transform.position, MonsterShells[i].transform.position))
                    {
                        Vector2 PointReclining = new Vector2(-(gameObject.transform.position.x - MonsterShells[i].transform.position.x), -(gameObject.transform.position.y - MonsterShells[i].transform.position.y));
                        MonsterShells[i].GetComponent<Rigidbody2D>().AddForce(PointReclining * 2000f * (BoomRadus - Vector2.Distance(transform.position, MonsterShells[i].transform.position)));
                    }
                }
            }
        }
        if (Type == 2)
        {
            if (TargetMonster != null)
            {
                Vector2 direction = TargetMonster.transform.position - transform.position;
                direction.Normalize();

                currentDirection = Vector2.Lerp(currentDirection, direction, Time.deltaTime * 300);
               transform.right = -currentDirection;
            }
            TimeToNextLight -= Time.deltaTime;
            if (TimeToNextLight <= 0)
            {
                if (DistanceToTarget*100 > LevelEndMaxPixels[SetLevel].x)
                {
                    SetLevel += 1;
                    gameObject.GetComponent<SpriteRenderer>().sprite = SpritesEnergi[(Random.Range(1, 5) * (SetLevel + 1)) - 1];
                    TimeToNextLight = 0.1f;
                }
                else
                {
                    Destroy(gameObject);
                    if (TargetMonster != null)
                    {
                        TargetMonster.GetComponent<ZombieFish>().HP -= 30;
                        TargetMonster.GetComponent<ZombieFish>().RecreationTime = TargetMonster.GetComponent<ZombieFish>().RecomendetRecreationTime;
                    }
                }
            }
        }
        if (Type == 3 )
        {
            if (Active == false)
            {
                TimeToDestroi -= Time.deltaTime;
                gameObject.GetComponent<SpriteRenderer>().color = new Color(gameObject.gameObject.GetComponent<SpriteRenderer>().color.r, gameObject.GetComponent<SpriteRenderer>().color.g, gameObject.GetComponent<SpriteRenderer>().color.b, TimeToDestroi);
                if (TimeToDestroi <= 0) { Destroy(gameObject); }
                if (AttachedObject != null)
                {
                    if (Flip != AttachedObject.GetComponent<SpriteRenderer>().flipX)
                    {
                        Flip = AttachedObject.GetComponent<SpriteRenderer>().flipX;
                        RelativePosition.x = -RelativePosition.x;
                        gameObject.GetComponent<SpriteRenderer>().flipX = !gameObject.GetComponent<SpriteRenderer>().flipX;
                    }
                    gameObject.transform.position = new Vector2(AttachedObject.transform.position.x, AttachedObject.transform.position.y) + RelativePosition;
                }
                else { transform.position = RelativePosition; }
            }
            else
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = transform.right * -speed;
            }
        }
    }
    public void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "wall" && Type == 0) { Destroy(gameObject); }
        if (collider.gameObject.tag == "wall" && Type == 4) { Destroy(gameObject);Destroy(Laser); }
        if (collider.gameObject.tag == "wall" && Type == 3) { Active = false; gameObject.GetComponent<BoxCollider2D>().enabled = false; RelativePosition = gameObject.transform.position;gameObject.GetComponent<Rigidbody2D>().drag = 1000; }
        if (collider.gameObject.tag == "Monster" && Type == 0)
        {
           GameObject[] NotDeadMonster = new GameObject[Monsters.Length];
           for(int i = 0; i < Monsters.Length; i++)
           {
              if(Monsters[i] != null && Monsters[i].GetComponent<ZombieFish>().HP > 0) { NotDeadMonster[i] = Monsters[i]; }
           }
           GameObject TargetMonster = NotDeadMonster[0];
           for (int i = 0; i < NotDeadMonster.Length; i++)
           {
              if (TargetMonster == null) { TargetMonster = NotDeadMonster[i]; }
              if (NotDeadMonster[i] != null)
              {
                  if (Vector2.Distance(NotDeadMonster[i].transform.position,gameObject.transform.position) < Vector2.Distance(TargetMonster.transform.position,gameObject.transform.position))
                  {
                      TargetMonster = NotDeadMonster[i];
                  }
              }
           }
           
           TargetMonster.GetComponent<ZombieFish>().HP -= Damage;
           TargetMonster.GetComponent<SpriteRenderer>().material.SetFloat("_Light", 1.5f);
           Destroy(gameObject);
        }
        if (collider.gameObject.tag == "Monster" && Type == 4)
        {
            GameObject[] NotDeadMonster = new GameObject[Monsters.Length];
            for (int i = 0; i < Monsters.Length; i++)
            {
                if (Monsters[i] != null && Monsters[i].GetComponent<ZombieFish>().HP > 0) { NotDeadMonster[i] = Monsters[i]; }
            }
            GameObject TargetMonster = NotDeadMonster[0];
            for (int i = 0; i < NotDeadMonster.Length; i++)
            {
                if (TargetMonster == null) { TargetMonster = NotDeadMonster[i]; }
                if (NotDeadMonster[i] != null)
                {
                    if (Vector2.Distance(NotDeadMonster[i].transform.position, gameObject.transform.position) < Vector2.Distance(TargetMonster.transform.position, gameObject.transform.position))
                    {
                        TargetMonster = NotDeadMonster[i];
                    }
                }
            }
            if (Diminish == true)
            {
                if (TargetMonster.GetComponent<ZombieFish>().Diminish == false)
                {
                    TargetMonster.GetComponent<ZombieFish>().TargetScale = 0.2f;
                    TargetMonster.GetComponent<ZombieFish>().HP = Mathf.RoundToInt(TargetMonster.GetComponent<ZombieFish>().HP / 5);
                    TargetMonster.GetComponent<ZombieFish>().Speed = Mathf.RoundToInt(TargetMonster.GetComponent<ZombieFish>().Speed / 5);
                    TargetMonster.GetComponent<ZombieFish>().Diminish = true;
                    if (TargetMonster.GetComponent<Rigidbody2D>() != null) { TargetMonster.GetComponent<Rigidbody2D>().mass = TargetMonster.GetComponent<Rigidbody2D>().mass / 5; }
                    if (TargetMonster.GetComponent<ZombieFish>().MonsterType == 2) { TargetMonster.GetComponent<ZombieFish>().NumberOfShots = Mathf.RoundToInt(TargetMonster.GetComponent<ZombieFish>().NumberOfShots / 4); }
                    TargetMonster.GetComponent<ZombieFish>().RecomendetRecreationTime = TargetMonster.GetComponent<ZombieFish>().RecomendetRecreationTime * 2;
                }
                else
                {
                    TargetMonster.GetComponent<ZombieFish>().TargetScale = 0;
                    TargetMonster.GetComponent<ZombieFish>().HP = 0;
                }
            }
            
            TargetMonster.GetComponent<ZombieFish>().HP -= Damage;
            Destroy(gameObject);
            Destroy(Laser);
        }
        if (collider.gameObject.tag == "Monster" && Type == 3)
        {
            GameObject[] NotDeadMonster = new GameObject[Monsters.Length];
            for (int i = 0; i < Monsters.Length; i++)
            {
                if (Monsters[i] != null && Monsters[i].GetComponent<ZombieFish>().HP > 0) { NotDeadMonster[i] = Monsters[i]; }
            }
            GameObject TargetMonster = NotDeadMonster[0];
            for (int i = 0; i < NotDeadMonster.Length; i++)
            {
                if (TargetMonster == null) { TargetMonster = NotDeadMonster[i]; }
                if (NotDeadMonster[i] != null)
                {
                    if (Vector2.Distance(NotDeadMonster[i].transform.position, gameObject.transform.position) < Vector2.Distance(TargetMonster.transform.position, gameObject.transform.position))
                    {
                        TargetMonster = NotDeadMonster[i];
                    }
                }
            }
            
            TargetMonster.GetComponent<ZombieFish>().HP -= Damage;
            Active = false;
            AttachedObject = TargetMonster;
            RelativePosition = gameObject.transform.position - TargetMonster.transform.position ;
            Flip = TargetMonster.GetComponent<SpriteRenderer>().flipX;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().drag = 1000;
        }
        if (collider.gameObject.tag == "Monster" && Type == 1) { TimeS = SetTime - 0.5f; }
        if (collider.gameObject.tag == "Boss" && Type == 0)
        {
            if (Boss.GetComponent<Sudak>() != null)
            {
                Boss.GetComponent<Sudak>().HP -= Damage;
                Boss.GetComponent<SpriteRenderer>().material.SetFloat("_Light", 1.25f);
                Destroy(gameObject);
            }
        }
        if (collider.gameObject.tag == "Boss" && Type == 1) { TimeS = SetTime - 0.5f; }
        if (collider.gameObject.tag == "Boss" && Type == 3)
        {
            Boss.GetComponent<Sudak>().HP -= Damage;
            Active = false;
            AttachedObject = Boss;
            RelativePosition = gameObject.transform.position - Boss.transform.position;
            Flip = Boss.GetComponent<SpriteRenderer>().flipX;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().drag = 1000;
        }
    }
}
