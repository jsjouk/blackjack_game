using System.ComponentModel.Design;

class Program
{
    internal static string CWD = Directory.GetCurrentDirectory();
    internal static string path = Path.Combine(CWD ,"userdata");

    internal enum Input{Hit, Stand, DoubleDown, Split, Insurance}
    enum Outcome{Blackjack,Win,Tie,Outdealt,Bust};

    internal static int LastSelected = 0;
    internal static bool IsPlayerDone = false;
    internal static List<Hand> UserHands = new ();
    internal static Hand DealerHand = new();
    static Hand.Status DealerStatus = Hand.Status.Null;
    
    static int balance;
    static int bet;
    static int numberOfHands = 1;
    static int userScore;
    static int dealerScore;
    static bool firstInput;

    static void Main()
    {
        LoadedUser player = StartUp();
        balance = player.Chips;
        ConsoleKeyInfo keyPress;
        do
        {
            HandLoop(player);
        }
        while(true);
    }
    static Input GetUserDecision(Hand hand)
    {
        int selectionIndex;
        ConsoleKeyInfo userInput;
        selectionIndex = LastSelected;
        Console.SetCursorPosition(0, Renderer.screenBottom - 3);
        bool split = hand.Cards[0].Evaluate() == hand.Cards[1].Evaluate();
        bool insurance = DealerHand.Cards[0].CardRank == Card.Rank.Ace && firstInput == true;
        bool blackjack = hand.Evaluate() == 21 && DealerHand.Evaluate() != 21 && firstInput == true;
        List<Input> actionsAvailable = [];
        foreach(Input _ in Enum.GetValues(typeof(Input)))
        {
            if(!split && _ == Input.Split)
            {
                
            }
            else if(!insurance && _ == Input.Insurance)
            {
                
            }
            else if(!firstInput && _ == Input.DoubleDown)
            {
                
            }
            else if (insurance && hand.Insured == true)
            {
                
            }
            else
            {
                actionsAvailable.Add(_);
            }
        }
        string menuText = (split, insurance)switch
        {
            (true, true)=> Renderer.GetInputString(blackjack,selectionIndex, actionsAvailable),
            (true, false)=> Renderer.GetInputString(blackjack,selectionIndex, actionsAvailable),
            (false, true)=> Renderer.GetInputString(blackjack,selectionIndex, actionsAvailable),
            _=> Renderer.GetInputString(blackjack,selectionIndex, actionsAvailable)
        };
        Renderer.WriteCenterText(menuText, Renderer.screenBottom - 2, ConsoleColor.Magenta, ConsoleColor.DarkMagenta);
        do
        {
            userInput = Console.ReadKey(true);
            if(userInput.Key == ConsoleKey.LeftArrow && selectionIndex > 0)
            {
                Console.Beep();
                selectionIndex--;
                int actionMagicNumber = (int)actionsAvailable[selectionIndex];
                Renderer.ClearInputGUI();
                string userGUI = Renderer.GetInputString(blackjack, actionMagicNumber,actionsAvailable);
                Renderer.WriteCenterText(userGUI, Renderer.screenBottom - 2, ConsoleColor.Magenta, ConsoleColor.DarkMagenta);
            }
            if(userInput.Key == ConsoleKey.RightArrow && selectionIndex < actionsAvailable.Count() - 1)
            {
                Console.Beep();
                selectionIndex++;
                int actionMagicNumber = (int)actionsAvailable[selectionIndex];
                Renderer.ClearInputGUI();
                string userGUI = Renderer.GetInputString(blackjack, actionMagicNumber,actionsAvailable);
                Renderer.WriteCenterText(userGUI, Renderer.screenBottom - 2, ConsoleColor.Magenta, ConsoleColor.DarkMagenta);
            }
        }
        while(userInput.Key != ConsoleKey.Enter);
        LastSelected = selectionIndex;
        return actionsAvailable[selectionIndex];
    }
    static LoadedUser StartUp()
    {
        Console.CursorVisible = false;
        Console.Title = "GOATED BLACKJACK -- MENU";
        Console.Clear();
        LoadedUser player;
        string[] menuArtLines = Art.MenuArt.Split("\n"); //setup for drawing skull 
        int menuArtLeft = Renderer.midpointX - (menuArtLines[0].Length / 2);
        int menuArtTop = Renderer.midpointY - (menuArtLines.Count() / 2);
        Renderer.DrawArt(menuArtLeft, menuArtTop, 2, Art.MenuArt, ConsoleColor.Green, ConsoleColor.DarkGreen);
        
        string[] titleLines = Art.Logo2.Split("\n"); //setup for drawing main menu logo text
        int titleLeft = Renderer.midpointX - (titleLines[0].Length / 2);
        int titleTop = 1;
        Renderer.DrawArt(titleLeft, titleTop, 2, Art.Logo2, ConsoleColor.Green, ConsoleColor.DarkGreen);

        Menu.AwaitKeystroke("[PRESS ANY KEY]",Renderer.midpointX,Console.WindowHeight - 3,true,ConsoleColor.Green,ConsoleColor.DarkGreen);
    
        if(Menu.BinaryMenu(true,"MAIN MENU","New file"," Load file", Renderer.centerpointQ1.Item1, Renderer.midpointY, ConsoleColor.Green, ConsoleColor.DarkGreen))
        {
            User newUser = User.CreateUser();
            player = new LoadedUser(newUser.Name, newUser.LuckyNumber, newUser.Chips);
        }
        else
        {
            Renderer.ClearLine(Console.GetCursorPosition().Item2);
            player = Menu.LoadSaveFile();
            Console.Clear();
        }
        return player;
    }
    static void CleanUp()
    {
        UserHands.Clear();
        DealerHand.Cards.Clear();
    }
    static Outcome GetOutcome(Hand player, Hand dealer)
    {
        if(player._Status == Hand.Status.Bust) // check for busting first so that you cannot win with a 25 vs a 21 for example
        {
            return Outcome.Bust;
        }
        if(player.Evaluate() > dealer.Evaluate()) // then a regular win
        {
            if(player._Status == Hand.Status.Blackjack)
            {
                return Outcome.Blackjack;
            }
            return Outcome.Win;
        }
        if(player.Evaluate() == dealer.Evaluate()) // checking for ties in score first
        {
            if(player._Status == Hand.Status.Blackjack && dealer._Status != Hand.Status.Blackjack) // tie breaker
            {
                return Outcome.Blackjack;
            }
            else if(player._Status != Hand.Status.Blackjack && dealer._Status == Hand.Status.Blackjack)
            {
                return Outcome.Outdealt;
            }
            return Outcome.Tie;
        }
        if(player._Status != Hand.Status.Bust && dealer._Status == Hand.Status.Bust)
        {
            return Outcome.Win;
        }
        return Outcome.Outdealt;
    }

    static void HandLoop(LoadedUser player) // blackjack exclusive logic
    {
        IsPlayerDone = false;
        for(int i = 0; i < numberOfHands; i++) // first round of dealing to the player
        {
            Hand thisHand = new Hand();
            thisHand.Cards.Add(Shoe.DealCard());
            UserHands.Add(thisHand);
        }
        DealerHand.Cards.Add(Shoe.DealCard()); // dealer's face up card
        foreach(Hand hand in UserHands) // second round of dealing to the player
        {
            hand.Cards.Add(Shoe.DealCard());
        }
        DealerHand.Cards.Add(Shoe.DealCard()); // dealer's face down card
        int handSelected = 0;
        userScore = UserHands[handSelected].Evaluate();
        dealerScore = DealerHand.Cards[0].Evaluate();
        Renderer.RefreshDisplay(player,userScore,dealerScore,handSelected);    
        foreach(Hand hand in UserHands) // handling the player's turn for each of their bets
        {
            IsPlayerDone = false;
            firstInput = true;
            handSelected = UserHands.IndexOf(hand);
            while (hand._Status is not(Hand.Status.Stand or Hand.Status.Bust)) 
            {
                Input selectedAction = GetUserDecision(hand);
                if(hand.Evaluate() == 21)
                {
                    break;
                }
                if(selectedAction == Input.Hit || selectedAction == Input.DoubleDown) 
                {
                    firstInput = false;
                    hand._Status = Hand.Status.Draw;
                    hand.Cards.Add(Shoe.DealCard());
                }
                if(selectedAction == Input.Stand)
                {
                    firstInput = false;
                    hand._Status = Hand.Status.Stand;
                }
                if(selectedAction == Input.Insurance && hand.Insured == false)
                {
                    hand.Insure();
                }
                
                userScore = hand.Evaluate();
                Renderer.RefreshDisplay(player,userScore,dealerScore,handSelected);


                Renderer.WriteCenterText(selectedAction.ToString(),Console.WindowHeight-2,ConsoleColor.Red,ConsoleColor.DarkRed);
                Console.ReadLine();
                if(selectedAction == Input.DoubleDown)
                {
                    break;
                }
            }
        }
        IsPlayerDone = true;
        int dealerDraws = 0;
        while(true) // dealer's turn
        {
            DealerStatus = DealerHand.GetStatus();
            if(DealerStatus is Hand.Status.Stand or Hand.Status.Max)
            {
                dealerScore = DealerHand.Evaluate();
                Thread.Sleep(350);
                Renderer.RefreshDisplay(player,userScore,dealerScore,0);
                break;
            }
            else if(DealerStatus is Hand.Status.Draw)
            {
                if(dealerDraws == 0)
                {
                    dealerScore = DealerHand.Evaluate();
                    Renderer.RefreshDisplay(player,userScore,dealerScore,0);
                    Thread.Sleep(350);
                }
                DealerHand.Cards.Add(Shoe.DealCard());
                dealerDraws++;
                dealerScore = DealerHand.Evaluate();
                Thread.Sleep(500);
                Renderer.RefreshDisplay(player,userScore,dealerScore,0);
            }
            else if(DealerStatus is Hand.Status.Bust)
            {
                dealerScore = DealerHand.Evaluate();
                Thread.Sleep(500);
                Renderer.RefreshDisplay(player,userScore,dealerScore,0);
                break;
            }
            else
            {
                dealerScore = DealerHand.Evaluate();
                Thread.Sleep(500);
                Renderer.RefreshDisplay(player,userScore,dealerScore,0);
            }
        }
        foreach(Hand hand in UserHands)
        {
            Outcome result = GetOutcome(hand,DealerHand);
            Renderer.WriteCenterText(result.ToString(),Renderer.midpointY,ConsoleColor.Yellow,ConsoleColor.DarkYellow);
            Console.ReadLine();
        }
        CleanUp();
    }
}                                       
