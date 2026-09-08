using UnityEngine;
using System.Collections.Generic;

public class Deck : MonoBehaviour
{
    [SerializeField] private List<Card> drawPile;
    [SerializeField] private List<Card> hand;
    [SerializeField] private List<Card> discardPile;


    private void Awake()
    {
        ShuffleDrawPile();
    }

    //山札からカードを引く
    public Card DrawCard()
    {
        if (drawPile.Count == 0)
        {
            ReshuffleDiscard();
        }

        Card card = drawPile[0];

        drawPile.RemoveAt(0);
        hand.Add(card);

        return card;
    }
    
    //手札からカードを捨て札に移動
    public void DiscardCard(Card card)
    {
        hand.Remove(card);
        discardPile.Add(card);
    }

    //捨て札から山札に移動し、シャフルする
    private void ReshuffleDiscard()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();

        ShuffleDrawPile();
    }

    //シャッフルする
    private void ShuffleDrawPile()
    {
        for (int i = drawPile.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            Card temp = drawPile[i];
            drawPile[i] = drawPile[randomIndex];
            drawPile[randomIndex] = temp;
        }
    }
    
    //手札全捨て
    public void DiscardHand()
    {
        while (hand.Count > 0)
        {
            DiscardCard(hand[0]);
        }
    }

    //捨て札から手札に戻す
    public void RetrieveFromDiscard(Card card)
    {
        if (discardPile.Contains(card))
        {
            discardPile.Remove(card);
            hand.Add(card);
        }
    }

    public IReadOnlyList<Card> DiscardPile => discardPile;

}
