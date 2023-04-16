using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Split : MonoBehaviour
{
    public SpriteRenderer Shadow;
    public Transform EndFallPoint;
    public Transform StartSplitFall;
    public float FallProgress;
    private GameObject Player;
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }
    void FixedUpdate()
    {
        if (FallProgress < 1) { FallProgress += Time.deltaTime / 2; }
        Shadow.color = new Color(1, 1, 1, FallProgress);
        gameObject.transform.position = Vector2.Lerp(StartSplitFall.position, EndFallPoint.position, FallProgress);
        if (FallProgress > 0.9f) { gameObject.GetComponent<BoxCollider2D>().enabled = true; }
        if (FallProgress >= 1) { gameObject.GetComponent<Animator>().SetBool("Destroy", true);Shadow.enabled = false; }
    }
    public void Destroing()
    {
        Destroy(EndFallPoint.gameObject);
    }
    public void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            
            int R = Random.Range(1, 5);
            if (R > Player.GetComponent<RangerScript>().Armor)
            {
                Player.GetComponent<RangerScript>().HP -= 1;
                Player.GetComponent<RangerScript>().CameraShakeTime = 0.4f; Player.GetComponent<RangerScript>().SpeedShakeCam = 1.4f; Player.GetComponent<RangerScript>().ShakeCameraAmplitude = 1;
            }
            Vector2 PointReclining = new Vector2(-(gameObject.transform.position.x - Player.transform.position.x), -(gameObject.transform.position.y - Player.transform.position.y));
            Player.GetComponent<Rigidbody2D>().AddForce(PointReclining * 500f);
            Destroing();
            
        }
    }
}
