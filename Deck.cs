public class Deck
{
    public List<Card> deck = [];

    public Deck()
    {
        FillDeck();
        ShuffleDeck();
    }

    public void FillDeck()
    {
        foreach(Card.Suit s in Enum.GetValues(typeof(Card.Suit))) //looked up syntax 
        {
            foreach(Card.Rank r in Enum.GetValues(typeof(Card.Rank)))
            {
                Card _ = new Card()
                {
                    CardRank = r, CardSuit = s
                };
                deck.Add(_);
            }
        }
    }
    public void ShowDeck()
    {
        foreach(Card c in deck)
        {
            Console.WriteLine(c.ToString());
        }
    }
    static Random rng = new Random();
    public void ShuffleDeck()
    {
        for(int i = deck.Count-1; i > 0; i--)
        {
            int random = rng.Next(0,i+1);
            Card temp = deck[i];
            deck[i] = deck[random];
            deck[random] = temp;
        }
    }
}