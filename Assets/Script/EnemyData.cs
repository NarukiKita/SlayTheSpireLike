using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public int maxHP = 30;
    public int attackPower = 5;
    public int shieldPower = 5;
    public int buffAmount = 2;

    public int attackWeight = 70;
    public int defendWeight = 30;
    public int poisonWeight = 0;
    public int strongAttackWeight = 0;
    public int doubleAttackWeight = 0;
    public int buffWeight = 0;
}
