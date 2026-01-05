using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public class FarmingManager : MonoBehaviour
{
    private GameObject player;
    private Stats playerStats;
    private TileDataManager tdm;
    private XPController xpc;

    public TileManager tileManager;

    public List<Sprite> seed1GrowthStages;
    public float seed1GrowthTime = 360f;

    public List<Sprite> seed2GrowthStages;
    public float seed2GrowthTime = 720f;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerStats = player.GetComponent<Stats>();
        tdm = FindFirstObjectByType<TileDataManager>();
        tileManager = FindFirstObjectByType<TileManager>();
        xpc = FindFirstObjectByType<XPController>();

        seed1GrowthTime *= playerStats.Crop_Growth;
        seed2GrowthTime *= playerStats.Crop_Growth;
    }

    public void CanInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValue<float>() == 0) return;

        if (tileManager.hbc.usingPlant1 && tileManager.hbc.inventory.GetItemAt(tileManager.hbc.SelectedIndex).quantity > 0) 
        {
            tileManager.hbc.UseItem();
            playerStats.Health_Current += 15;
            xpc.PlantXp1();
        }
        else if (tileManager.hbc.usingPlant2 && tileManager.hbc.inventory.GetItemAt(tileManager.hbc.SelectedIndex).quantity > 0) 
        {
            tileManager.hbc.UseItem();
            playerStats.Health_Current += 25;
            xpc.PlantXp2();
        }
        else
        {
            if (TimeManager.instance.tileManager.IsInteractable(player.transform.position))
            {
                TimeManager.instance.tileManager.SetInteracted(player.transform.position);
            }
            else
            {
                Debug.Log("nah twn");
            }
        }       
    }

    public IEnumerator GrowSeed1()
    {
        Vector3Int cellPos = tileManager.interactableMap.WorldToCell(transform.position);

        if (!tileManager.farmTileSprites.TryGetValue(cellPos, out GameObject farmSpriteObj) || farmSpriteObj == null)
        {
            Debug.LogWarning($"no farm sprite for cell {cellPos}");
            yield break;
        }

        var farmSpriteRenderer = farmSpriteObj.GetComponent<SpriteRenderer>();

        int stageCount = seed1GrowthStages?.Count ?? 0;
        if (stageCount == 0)
        {
            Debug.LogWarning("No seed1 growth stages assigned");
            yield break;
        }

        float stageGrowthTime = seed1GrowthTime / stageCount;
        farmSpriteRenderer.enabled = true;

        for (int i = 0; i < stageCount; i++)
        {
            farmSpriteRenderer.sprite = seed1GrowthStages[i];
            yield return new WaitForSeconds(stageGrowthTime);
        }

        tdm.TriggerBool(cellPos, 2);
        Debug.Log("plant.");     
        yield break;
    }

    public IEnumerator GrowSeed2()
    {
        Vector3Int cellPos = tileManager.interactableMap.WorldToCell(transform.position);

        if (!tileManager.farmTileSprites.TryGetValue(cellPos, out GameObject farmSpriteObj) || farmSpriteObj == null)
        {
            Debug.LogWarning($"no farm sprite for cell {cellPos}");
            yield break;
        }

        var farmSpriteRenderer = farmSpriteObj.GetComponent<SpriteRenderer>();

        int stageCount = seed1GrowthStages?.Count ?? 0;
        if (stageCount == 0)
        {
            Debug.LogWarning("No seed1 growth stages assigned");
            yield break;
        }

        float stageGrowthTime = seed1GrowthTime / stageCount;
        farmSpriteRenderer.enabled = true;

        for (int i = 0; i < stageCount; i++)
        {
            farmSpriteRenderer.sprite = seed1GrowthStages[i];
            yield return new WaitForSeconds(stageGrowthTime);
        }

        Debug.Log(".tnalp");
        tdm.TriggerBool(cellPos, 2);
        yield return null;
    }
}
