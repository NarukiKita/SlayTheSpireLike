using UnityEngine;

public class ConditionManager : MonoBehaviour
{
    [SerializeField] private EnemyManager enemyManager;

    public bool CheckCondition(CardCondition condition)
    {
        int conditionValue;
        if (condition.conditionType == ConditionType.None)
        {
            return true;
        }

        switch (condition.conditionType)
        {
            case ConditionType.PlayerHP:
                conditionValue = PlayerManager.Instance.HP;
                break;

            case ConditionType.EnemyHP:
                conditionValue = enemyManager.HP;
                break;

            case ConditionType.CardsPlayedThisTurn:
                conditionValue = PlayerManager.Instance.CardsPlayedThisTurn;
                break;

            default:
                return false;
        }
        if (condition.conditionOperator == ConditionOperator.LessEqual)
        {
            return conditionValue <= condition.value;
        }
        else if (condition.conditionOperator == ConditionOperator.GreaterEqual)
        {
            return conditionValue >= condition.value;
        }
        return false;

    }
}
