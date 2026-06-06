internal class Hand
{
    internal enum Status{Max, Bust, Blackjack, Draw, Stand, Null}

    internal List<Card> Cards = new();
    public Status _Status {get; set;}
    public int Score {get; set;}
    public bool Insured {get; set;}
    
    public Hand()
    {
        _Status = Status.Null;
        Score = 0;
        Insured = false;
    }
    public int Evaluate()
    {
        int score = 0;
        int aceCount = 0;
        foreach(Card card in Cards)
        {
            if(card.CardRank == Card.Rank.Ace)
            {
                aceCount++;
            }
        }
        if(aceCount == 0) //handling hands with no aces
        {
            foreach(Card card in Cards)
            {
                score += card.Evaluate();
            }
        }
        else
        {
            List<Card> aces = Cards.Where(c => c.CardRank == Card.Rank.Ace).ToList();
            List<Card> nonAces = Cards.Except(aces).ToList();
            foreach(Card card in nonAces)
            {
                if(score + card.Evaluate() <= 21) 
                {
                    score += card.Evaluate();
                }
                else
                {
                    score += card.Evaluate();
                }
            }
            foreach(Card ace in aces)
            {
                if(score + 11 > 21)
                {
                    score += 1;
                }
                else
                {
                    score += 11;
                }
            }
        }
        if(score > 21)
        {
            _Status = Status.Bust;
        }
        Score = score;
        return Score;
    }
    internal Status GetStatus()
    {
        Score = Evaluate();
        _Status = Score switch
        {
            < 17 => Hand.Status.Draw,
            < 21 => Hand.Status.Stand,
            21 => Hand.Status.Max,
            _ => Hand.Status.Bust
        };
        if(_Status == Status.Max && Cards.Count() == 2)
        {
            _Status = Status.Blackjack;
        }
        return _Status;
    }
    internal void Insure()
    {
        if (!Insured)
        {
            Insured = true;
        }
    }
}
