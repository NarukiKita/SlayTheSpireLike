using UnityEngine;

public class EnemyManager : MonoBehaviour
{
// Inspector===========================================================================================================

    [SerializeField] private int poison = 0;
    [SerializeField] private int enemyShield = 0;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private EnemyData enemyData;


//=====================================================================================================================

//EnemyDataから受け取る変数==============================================================================================

    private int enemyHP;
    private int enemyAttackPower;
    private int enemyShieldPower;
    private int attackChance;
    private int defendChance;
    private int poisonChance;
    private int strongAttackChance;
    private int doubleAttackChance;
    private int buffChance;

//=====================================================================================================================

// 読み取り用===========================================================================================================

    public int HP => enemyHP;
    public int AttackPower => enemyAttackPower;
    public int Poison => poison;
    public int Shield => enemyShield;

//=====================================================================================================================

//Awake()===============================================================================================================

    private void Awake()
    {
        enemyData = MapManager.Instance.CurrentEnemyData;
        enemyHP = enemyData.maxHP;
        enemyAttackPower = enemyData.attackPower;
        enemyShieldPower = enemyData.shieldPower;
        attackChance = enemyData.attackWeight;
        defendChance = enemyData.defendWeight;
        poisonChance = enemyData.poisonWeight;
        strongAttackChance = enemyData.strongAttackWeight;
        doubleAttackChance = enemyData.doubleAttackWeight;
        buffChance = enemyData.buffWeight;
    }

//=====================================================================================================================

//行動選択==============================================================================================================

    public void TakeAction(PlayerManager playerManager)
    {
        int totalWeight =
            attackChance +
            defendChance +
            poisonChance +
            strongAttackChance +
            doubleAttackChance;

        int randomValue = Random.Range(0, totalWeight);

        if (randomValue < attackChance)
        {
            Attack(playerManager);
        }
        else if (randomValue < attackChance + defendChance)
        {
            AddShield(enemyShieldPower);
        }
        else if (randomValue < attackChance + defendChance + poisonChance)
        {
            PoisonAttack(playerManager);
        }
        else if (randomValue < attackChance + defendChance + poisonChance + strongAttackChance)
        {
            StrongAttack(playerManager);
        }
        else if (randomValue < attackChance + defendChance + poisonChance + strongAttackChance)
        {
            DoubleAttack(playerManager);
        }
        else
        {
            Strength();
        }
    }

//=====================================================================================================================

//毒付与される================================================================================================================

    public void AddPoison(int amount)
    {
        poison += amount;
    }

//=====================================================================================================================

//毒ダメージ============================================================================================================
    public void ProcessPoison()
    {
        if (poison <= 0)
        {
            return;
        }

        int poisonDamage = poison;

        TakeDamage(poisonDamage);
        poison --;
        uiManager.UpdatePoisonUIText();

        Debug.Log("毒により" + poisonDamage + "ダメージ");
    }

//=====================================================================================================================

// ダメージ=============================================================================================================

    public void TakeDamage(int damage)
    {
        if (enemyShield >= damage)
        {
            enemyShield -= damage;
            damage = 0;
        }
        else
        {
            damage -= enemyShield;
            enemyShield = 0;
        }

        enemyHP -= damage;
    }

//=====================================================================================================================

// 防御===============================================================================================================

    public void AddShield(int amount)
    {
        enemyShield += amount;
    }
//=====================================================================================================================

//攻撃==================================================================================================================

    public void Attack(PlayerManager playerManager)
    {
        playerManager.TakeDamage(enemyAttackPower);

        Debug.Log("敵が攻撃しました。");
    }
//=====================================================================================================================

//強攻撃================================================================================================================

    public void StrongAttack(PlayerManager playerManager)
    {
        playerManager.TakeDamage(enemyAttackPower * 2);
        Debug.Log("敵が強攻撃しました。");
    }

//=====================================================================================================================

//2回攻撃===============================================================================================================

    public void DoubleAttack(PlayerManager playerManager)
    {
        playerManager.TakeDamage(enemyAttackPower);
        playerManager.TakeDamage(enemyAttackPower);

        Debug.Log("敵が2回攻撃しました。");
    }

//=====================================================================================================================

//毒攻撃================================================================================================================

    public void PoisonAttack(PlayerManager playerManager)
    {
        playerManager.AddPoison(3);

        Debug.Log("敵が毒を付与しました。");
    }

//=====================================================================================================================

//バフ==================================================================================================================
    private void Strength()
    {
        enemyAttackPower += enemyData.buffAmount;
        Debug.Log("敵の攻撃力が上がりました。AttackPower +" + enemyData.buffAmount);
    }
//=====================================================================================================================

// 死亡判定=============================================================================================================

    public bool IsDead()
    {
        return enemyHP <= 0;
    }
//=====================================================================================================================
}