using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FarmingManager : MonoBehaviour
{
    private GameObject player;
    private TileDataManager tdm;

    public TileManager tileManager;

    public List<Sprite> seed1GrowthStages;
    public float seed1GrowthTime = 360f;

    public List<Sprite> seed2GrowthStages;
    public float seed2GrowthTime = 720f;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        tdm = FindFirstObjectByType<TileDataManager>();
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void CanInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValue<float>() == 0) return;

        if(tileManager.hbc.usingPlant1)
        if (TimeManager.instance.tileManager.IsInteractable(player.transform.position))
        {
            TimeManager.instance.tileManager.SetInteracted(player.transform.position);
        }
        else
        {
            Debug.Log("nah twn");
        }
    }

    public IEnumerator GrowSeed1()
    {
        Vector3Int intPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);

        Sprite growthStage = tileManager.farmSprite.GetComponent<Sprite>();
        float stageGrowthTime = seed1GrowthTime / 3;

        yield return new WaitForSeconds(stageGrowthTime);

        growthStage = seed1GrowthStages[1];

        yield return new WaitForSeconds(stageGrowthTime);

        growthStage = seed1GrowthStages[2];

        yield return new WaitForSeconds(stageGrowthTime);

        Debug.Log("plant.");
        tdm.TriggerBool(intPos, 2);
        yield return null;

    }

    public IEnumerator GrowSeed2()
    {
        Vector3Int intPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);

        Sprite growthStage = tileManager.farmSprite.GetComponent<Sprite>();
        float stageGrowthTime = seed2GrowthTime / 3;

        yield return new WaitForSeconds(stageGrowthTime);

        growthStage = seed2GrowthStages[1];

        yield return new WaitForSeconds(stageGrowthTime);

        growthStage = seed2GrowthStages[2];

        yield return new WaitForSeconds(stageGrowthTime);

        tdm.TriggerBool(intPos, 2);
        yield return null;

    }
}
