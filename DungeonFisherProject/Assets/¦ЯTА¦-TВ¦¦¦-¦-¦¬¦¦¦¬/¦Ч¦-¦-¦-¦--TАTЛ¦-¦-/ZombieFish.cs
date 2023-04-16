using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFish : MonoBehaviour
{
    private int StartHP;
    public int MonsterType;
    [Header("For All")]
    public PolygonCollider2D[] Colliders;
    public GameObject Player;
    public float Speed;
    public int HP;
    public float RecreationTime;
    public float RecomendetRecreationTime = 2; // for perc atack
    private bool jerk;
    private float PlayerRepulsionTime;
    private float ExtinctionTime = 10;
    public float TargetScale = 1;
    public bool Diminish;
    
    [Header("For Shooter (type = 1)")]
    public GameObject Gun;
    private Vector2 currentDirection = new Vector3(0, 1, 0);
    public Transform SpawnShell;
    public GameObject MonsterShell;
    public Vector2[] FallingGunPositions;
    private byte FallingGunNumber;
    [Header("For Ktulhik (type = 2)")]
    public int NumberOfShots;
    public GameObject Room;
    [Header("For hvatalka (type = 3)")]
    public List<GameObject> TentaclesNotR;
    public GameObject[] Tentacles;
    public SpriteRenderer[] TentaclesSprite;
    private float TentaclesTime =2;
    private int SetTentacleNumber;

// Start is called before the first frame update
    void Start()
    {
        if (MonsterType ==3)
        {
            for (int i = 0; i < Tentacles.Length; i++)
            {
                int R = Random.Range(0, TentaclesNotR.Count);
                Tentacles[i] = TentaclesNotR[R];TentaclesNotR.RemoveAt(R);
            }
            RecomendetRecreationTime = Random.Range(RecomendetRecreationTime - 1.5f, RecomendetRecreationTime + 1.5f);
        }
        Room.GetComponent<RoomScript>().Monsters.Add(gameObject);
        Player = GameObject.FindWithTag("Player");
        int perk = Random.Range(0,20);
        if (MonsterType == 1) { RecomendetRecreationTime = 3; }
        if (MonsterType == 2) { RecomendetRecreationTime = 5; }
        if (perk == 0) // nievidimka
        {
            Speed = Speed / 2;
            HP = HP / 2;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.1f);
            if (MonsterType == 3) {for (int i = 0; i < TentaclesSprite.Length; i++) { TentaclesSprite[i].color = new Color(1, 1, 1, 0.1f); } }
        }
        if (perk == 1) // atack perk
        {
            RecomendetRecreationTime = 0;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.8f, 0.8f, 1);
            if (MonsterType == 1) {RecomendetRecreationTime = 0.5f; }
            if (MonsterType == 2) { RecomendetRecreationTime = 4; NumberOfShots = NumberOfShots * 2; }
            if (MonsterType == 3) { RecomendetRecreationTime = 3; for (int i = 0; i < TentaclesSprite.Length; i++) { TentaclesSprite[i].color = new Color(1, 0.8f, 0.8f, 1); } }
        }
        if (perk == 2) // speed perk
        {
            Speed = Speed * 2;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.6f, 1, 1);
            if (MonsterType == 2) { gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0.8f, 1, 1); }
            if (MonsterType == 3) { perk = 3; }
        }
        if (perk == 3) // HP perk
        {
            HP = HP * 2;
            gameObject.GetComponent<Rigidbody2D>().mass = gameObject.GetComponent<Rigidbody2D>().mass * 8;
            Speed = Speed * 8;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.8f, 1, 0.8f, 1);
            if (MonsterType == 3) { for (int i = 0; i < TentaclesSprite.Length; i++) { TentaclesSprite[i].color = new Color(0.8f, 1, 0.8f, 1); } }
        }
        if (MonsterType == 1) { Gun.GetComponent<SpriteRenderer>().color = gameObject.GetComponent<SpriteRenderer>().color; }
        if (MonsterType == 1 || MonsterType == 2 || MonsterType == 3) { RecreationTime = Random.Range(0, RecomendetRecreationTime);}
        StartHP = HP;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameObject.GetComponent<SpriteRenderer>().material.GetFloat("_Light") > 1) { gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_Light", Mathf.Lerp(gameObject.GetComponent<SpriteRenderer>().material.GetFloat("_Light"), 1, 0.3f)); }
        if (MonsterType == 3) {for (int i = 0; i < TentaclesSprite.Length; i++) { TentaclesSprite[i].material.SetFloat("_Light", gameObject.GetComponent<SpriteRenderer>().material.GetFloat("_Light")); } }
        if (transform.localScale.x != TargetScale) { transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, TargetScale,0.5f), transform.localScale.x, 1);}
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(Mathf.RoundToInt(transform.position.y * 7.68f)) - 200;
        if (MonsterType == 1) { Gun.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;}
        if (Room.GetComponent<RoomScript>().Batle == true || Room.GetComponent<RoomScript>().Monsters.Count == 0)
        {
            if (PlayerRepulsionTime > 0)
            {
                PlayerRepulsionTime -= Time.deltaTime;
                Vector2 PointReclining = new Vector2(-(gameObject.transform.position.x - Player.transform.position.x), -(gameObject.transform.position.y - Player.transform.position.y));
                Player.GetComponent<Rigidbody2D>().AddForce(PointReclining * 50f);
            }
            if (HP > 0)
            {
                
                if (gameObject.GetComponent<SpriteRenderer>().flipX == false) { Colliders[0].enabled = false; Colliders[1].enabled = true; }
                else { Colliders[0].enabled = true; Colliders[1].enabled = false; }//колафдеры(тупые) чё их не получается просто редактировать?
                if (MonsterType == 0 || MonsterType == 2)//паваротики
                {
                    if (Player.transform.position.x > gameObject.transform.position.x) { gameObject.GetComponent<SpriteRenderer>().flipX = true; }
                    else { gameObject.GetComponent<SpriteRenderer>().flipX = false; }
                }
                if (MonsterType == 1)
                {
                    if (Player.transform.position.x > gameObject.transform.position.x) { gameObject.GetComponent<SpriteRenderer>().flipX = false; Gun.GetComponent<SpriteRenderer>().flipY = false; }
                    else { gameObject.GetComponent<SpriteRenderer>().flipX = true; Gun.GetComponent<SpriteRenderer>().flipY = true; }

                    Vector2 direction = Player.transform.position - Gun.transform.position;
                    direction.Normalize();

                    currentDirection = Vector2.Lerp(currentDirection, direction, Time.deltaTime * 300);
                    Gun.transform.right = currentDirection;

                    if (Vector2.Distance(gameObject.transform.position, Player.transform.position) > 1f) //стрелок может ходить пока перезаряжается
                    {
                        Vector2 dir = (new Vector2(Player.transform.position.x, Player.transform.position.y) - new Vector2(transform.position.x, transform.position.y)).normalized * 2 * Time.deltaTime;
                        gameObject.GetComponent<Rigidbody2D>().AddForce(dir * Speed);
                        Gun.GetComponent<Animator>().SetInteger("Anim", 0);
                        gameObject.GetComponent<Animator>().SetInteger("Anim", 0);
                        gameObject.GetComponent<Animator>().SetFloat("Speed", Speed / 400);
                    }
                    if (Vector2.Distance(gameObject.transform.position, Player.transform.position) < 0.45f)
                    {
                        Vector2 PointReclining = -(new Vector2(Player.transform.position.x, Player.transform.position.y) - new Vector2(transform.position.x, transform.position.y)).normalized * 2 * Time.deltaTime;
                        gameObject.GetComponent<Rigidbody2D>().AddForce(PointReclining * Speed);
                        gameObject.GetComponent<Animator>().SetInteger("Anim", 0);
                        gameObject.GetComponent<Animator>().SetFloat("Speed", -(Speed / 400));
                        Gun.GetComponent<Animator>().SetInteger("Anim", 0);
                    }
                    if (Vector2.Distance(gameObject.transform.position, Player.transform.position) < 1f && Vector2.Distance(gameObject.transform.position, Player.transform.position) > 0.45f) { gameObject.GetComponent<Animator>().SetInteger("Anim", 2); }
                }
                if (MonsterType == 3)
                {
                    for (int i = 0; i < Tentacles.Length; i++)
                    {
                        Tentacles[i].transform.localPosition = new Vector3(0, -0.045f, 0); 
                    }
                    if(Vector2.Distance(Player.transform.position,transform.position) < 0.25f)
                    {
                        gameObject.GetComponent<Animator>().SetInteger("Anim", 1);
                    } 
                }
                if (RecreationTime <= 0)
                {
                    if (MonsterType == 0)// если это обычный зомба-рыба
                    {
                        if (Vector2.Distance(gameObject.transform.position, Player.transform.position) > 0.35f)
                        {
                            Vector2 dir = (new Vector2(Player.transform.position.x, Player.transform.position.y) - new Vector2(transform.position.x, transform.position.y)).normalized * 2 * Time.deltaTime;
                            gameObject.GetComponent<Rigidbody2D>().AddForce(dir * Speed);
                            gameObject.GetComponent<Animator>().SetInteger("Anim", 0);
                        }
                        else
                        {
                            gameObject.GetComponent<Animator>().SetInteger("Anim", 1);
                        }
                    }
                    if (MonsterType == 1)// если это зомба-стрелок
                    {
                        if (Vector2.Distance(gameObject.transform.position, Player.transform.position) < 1f)
                        {
                            Gun.GetComponent<Animator>().SetInteger("Anim", 1);
                            gameObject.GetComponent<Animator>().SetInteger("Anim", 1);
                        }
                    }
                    if (MonsterType == 2)
                    {
                        if (Vector2.Distance(gameObject.transform.position, Player.transform.position) > 1f)
                        {
                            Vector2 dir = (new Vector2(Player.transform.position.x, Player.transform.position.y) - new Vector2(transform.position.x, transform.position.y)).normalized * 2 * Time.deltaTime;
                            gameObject.GetComponent<Rigidbody2D>().AddForce(dir * Speed);
                            gameObject.GetComponent<Animator>().SetInteger("Anim", 0);
                        }
                        if (Vector2.Distance(gameObject.transform.position, Player.transform.position) < 0.45f)
                        {
                            Vector2 PointReclining = new Vector2(-(Player.transform.position.x - gameObject.transform.position.x), -(Player.transform.position.y - gameObject.transform.position.y)).normalized * 2 * Time.deltaTime;
                            gameObject.GetComponent<Rigidbody2D>().AddForce(PointReclining * Speed);
                            gameObject.GetComponent<Animator>().SetInteger("Anim", 0);
                        }
                        if (Vector2.Distance(gameObject.transform.position, Player.transform.position) < 1f && Vector2.Distance(gameObject.transform.position, Player.transform.position) > 0.45f) { gameObject.GetComponent<Animator>().SetInteger("Anim", 2); }

                        if (Vector2.Distance(gameObject.transform.position, Player.transform.position) < 1f)
                        {
                            gameObject.GetComponent<Animator>().SetInteger("Anim", 1);
                        }
                    }
                    if (MonsterType == 3)
                    {
                        int ExcludedTentacleNum = 400;
                        for (int i = 0; i < Tentacles.Length; i++)
                        {
                            if (Tentacles[i].GetComponent<MonsterShell>().ItsPlayer == true)
                            {
                                Tentacles[i].transform.localScale = new Vector3(Tentacles[i].transform.localScale.x - Time.deltaTime * ((Tentacles[i].transform.localScale.x + 0.1f) * 5), 1, 1);
                                if (Tentacles[i].transform.localScale.x < 0) { new Vector3(0, 1, 1); }
                                ExcludedTentacleNum = i;
                            }
                        }

                        if (TentaclesTime > 0)
                        {
                            if (SetTentacleNumber < Tentacles.Length)
                            {
                                if (Tentacles[SetTentacleNumber].GetComponent<MonsterShell>().Stop == false && SetTentacleNumber != ExcludedTentacleNum)
                                {
                                    Tentacles[SetTentacleNumber].transform.localScale = new Vector3(Tentacles[SetTentacleNumber].transform.localScale.x + Time.deltaTime * ((Tentacles[SetTentacleNumber].transform.localScale.x +0.1f) * 20),1,1);
                                }
                                else { SetTentacleNumber += 1; }
                            }
                            else { TentaclesTime -= Time.deltaTime; }
                        }
                        else
                        {
                            if (SetTentacleNumber > Tentacles.Length - 1) { SetTentacleNumber = Tentacles.Length - 1; }
                            if (SetTentacleNumber >= 0)
                            {
                                if (Tentacles[SetTentacleNumber].transform.localScale.x > 0 && SetTentacleNumber != ExcludedTentacleNum)
                                {
                                    Tentacles[SetTentacleNumber].transform.localScale = new Vector3(Tentacles[SetTentacleNumber].transform.localScale.x - Time.deltaTime * ((Tentacles[SetTentacleNumber].transform.localScale.x + 0.1f) * 20), 1, 1);
                                    if (Tentacles[SetTentacleNumber].transform.localScale.x < 0) { new Vector3(0, 1, 1); }
                                }
                                else { SetTentacleNumber -= 1; }
                            }
                            else { SetTentacleNumber = 0; RecreationTime = RecomendetRecreationTime; TentaclesTime = 2; }
                            
                        }
                    }
                }
                else
                {
                    RecreationTime -= Time.deltaTime;
                    if (MonsterType == 0 || MonsterType == 2) { gameObject.GetComponent<Animator>().SetInteger("Anim", 2); }
                    //if (MonsterType == 1) { Gun.GetComponent<Animator>().SetInteger("Anim", 0); gameObject.GetComponent<Animator>().SetInteger("Anim", 0); }

                }
                if (jerk == true)
                {
                    Vector2 dir = (new Vector2(Player.transform.position.x, Player.transform.position.y) - new Vector2(transform.position.x, transform.position.y)).normalized * 2 * Time.deltaTime;
                    gameObject.GetComponent<Rigidbody2D>().AddForce(dir * Speed * 20);
                }
            }
            else //dead
            {
                gameObject.GetComponent<Animator>().SetInteger("Anim", 3);
                if (ExtinctionTime > 0 && ExtinctionTime < 10)
                {
                    ExtinctionTime -= Time.deltaTime;
                    if (ExtinctionTime / 5 < gameObject.GetComponent<SpriteRenderer>().color.a)
                    {
                        gameObject.GetComponent<SpriteRenderer>().color = new Color(gameObject.GetComponent<SpriteRenderer>().color.r, gameObject.GetComponent<SpriteRenderer>().color.g, gameObject.GetComponent<SpriteRenderer>().color.b, ExtinctionTime / 5);
                    }
                    if (MonsterType == 1) { Gun.GetComponent<SpriteRenderer>().color = gameObject.GetComponent<SpriteRenderer>().color; }
                    if (MonsterType == 3)
                    {
                        for (int i = 0; i < TentaclesSprite.Length; i++)
                        {
                            TentaclesSprite[i].color = gameObject.GetComponent<SpriteRenderer>().color;
                        }
                    }
                }

            }
        }
        else { if (Diminish == false) { HP = StartHP; } }
    }
    public void JercActive() { jerk = true; }
    public void Atack()
    {
        if (MonsterType == 0)
        {
            jerk = false;
            if (Vector2.Distance(gameObject.transform.position, Player.transform.position) < 0.3f)
            {
                int R = Random.Range(1, 5);
                if (R > Player.GetComponent<RangerScript>().Armor && Diminish == false)
                {
                    Player.GetComponent<RangerScript>().HP -= 1;
                }
                PlayerRepulsionTime = 0.2f;
                Player.GetComponent<RangerScript>().CameraShakeTime = 0.4f; Player.GetComponent<RangerScript>().SpeedShakeCam = 1.4f; Player.GetComponent<RangerScript>().ShakeCameraAmplitude = 1;
            }
            RecreationTime = RecomendetRecreationTime;
        }
        if (MonsterType == 1)
        {
            var rotationY = Quaternion.AngleAxis(Random.Range(-15, 15), transform.forward);
            var rotationX = Quaternion.AngleAxis(Random.Range(-15, 15), transform.right);
            MonsterShell.GetComponent<MonsterShell>().Player = Player;
            Instantiate(MonsterShell, SpawnShell.position, Gun.transform.rotation * rotationX * rotationY);
            RecreationTime = RecomendetRecreationTime;
            Gun.GetComponent<Animator>().SetInteger("Anim", 0);
        }
        if (MonsterType == 2)
        {
            if (gameObject.GetComponent<SpriteRenderer>().flipX == true) { SpawnShell.transform.localPosition = new Vector2(0.04f, 0); }
            else { SpawnShell.transform.localPosition = new Vector2(-0.04f, 0); }
            MonsterShell.GetComponent<MonsterShell>().Player = Player;
            int QuantityShots = Random.Range(NumberOfShots - 1, NumberOfShots + 2);//в максимум добавляет 2 потому что int в Random.Range
            float DegreeOfRotation = 360 / QuantityShots;
            for (int i=1; i <= QuantityShots; i++)
            {
                Instantiate(MonsterShell, SpawnShell.position, Quaternion.Euler(0, 0, DegreeOfRotation * i));
            }
            RecreationTime = RecomendetRecreationTime;
        }
        if (MonsterType == 3)
        {
            if (Vector2.Distance(gameObject.transform.position, Player.transform.position) < 0.3f)
            {
                int R = Random.Range(1, 5);
                if (R > Player.GetComponent<RangerScript>().Armor && Diminish == false)
                {
                    Player.GetComponent<RangerScript>().HP -= 1;
                }
                PlayerRepulsionTime = 0.2f;
                Player.GetComponent<RangerScript>().CameraShakeTime = 0.4f; Player.GetComponent<RangerScript>().SpeedShakeCam = 1.4f; Player.GetComponent<RangerScript>().ShakeCameraAmplitude = 1;
            }
            if (HP > 0) { gameObject.GetComponent<Animator>().SetInteger("Anim", 0); }
        }
    }
    public void DestroyZomb()
    {
        Destroy(gameObject);
    }
    public void StartExtinction()
    {
        ExtinctionTime = 5;
    }
    public void DizActiveZomb()
    {
        Colliders[0].enabled = false;
        Colliders[1].enabled = false;
        Room.GetComponent<RoomScript>().Monsters.Remove(gameObject);
        if (MonsterType == 3)
        {
            for (int i = 0; i < Tentacles.Length; i++)
            {
                Tentacles[i].GetComponent<MonsterShell>().enabled = false;
            }
            //Player.GetComponent<RangerScript>().Speed = Tentacles[0].GetComponent<MonsterShell>().PlayerSpeed.x;
        }
    }
    public void FallingGun()
    {
        if (gameObject.GetComponent<SpriteRenderer>().flipX == false)
        {
           Gun.transform.localPosition = FallingGunPositions[FallingGunNumber];
        }
        else { Gun.transform.localPosition = new Vector2(-FallingGunPositions[FallingGunNumber].x, FallingGunPositions[FallingGunNumber].y); }
        FallingGunNumber += 1;
    }
    public void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (Diminish == true) { HP = 0; }
        }
    }
}
