using UnityEngine;

public class BattleManager : MonoBehaviour
{
//inspector============================================================================================================

    [SerializeField] private Deck deck;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private CardEffectManager cardEffectManager;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private EnemyManager enemyManager;    

//========================================================================================================================

//変数===========================================================================================================================
    private bool battleEnded = false;
//===========================================================================================================================

//Start()=====================================================================================================================-==

    private void Start()
    {
        Debug.Log("BMでStart起動");
        StartPlayerTurn();
    }

//======================================================================================================================

//PlayCard()==============================================================================================================

    public void PlayCard(CardUI cardUI)
    {
        Card card = cardUI.GetCard();
        if (battleEnded)
        {
            return;
        }
        if (playerManager.CanUseCard(card))
        {
            //コスト消費
            playerManager.SpendEnergy(card);
            playerManager.RecordCardPlayed();
            if (card.HasEffect(EffectType.Attack))
            {
                playerManager.RecordAttackPlayed();
            }
            uiManager.UpdateEnergyUI();
            Debug.Log(card.cardName + "を使用しました");

            //種類ごと
            cardEffectManager.ApplyEffect(card, () => FinishPlayCard(card, cardUI));
            //共通領域
        }
        else
        {
            Debug.Log("No enough Energy");
            StartCoroutine(uiManager.ShowEnergyWarning());
        }
    }

//=======================================================================================================================

//FInishPlayCard()=======================================================================================================
    private void FinishPlayCard(Card card, CardUI cardUI)
    {
        Debug.Log($"FinishPlayCard 呼び出し：{card.cardName} / cardUI == null : {cardUI == null}");
        uiManager.UpdatePoisonUI();
        uiManager.UpdatePoisonUIText();
        uiManager.UpdateStrengthUI();
        uiManager.UpdateStrengthUIText();
        uiManager.UpdateEnemyHPUI();
        uiManager.UpdatePlayerHPUI();
        uiManager.UpdateEnergyUI();
        deck.DiscardCard(card);
        Destroy(cardUI.gameObject);
        if (enemyManager.IsDead())
        {
            WinBattle();
        }
    }
//=======================================================================================================================

//StartPlayerTurn()=====================================================================================================

    private void StartPlayerTurn()
    {
        playerManager.StartTurn();
        uiManager.UpdateEnergyUI();
        for (int i = 0; i < 5; i++)
        {
            Card card = deck.DrawCard();
            uiManager.AddCardToHand(card);
        }
    }

//==========================================================================================================================

//EndPlayerTurn()=============================================================================================================

    public void EndPlayerTurn()
    {
        if (battleEnded)
        {
            return;
        }
        Debug.Log("プレイヤーターン終了");
        deck.DiscardHand();
        uiManager.ClearHand();
        playerManager.ResetStrength();
        playerManager.ResetCardsPlayedThisTurn();
        uiManager.UpdateStrengthUI();
        StartEnemyTurn();
    }
//====================================================================================================================================

//EnemyTurn()======================================================================================================================

    private void StartEnemyTurn()
    {
        Debug.Log("敵のターン開始");

        Debug.Log("敵の行動");

        enemyManager.ProcessPoison();
        uiManager.UpdateEnemyHPUI();

        if (enemyManager.IsDead())
        {
            WinBattle();
            return;
        }

        playerManager.TakeDamage(enemyManager.AttackPower);

        uiManager.UpdatePlayerHPUI();
        uiManager.UpdatePlayerShieldUI();
        uiManager.UpdateEnemyHPUI();

        Debug.Log("敵のターン終了");
        if (playerManager.IsDead())
        {
            LoseBattle();
            return;
        }


        StartPlayerTurn();
    }

//====================================================================================================================================

//Win・Defeat========================================================================================================================

    private void WinBattle()
    {
        Debug.Log("勝利！");
        uiManager.WinUI();
        battleEnded = true;
    }

    private void LoseBattle()
    {
        Debug.Log("敗北！");
        uiManager.DefeatUI();
        battleEnded = true;
    }

//====================================================================================================================================

}
