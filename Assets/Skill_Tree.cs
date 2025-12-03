using UnityEngine;
using UnityEngine.SceneManagement;

public class Skill_Tree : MonoBehaviour
{
    public Player_Movement player_Movement;
    public Enemy_AI enemy_AI;
    public Stats stats;

    public void Movement()
    {
        player_Movement.movespeed = player_Movement.movespeed + 1;
    }
    public void Enemy_Speed_and_Move_Speed()
    {
        player_Movement.movespeed = player_Movement.movespeed + 2;
        enemy_AI.movespeed = enemy_AI.movespeed + 2;
    }
    public void Move_Speed()
    {
        player_Movement.movespeed = player_Movement.movespeed + 1;
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
    public void Attack()
    {

    }
    public void Attack_Power()
    {
        stats.Attack_Power =+ 10;
    }
    public void Attack_Speed()
    {stats.Attack_Speed =+ 1;

    }
    public void Knockback()
    {

    }
    public void Return()
    {
        SceneManager.LoadScene("Shu's scene");
    }

}
