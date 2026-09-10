using UnityEngine;

public class RewordManager : MonoBehaviour
{
    [SerializeField] private CardUI cardUIPrefab;
    [SerializeField] private Transform rewardCardPanel;
    [SerializeField] private Card[] rewardPool;
    [SerializeField] private Deck deck;
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private GameObject victoryPanel;


    public void ShowReward()
    {
        victoryPanel.SetActive(false);
        rewardPanel.SetActive(true);
        foreach (Transform child in rewardCardPanel)
        {
            Destroy(child.gameObject);
        }

        // 3枚表示
        for (int i = 0; i < 3; i++)
        {
            Card card = rewardPool[Random.Range(0, rewardPool.Length)];

            CardUI cardUI = Instantiate(cardUIPrefab, rewardCardPanel);
            cardUI.Setup(card);
            cardUI.OnCardClicked += OnRewardCardClicked;
        }
    }

    private void OnRewardCardClicked(CardUI cardUI)
    {
        Card selectedCard = cardUI.GetCard();
        Debug.Log("報酬カードを選択しました: " + selectedCard.cardName);
        deck.AddCard(selectedCard);
        Debug.Log("デッキに追加しました: " + selectedCard.cardName);
        rewardPanel.SetActive(false);
    }
}
