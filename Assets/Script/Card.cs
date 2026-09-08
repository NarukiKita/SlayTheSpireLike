using UnityEngine;

public enum EffectType
{
    Attack,
    Defense,
    Heal,
    Draw,
    Strength,
    Poison,
    Energy,
    ShieldAttack,
    Retrieve,
    Discard,
    CardsPlayedDamage,
    AttackPlayedDamage,
    Execute
}

public enum ConditionType
{
    None,
    PlayerHP,
    EnemyHP,
    CardsPlayedThisTurn
}

public enum ConditionOperator
{
    LessEqual,
    GreaterEqual
}



[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public string effectText;
    public int cost;
    public bool freeWhenEnergyZero = false;
    public CardEffect[] effects; 
    public CardEffect[] discardFollowUpEffects;
    public bool HasEffect(EffectType effectType)
    {
        foreach (CardEffect effect in effects)
        {
            if (effect.effectType == effectType)
            {
                return true;
            }
        }

        return false;
    }   
}
