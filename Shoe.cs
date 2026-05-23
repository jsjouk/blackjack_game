static class Shoe
{
    public static int Decks {get; set;}
    private static readonly List<Card> CardShoe;
    static Shoe()
    {
        CardShoe = new List<Card>();
        for(int i = 0; i < 7 + 1; i++)
        {
            Deck deck = new Deck();
            foreach(Card c in deck.deck)
            {
                CardShoe.Add(c);
            }
        }
        ShuffleShoe();
    }
    private static void ShuffleShoe()
    {
         Random rng = new()
;        for(int i = CardShoe.Count-1; i > 0; i--)
        {
            int random = rng.Next(0,i+1);
            Card temp = CardShoe[i];
            CardShoe[i] = CardShoe[random];
            CardShoe[random] = temp;
        }
    }
    public static Card DealCard()
    {
        Card dealt = CardShoe[0];
        CardShoe.Remove(CardShoe[0]);
        return dealt;
    }
    public static int GetCardsLeft()
    {
        return CardShoe.Count();
    }
}