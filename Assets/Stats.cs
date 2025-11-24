using UnityEngine;

public class Stats : MonoBehaviour
{
    public float Health_Max;
    public float Health_Current;
    public float Attack_Speed;
    public float Attack_Power;
    public float Defense;
    public float Crop_Exp;
    public float Crop_Growth;
    public float Speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Health_Max = 10f;
        Health_Current = 10f;
        Attack_Speed = 1f;
        Attack_Power = 1f;
        Defense = 1f;
        Crop_Exp = 1f;
        Crop_Growth = 1f;
        Speed = 10f;
}

    // Update is called once per frame
    void Update()
    {
        
    }
}
