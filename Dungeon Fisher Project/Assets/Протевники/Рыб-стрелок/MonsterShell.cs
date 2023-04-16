using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterShell : MonoBehaviour
{
    public GameObject Player;
    public int Type; //0-shell,1-tentakle,2-SudakSplit
    public bool Stop;
    public bool ItsPlayer;
    private Vector2Int PlayerSpeed;
    private Vector2 PlayerAnimSpeed;
    // Start is called before the first frame update
    void Start()
    {
        if (Type == 1)
        {
            Player = GameObject.FindWithTag("Player");
            PlayerSpeed = new Vector2Int(Player.GetComponent<RangerScript>().Speed, Mathf.RoundToInt(Player.GetComponent<RangerScript>().Speed / 2));
            PlayerAnimSpeed = new Vector2(Player.GetComponent<RangerScript>().AnimSpeed, Player.GetComponent<RangerScript>().AnimSpeed /2);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Type == 0) { gameObject.GetComponent<Rigidbody2D>().velocity = transform.right * 0.5f; }
        if (Type ==1 && ItsPlayer == true)
        {
            Vector2 PointReclining = -new Vector2 (-(gameObject.transform.position.x - Player.transform.position.x), -(gameObject.transform.position.y - Player.transform.position.y));
            Player.GetComponent<Rigidbody2D>().velocity = PointReclining * 2f;
            if (transform.localScale.x == 0) { ItsPlayer = false; }
            if (Player.GetComponent<BoxCollider2D>().isTrigger == true) { Player.GetComponent<RangerScript>().Speed = PlayerSpeed.x; Player.GetComponent<RangerScript>().AnimSpeed = PlayerAnimSpeed.x; }
            
        }
    }
    public void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "wall")
        {
            if (Type == 0) { Destroy(gameObject); }
        }
        if (collider.gameObject.tag == "Player")
        {
            if (Type == 0)
            {
                int R = Random.Range(1, 5);
                if (R > Player.GetComponent<RangerScript>().Armor)
                {
                    Player.GetComponent<RangerScript>().HP -= 1;
                    Player.GetComponent<RangerScript>().CameraShakeTime = 0.4f; Player.GetComponent<RangerScript>().SpeedShakeCam = 1.4f; Player.GetComponent<RangerScript>().ShakeCameraAmplitude = 1;
                }
                Vector2 PointReclining = new Vector2(-(gameObject.transform.position.x - Player.transform.position.x), -(gameObject.transform.position.y - Player.transform.position.y));
                Player.GetComponent<Rigidbody2D>().AddForce(PointReclining * 500f);
                Destroy(gameObject);
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (Type == 1) { ItsPlayer = true; Player.GetComponent<RangerScript>().Speed = PlayerSpeed.y; Player.GetComponent<RangerScript>().AnimSpeed = PlayerAnimSpeed.y; }
        }
        if (collider.gameObject.tag == "wall")
        {
            if (Type == 1) { Stop = true; }
        }
    }
    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (Type == 1) { ItsPlayer = false; Player.GetComponent<RangerScript>().Speed = PlayerSpeed.x; Player.GetComponent<RangerScript>().AnimSpeed = PlayerAnimSpeed.x; }
        }
        if (collider.gameObject.tag == "wall")
        {
            if (Type == 1) { Stop = false; }
        }
    }
}
