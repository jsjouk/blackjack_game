public class Card
{
    public enum Rank{Two=2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace}
    public enum Suit{Hearts, Diamonds, Clubs, Spades}

    public Rank CardRank {get; set;}
    public Suit CardSuit {get; set;}

    public Card()
    {
        
    }
    public override string ToString()
    {
        string rankStr ;
        switch (Enum.GetName(CardRank))
        {
            case "Ace": rankStr = "A"; break;
            case "King": rankStr = "K"; break;
            case "Queen": rankStr = "Q"; break;
            case "Jack": rankStr = "J"; break;
            case "Ten": rankStr = "T"; break;
            default: rankStr = ((int)CardRank).ToString(); break;
        }
        string suitStr;
        switch (Enum.GetName(CardSuit))
        {
            case "Hearts": suitStr = "♥"; break;
            case "Diamonds": suitStr = "♦ "; break;
            case "Clubs": suitStr = "♣"; break;
            case "Spades": suitStr = "♠"; break;
            default: suitStr = ""; break;
        }
        return rankStr + suitStr;
    }
    public int Evaluate()
    {
        if((int)CardRank < (int)Rank.Ten)
        {
            return (int)CardRank;
        }
        else if((int)CardRank == (int)Rank.Ace)
        {
            return 11;
        }
        else
        {
            return 10;
        }
    }
    public static int EvaluateHand(List<Card> hand)
    {
        int score = 0;
        int aceCount = 0;
        foreach(Card card in hand)
        {
            if(card.CardRank == Rank.Ace)
            {
                aceCount++;
            }
        }
        if(aceCount == 0) //handling hands with no aces
        {
            foreach(Card card in hand)
            {
                if(score + card.Evaluate() <= 21) 
                {
                    score += card.Evaluate();
                }
                else if(score + card.Evaluate() > 21)
                {
                    return score += card.Evaluate();
                }
            }
        }
        else
        {
            List<Card> aces = hand.Where(c=>c.CardRank == Rank.Ace).ToList();
            List<Card> nonAces = hand.Except(aces).ToList();
            foreach(Card card in nonAces)
            {
                if(score + card.Evaluate() <= 21) 
                {
                    score += card.Evaluate();
                }
                else
                {
                    return score += card.Evaluate();
                }
            }
            foreach(Card ace in aces)
            {
                if(score + 11 > 21)
                {
                    if(score + 1 > 21)
                    {
                        return score += 1;
                    }
                    else
                    {
                        score += 1;
                    }
                }
                else
                {
                    score += 11;
                }
            }
        }
        return score;
    }
}