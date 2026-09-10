using UnityEngine;

public class CardEffectManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private ConditionManager conditionManager;


    private CardEffect[] pendingFollowUpEffects;
    private CardEffect pendingEffect;
    private System.Action pendingOnComplete;
    private Deck deck;

    private void Start()
    {
        uiManager.OnCardSelectedForDiscard += OnCardSelectedForDiscard;
        deck = Deck.Instance;
    }

    public void ApplyEffect(Card card, System.Action onComplete)
    {
        foreach (CardEffect effect in card.effects)
        {
            if (effect.condition != null)
            {
                if (!conditionManager.CheckCondition(effect.condition))
                {
                    continue;
                }
            }

            if (effect.effectType == EffectType.Discard)
            {
                pendingOnComplete = onComplete;
                ApplyDiscardEffect(effect, card.discardFollowUpEffects);
                return;
            }

            ExecuteEffect(effect);


        }
        onComplete?.Invoke();

    }

    //犠牲の一撃でカードを選んだ時の処理
    private void OnCardSelectedForDiscard(Card card)
    {
        deck.DiscardCard(card);

        if (pendingFollowUpEffects != null)
        {
            foreach (CardEffect effect in pendingFollowUpEffects)
            {
                ExecuteEffect(effect);
            }
            pendingFollowUpEffects = null;
        }

        pendingOnComplete?.Invoke();
        pendingOnComplete = null;
    }

    private void ExecuteEffect(CardEffect effect)
    {
        switch (effect.effectType)
        {
            case EffectType.Attack:
                ApplyAttackEffect(effect);
                break;

            case EffectType.Defense:
                ApplyDefenseEffect(effect);
                break;

            case EffectType.Heal:
                ApplyHealEffect(effect);
                break;

            case EffectType.Draw:
                ApplyDrawEffect(effect);
                break;

            case EffectType.Strength:
                ApplyStrengthEffect(effect);
                break;

            case EffectType.Poison:
                ApplyPoisonEffect(effect);
                break;

            case EffectType.Energy:
                ApplyEnergyEffect(effect);
                break;

            case EffectType.ShieldAttack:
                ApplyShieldAttackEffect(effect);
                break;

            case EffectType.Retrieve:
                ApplyRetrieveEffect(effect);
                break;

            case EffectType.CardsPlayedDamage:
                ApplyCardsPlayedDamageEffect(effect);
                break;

            case EffectType.AttackPlayedDamage:
                ApplyAttackPlayedDamageEffect(effect);
                break;

            case EffectType.Execute:
                ApplyExcuteEffect(effect);
                break;
        }

    }


    private void ApplyAttackEffect(CardEffect effect)
    {
        int damage = effect.value + playerManager.Strength;
        enemyManager.TakeDamage(damage);
    }

    private void ApplyDefenseEffect(CardEffect effect)
    {
        playerManager.AddShield(effect.value);
        uiManager.UpdatePlayerShieldUI();
    }

    private void ApplyHealEffect(CardEffect effect)
    {
        playerManager.Heal(effect.value);
    }

    private void ApplyDrawEffect(CardEffect effect)
    {
        for (int i = 0; i < effect.value; i++)
        {
            Card drawCard = deck.DrawCard();
            uiManager.AddCardToHand(drawCard);
        }
    }

    private void ApplyStrengthEffect(CardEffect effect)
    {
        playerManager.AddStrength(effect.value);
    }

    private void ApplyPoisonEffect(CardEffect effect)
    {
        enemyManager.AddPoison(effect.value);
    }

    private void ApplyEnergyEffect(CardEffect effect)
    {
        playerManager.AddEnergy(effect.value);
    }

    private void ApplyShieldAttackEffect(CardEffect effect)
    {
        int damage = playerManager.Shield;
        enemyManager.TakeDamage(damage);
        playerManager.ResetShield();
    }

    private void ApplyRetrieveEffect(CardEffect effect)
    {
        uiManager.ShowDiscardPile();
    }

    private void ApplyDiscardEffect(CardEffect effect, CardEffect[] followUpEffects)
    {
        pendingFollowUpEffects = followUpEffects;
        uiManager.ShowHandForDiscardSelection();
    }

    private void ApplyCardsPlayedDamageEffect(CardEffect effect)
    {
        int damage = playerManager.CardsPlayedThisTurn * effect.value;
        enemyManager.TakeDamage(damage);
    }

    private void ApplyAttackPlayedDamageEffect(CardEffect effect)
    {
        int damage = playerManager.AttacksPlayedThisTurn * effect.value;
        enemyManager.TakeDamage(damage);
    }

    private void ApplyExcuteEffect(CardEffect effect)
    {
        enemyManager.TakeDamage(enemyManager.HP);
    }


}
