using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsScript : MonoBehaviour
{
    // Start is called before the first frame update
    public bool FirstShot;
    public GameObject Shell;
    public float Scatter;
    public int SellQuantity;
    public RangerScript Player;
    public GameObject[] Monsters;
    public Transform[] SpawnSellPoints;
    public int TypeGun;
    public int GunNumber;
    [Header("Cam Shake")]
    public float Amplitude;
    public float Speed;
    [Header("If This Tesla")]
    private int Energi;
    
    
    void Start()
    {
        FirstShot = true;
        
    }
    public void ShotNoMinusShell()
    {
        Player.ShellsQuantity[Player.ShellTypeInGun[GunNumber]] += 1;
        Shot();
    }
    public void Shot()
    {
        if (Shell.GetComponent<ShellScript>().Type == 4) { Shell.GetComponent<ShellScript>().LaserGun = gameObject.transform; }
        if (TypeGun == 0)
        {
            
            Player.ShellsQuantity[Player.ShellTypeInGun[GunNumber]] -= 1;
            if (FirstShot == true)
            {
                FirstShot = false;
                Monsters = GameObject.FindGameObjectsWithTag("Monster");
                Shell.GetComponent<ShellScript>().Monsters = new GameObject[Monsters.Length];
                Shell.GetComponent<ShellScript>().Boss = GameObject.FindWithTag("Boss");
                for (int g = 0; g < Monsters.Length; g++)
                {
                    Shell.GetComponent<ShellScript>().Monsters[g] = Monsters[g];
                }
            }
            for (int i = 0; i < SellQuantity; i++)
            {
                if (SpawnSellPoints.Length == 0)
                {
                    var rotationY = Quaternion.AngleAxis(Random.Range(-Scatter, Scatter), transform.forward);
                    var rotationX = Quaternion.AngleAxis(Random.Range(-Scatter, Scatter), transform.right);
                    Instantiate(Shell, transform.position, transform.rotation * rotationX * rotationY);
                }
                else
                {
                    Instantiate(Shell, SpawnSellPoints[i].position, transform.rotation);
                }
            }
            Player.ShakeCameraAmplitude = Amplitude;
            Player.SpeedShakeCam = Speed;
            Player.CameraShakeTime = 0.4f;
        }
        if (TypeGun == 1) // Tesla
        {
            Energi -= 1;
            if (Energi > 0 && ( Mathf.Abs(Player.JoystT.Direction.x) > 0 || Mathf.Abs(Player.JoystT.Direction.y) > 0)) { gameObject.GetComponent<Animator>().SetBool("NextShot", true); }
            else { gameObject.GetComponent<Animator>().SetBool("NextShot", false); }
            Monsters = GameObject.FindGameObjectsWithTag("Monster");
            GameObject TargetMonster = null;
            float DistanceToMonster = 1.1f;
            for (int i = 0; i < Monsters.Length; i++)
            {
                if (Vector2.Distance(Monsters[i].transform.position, transform.position) < DistanceToMonster && Monsters[i].GetComponent<ZombieFish>().HP > 0)
                {
                    if (gameObject.GetComponent<SpriteRenderer>().flipY == false && Monsters[i].transform.position.x < transform.position.x)
                    {
                        TargetMonster = Monsters[i];
                        DistanceToMonster = Vector2.Distance(Monsters[i].transform.position, transform.position);
                    }
                    if (gameObject.GetComponent<SpriteRenderer>().flipY == true && Monsters[i].transform.position.x > transform.position.x)
                    {
                        TargetMonster = Monsters[i];
                        DistanceToMonster = Vector2.Distance(Monsters[i].transform.position, transform.position);
                    }
                }
            }
            if (TargetMonster != null)
            {
                Player.ShellsQuantity[Player.ShellTypeInGun[GunNumber]] -= 1;
                Shell.GetComponent<ShellScript>().TargetMonster = TargetMonster;
                Shell.GetComponent<ShellScript>().DistanceToTarget = Vector2.Distance(transform.position, TargetMonster.transform.position);
                Instantiate(Shell, transform.position, transform.rotation);

                Player.ShakeCameraAmplitude = Amplitude*2;
                Player.SpeedShakeCam = Speed*2;
                Player.CameraShakeTime = 0.4f;
            }
            else
            {
                Player.ShellsQuantity[Player.ShellTypeInGun[GunNumber]] -= 1;
                Shell.GetComponent<ShellScript>().DistanceToTarget = 1;
                Instantiate(Shell, transform.position, transform.rotation);

                Player.ShakeCameraAmplitude = Amplitude*4;
                Player.SpeedShakeCam = Speed*4;
                Player.CameraShakeTime = 0.7f;
            }
            
        }
    }
    public void End()
    {
       gameObject.GetComponent<Animator>().ResetTrigger("Shot");
    }
    public void EnergiPlus()
    {
        Energi = 3;
        Player.ShakeCameraAmplitude = 0;
        Player.SpeedShakeCam = 0;
        Player.CameraShakeTime = 0;
    }
    public void CamShakePlus()
    {
        Player.ShakeCameraAmplitude += Amplitude;
        Player.SpeedShakeCam += Speed;
        Player.CameraShakeTime += 0.2f;
    }
}
