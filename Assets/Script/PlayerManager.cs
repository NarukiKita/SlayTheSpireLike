using UnityEngine;

public class PlayerManager : MonoBehaviour
{
// Inspector========================================================================================================

    [SerializeField] private int maxHP = 50;
    [SerializeField] private int playerHP = 50;

    [SerializeField] private int playerShield = 0;

    [SerializeField] private int strength = 0;

    [SerializeField] private int maxEnergy = 3;
    [SerializeField] private int currentEnergy = 3;

    [SerializeField] private int cardsPlayedThisTurn = 0;
    [SerializeField] private int attacksPlayedThisTurn = 0;



//=====================================================================================================================

//変数==============================================================================================================

    // HP
    public int HP => playerHP;
    public int MaxHP => maxHP;

    // Shield
    public int Shield => playerShield;

    // Strength
    public int Strength => strength;

    // Energy
    public int CurrentEnergy => currentEnergy;
    public int MaxEnergy => maxEnergy;

    //count
    public int CardsPlayedThisTurn => cardsPlayedThisTurn;
    public int AttacksPlayedThisTurn => attacksPlayedThisTurn;


//==================================================================================================================

// Energyシステム======================================================================================================

    public void StartTurn()
    {
        currentEnergy = maxEnergy;
    }

    public bool CanUseCard(Card card)
    {
        return currentEnergy >= GetCardCost(card);
    }

    public void SpendEnergy(Card card)
    {
        currentEnergy -= GetCardCost(card);
    }

    private int GetCardCost(Card card)
    {
        if (card.freeWhenEnergyZero && CurrentEnergy == 0)
        {
            return 0;
        }

        return card.cost;
    }

//========================================================================================================================


// Shield付与============================================================================================================

    public void AddShield(int amount)
    {
        playerShield += amount;
    }

    public void ResetShield()
    {
        playerShield = 0;
    }

//=========================================================================================================================

//ClearShield=============================================================================================================
//========================================================================================================================

// HP回復==================================================================================================================

    public void Heal(int amount)
    {
        playerHP += amount;

        if (playerHP > maxHP)
        {
            playerHP = maxHP;
        }
    }

//============================================================================================================================

//ダメージ======================================================================================================================

    public void TakeDamage(int damage)
    {
        if (playerShield >= damage)
        {
            playerShield -= damage;
            damage = 0;
        }
        else
        {
            damage -= playerShield;
            playerShield = 0;
        }

        playerHP -= damage;
    }

//================================================================================================================================

// Strength付与=================================================================================================================

    public void AddStrength(int amount)
    {
        strength += amount;
    }

    public void ResetStrength()
    {
        strength = 0;
    }

//===================================================================================================================================

//Energy回復==================================================================================================================

    public void AddEnergy(int amount)
    {
        currentEnergy += amount;

        if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
    }

//============================================================================================================================

// 死亡判定====================================================================================================================

    public bool IsDead()
    {
        return playerHP <= 0;
    }

//===========================================================================================================================

//Card Play Count===============================================================================================================
    
    //PlayCount Turn
    public void RecordCardPlayed()
    {
        cardsPlayedThisTurn++;
        Debug.Log("このターンの使用枚数：" + cardsPlayedThisTurn);
    }

    public void ResetCardsPlayedThisTurn()
    {
        cardsPlayedThisTurn = 0;
    }

    //AttackCount 
    public void RecordAttackPlayed()
    {
        attacksPlayedThisTurn++;
    }

    public void ResetAttacksPlayedThisTurn()
    {
        attacksPlayedThisTurn = 0;
    }
//====================
}
