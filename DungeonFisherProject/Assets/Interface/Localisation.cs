using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Localisation : MonoBehaviour
{
    public Text[] AllInterfaceText;
    public FishingMiniGameScript FishingG;
    public string[] GunND_BY;
    public string[] GunND_EG;
    public string[] GunND_RU;
    public string[] MonsterND_BY;
    public string[] MonsterND_EG;
    public string[] MonsterND_RU;
    public string[] TrashND_BY;
    public string[] TrashND_EG;
    public string[] TrashND_RU;
    public string[] PerckND_BY;
    public string[] PerckND_EG;
    public string[] PerckND_RU;
    public string[] StimulatorND_BY_EG_RU;
    public string[] QuantityAndHaveND_BY_EG_RU;
    public string[] ShellND_BY;
    public string[] ShellND_EG;
    public string[] ShellND_RU;

    // Start is called before the first frame update
    void Start()
    {
        Belarusian();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Belarusian()
    {
        FishingG.NameAndDescriptionGuns = GunND_BY;
        FishingG.NameAndDescriptionMonsters = MonsterND_BY;
        FishingG.NameAndDescriptionTrash = TrashND_BY;
        FishingG.NameAndDescriptionPercks = PerckND_BY;
        FishingG.NameAndDescriptionStimulants[0] = StimulatorND_BY_EG_RU[0];
        FishingG.NameAndDescriptionStimulants[1] = StimulatorND_BY_EG_RU[1];
        AllInterfaceText[0].text = "гуляць";
        AllInterfaceText[1].text = "налады";
        AllInterfaceText[2].text = "у падводны горад";
        AllInterfaceText[3].text = "рыбачыць";
        AllInterfaceText[4].text = "назад у меню";
        AllInterfaceText[5].text = "далей";
        FishingG.NonText = "Нічога";
        FishingG.QuantityAndHave[0] = QuantityAndHaveND_BY_EG_RU[0];
        FishingG.QuantityAndHave[1] = QuantityAndHaveND_BY_EG_RU[1];
        FishingG.ShellDescriptions = ShellND_BY;
    }
    public void English()
    {
        FishingG.NameAndDescriptionGuns = GunND_EG;
        FishingG.NameAndDescriptionMonsters = MonsterND_EG;
        FishingG.NameAndDescriptionTrash = TrashND_EG;
        FishingG.NameAndDescriptionPercks = PerckND_EG;
        FishingG.NameAndDescriptionStimulants[0] = StimulatorND_BY_EG_RU[2];
        FishingG.NameAndDescriptionStimulants[1] = StimulatorND_BY_EG_RU[3];
        AllInterfaceText[0].text = "play";
        AllInterfaceText[1].text = "settings";
        AllInterfaceText[2].text = "enter to dangeon";
        AllInterfaceText[3].text = "fishing";
        AllInterfaceText[4].text = "next to menu";
        AllInterfaceText[5].text = "next";
        FishingG.NonText = "nothing";
        FishingG.QuantityAndHave[0] = QuantityAndHaveND_BY_EG_RU[2];
        FishingG.QuantityAndHave[1] = QuantityAndHaveND_BY_EG_RU[3];
        FishingG.ShellDescriptions = ShellND_EG;
    }
    public void Russian()
    {
        FishingG.NameAndDescriptionGuns = GunND_RU;
        FishingG.NameAndDescriptionMonsters = MonsterND_RU;
        FishingG.NameAndDescriptionTrash = TrashND_RU;
        FishingG.NameAndDescriptionPercks = PerckND_RU;
        FishingG.NameAndDescriptionStimulants[0] = StimulatorND_BY_EG_RU[4];
        FishingG.NameAndDescriptionStimulants[1] = StimulatorND_BY_EG_RU[5];
        AllInterfaceText[0].text = "играть";
        AllInterfaceText[1].text = "настройки";
        AllInterfaceText[2].text = "в подводный город";
        AllInterfaceText[3].text = "рыбачить";
        AllInterfaceText[4].text = "назад в меню";
        AllInterfaceText[5].text = "далее";
        FishingG.NonText = "Ничего";
        FishingG.QuantityAndHave[0] = QuantityAndHaveND_BY_EG_RU[4];
        FishingG.QuantityAndHave[1] = QuantityAndHaveND_BY_EG_RU[5];
        FishingG.ShellDescriptions = ShellND_RU;
    }

}
