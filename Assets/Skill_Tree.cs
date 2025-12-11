using UnityEngine;
using UnityEngine.SceneManagement;

public class Skill_Tree : MonoBehaviour
{
    private SaveData sd;

    public Player_Movement player_Movement;
    public Enemy_AI enemy_AI;
    public Stats stats;
    public SpawnHitBox spawnHitBox;

    private void Awake()
    {
        sd = GameObject.FindGameObjectWithTag("SaveData").GetComponent<SaveData>();
    }

    public void Movement()
    {
        player_Movement.movespeed += 1;
        sd.SavePlayerData();
    }
    public void Enemy_Speed_and_Move_Speed()
    {
        stats.Speed += 2;
        enemy_AI.movespeed += 2;
        sd.SavePlayerData();
    }
    public void Move_Speed()
    {
        stats.Speed += 1;
        sd.SavePlayerData();
    }
    public void Farming()
    {

    }
    public void EXP_Gain()
    {

    }
    public void New_Plant()
    {

    }
    public void Grow_Speed()
    {

    }
    public void Grow_SpeedE()
    {

    }
    public void Grow_SpeedS()
    {

    }
    public void Attack()
    {
        stats.Attack_Power += 10;
        stats.Attack_Speed += 1;
        sd.SavePlayerData();
    }
    public void Attack_Power()
    {
        stats.Attack_Power += 10;
        sd.SavePlayerData();
    }
    public void Defense()
    {
        stats.Defense += + 7;
        sd.SavePlayerData();
    }
    public void Knockback()
    {
        spawnHitBox.knockbackForce += 50;
        sd.SavePlayerData();
    }
    public void Stun()
    {
        spawnHitBox.stunDuration += 1;
        sd.SavePlayerData();
    }
    public void Return(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        sd.LoadPlayerData();
    }

}
