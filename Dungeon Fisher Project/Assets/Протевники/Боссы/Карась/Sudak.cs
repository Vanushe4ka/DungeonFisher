using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sudak : MonoBehaviour
{
    public int Stadya;
    public RoomScript Room;
    public RangerScript Player;
    public int HP;
    public float TimeToNextAtack;
    public float ReloadTime = 2;

    public Transform[] Eye;
    
    public Animator Anim;
    public GameObject[] Lasers;
    public Vector2[] PointsTo2LAtack;
    public LayerMask LMask;
    public float TimeAtack;
    public int StartAtack1Rotate;
    public float AtackRotationChange;
    public float TimeToEnd;
    private float EndTime;
    public GameObject[] MonsterPrefabs;
    public GameObject Split;
    private int SplitQuantity;
    private float TimeToSplitSpawning;
    public GameObject BigSplit;
    public Vector2 LocalStartPosBigSplit;

    public bool LAtack1;
    public bool LAtack2;
    public bool SplitAtack;
    private bool LA2Warning;

    public SpriteRenderer Flash;
    public bool FlashActive;

    public void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<RangerScript>();
        Player.BossDemonstration = true;
        Player.Boss = gameObject;
        Room.GetComponent<RoomScript>().Monsters.Add(gameObject);
        Anim.enabled = false;
    }
    void FixedUpdate()
    {
        if (Room.Batle == true) { Anim.enabled = true; }
        if (HP > 0)
        {
            if (Room.Batle == true)
            {
                Anim.enabled = true;
                Player.BossFight = true;
            }

            if (HP < 400 - (100 * Stadya)) { Stadya += 1; Anim.SetTrigger("NextStadia"); LAtack1 = false;End2LAtack(); ReloadTime -= 0.4f; }
            if (gameObject.GetComponent<SpriteRenderer>().material.GetFloat("_Light") > 1) { gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_Light", Mathf.Lerp(gameObject.GetComponent<SpriteRenderer>().material.GetFloat("_Light"), 1, 0.3f)); }
            Eye[1].gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_Light", gameObject.GetComponent<SpriteRenderer>().material.GetFloat("_Light"));

            if (LAtack1 == false && Anim.GetInteger("Anim") != 2 && SplitAtack == false)
            {
                TimeToNextAtack += Time.deltaTime;
                if (TimeToNextAtack > ReloadTime)
                {
                    int R = Random.Range(1, Stadya + 1);
                    Anim.SetInteger("Anim", R);
                    TimeToNextAtack = 0;
                }
                Vector2 direction = Player.transform.position - Eye[0].transform.position;
                direction.Normalize();
                
                Eye[0].transform.right = direction;
                Eye[1].transform.rotation = Quaternion.Euler(0, 0, -Eye[0].transform.rotation.z);

                for (int i = 0; i < Lasers.Length; i++)
                {
                    if (i%2 == 0)//лазеры
                    {
                        Lasers[i].transform.localScale = new Vector3(1, 0, 1);
                        Lasers[i].gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    }
                    else//концы лазеров
                    {
                        Lasers[i].gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0);
                    }
                }
            }
            if (LAtack1 == true)
            {
                Eye[0].Rotate(0, 0, AtackRotationChange);
                TimeToEnd += Time.deltaTime;
                if (TimeToEnd >= EndTime) { LAtack1 = false; Anim.SetInteger("Anim", 0); TimeToEnd = 0; }
                Lasers[1].gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.8f);
                RaycastHit2D hit = Physics2D.Raycast(Eye[0].transform.position, Eye[0].transform.right, Mathf.Infinity, LMask);
                Debug.DrawRay(Eye[0].transform.position, Eye[0].right, Color.red);
                if (hit.collider != null)
                {
                    Debug.Log(hit.point);
                    float distance = Vector2.Distance(Eye[0].transform.position, hit.point);
                    Lasers[0].transform.position = Vector2.Lerp(Eye[0].transform.position, hit.point, 0.5f);

                    Vector2 direction = new Vector3(hit.point.x, hit.point.y, 0) - Eye[0].transform.position; direction.Normalize();
                    
                    Lasers[0].transform.up = direction;
                    Lasers[0].transform.localScale = new Vector3(1, distance * 12.5f, 1);
                    Lasers[1].transform.position = hit.point; Lasers[1].transform.up = direction;
                    if (hit.collider.gameObject.GetComponent<RangerScript>() != null)
                    {
                        
                        Vector2 PointReclining = new Vector2(-(Eye[0].position.x - Player.transform.position.x), -(Eye[0].position.y - Player.transform.position.y));
                        hit.collider.gameObject.GetComponent<Rigidbody2D>().AddForce(PointReclining * 5f);
                        TimeAtack -= Time.deltaTime;
                        if (TimeAtack <= 0)
                        {
                            TimeAtack = 0.5f;
                            hit.collider.gameObject.GetComponent<RangerScript>().HP -= 1;
                            ShakeCam();
                        }

                    }
                    else { TimeAtack = 0; }
                }
            }
            if (LAtack2 == true)
            {
                if (LA2Warning == true)//предупреждение о атаке
                {
                    for (int i = 0; i < Lasers.Length; i++)
                    {
                        if (i % 2 == 0)
                        {
                            Lasers[i].gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.25f);
                            Lasers[i + 1].gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.25f);
                            RaycastHit2D hit = Physics2D.Raycast(Eye[0].transform.position, PointsTo2LAtack[i/2], Mathf.Infinity, LMask);
                            Debug.DrawRay(Eye[0].transform.position,  PointsTo2LAtack[i/2], Color.red);
                            if (hit.collider != null)
                            {
                                
                                float distance = Vector2.Distance(Eye[0].transform.position, hit.point);
                                Lasers[i].transform.position = Vector2.Lerp(Eye[0].transform.position, hit.point, 0.5f);
                                Lasers[i].transform.localScale = new Vector3(1, distance * 12.5f, 1);
                                Lasers[i + 1].transform.position = hit.point;

                                Vector2 direction = new Vector3(hit.point.x, hit.point.y, 0) - Eye[0].transform.position; direction.Normalize();
                                
                                Lasers[i].transform.up = direction;
                            }

                        }
                    }
                }
                else//собсна сама атака
                {
                    for (int i = 0; i < Lasers.Length; i++)
                    {
                        
                        if (i % 2 == 0)
                        {
                            if (Lasers[i].GetComponent<SpriteRenderer>().color.a < 1) { Lasers[i].gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Lasers[i].GetComponent<SpriteRenderer>().color.a + Time.deltaTime * 2); }
                            if (Lasers[i+1].GetComponent<SpriteRenderer>().color.a < 0.8f) { Lasers[i + 1].gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, Lasers[i + 1].GetComponent<SpriteRenderer>().color.a + Time.deltaTime * 2); }
                                
                            RaycastHit2D hit = Physics2D.Raycast(Eye[0].transform.position, PointsTo2LAtack[i / 2], Mathf.Infinity, LMask);
                            Debug.DrawRay(Eye[0].transform.position, PointsTo2LAtack[i / 2], Color.red);
                            if (hit.collider != null)
                            {
                                Debug.Log(hit.point);
                                float distance = Vector2.Distance(Eye[0].transform.position, hit.point);
                                Lasers[i].transform.position = Vector2.Lerp(Eye[0].transform.position, hit.point, 0.5f);
                                Lasers[i].transform.localScale = new Vector3(1, distance * 12.5f, 1);
                                Lasers[i + 1].transform.position = hit.point;

                                Vector2 direction = new Vector3(hit.point.x, hit.point.y, 0) - Eye[0].transform.position; direction.Normalize();
                                
                                Lasers[i].transform.up = direction;

                                if (hit.collider.gameObject.GetComponent<RangerScript>() != null)
                                {
                                    Vector2 PointReclining = new Vector2(-(Eye[0].position.x - Player.transform.position.x), -(Eye[0].position.y - Player.transform.position.y));
                                    hit.collider.gameObject.GetComponent<Rigidbody2D>().AddForce(PointReclining * 5f);
                                    TimeAtack -= Time.deltaTime;
                                    if (TimeAtack <= 0)
                                    {
                                        TimeAtack = 0.25f;
                                        hit.collider.gameObject.GetComponent<RangerScript>().HP -= 1;
                                        ShakeCam();
                                    }

                                }
                                //else { TimeAtack = 0; }
                            }

                        }
                    }
                }
            }
            if (SplitAtack == true)//плевок лоха
            {
                BigSplit.transform.Translate(0, 0.025f, 0);
                if (BigSplit.transform.localPosition.y > LocalStartPosBigSplit.y + 2)
                {
                    TimeToSplitSpawning -= Time.deltaTime;
                    if (TimeToSplitSpawning <= 0)
                    {
                        TimeToSplitSpawning = Random.Range(0.05f, 0.2f);
                        Instantiate(Split, new Vector2(Random.Range(Player.transform.position.x - 0.3f, Player.transform.position.x + 0.3f), Random.Range(Player.transform.position.y - 0.3f, Player.transform.position.y + 0.3f)), transform.rotation);
                        SplitQuantity -= 1;
                    }
                    if (SplitQuantity <= 0)
                    {
                        SplitAtack = false;
                        BigSplit.GetComponent<SpriteRenderer>().enabled = true;
                    }
                }
            }
        }
        else//если сдох
        {
            Anim.SetBool("Dead", true);
            LAtack1 = false;
            End2LAtack();
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            for (int i = 0; i < Lasers.Length; i++) { Lasers[i].SetActive(false); }
            Player.BossDemonstration = true;

            if (FlashActive == true) { Flash.color = new Color(1, 1, 1, Flash.color.a + 0.2f); }
            else { Flash.color = new Color(1, 1, 1, Mathf.Lerp(Flash.color.a, 0, 0.1f)); }
        }
        
    }
    public void StartLAtack1()
    {
        LAtack1 = true;
        if (Player.gameObject.transform.position.y > Eye[0].transform.position.y)
        {
            StartAtack1Rotate = 90;
            if (Player.gameObject.transform.position.x > Eye[0].transform.position.x) { AtackRotationChange = -0.4f;}
            else { AtackRotationChange = 0.4f; }
        }
        else
        {
            StartAtack1Rotate = 270;
            if (Player.gameObject.transform.position.x > Eye[0].transform.position.x) {AtackRotationChange = 0.4f; }
            else { AtackRotationChange = -0.4f; }
        }
        Eye[0].rotation = Quaternion.Euler(0, 0, StartAtack1Rotate);
        int R = Random.Range(0, 2);if (R == 0) { EndTime = 7.7f * 2; } else { EndTime = 7.7f; }
    }
    public void StartLAtack2()
    {
        LAtack2 = true;
        LA2Warning = true;
    }
    public void NotWarningLAtack2() { LA2Warning = false; }
    public void End2LAtack() { LAtack2 = false;LA2Warning = false; Anim.SetInteger("Anim", 0); TimeToEnd = 0; }

    public void StartSplitAtack()
    {
        BigSplit.transform.localPosition = LocalStartPosBigSplit;
        BigSplit.GetComponent<SpriteRenderer>().enabled = true;
        SplitAtack = true;
        SplitQuantity = Random.Range((Stadya-2)* 15, ((Stadya - 2) * 25) + 1);
        Anim.SetInteger("Anim", 0);
    }

    public void ShakeCam()
    {
        Player.CameraShakeTime = 0.4f;
        Player.SpeedShakeCam = 2.8f;
        Player.ShakeCameraAmplitude = 1.5f;
    }
    public void SpawnMonsters()
    {
        int R = Random.Range(Stadya, Stadya * 2);
        for (int i = 0; i < Player.Guns.Length; i++)
        {
            Player.Guns[i].GetComponent<GunsScript>().FirstShot = true;
        }
        for (int i =0;i< R; i++)
        {
            int F = Random.Range(0, MonsterPrefabs.Length);
            MonsterPrefabs[F].GetComponent<ZombieFish>().Room = Room.gameObject;
            Instantiate(MonsterPrefabs[F], new Vector2(Random.Range(gameObject.transform.position.x - 0.25f, gameObject.transform.position.x + 0.25f), Random.Range(gameObject.transform.position.y - 0.8f, gameObject.transform.position.y - 0.6f)), transform.rotation);
        }
    }
    public void EndDemonstration()
    {
        Player.BossDemonstration = false;
    }
    public void EndDead()
    {
        Player.BossFight = false;
        Player.Cam.GetComponent<Camera>().orthographicSize = Player.CamScaleWithoutBoss;
    }
    public void WhiteFlash() { FlashActive = true; }
    public void DisActiveWhiteFlash() { FlashActive = false; }

}
