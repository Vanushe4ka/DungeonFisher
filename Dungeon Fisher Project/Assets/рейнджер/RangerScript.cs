using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RangerScript : MonoBehaviour
{
    public GameObject RStickLeg;
    public GameObject LStickLeg;

    public int HP = 8;
    public Animator HPBarAnim;
    private Animator RangerAnim;
    private float TransfusionTime;
    private Rigidbody2D RG;
    public Joystick JoystW;
    public Joystick JoystT;
    public int Speed;
    public float AnimSpeed;
    public GameObject Cam;
    private Vector2 CamPos;
    public float SpeedCam;
    public GameObject GunsObject;
    public GameObject[] Guns;
    public List<int> PresentGun;
    public int SetGun;
    private int GunN;
    public int[] ShellTypeInGun;
    public int[] ShellsQuantity;//1-shotgun,2-mashine gun,3-grenade,4-energi,5-rifle,6-arrow,7-steam,fire,etc.
    public Text ShellText;

    private float ShotDelay;
    private bool Shoter;
    private float TimeWalkToWalkerJ;
    private bool WalkerInWJ;
    public int Stimulators;
    public Text StimulatorQuantityText;
    public GameObject StimulatorButton;

    public bool FastTravel;
    private bool FastTravelMouseDown;
    public bool ExitFastTravel;
    private Vector2 StartScrolPos;
    private Vector2 StartPlayerPos;
    private bool StartTouches;
    private Vector2 TouchStart1;
    private Vector2 TouchStart2;
    public float StandartCamScale;
    public float FastTravelCamScale;
    public GameObject[] GameInterfaceElements;
    public GameObject[] FastTravelInterfaceElements;
    public Button ButtonExitFastTravel;

    //public Transform CameraShake;
    public float SpeedShakeCam;
    public float ShakeCameraAmplitude;     //назначается как стартовое значение для ShakeCamAmplitude
    private float ShakeCamAmplitude;  // перемменная итоговой амплитуды движения камеры с учётом времени
    public float CameraShakeTime;

    public bool BossFight;
    public bool BossDemonstration;
    public GameObject Boss;
    public float CamScaleWithoutBoss;


    public int Armor;
    
    void Start()
    {
        CamScaleWithoutBoss = StandartCamScale;
        StimulatorQuantityText.text = "" + Stimulators;
        if (Stimulators <= 0) { StimulatorButton.SetActive(false); }
        RangerAnim = gameObject.GetComponent<Animator>();
        RG = gameObject.GetComponent<Rigidbody2D>();
        TransfusionTime = Random.Range(5, 20);
        for (int i = 0; i < Guns.Length; i++)
        {
            Guns[i].GetComponent<SpriteRenderer>().enabled = false;
        }
        Guns[SetGun].GetComponent<SpriteRenderer>().enabled = true;
    }

    void Update()
    {
        
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(Mathf.RoundToInt(transform.position.y*7.68f)) - 199;
        Guns[SetGun].GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
        if (CameraShakeTime > 0)
        {
            CameraShakeTime -= Time.deltaTime;
            ShakeCamAmplitude = ShakeCameraAmplitude * CameraShakeTime;
            Quaternion CameraRot = Quaternion.Euler(Random.Range(-ShakeCamAmplitude, ShakeCamAmplitude), Random.Range(-ShakeCamAmplitude, ShakeCamAmplitude), 0);
            if (Mathf.Abs(Cam.transform.rotation.x) < 2 || Mathf.Abs(Cam.transform.rotation.y) < 2)
            {
                Cam.transform.localRotation = Quaternion.Slerp(Cam.transform.localRotation, Cam.transform.localRotation * CameraRot, SpeedShakeCam);
            }
        }
        if (CameraShakeTime <= 0)
        {
            Quaternion StartCamRotation = Quaternion.Euler(0, 0, 0);
            Cam.transform.localRotation = Quaternion.Lerp(Cam.transform.localRotation, StartCamRotation, 0.1f);
        }
        if (SetGun == 0 || SetGun == 18) { ShellText.text = ""; } else { ShellText.text = "shells: " + ShellsQuantity[ShellTypeInGun[SetGun]];}
        if (SetGun != 4) { GunsObject.transform.localPosition = new Vector2(0, 0.025f);} else { GunsObject.transform.localPosition = new Vector2(0, -0.045f); }
        RStickLeg.transform.right = -new Vector2(-JoystT.Direction.x, -JoystT.Direction.y);
        RStickLeg.transform.localScale = new Vector3 (Vector2.Distance(new Vector2(0, 0), JoystT.Direction),1,1);
        LStickLeg.transform.right = -new Vector2(-JoystW.Direction.x, -JoystW.Direction.y);
        LStickLeg.transform.localScale = new Vector3(Vector2.Distance(new Vector2(0, 0), JoystW.Direction), 1, 1);
        if (HP > 8) { HP = 8; }
        if (HP == 8 && TransfusionTime < 0)
        {
            TransfusionTime = Random.Range(5, 20);
            HPBarAnim.SetTrigger("Transfusion");
        }
        if (TransfusionTime > 0) { TransfusionTime -= Time.deltaTime; }
        HPBarAnim.SetInteger("HP", HP);
        if (HP > 0)
        {
            if (JoystT.Direction.x != 0 && JoystT.Direction.y != 0)
            {
                if (Shoter == false) { ShotDelay = 0.1f; }
                if (ShotDelay > 0) { ShotDelay -= Time.deltaTime; }
                Shoter = true;
                if (ShotDelay <= 0)
                {
                    Shot();
                }
                GunsObject.transform.right = new Vector2(-JoystT.Direction.x, -JoystT.Direction.y);
                if (JoystT.Direction.x < 0)
                {
                    for (int i = 0; i < Guns.Length; i++) { Guns[i].GetComponent<SpriteRenderer>().flipY = false; }
                }
                if (JoystT.Direction.x > 0)
                {
                    for (int i = 0; i < Guns.Length; i++) { Guns[i].GetComponent<SpriteRenderer>().flipY = true; }
                }
                if (JoystT.Direction.x > 0) { gameObject.GetComponent<SpriteRenderer>().flipX = false; }
                if (JoystT.Direction.x < 0) { gameObject.GetComponent<SpriteRenderer>().flipX = true; }
                WalkerInWJ = false;
            }
            else
            {
                Shoter = false;
                if (WalkerInWJ == false) { TimeWalkToWalkerJ = 1; }
                if (TimeWalkToWalkerJ > 0) { TimeWalkToWalkerJ -= Time.deltaTime; }
                WalkerInWJ = true;
                if (TimeWalkToWalkerJ <= 0)
                {
                  if (JoystW.Direction.x > 0) { gameObject.GetComponent<SpriteRenderer>().flipX = false; }
                  if (JoystW.Direction.x < 0) { gameObject.GetComponent<SpriteRenderer>().flipX = true; }
                  if (JoystW.Direction.x != 0 || JoystW.Direction.y != 0)
                  {
                      GunsObject.transform.right = new Vector2(-JoystW.Direction.x, 1);
                  }
                  if (JoystW.Direction.x < 0)
                  {
                      for (int i = 0; i < Guns.Length; i++) { Guns[i].GetComponent<SpriteRenderer>().flipY = false; }
                  }
                  if (JoystW.Direction.x > 0)
                  {
                      for (int i = 0; i < Guns.Length; i++) { Guns[i].GetComponent<SpriteRenderer>().flipY = true; }
                  }
                }
            }
            if (BossFight == true)/////////////////////////////////////////
            {
                //стандартный размер камеры восстанавливается из скрипта босса при его смерти по переменной "CamScaleWithoutBoss"
                if (BossDemonstration == true)
                {
                    CamPos = Vector2.Lerp(CamPos, Boss.transform.position, SpeedCam);
                }
                else
                {
                    CamPos = Vector2.Lerp(CamPos,Vector2.Lerp(gameObject.transform.position,  Boss.transform.position,0.5f) , SpeedCam);
                    if (Vector2.Distance(gameObject.transform.position,Boss.transform.position) > 0.65)
                    {
                        Cam.GetComponent<Camera>().orthographicSize = Vector2.Distance(gameObject.transform.position, Boss.transform.position);
                    }
                }
            }
            else { CamPos = Vector2.Lerp(CamPos, gameObject.transform.position, SpeedCam); }
            Cam.transform.position = new Vector3(CamPos.x, CamPos.y, -1.758f);
            Vector2 WDirection = new Vector2(0, 0);
            if (Mathf.Abs(JoystW.Direction.x) > 0.05f || Mathf.Abs(JoystW.Direction.y) > 0.05f)
            {
                WDirection = JoystW.Direction;
                RangerAnim.SetInteger("anim", 1);
                float SpeedRunA = (Vector2.Distance(new Vector2(0, 0), JoystW.Direction)) * AnimSpeed;
                if (JoystW.Direction.x < 0 && gameObject.GetComponent<SpriteRenderer>().flipX == false) { SpeedRunA *= -1; }
                if (JoystW.Direction.x > 0 && gameObject.GetComponent<SpriteRenderer>().flipX == true) { SpeedRunA *= -1; }

                RangerAnim.SetFloat("SpeedRun", SpeedRunA);
            }
            else
            {
                RangerAnim.SetInteger("anim", 0);
                WDirection.x = Input.GetAxis("Horizontal1");
                WDirection.y = Input.GetAxis("Vertical1");
            }

            RG.AddForce(WDirection * Speed * Time.deltaTime);

            if (Input.touchCount == 2 && JoystW.Direction == new Vector2(0,0) && JoystT.Direction == new Vector2 (0,0))
            {
                Touch touth1 = Input.GetTouch(0);
                Touch touth2 = Input.GetTouch(1);
                if (StartTouches == false)
                {
                    TouchStart1 = touth1.position;
                    TouchStart2 = touth2.position;
                    StartTouches = true;
                }
                if (Vector2.Distance(TouchStart1,TouchStart2) > Vector2.Distance(touth1.position,touth2.position) * 2)
                {
                    GameObject[] Rooms = GameObject.FindGameObjectsWithTag("Room");
                    GameObject NearestRoom = Rooms[0];
                    for (int i = 1; i < Rooms.Length; i++)
                    {
                        if (Vector2.Distance(gameObject.transform.position, Rooms[i].transform.position) < Vector2.Distance(gameObject.transform.position, NearestRoom.transform.position)) { NearestRoom = Rooms[i]; }
                    }
                    if (NearestRoom.GetComponent<RoomScript>().Batle == false)
                    {
                        RangerAnim.SetBool("EnterToVentilation",true); StartTouches = false; GunsObject.SetActive(false);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.F)) { RangerAnim.SetBool("EnterToVentilation", true); StartTouches = false; GunsObject.SetActive(false); } // для проверки не пека

            if (Input.touchCount == 0) { StartTouches = false; }
            if (FastTravel == false)
            {
                
                if (gameObject.GetComponent<SpriteRenderer>().color.a < 1) { gameObject.GetComponent<SpriteRenderer>().color = new Color(gameObject.GetComponent<SpriteRenderer>().color.r, gameObject.GetComponent<SpriteRenderer>().color.g, gameObject.GetComponent<SpriteRenderer>().color.b, gameObject.GetComponent<SpriteRenderer>().color.a + Time.deltaTime * 2); }

                FastTravelMouseDown = false;
                Cam.GetComponent<Camera>().orthographicSize = Mathf.Lerp(Cam.GetComponent<Camera>().orthographicSize, StandartCamScale,0.25f);
                gameObject.GetComponent<BoxCollider2D>().tag = "Player"; gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                
                if (Cam.GetComponent<Camera>().orthographicSize > (FastTravelCamScale - StandartCamScale) / 2)//исчезновение интерфейса быстрого перемещения
                {
                    for (int i = 0; i < FastTravelInterfaceElements.Length ; i++)
                    {
                        if (FastTravelInterfaceElements[i].GetComponent<Image>() != null)
                        {
                            FastTravelInterfaceElements[i].GetComponent<Image>().color = new Color(FastTravelInterfaceElements[i].GetComponent<Image>().color.r, FastTravelInterfaceElements[i].GetComponent<Image>().color.g, FastTravelInterfaceElements[i].GetComponent<Image>().color.b, (Cam.GetComponent<Camera>().orthographicSize - ((FastTravelCamScale - StandartCamScale) / 2) - StandartCamScale) / ((FastTravelCamScale - StandartCamScale) / 2));
                        }
                        if (FastTravelInterfaceElements[i].GetComponent<Text>() != null)
                        {
                            FastTravelInterfaceElements[i].GetComponent<Text>().color = new Color(FastTravelInterfaceElements[i].GetComponent<Text>().color.r, FastTravelInterfaceElements[i].GetComponent<Text>().color.g, FastTravelInterfaceElements[i].GetComponent<Text>().color.b, (Cam.GetComponent<Camera>().orthographicSize - ((FastTravelCamScale - StandartCamScale) / 2) - StandartCamScale) / ((FastTravelCamScale - StandartCamScale) / 2));
                        }
                    }
                }
                if (Cam.GetComponent<Camera>().orthographicSize < (FastTravelCamScale - StandartCamScale) / 2)// появление интерфейся игры
                {
                    for (int i = 0; i < GameInterfaceElements.Length; i++)
                    {
                        GameInterfaceElements[i].SetActive(true);
                        if (GameInterfaceElements[i].GetComponent<Image>() != null)
                        {
                            GameInterfaceElements[i].GetComponent<Image>().color = new Color(GameInterfaceElements[i].GetComponent<Image>().color.r, GameInterfaceElements[i].GetComponent<Image>().color.g, GameInterfaceElements[i].GetComponent<Image>().color.b, Mathf.Abs((Cam.GetComponent<Camera>().orthographicSize - ((FastTravelCamScale - StandartCamScale) / 2) - StandartCamScale) / ((FastTravelCamScale - StandartCamScale) / 2)) );
                        }
                        if (GameInterfaceElements[i].GetComponent<Text>() != null)
                        {
                            GameInterfaceElements[i].GetComponent<Text>().color = new Color(GameInterfaceElements[i].GetComponent<Text>().color.r, GameInterfaceElements[i].GetComponent<Text>().color.g, GameInterfaceElements[i].GetComponent<Text>().color.b, Mathf.Abs((Cam.GetComponent<Camera>().orthographicSize - ((FastTravelCamScale - StandartCamScale) / 2) - StandartCamScale) / ((FastTravelCamScale - StandartCamScale) / 2)));
                        }
                    }
                    for (int i = 0; i < FastTravelInterfaceElements.Length; i++) { FastTravelInterfaceElements[i].SetActive(false); }
                    for (int i = 0; i < GameInterfaceElements.Length; i++) { GameInterfaceElements[i].SetActive(true); }
                }
               
            }
            else
            {
                
                Cam.GetComponent<Camera>().orthographicSize = Mathf.Lerp(Cam.GetComponent<Camera>().orthographicSize, FastTravelCamScale, 0.25f);
                if (Cam.GetComponent<Camera>().orthographicSize > (FastTravelCamScale - StandartCamScale) / 2)//появление интерфейса быстрого перемещения
                {
                    for (int i = 0; i < FastTravelInterfaceElements.Length; i++)
                    {
                        if (FastTravelInterfaceElements[i].GetComponent<Image>() != null)
                        {
                            FastTravelInterfaceElements[i].GetComponent<Image>().color = new Color(FastTravelInterfaceElements[i].GetComponent<Image>().color.r, FastTravelInterfaceElements[i].GetComponent<Image>().color.g, FastTravelInterfaceElements[i].GetComponent<Image>().color.b, (Cam.GetComponent<Camera>().orthographicSize - ((FastTravelCamScale - StandartCamScale) / 2) - StandartCamScale) / ((FastTravelCamScale - StandartCamScale) / 2));
                        }
                        if (FastTravelInterfaceElements[i].GetComponent<Text>() != null)
                        {
                            FastTravelInterfaceElements[i].GetComponent<Text>().color = new Color(FastTravelInterfaceElements[i].GetComponent<Text>().color.r, FastTravelInterfaceElements[i].GetComponent<Text>().color.g, FastTravelInterfaceElements[i].GetComponent<Text>().color.b, (Cam.GetComponent<Camera>().orthographicSize - ((FastTravelCamScale - StandartCamScale) / 2) - StandartCamScale) / ((FastTravelCamScale - StandartCamScale) / 2));
                        }
                    }
                    for (int i = 0; i < FastTravelInterfaceElements.Length; i++) { FastTravelInterfaceElements[i].SetActive(true); }
                    for (int i = 0; i < GameInterfaceElements.Length; i++) { GameInterfaceElements[i].SetActive(false); }
                }
                if (Cam.GetComponent<Camera>().orthographicSize < (FastTravelCamScale - StandartCamScale) / 2)// исчезновение интерфейся игры
                {
                    for (int i = 0; i < GameInterfaceElements.Length; i++)
                    {
                        GameInterfaceElements[i].SetActive(true);
                        if (GameInterfaceElements[i].GetComponent<Image>() != null)
                        {
                            GameInterfaceElements[i].GetComponent<Image>().color = new Color(GameInterfaceElements[i].GetComponent<Image>().color.r, GameInterfaceElements[i].GetComponent<Image>().color.g, GameInterfaceElements[i].GetComponent<Image>().color.b, Mathf.Abs((Cam.GetComponent<Camera>().orthographicSize - ((FastTravelCamScale - StandartCamScale) / 2) - StandartCamScale) / ((FastTravelCamScale - StandartCamScale) / 2)));
                        }
                        if (GameInterfaceElements[i].GetComponent<Text>() != null)
                        {
                            GameInterfaceElements[i].GetComponent<Text>().color = new Color(GameInterfaceElements[i].GetComponent<Text>().color.r, GameInterfaceElements[i].GetComponent<Text>().color.g, GameInterfaceElements[i].GetComponent<Text>().color.b, Mathf.Abs((Cam.GetComponent<Camera>().orthographicSize - ((FastTravelCamScale - StandartCamScale) / 2) - StandartCamScale) / ((FastTravelCamScale - StandartCamScale) / 2)));
                        }
                    }
                    
                }
                gameObject.GetComponent<BoxCollider2D>().tag = "FastTravel"; gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                if (gameObject.GetComponent<SpriteRenderer>().color.a > 0) { gameObject.GetComponent<SpriteRenderer>().color = new Color(gameObject.GetComponent<SpriteRenderer>().color.r, gameObject.GetComponent<SpriteRenderer>().color.g, gameObject.GetComponent<SpriteRenderer>().color.b, gameObject.GetComponent<SpriteRenderer>().color.a - Time.deltaTime * 2); }
                
                if (Input.GetMouseButtonDown(0)) { StartScrolPos = Cam.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition) ; StartPlayerPos = gameObject.transform.position;FastTravelMouseDown = true; }
                else if (Input.GetMouseButton(0) && FastTravelMouseDown == true)
                {
                    Vector2 SetPos = (new Vector2(Cam.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition).x, Cam.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition).y)) - StartScrolPos;
                    gameObject.transform.position =StartPlayerPos  -  SetPos;
                }
            }
        }
        
    }
    public void ExitInFastTravel()
    {
        if (ExitFastTravel == true) { RangerAnim.SetBool("EnterToVentilation", false);  }
    }
    public void EnterFasttravelForAnim() { FastTravel = true; }
    public void ExitfastTravelForAnim()  { FastTravel = false; }
    public void EndExitFTAnim() { GunsObject.SetActive(true); }
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Room")
        {
            GameObject[] Rooms = GameObject.FindGameObjectsWithTag("Room");
            GameObject NearestRoom = Rooms[0];
            for (int i = 1; i < Rooms.Length; i++)
            {
                if (Vector2.Distance(gameObject.transform.position,Rooms[i].transform.position) < Vector2.Distance(gameObject.transform.position, NearestRoom.transform.position)) {NearestRoom = Rooms[i]; }
            }
            if (NearestRoom.GetComponent<RoomScript>().Monsters.Count == 0) { ExitFastTravel = true; }
        }
        if (collider.gameObject.tag == "StartRoom")
        {
            ExitFastTravel = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "StartRoom")
        {
            ExitFastTravel = false;
        }
        if (collider.gameObject.tag == "Room")
        {
            ExitFastTravel = false;
        }
    }
    void Shot()
    {
        if (SetGun == 0)
        {
            Guns[SetGun].GetComponent<Animator>().SetTrigger("Shot");
        }
        else
        {
            if (ShellsQuantity[ShellTypeInGun[SetGun]] > 0)
            {
                Guns[SetGun].GetComponent<Animator>().SetTrigger("Shot");
            }
            else
            {
                ShellsQuantity[ShellTypeInGun[SetGun]] = 0;
                Guns[SetGun].GetComponent<Animator>().ResetTrigger("Shot");
            }
        }
        
    }
    public void WeaponChange()
    {
        for (int i = 0; i < Guns.Length; i++)
        {
            Guns[i].GetComponent<SpriteRenderer>().enabled = false;
        }
        if (GunN < PresentGun.Count - 1) { GunN += 1; SetGun = PresentGun[GunN]; }
        else { GunN = 0; SetGun = PresentGun[GunN]; }
        Guns[SetGun].GetComponent<SpriteRenderer>().enabled = true;
    }
    
    public void Stimulator()
    {
        Stimulators -= 1;
        HP += 3;
        StimulatorQuantityText.text = "" + Stimulators;
        if (Stimulators <= 0) { StimulatorButton.SetActive(false); }
    }

}
