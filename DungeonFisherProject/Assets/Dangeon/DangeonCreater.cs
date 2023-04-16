using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangeonCreater : MonoBehaviour
{
    public GameObject[] RoomPrefabs;
    public GameObject[] BossRoomPrefabs;
    public List<Vector2Int> SpawnedRoom;
    public float TimeTesting = 1;
    public float WidthRoom;
    public float HeightRoom;
    public GameObject EventSystem;
    public GameObject LoadingTable;
    public GameObject LoadingCircle;
    public int[] AllMonsterQuantity;
    public GameObject[] MonsterPrefabs;
    public List<int> TrashForSpawn;
    public GameObject TrashSpriteObject;
    public Sprite[] Trash;

    public GameObject[] BigDecorPrefab;
    public GameObject[] TablePrefabs;
    public GameObject[] ChairPrefabs;
    public GameObject[] FloorElementsPrefab;
    public GameObject[] WallDecorPrefab;
    private bool MonsterIsSpawn;// for rooms (чтобы самому ручками не назначать)
    
    void Start()
    {
        for (int i = 0; i < TrashForSpawn.Count; i++)
        {
            TrashSpriteObject.GetComponent<SpriteRenderer>().sprite = Trash[TrashForSpawn[i]];
            Instantiate(TrashSpriteObject, new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)), Quaternion.Euler(0,0,Random.Range(0,360)));
        }
        while (TrashForSpawn.Count > 0) { TrashForSpawn.RemoveAt(0); }
    }

    void Update()
    {
        if (TimeTesting == 10)
        {
            if (MonsterIsSpawn == false)
            {
                MonsterIsSpawn = true;
                GameObject[] Rooms = GameObject.FindGameObjectsWithTag("Room");
                //расставляем противников
                for (int i = 1; i < AllMonsterQuantity.Length; i++)
                {
                    while (AllMonsterQuantity[i] > 0)
                    {
                        int G = Random.Range(0, Rooms.Length - 1);
                        if (Rooms[G].GetComponent<RoomScript>().BossRoom == false)
                        {
                            Rooms[G].GetComponent<RoomScript>().MonstersQuantity[i] += 1; AllMonsterQuantity[i] -= 1;
                        }
                    }
                }
                for (int i = 0; i < Rooms.Length; i++)
                {
                    if (Rooms[i].GetComponent<RoomScript>().BossRoom == false)
                    {
                        Rooms[i].GetComponent<RoomScript>().MonstersQuantity[0] = Random.Range(1, 5);
                        Rooms[i].GetComponent<RoomScript>().SpawnMonster();
                    }
                    else
                    {
                        Rooms[i].GetComponent<RoomScript>().SpawnBoss();
                    }
                }
            }
            EventSystem.SetActive(true);
            LoadingTable.SetActive(false);
            LoadingCircle.SetActive(false);
        }
        else
        {
            EventSystem.SetActive(false);
            LoadingTable.SetActive(true);
            LoadingCircle.SetActive(true);
            LoadingCircle.transform.Rotate(0, 0, 1);
        }
        if (TimeTesting > 0 && TimeTesting < 5) { TimeTesting -= Time.deltaTime; }
        if (TimeTesting <= 0)
        {
            bool Despned = false;
            for (int i =0;i< SpawnedRoom.Count; i++)
            {
                for (int g = 0; g < SpawnedRoom.Count; g++)
                {
                    if (SpawnedRoom[i] == SpawnedRoom[g] && g!= i)
                    {
                        DeSpawn();
                        Despned = true;
                    }
                }
            }
            GameObject[] Rooms = GameObject.FindGameObjectsWithTag("Room");
            bool Branch = false;
            for (int i = 0; i < Rooms.Length; i++)
            {
                if (Rooms[i].GetComponent<RoomScript>().Branching == true)
                {
                    Branch = true;
                }
            }
            if (Branch == false)
            {
                DeSpawn();
                Despned = true;
            }
            if (Despned == false)
            {
                TimeTesting = 10;
                GameObject BossRoom = null;
                for (int i = 0; i < Rooms.Length; i++)//выбираем комнату с боссом и комнаты с сундучками
                {
                    if (Rooms[i].GetComponent<RoomScript>().EndRoom == true) { BossRoom = Rooms[i]; }
                }
                for (int i = 0; i < BossRoomPrefabs.Length; i++)
                {
                    if (BossRoomPrefabs[i].GetComponent<RoomScript>().Entrance == BossRoom.GetComponent<RoomScript>().Entrance)
                    {
                        Instantiate(BossRoomPrefabs[i], new Vector2(BossRoom.GetComponent<RoomScript>().IntPos.x * WidthRoom, BossRoom.GetComponent<RoomScript>().IntPos.y * HeightRoom), transform.rotation);
                        Destroy(BossRoom);
                        i = BossRoomPrefabs.Length;
                    }
                }
            }      
        }
    }
    void DeSpawn()
    {
        while (SpawnedRoom.Count > 0) { SpawnedRoom.RemoveAt(0); }
        SpawnedRoom.Add(new Vector2Int( 0, 0));
        GameObject[] Rooms = GameObject.FindGameObjectsWithTag("Room");
        for (int i = 0; i < Rooms.Length; i++) { Destroy(Rooms[i]);}
        GameObject StartRoom = GameObject.FindWithTag("StartRoom");
        StartRoom.GetComponent<RoomScript>().SpawnNextRoom();
        TimeTesting = 1;
    }
}
