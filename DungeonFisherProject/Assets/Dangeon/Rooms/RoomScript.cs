using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public Vector2Int IntPos;
    public Vector2Int Entrance;
    public Vector2Int[] ExitsPoint;
    //public GameObject[] SubsidiariesRoom;
    public DangeonCreater RoomController;
    public GameObject[] RoomsRrefab;
    public Transform PointSpawnM1;
    public Transform PointSpawnM2;
    public bool Branching;
    public GameObject[] MonsterPrefab;
    public int[] MonstersQuantity;
    public List<GameObject> Monsters;
    public bool Batle;
    public bool EndRoom;
    public bool BossRoom;

    public BoxCollider2D[] WithOutPassages;
    public BoxCollider2D[] WithPassages;

    public SpriteRenderer[] FogOfWar;
    public Animator[] Doors;
    public SpriteRenderer BottomDoor;

    [Header("Decor")]
    public Transform StartDecorPoint;
    public Vector2Int DecorPointsDiapason;
    private GameObject[] BigDecorPrefab;
    private GameObject[] TablePrefabs;
    private GameObject[] ChairPrefabs;
    private GameObject[] FloorElementsPrefab;
    private GameObject[] WallDecorPrefab;
    public List<Vector2> ChairPoints;
    public Vector2[] PointsForBigDecor;
    public Vector2[] WallDecorPoints;
    public List<Vector2> PointsExpensions;
    public List<Vector2> PointsForDecoration;
    public int DecorationMaxQuantity;
    public int DecorationMinQuantity;
    private int DecorationQuantity;

    public GameObject Sudak;
    void Start()
    {
        RoomController = GameObject.FindWithTag("Dangeon").GetComponent<DangeonCreater>();
        MonsterPrefab = RoomController.GetComponent<DangeonCreater>().MonsterPrefabs;
        BigDecorPrefab = RoomController.GetComponent<DangeonCreater>().BigDecorPrefab;
        TablePrefabs = RoomController.GetComponent<DangeonCreater>().TablePrefabs;
        ChairPrefabs = RoomController.GetComponent<DangeonCreater>().ChairPrefabs;
        FloorElementsPrefab = RoomController.GetComponent<DangeonCreater>().FloorElementsPrefab;
        WallDecorPrefab = RoomController.GetComponent<DangeonCreater>().WallDecorPrefab;

        RoomsRrefab = RoomController.RoomPrefabs;
        RoomController.SpawnedRoom.Add(IntPos);
        //SubsidiariesRoom = new GameObject[ExitsPoint.Length];
        SpawnNextRoom();
    }
    public void SpawnBoss()
    {
        Sudak.GetComponent<Sudak>().Room = gameObject.GetComponent<RoomScript>();
        Instantiate(Sudak, transform.position, transform.rotation);
    }
    public void SpawnMonster()
    {
        MonstersQuantity[0] = Random.Range(1, 4);
        for (int f = 0; f < MonstersQuantity.Length; f++)
        {
            for (int i = 0; i < MonstersQuantity[f]; i++)
            {
                MonsterPrefab[f].GetComponent<ZombieFish>().Room = gameObject;
                Instantiate(MonsterPrefab[f], new Vector2(Random.Range(PointSpawnM1.position.x, PointSpawnM2.position.x), Random.Range(PointSpawnM1.position.y, PointSpawnM2.position.y)), transform.rotation);
            }
        }
        //SpawnDecor
        DecorationQuantity = Random.Range(DecorationMinQuantity, DecorationMaxQuantity + 1);
        for (int i =0; i < DecorPointsDiapason.x; i++)
        {
            for (int g = 0; g < DecorPointsDiapason.y; g++)
            {
                if (PointsExpensions.Contains(new Vector2(StartDecorPoint.position.x + (i * 0.24f), StartDecorPoint.position.y - (g * 0.24f))) == false)
                {
                    PointsForDecoration.Add(new Vector2(StartDecorPoint.position.x + (i * 0.24f), StartDecorPoint.position.y - (g * 0.24f)));
                }
            }
        }
        for (int i = 0; i < DecorationQuantity; i++)
        {
            int H = Random.Range(0, 3 + 1);
            if (H == 0) // Tables and Chair
            {
                if (ChairPoints.Count == 0)
                {
                    Vector2 Point = PointsForDecoration[Random.Range(0, PointsForDecoration.Count)];
                    int F = Random.Range(0, TablePrefabs.Length);
                    TablePrefabs[F].GetComponent<SpriteRenderer>().sortingOrder = -(Mathf.RoundToInt(Point.y * 7.68f)) - 200;
                    Instantiate(TablePrefabs[F], Point, transform.rotation);
                    PointsForDecoration.Remove(Point);

                    if (PointsForDecoration.Contains(new Vector2(Point.x + 0.24f, Point.y))) { ChairPoints.Add(new Vector2(Point.x + 0.24f, Point.y)); PointsForDecoration.Remove(new Vector2(Point.x + 0.24f, Point.y));}
                    if (PointsForDecoration.Contains(new Vector2(Point.x - 0.24f, Point.y))) { ChairPoints.Add(new Vector2(Point.x - 0.24f, Point.y)); PointsForDecoration.Remove(new Vector2(Point.x - 0.24f, Point.y)); }
                    if (PointsForDecoration.Contains(new Vector2(Point.x, Point.y + 0.24f))) { ChairPoints.Add(new Vector2(Point.x, Point.y + 0.24f)); PointsForDecoration.Remove(new Vector2(Point.x, Point.y + 0.24f)); }
                    if (PointsForDecoration.Contains(new Vector2(Point.x, Point.y - 0.24f))) { ChairPoints.Add(new Vector2(Point.x, Point.y - 0.24f)); PointsForDecoration.Remove(new Vector2(Point.x, Point.y - 0.24f)); }
                    
                }
                else
                {
                    int R = Random.Range(0, 5);
                    if (R == 0)
                    {
                        Vector2 Point = PointsForDecoration[Random.Range(0, PointsForDecoration.Count)];
                        int F = Random.Range(0, TablePrefabs.Length);
                        TablePrefabs[F].GetComponent<SpriteRenderer>().sortingOrder = -(Mathf.RoundToInt(Point.y * 7.68f)) - 200;
                        Instantiate(TablePrefabs[F], Point, transform.rotation);
                        PointsForDecoration.Remove(Point);

                        if (PointsForDecoration.Contains(new Vector2(Point.x + 0.24f, Point.y))) { ChairPoints.Add(new Vector2(Point.x + 0.24f, Point.y)); PointsForDecoration.Remove(new Vector2(Point.x + 0.24f, Point.y)); }
                        if (PointsForDecoration.Contains(new Vector2(Point.x - 0.24f, Point.y))) { ChairPoints.Add(new Vector2(Point.x - 0.24f, Point.y)); PointsForDecoration.Remove(new Vector2(Point.x - 0.24f, Point.y)); }
                        if (PointsForDecoration.Contains(new Vector2(Point.x, Point.y + 0.24f))) { ChairPoints.Add(new Vector2(Point.x, Point.y + 0.24f)); PointsForDecoration.Remove(new Vector2(Point.x, Point.y + 0.24f)); }
                        if (PointsForDecoration.Contains(new Vector2(Point.x, Point.y - 0.24f))) { ChairPoints.Add(new Vector2(Point.x, Point.y - 0.24f)); PointsForDecoration.Remove(new Vector2(Point.x, Point.y - 0.24f)); }
                        
                    }
                    else
                    {
                        int F = Random.Range(0, ChairPrefabs.Length);
                        int G = Random.Range(0, ChairPoints.Count);
                        
                        ChairPrefabs[F].GetComponent<SpriteRenderer>().sortingOrder = -(Mathf.RoundToInt(ChairPoints[G].y * 7.68f)) - 200;
                        Instantiate(ChairPrefabs[F], ChairPoints[G], transform.rotation);
                        ChairPoints.RemoveAt(G);
                    }
                }
            }
            if (H == 1)//Big Decoration
            {
                Vector2 Point = PointsForBigDecor[Random.Range(0, PointsForBigDecor.Length)];
                int F = Random.Range(0, BigDecorPrefab.Length);
                if (PointsExpensions.Contains(Point) == false)
                {
                    BigDecorPrefab[F].GetComponent<SpriteRenderer>().sortingOrder = -(Mathf.RoundToInt((transform.position.y + Point.y) * 7.68f)) - 200;
                    Instantiate(BigDecorPrefab[F], new Vector2(transform.position.x + Point.x, transform.position.y + Point.y), transform.rotation);
                    PointsExpensions.Add(Point);
                    if (PointsForDecoration.Contains(Point)) { PointsForDecoration.Remove(Point); }
                }
                
            }
            if (H == 2)//FloorDecor
            {
                Vector2 Point = PointsForDecoration[Random.Range(0, PointsForDecoration.Count)];
                int F = Random.Range(0, FloorElementsPrefab.Length);
                Instantiate(FloorElementsPrefab[F], Point, transform.rotation);
                PointsForDecoration.Remove(Point);
            }
            if (H == 3)//WallDecor
            {
                Vector2 Point = new Vector2 (WallDecorPoints[Random.Range(0, WallDecorPoints.Length)].x + (Random.Range (-0.12f,0.12f)), WallDecorPoints[Random.Range(0, WallDecorPoints.Length)].y + (Random.Range(-0.04f,0.04f)));// точка уже с учётом разбросса
                int R = Random.Range(0, WallDecorPrefab.Length);
                Instantiate(WallDecorPrefab[R], new Vector2 (transform.position.x + Point.x,transform.position.y+Point.y),  Quaternion.Euler(0,0,Random.Range(-30f,30f)));
            }
        }
    }
    public void Update()
    {
        if (Monsters.Count <= 0) { Batle = false; }
        if (Batle == true)
        {
            for (int i = 0; i < WithOutPassages.Length; i++) { WithOutPassages[i].enabled = false; }
            for (int i = 0; i < WithPassages.Length; i++) { WithPassages[i].enabled = true; }
            for (int i =0;i<FogOfWar.Length;i++) { FogOfWar[i].color = new Color(FogOfWar[i].color.r, FogOfWar[i].color.g, FogOfWar[i].color.b, FogOfWar[i].color.a - Time.deltaTime); }
        }
        else
        {
            for (int i = 0; i < WithOutPassages.Length; i++) { WithOutPassages[i].enabled = true; }
            for (int i = 0; i < WithPassages.Length; i++) { WithPassages[i].enabled = false; }
        }
        for (int i = 0; i < Doors.Length; i++)
        {
            Doors[i].SetBool("Close", Batle);
        }
        if (BottomDoor != null)
        {
            if (Batle == false) { BottomDoor.sortingOrder = -499; }
            else { BottomDoor.sortingOrder = -1; }
        }
    }
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (Monsters.Count > 0)
            {
                Batle = true;
            }
            
        }
    }
    public void SpawnNextRoom()
    {
        if (RoomController.SpawnedRoom.Count < 5)
        {
            for (int i = 0; i < ExitsPoint.Length; i++)
            {
                List<GameObject> Podhodiach4ieRoom = new List<GameObject>();
                List<GameObject> Podhodiach4iepoExitsRoom = new List<GameObject>();
                List<GameObject> Podhodiach4iePoEntranceRoom = new List<GameObject>();
                for (int g = 4; g < RoomsRrefab.Length; g++)
                {
                    if (RoomsRrefab[g].GetComponent<RoomScript>().Entrance.x == ExitsPoint[i].x && ExitsPoint[i].x != 0)
                    {
                        Podhodiach4iePoEntranceRoom.Add(RoomsRrefab[g]);
                    }
                    if (RoomsRrefab[g].GetComponent<RoomScript>().Entrance.y == ExitsPoint[i].y && ExitsPoint[i].y != 0)
                    {
                        Podhodiach4iePoEntranceRoom.Add(RoomsRrefab[g]);
                    }
                    bool k = false;
                    for (int h = 0; h < RoomsRrefab[g].GetComponent<RoomScript>().ExitsPoint.Length; h++)
                    {
                        // k = RoomController.SpawnedRoom.Contains(RoomsRrefab[g].GetComponent<RoomScript>().ExitsPoint[h]);
                        if (RoomController.SpawnedRoom.Contains(new Vector2Int(RoomsRrefab[g].GetComponent<RoomScript>().ExitsPoint[h].x + IntPos.x + ExitsPoint[i].x, RoomsRrefab[g].GetComponent<RoomScript>().ExitsPoint[h].y + IntPos.y + ExitsPoint[i].y)) == true)
                        {
                            k = true;
                        }
                    }
                    if (k == false) { Podhodiach4iepoExitsRoom.Add(RoomsRrefab[g]); }
                }
                if (Podhodiach4iepoExitsRoom.Count > Podhodiach4iePoEntranceRoom.Count)
                {
                    for (int f = 0; f < Podhodiach4iepoExitsRoom.Count; f++)
                    {
                        if (Podhodiach4iePoEntranceRoom.Contains(Podhodiach4iepoExitsRoom[f])) { Podhodiach4ieRoom.Add(Podhodiach4iepoExitsRoom[f]); }
                    }
                }
                else
                {
                    for (int f = 0; f < Podhodiach4iePoEntranceRoom.Count; f++)
                    {
                        if (Podhodiach4iepoExitsRoom.Contains(Podhodiach4iePoEntranceRoom[f])) { Podhodiach4ieRoom.Add(Podhodiach4iePoEntranceRoom[f]); }
                    }
                }
                
                int q = Random.Range(0, Podhodiach4ieRoom.Count);
                Podhodiach4ieRoom[q].GetComponent<RoomScript>().IntPos = new Vector2Int(IntPos.x + ExitsPoint[i].x, IntPos.y + ExitsPoint[i].y);
                Instantiate(Podhodiach4ieRoom[q], new Vector2((IntPos.x + ExitsPoint[i].x) * RoomController.WidthRoom, (IntPos.y + ExitsPoint[i].y) * RoomController.HeightRoom), transform.rotation);


            }
        }
        if (RoomController.SpawnedRoom.Count >= 5 && RoomController.SpawnedRoom.Count < 8)
        {
            for (int i = 0; i < ExitsPoint.Length; i++)
            {
                List<GameObject> Podhodiach4ieRoom = new List<GameObject>();
                List<GameObject> Podhodiach4iepoExitsRoom = new List<GameObject>();
                List<GameObject> Podhodiach4iePoEntranceRoom = new List<GameObject>();
                for (int g = 0; g < RoomsRrefab.Length; g++)
                {
                    if (RoomsRrefab[g].GetComponent<RoomScript>().Entrance.x == ExitsPoint[i].x && ExitsPoint[i].x != 0)
                    {
                        Podhodiach4iePoEntranceRoom.Add(RoomsRrefab[g]);
                    }
                    if (RoomsRrefab[g].GetComponent<RoomScript>().Entrance.y == ExitsPoint[i].y && ExitsPoint[i].y != 0)
                    {
                        Podhodiach4iePoEntranceRoom.Add(RoomsRrefab[g]);
                    }
                    bool k = false;
                    for (int h = 0; h < RoomsRrefab[g].GetComponent<RoomScript>().ExitsPoint.Length; h++)
                    {
                        // k = RoomController.SpawnedRoom.Contains(RoomsRrefab[g].GetComponent<RoomScript>().ExitsPoint[h]);
                        k = RoomController.SpawnedRoom.Contains(new Vector2Int(RoomsRrefab[g].GetComponent<RoomScript>().ExitsPoint[h].x + IntPos.x + ExitsPoint[i].x, RoomsRrefab[g].GetComponent<RoomScript>().ExitsPoint[h].y + IntPos.y + ExitsPoint[i].y));
                    }
                    if (k == false) { Podhodiach4iepoExitsRoom.Add(RoomsRrefab[g]); }
                }
                if (Podhodiach4iepoExitsRoom.Count > Podhodiach4iePoEntranceRoom.Count)
                {
                    for (int f = 0; f < Podhodiach4iepoExitsRoom.Count; f++)
                    {
                        if (Podhodiach4iePoEntranceRoom.Contains(Podhodiach4iepoExitsRoom[f])) { Podhodiach4ieRoom.Add(Podhodiach4iepoExitsRoom[f]); }
                    }
                }
                else
                {
                    for (int f = 0; f < Podhodiach4iePoEntranceRoom.Count; f++)
                    {
                        if (Podhodiach4iepoExitsRoom.Contains(Podhodiach4iePoEntranceRoom[f])) { Podhodiach4ieRoom.Add(Podhodiach4iePoEntranceRoom[f]); }
                    }
                }
                int q = Random.Range(0, Podhodiach4ieRoom.Count);
                Podhodiach4ieRoom[q].GetComponent<RoomScript>().IntPos = new Vector2Int(IntPos.x + ExitsPoint[i].x, IntPos.y + ExitsPoint[i].y);
                Instantiate(Podhodiach4ieRoom[q], new Vector2((IntPos.x + ExitsPoint[i].x) * RoomController.WidthRoom, (IntPos.y + ExitsPoint[i].y) * RoomController.HeightRoom), transform.rotation);


            }
        }
        if (RoomController.SpawnedRoom.Count >= 8)
        {
            for (int i = 0; i < ExitsPoint.Length; i++)
            {
                List<GameObject> Podhodiach4ieRoom = new List<GameObject>();
                List<GameObject> Podhodiach4iepoExitsRoom = new List<GameObject>();
                List<GameObject> Podhodiach4iePoEntranceRoom = new List<GameObject>();
                for (int g = 0; g < 4; g++)
                {
                    if (RoomsRrefab[g].GetComponent<RoomScript>().Entrance.x == ExitsPoint[i].x && ExitsPoint[i].x != 0)
                    {
                        Podhodiach4iePoEntranceRoom.Add(RoomsRrefab[g]);
                    }
                    if (RoomsRrefab[g].GetComponent<RoomScript>().Entrance.y == ExitsPoint[i].y && ExitsPoint[i].y != 0)
                    {
                        Podhodiach4iePoEntranceRoom.Add(RoomsRrefab[g]);
                    }
                    bool k = false;
                    for (int h = 0; h < RoomsRrefab[g].GetComponent<RoomScript>().ExitsPoint.Length; h++)
                    {
                        // k = RoomController.SpawnedRoom.Contains(RoomsRrefab[g].GetComponent<RoomScript>().ExitsPoint[h]);
                        k = RoomController.SpawnedRoom.Contains(new Vector2Int(RoomsRrefab[g].GetComponent<RoomScript>().ExitsPoint[h].x + IntPos.x + ExitsPoint[i].x, RoomsRrefab[g].GetComponent<RoomScript>().ExitsPoint[h].y + IntPos.y + ExitsPoint[i].y));
                    }
                    if (k == false) { Podhodiach4iepoExitsRoom.Add(RoomsRrefab[g]); }
                }
                if (Podhodiach4iepoExitsRoom.Count > Podhodiach4iePoEntranceRoom.Count)
                {
                    for (int f = 0; f < Podhodiach4iepoExitsRoom.Count; f++)
                    {
                        if (Podhodiach4iePoEntranceRoom.Contains(Podhodiach4iepoExitsRoom[f])) { Podhodiach4ieRoom.Add(Podhodiach4iepoExitsRoom[f]); }
                    }
                }
                else
                {
                    for (int f = 0; f < Podhodiach4iePoEntranceRoom.Count; f++)
                    {
                        if (Podhodiach4iepoExitsRoom.Contains(Podhodiach4iePoEntranceRoom[f])) { Podhodiach4ieRoom.Add(Podhodiach4iePoEntranceRoom[f]); }
                    }
                }
                int q = Random.Range(0, Podhodiach4ieRoom.Count);
                Podhodiach4ieRoom[q].GetComponent<RoomScript>().IntPos = new Vector2Int(IntPos.x + ExitsPoint[i].x, IntPos.y + ExitsPoint[i].y);
                Instantiate(Podhodiach4ieRoom[q], new Vector2((IntPos.x + ExitsPoint[i].x) * RoomController.WidthRoom, (IntPos.y + ExitsPoint[i].y) * RoomController.HeightRoom), transform.rotation);


            }
        }
    }
    
}
