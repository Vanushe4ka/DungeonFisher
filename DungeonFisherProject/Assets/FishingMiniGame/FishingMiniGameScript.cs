using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FishingMiniGameScript : MonoBehaviour
{
    private bool StartMenu;
    public Image[] MenuElements;
    public Text[] MenuText;
    private float TransparencyeMenu;
    private float TargetTransparencyeMenu;
    public GameObject Menu;
    public GameObject Camera;
    public Transform CamTargetPosition;

    public Transform Cursor;
    public float Neudachnost;
    private int NeudachnostTarget =1;
    public bool Fishing = true;

    public Animator Ranger;

    public float RandomPercent;
    public float MonsterDropChance;
    public float GunDropChance;
    public float PerckDropChance;
    public float StimulantDropChance;
    public float TrachDropChance;

    public int FishingGearQuantity;
    public Text QuantityText;

    public GameObject Catch;
    public Vector2[] CatchPositions;
    public int[] CatchRotations;
    public int NumberCatchPosition;
    public Sprite[] Guns;
    public Sprite[] Monsters;
    public Sprite[] Percks;
    public Sprite StimulatorSprite;
    public Sprite[] Trachs;
    
    [Header("Descriptions")]
    public GameObject[] FishingGameButtons;
    public Text[] FishingButtonText;
    public SpriteRenderer[] FishingBarImage;
    public GameObject DescriptionTable;
    public Button DescriptionButton;
    public Image[] DescriptionImages;
    public Text[] DescriptionsText;//0-name,1-description,2-Quantity,3-ButtonText
    public string[] NameAndDescriptionGuns;
    public string[] NameAndDescriptionMonsters;
    public string[] NameAndDescriptionTrash;
    public string[] NameAndDescriptionStimulants;
    public string[] NameAndDescriptionPercks;
    public bool Description;
    public string NonText;
    public Sprite NotingSprite;
    public string[] QuantityAndHave;
    public string[] ShellDescriptions;
    public int[] ShellPlusQuantityInType;

    private int[] PercksQuantity = new int[7];
    [Header("Roge Like Game")]
    public RangerScript RangerInRogeLike;
    public DangeonCreater CreaterDangeon;
    public GameObject RogeLikeGame;
    public GameObject FishingGame;

    // Start is called before the first frame update
    void Start()
    {
        QuantityText.text = "" + FishingGearQuantity;
    }
    void Update()
    {
        Vector2 CamPos = Vector2.Lerp(Camera.transform.position, CamTargetPosition.position, 0.01f);
        Camera.transform.position = new Vector3(CamPos.x, CamPos.y, -10);
        if (Camera.transform.position.y < 0.1f && StartMenu == false) { TargetTransparencyeMenu = 1; StartMenu = true; }
        TransparencyeMenu = Mathf.Lerp(TransparencyeMenu, TargetTransparencyeMenu, Time.deltaTime*1.5f);
        if (TransparencyeMenu < 0.05f) { Menu.SetActive(false); } else { Menu.SetActive(true); }
        for (int i = 0; i < MenuElements.Length; i++)
        {
            MenuElements[i].color = new Color(MenuElements[i].color.r, MenuElements[i].color.g, MenuElements[i].color.b, TransparencyeMenu);
            MenuText[i].color = new Color(MenuText[i].color.r, MenuText[i].color.g, MenuText[i].color.b, TransparencyeMenu);
        }
        if (Fishing == true)
        {
            if (NeudachnostTarget == 1) { Neudachnost += Time.deltaTime * 4 * ((1 - Mathf.Abs(Neudachnost))*1.5f); } else { Neudachnost -= Time.deltaTime * 4 * ((1 - Mathf.Abs(Neudachnost)) * 1.5f); }
            if (NeudachnostTarget == 1 && Neudachnost > 0.9f) { NeudachnostTarget = -1; }
            if (NeudachnostTarget == -1 && Neudachnost < -0.9) { NeudachnostTarget = 1; }
        }
        Cursor.localPosition = new Vector2(0, Neudachnost / 10);

        if (Description == true)
        {
            DescriptionTable.SetActive(true);
            if (FishingGameButtons[0].GetComponent<Image>().color.a > 0)
            {
                for (int i =0;i<FishingGameButtons.Length; i++)
                {
                    FishingGameButtons[i].GetComponent<Button>().enabled = false;
                    FishingGameButtons[i].GetComponent<Image>().color = new Color(FishingGameButtons[i].GetComponent<Image>().color.r, FishingGameButtons[i].GetComponent<Image>().color.g, FishingGameButtons[i].GetComponent<Image>().color.b, FishingGameButtons[0].GetComponent<Image>().color.a - Time.deltaTime*2);
                    FishingBarImage[i].color = new Color(FishingBarImage[i].color.r, FishingBarImage[i].color.g, FishingBarImage[i].color.b, FishingGameButtons[0].GetComponent<Image>().color.a - Time.deltaTime * 2);
                }
                for (int i = 0; i < FishingButtonText.Length; i++)
                {
                    FishingButtonText[i].color = new Color(FishingButtonText[i].color.r, FishingButtonText[i].color.g, FishingButtonText[i].color.b, FishingGameButtons[0].GetComponent<Image>().color.a + Time.deltaTime * 2);
                }
            }
            if (DescriptionImages[0].GetComponent<Image>().color.a < 1)
            {
                for (int i = 0; i < DescriptionImages.Length; i++)
                {
                    DescriptionImages[i].GetComponent<Image>().color = new Color(DescriptionImages[i].GetComponent<Image>().color.r, DescriptionImages[i].GetComponent<Image>().color.g, DescriptionImages[i].GetComponent<Image>().color.b, DescriptionImages[0].GetComponent<Image>().color.a + Time.deltaTime * 2);
                    DescriptionButton.enabled = true;
                }
                for (int i = 0; i < DescriptionsText.Length; i++)
                {
                    DescriptionsText[i].color = new Color(DescriptionsText[i].color.r, DescriptionsText[i].color.g, DescriptionsText[i].color.b, DescriptionsText[0].color.a + Time.deltaTime * 2);
                }
            }
        }
        else
        {
            
            if (FishingGameButtons[0].GetComponent<Image>().color.a < 1)
            {
                for (int i = 0; i < FishingGameButtons.Length; i++)
                {
                    FishingGameButtons[i].GetComponent<Button>().enabled = true;
                    FishingGameButtons[i].GetComponent<Image>().color = new Color(FishingGameButtons[i].GetComponent<Image>().color.r, FishingGameButtons[i].GetComponent<Image>().color.g, FishingGameButtons[i].GetComponent<Image>().color.b, FishingGameButtons[0].GetComponent<Image>().color.a + Time.deltaTime * 2);
                    FishingBarImage[i].color = new Color(FishingBarImage[i].color.r, FishingBarImage[i].color.g, FishingBarImage[i].color.b, FishingGameButtons[0].GetComponent<Image>().color.a + Time.deltaTime * 2);
                }
                for (int i = 0; i < FishingButtonText.Length; i++)
                {
                    FishingButtonText[i].color = new Color(FishingButtonText[i].color.r, FishingButtonText[i].color.g, FishingButtonText[i].color.b, FishingGameButtons[0].GetComponent<Image>().color.a + Time.deltaTime * 2);
                }
            }
            if (DescriptionImages[0].GetComponent<Image>().color.a > 0)
            {
                for (int i = 0; i < DescriptionImages.Length; i++)
                {
                    DescriptionImages[i].GetComponent<Image>().color = new Color(DescriptionImages[i].GetComponent<Image>().color.r, DescriptionImages[i].GetComponent<Image>().color.g, DescriptionImages[i].GetComponent<Image>().color.b, DescriptionImages[0].GetComponent<Image>().color.a - Time.deltaTime * 2);
                    DescriptionButton.enabled = false;
                }
                for (int i = 0; i < DescriptionsText.Length; i++)
                {
                    DescriptionsText[i].color = new Color(DescriptionsText[i].color.r, DescriptionsText[i].color.g, DescriptionsText[i].color.b, DescriptionsText[0].color.a - Time.deltaTime * 2);
                }
            }
            else { DescriptionTable.SetActive(false); }
        }
    }
    public void ExitInDescription()
    {
        Description = false;
    }
    public void ToPlay()
    {
        CamTargetPosition.position = new Vector2(0.7f, 0);
        TargetTransparencyeMenu = 0;
    }
    public void ToSetting()
    {
        CamTargetPosition.position = new Vector2(-0.4f, -1.5f);
        TargetTransparencyeMenu = 0;
    }
    public void ToMenu()
    {
        CamTargetPosition.position = new Vector2(-0.4f, 0);
        TargetTransparencyeMenu = 1;
    }
    public void FishingButton()
    {
        if (FishingGearQuantity > 0 && Fishing == true)//EntertToFishing
        {
            Description = true;
            FishingGearQuantity -= 1;
            QuantityText.text = "" + FishingGearQuantity;
            Fishing = false;
            Ranger.SetTrigger("Fishing");
            RandomPercent = Random.Range(1f, 215f);//процент выпадения предмета с учётом того что может ничего не выпасть
            float MonsterChance = MonsterDropChance + Mathf.Abs(Neudachnost) * 100;
            float GunChance = GunDropChance + ((1 - Mathf.Abs(Neudachnost)) / 3) * 100;
            float PerckChance = PerckDropChance + ((1 - Mathf.Abs(Neudachnost)) / 3) * 100;
            float StimulChance = StimulantDropChance + ((1 - Mathf.Abs(Neudachnost)) / 3) * 100; ;
            float TrachChance = TrachDropChance;
            if (RandomPercent >= 0 && RandomPercent <= MonsterChance)//Drop Monster
            {
                Debug.Log("Monster");
                int Number = Random.Range(1, CreaterDangeon.AllMonsterQuantity.Length);
                int Quantity = Random.Range(1, 8);
                CreaterDangeon.AllMonsterQuantity[Number] += Quantity;
                Catch.GetComponent<SpriteRenderer>().sprite = Monsters[Number];
                DescriptionImages[0].GetComponent<Image>().sprite = Monsters[Number];
                DescriptionsText[0].text = NameAndDescriptionMonsters[Number * 2];
                DescriptionsText[1].text = NameAndDescriptionMonsters[(Number * 2)+1];
                DescriptionsText[2].text = QuantityAndHave[0] + ": " + Quantity;
            }
            if (RandomPercent > MonsterChance && RandomPercent <= GunChance + MonsterChance)// Drop Gun
            {
                Debug.Log("Gun");
                int Number = Random.Range(1, RangerInRogeLike.Guns.Length);
                if (RangerInRogeLike.PresentGun.Contains(Number) == true)
                {
                    Catch.GetComponent<SpriteRenderer>().sprite = Percks[0];
                    DescriptionImages[0].GetComponent<Image>().sprite = Percks[0];
                    DescriptionsText[0].text = NameAndDescriptionPercks[0];
                    int R = Random.Range(0, 2);
                    if (R == 0)//всех патронов по чуть-чуть
                    {
                        for (int i = 1; i < RangerInRogeLike.ShellsQuantity.Length; i++)
                        {
                            RangerInRogeLike.ShellsQuantity[i] += ShellPlusQuantityInType[i];
                           
                        }
                        DescriptionsText[1].text = ShellDescriptions[0];
                        DescriptionsText[2].text = "";

                    }
                    if (R == 1)//один тип патронов +
                    {
                        int Type = Random.Range(1, RangerInRogeLike.ShellsQuantity.Length);
                        RangerInRogeLike.ShellsQuantity[Type] += ShellPlusQuantityInType[Type] * 2;
                        DescriptionsText[1].text = ShellDescriptions[Type];
                        DescriptionsText[2].text = QuantityAndHave[1] + ": " + RangerInRogeLike.ShellsQuantity[Type];
                    }
                }
                else
                {
                    RangerInRogeLike.PresentGun.Add(Number);
                    
                    RangerInRogeLike.ShellsQuantity[RangerInRogeLike.ShellTypeInGun[Number]] += ShellPlusQuantityInType[RangerInRogeLike.ShellTypeInGun[Number]] * 2;
                    Catch.GetComponent<SpriteRenderer>().sprite = Guns[Number];
                    DescriptionImages[0].GetComponent<Image>().sprite = Guns[Number];
                    DescriptionsText[0].text = NameAndDescriptionGuns[Number * 2];
                    DescriptionsText[1].text = NameAndDescriptionGuns[(Number * 2) + 1];
                    DescriptionsText[2].text = "";
                }
                
            }
            if (RandomPercent > MonsterChance + GunChance && RandomPercent <= GunChance + MonsterChance + PerckChance)// Drop Perck
            {
                Debug.Log("Perck");
                int Number = Random.Range(0, Percks.Length);
                
                Catch.GetComponent<SpriteRenderer>().sprite = Percks[Number];
                DescriptionImages[0].GetComponent<Image>().sprite = Percks[Number];
                DescriptionsText[0].text = NameAndDescriptionPercks[Number*2];
                DescriptionsText[1].text = NameAndDescriptionPercks[(Number*2) + 1];
                PercksQuantity[Number] += 1;
                DescriptionsText[2].text = QuantityAndHave[1] + ": "+PercksQuantity[Number];
                if (Number == 0)//Patron Perck
                {
                    int R = Random.Range(0, 2);
                    if (R == 0)//всех патронов по чуть-чуть
                    {
                        for (int i = 1; i < RangerInRogeLike.ShellsQuantity.Length; i++)
                        {
                            RangerInRogeLike.ShellsQuantity[i] += ShellPlusQuantityInType[i];
                           
                        }
                        DescriptionsText[1].text = ShellDescriptions[0];
                        DescriptionsText[2].text = "";

                    }
                    if (R == 1)//один тип патронов +
                    {
                        int Type = Random.Range(1, RangerInRogeLike.ShellsQuantity.Length);
                        RangerInRogeLike.ShellsQuantity[Type] += ShellPlusQuantityInType[Type] * 2;
                        DescriptionsText[1].text = ShellDescriptions[Type];
                        DescriptionsText[2].text = QuantityAndHave[1] + ": " + RangerInRogeLike.ShellsQuantity[Type];
                    }
                   
                }
                if (Number == 1)//Damage Perck
                {

                    for (int i = 0; i < RangerInRogeLike.Guns.Length; i++) { RangerInRogeLike.Guns[i].GetComponent<GunsScript>().Shell.GetComponent<ShellScript>().Damage = Mathf.RoundToInt(1.5f * RangerInRogeLike.Guns[i].GetComponent<GunsScript>().Shell.GetComponent<ShellScript>().Damage); }
                }
                if (Number == 2)//Quantity Shell Perck
                {
                    for (int i =0;i< RangerInRogeLike.Guns.Length; i++) { RangerInRogeLike.Guns[i].GetComponent<GunsScript>().SellQuantity = RangerInRogeLike.Guns[i].GetComponent<GunsScript>().SellQuantity * 2;  }
                }
                if (Number == 3) {if (RangerInRogeLike.Armor < 3) { RangerInRogeLike.Armor += 1; } }
                if (Number == 4)//Time Perck
                {
                    if (Time.timeScale > 0.2) { Time.timeScale -= 0.1f; }
                    RangerInRogeLike.Speed = Mathf.RoundToInt(1.1f * RangerInRogeLike.Speed); ;
                    RangerInRogeLike.AnimSpeed = RangerInRogeLike.AnimSpeed * 1.1f;
                    Ranger.SetFloat("Speed", Ranger.GetFloat("Speed") * 1.1f);
                }
                if (Number == 5)//To4nost Perck
                {
                    for (int i = 0; i < RangerInRogeLike.Guns.Length; i++) { RangerInRogeLike.Guns[i].GetComponent<GunsScript>().Scatter = RangerInRogeLike.Guns[i].GetComponent<GunsScript>().Scatter / 2; }
                }
                if (Number == 6)//Speed Perck
                {
                    RangerInRogeLike.Speed = Mathf.RoundToInt( 1.25f * RangerInRogeLike.Speed) ;
                    RangerInRogeLike.AnimSpeed = RangerInRogeLike.AnimSpeed * 1.25f;
                }
            }
            if (RandomPercent > GunChance + MonsterChance + PerckChance && RandomPercent <= GunChance + MonsterChance + PerckChance + StimulChance)// Drop Stimul 
            {
                Debug.Log("Stimul");
                int StimulantQuantity = Random.Range(1, 5);
                RangerInRogeLike.Stimulators += StimulantQuantity;
                Catch.GetComponent<SpriteRenderer>().sprite = StimulatorSprite;
                DescriptionImages[0].GetComponent<Image>().sprite = StimulatorSprite;
                DescriptionsText[0].text = NameAndDescriptionStimulants[0];
                DescriptionsText[1].text = NameAndDescriptionStimulants[1];
                DescriptionsText[2].text = QuantityAndHave[0]+ ": "  + StimulantQuantity;
            }
            if (RandomPercent > GunChance + MonsterChance + PerckChance + StimulChance && RandomPercent <= GunChance + MonsterChance + PerckChance + StimulChance + TrachChance)// Drop Trach
            {
                Debug.Log("Trach");
                int Number = Random.Range(0, Trachs.Length);
                Catch.GetComponent<SpriteRenderer>().sprite = Trachs[Number];
                DescriptionImages[0].GetComponent<Image>().sprite = Trachs[Number];
                DescriptionsText[0].text = NameAndDescriptionTrash[Number * 2];
                DescriptionsText[1].text = NameAndDescriptionTrash[(Number * 2) + 1];
                DescriptionsText[2].text = "";
                CreaterDangeon.TrashForSpawn.Add(Number);
            }
            if (RandomPercent > GunChance + MonsterChance + PerckChance + StimulChance + TrachChance)//non
            {
                DescriptionImages[0].GetComponent<Image>().sprite = NotingSprite;
                DescriptionsText[0].text = NonText;
                DescriptionsText[1].text = "";
                DescriptionsText[2].text = "";
            }
        }
        else//EnterToPlay
        {
            EnterToDangeon();
        }
    }
    public void EndFishing()
    {
        Fishing = true;
        Ranger.ResetTrigger("Fishing");
    }
    public void EnterToDangeon()
    {
        RogeLikeGame.SetActive(true);
        FishingGame.SetActive(false);
    }
    
}
