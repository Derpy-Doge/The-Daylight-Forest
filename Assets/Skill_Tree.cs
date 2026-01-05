using Inventory.Model;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Skill_Tree : MonoBehaviour
{
    private SaveData sd;

    public Player_Movement player_Movement;
    public Enemy_AI enemy_AI;
    public Stats stats;
    public SpawnHitBox spawnHitBox;
    public XPController xpc;
    public TMP_Text skillPointText;
    [SerializeField] private List<Button> upgrades; //this is gonna be so freakin tedious :sob:
    private static List<bool> purchasedUpgrades = new List<bool>(new bool[14]);    

    [SerializeField] private ItemSO seed2; //for unlock

    private void Awake()
    {
        sd = GameObject.FindGameObjectWithTag("SaveData").GetComponent<SaveData>();
    }

    public void Start()
    {
        xpc = FindAnyObjectByType<XPController>();

        skillPointText.text = "Skill Points: " + xpc.Skill.ToString();

        for (int i = 0; i < upgrades.Count && i < purchasedUpgrades.Count; i++)
        {
            if (purchasedUpgrades[i])
            {
                upgrades[i].interactable = false;
            }
        }
    }

    public void Update()
    {
        if (!TileManager.plant2Unlocked)
        {
            seed2.Name = "Mysterious Seed";
            seed2.Description = "I'm not sure what plant this is yet... I don't know how to grow it";
        }
        else
        {
            seed2.Name = "seed2 uhh idk what to put yet";
            seed2.Description = "Doesn't grow as fast as carrots, but gives more health and a bit more EXP";
        }
    }

    // ok so starting with movement being 0, go down from there
    // so make sure the buttons are in this order in the list
    public void Movement() //0
    {
        if(xpc.Skill == 0)
        {
            return;
        }

        upgrades[0].interactable = false;
        player_Movement.movespeed += 1;

        upgrades[1].interactable = true; // the movement stuff is available now
        upgrades[2].interactable = true;

        purchasedUpgrades[0] = true;
        xpc.SpendXp();
        sd.SavePlayerData();
    }
    public void Enemy_Speed_and_Move_Speed() //1
    {
        if (xpc.Skill == 0)
        {
            return;
        }

        upgrades[1].interactable = false;
        stats.Speed += 2;
        enemy_AI.movespeed += 2;
        purchasedUpgrades[1] = true;
        xpc.SpendXp();
        sd.SavePlayerData();
    }
    public void Move_Speed() //2
    {
        if (xpc.Skill == 0)
        {
            return;
        }

        upgrades[2].interactable = false;
        stats.Speed += 1;
        purchasedUpgrades[2] = true;
        xpc.SpendXp();
        sd.SavePlayerData();
    }
    public void Farming() //3
    {
        if (xpc.Skill == 0)
        {
            return;
        }

        stats.Crop_Growth += .25f;
        stats.Crop_Exp += 5f;
        xpc.Plant1 += stats.Crop_Exp;
        xpc.Plant2 += stats.Crop_Exp;

        upgrades[3].interactable = false;

        upgrades[4].interactable = true;
        upgrades[5].interactable = true;
        upgrades[6].interactable = true;

        purchasedUpgrades[3] = true;
        xpc.SpendXp();
        sd.SavePlayerData();
    }
    public void EXP_Gain() //4 
    {
        if (xpc.Skill == 0)
        {
            return;
        }

        stats.Crop_Exp += 5f; // trust me on this itll add 10 not 5 but if i put the number to 10 itll add 15 :man_juggling:
        xpc.Plant1 += stats.Crop_Exp;
        xpc.Plant2 += stats.Crop_Exp;

        upgrades[4].interactable = false;

        purchasedUpgrades[4] = true;
        xpc.SpendXp();
        sd.SavePlayerData();
    }
    public void New_Plant() //5
    {
        if (xpc.Skill == 0)
        {
            return;
        }

        TileManager.plant2Unlocked = true;

        upgrades[5].interactable = false;

        purchasedUpgrades[5] = true;
        xpc.SpendXp();
        sd.SavePlayerData();
    }
    public void Grow_Speed() //6
    {
        if (xpc.Skill == 0)
        {
            return;
        }

        stats.Crop_Growth += .5f;

        upgrades[6].interactable = false;

        upgrades[7].interactable = true;
        upgrades[8].interactable = true;

        purchasedUpgrades[6] = true;
        xpc.SpendXp();
        sd.SavePlayerData();
    }
    public void Grow_SpeedE() //7
    {
        if (xpc.Skill == 0)
        {
            return;
        }

        stats.Crop_Growth += .5f;

        upgrades[7].interactable = false;

        purchasedUpgrades[7] = true;
        xpc.SpendXp();
        sd.SavePlayerData();
    }
    public void Grow_SpeedS() //8
    {
        if (xpc.Skill == 0)
        {
            return;
        }

        stats.Crop_Growth += .5f;

        upgrades[8].interactable = false;

        purchasedUpgrades[8] = true;
        xpc.SpendXp();
        sd.SavePlayerData();
    }
    public void Attack() //9
    {
        if (xpc.Skill == 0)
        {
            return;
        }

        upgrades[9].interactable = false;
        stats.Attack_Power += 10;

        upgrades[10].interactable = true;
        upgrades[11].interactable = true;
        upgrades[12].interactable = true;
        upgrades[13].interactable = true;

        purchasedUpgrades[9] = true;
        xpc.SpendXp();
        sd.SavePlayerData();
    }
    public void Attack_Power() //10
    {
        if (xpc.Skill == 0)
        {
            return;
        }

        upgrades[10].interactable = false;
        stats.Attack_Power += 10;

        purchasedUpgrades[10] = true;
        xpc.SpendXp();
        sd.SavePlayerData();
    }
    public void Defense() //11
    {
        if (xpc.Skill == 0)
        {
            return;
        }

        upgrades[11].interactable = false;
        stats.Defense += 7;

        purchasedUpgrades[11] = true;
        xpc.SpendXp();
        sd.SavePlayerData();
    }
    public void Knockback() //12
    {
        if (xpc.Skill == 0)
        {
            return;
        }

        upgrades[12].interactable = false;
        spawnHitBox.knockbackForce += 50;

        purchasedUpgrades[12] = true;
        xpc.SpendXp();
        sd.SavePlayerData();
    }
    public void Stun() //13
    {
        if (xpc.Skill == 0)
        {
            return;
        }

        upgrades[13].interactable = false;
        spawnHitBox.stunDuration += 1;

        purchasedUpgrades[13] = true;
        xpc.SpendXp();
        sd.SavePlayerData();
    }
    public void Return(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        sd.LoadPlayerData();
    }

}
