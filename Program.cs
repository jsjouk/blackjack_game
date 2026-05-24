class Program
{
    internal static string CWD = Directory.GetCurrentDirectory();
    internal static int LastSelected = 0;
    public static List<Card> UserHand = new List<Card>();
    public static List<Card> DealerHand = new List<Card>();
    public enum Input{Hit, Stand, DoubleDown, Split, Insurance}
    static int balance;
    static int bet;
    static int userScore;
    static int dealerScore;
    static bool firstDeal;
    public static string path = CWD + "/userdata";
    static void Main()
    {
        LoadedUser player = StartUp();
        balance = player.Chips;
        Renderer.RefreshDisplay(player,userScore,dealerScore);
        static void HandLoop(LoadedUser player)
        {
            firstDeal = true;
            while (true)
            {
                if (firstDeal) //distribute cards to set up first deal of the hand
                {
                    UserHand.Add(Shoe.DealCard());
                    DealerHand.Add(Shoe.DealCard());
                    UserHand.Add(Shoe.DealCard());
                    DealerHand.Add(Shoe.DealCard());
                    firstDeal = false;
                }
                userScore = Card.EvaluateHand(UserHand);
                dealerScore = Card.EvaluateHand(DealerHand); 
                Renderer.RefreshDisplay(player,userScore,dealerScore);
                switch (GetUserDecision(LastSelected))
                {
                    case Input.Hit: 
                    {
                        Renderer.WriteCenterText("hit",Renderer.midpointY,ConsoleColor.Magenta,ConsoleColor.DarkMagenta);
                        break;
                    }
                    
                    case Input.Stand:
                    {
                        Renderer.WriteCenterText("stand",Renderer.midpointY,ConsoleColor.Cyan,ConsoleColor.DarkCyan);
                        break;
                    }
                    case Input.DoubleDown:
                    {
                        Renderer.WriteCenterText("doubledown",Renderer.midpointY,ConsoleColor.Red,ConsoleColor.DarkRed);
                        break;
                    }
                    case Input.Split:
                    {
                        Renderer.WriteCenterText("split",Renderer.midpointY,ConsoleColor.Green,ConsoleColor.DarkGreen);
                        break;
                    }
                    case Input.Insurance:
                    {
                        Renderer.WriteCenterText("insurance",Renderer.midpointY,ConsoleColor.Yellow,ConsoleColor.DarkYellow);
                        break;        
                    }
                }
                Console.ReadLine();
            }
        }   
        HandLoop(player);
        while (true)
        {
            ConsoleKeyInfo keyPress;
            keyPress = Console.ReadKey(true);
            string[] skullLines = Art.Skull.Split("\n");
            int skullOffsetX = skullLines[0].Length / 2;
            int skullOffsetY = skullLines.Count() / 2;
            if(keyPress.Key == ConsoleKey.Escape)
            {
                Console.Clear();
                player.Chips -= 2;
                player.WriteSaveFile();
                Renderer.DrawArt(Renderer.midpointX - skullOffsetX, Renderer.midpointY - skullOffsetY, 2, Art.Skull, ConsoleColor.Red, ConsoleColor.DarkRed);
                Environment.Exit(0);
            }
        }
    }
    static Input GetUserDecision(int lastSelected)
    {
        int selectionIndex;
        ConsoleKeyInfo userInput;
        selectionIndex = lastSelected;
        Console.SetCursorPosition(0, Renderer.screenBottom - 3);
        bool split = (int)UserHand[0].Evaluate() == ((int)UserHand[1].Evaluate());
        bool insurance = DealerHand[0].CardRank == Card.Rank.Ace;
        List<Input> actionsAvailable = [];
        foreach(Input input in Enum.GetValues(typeof(Input)))
        {
            if(!split && input == Input.Split)
            {
                
            }
            else if(!insurance && input == Input.Insurance)
            {
                
            }
            else
            {
                actionsAvailable.Add(input);
            }
        }
        string menuText = (split, insurance)switch
        {
            (true, true)=> Renderer.GetInputString(true, true, lastSelected, actionsAvailable),
            (true, false)=> Renderer.GetInputString(true, false, lastSelected, actionsAvailable),
            (false, true)=> Renderer.GetInputString(false, true, lastSelected, actionsAvailable),
            _=> Renderer.GetInputString(false, false, lastSelected, actionsAvailable)
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
                string userGUI = Renderer.GetInputString(split, insurance, actionMagicNumber,actionsAvailable);
                Renderer.WriteCenterText(userGUI, Renderer.screenBottom - 2, ConsoleColor.Magenta, ConsoleColor.DarkMagenta);
            }
            if(userInput.Key == ConsoleKey.RightArrow && selectionIndex < actionsAvailable.Count() - 1)
            {
                Console.Beep();
                selectionIndex++;
                int actionMagicNumber = (int)actionsAvailable[selectionIndex];
                Renderer.ClearInputGUI();
                string userGUI = Renderer.GetInputString(split, insurance, actionMagicNumber,actionsAvailable);
                Renderer.WriteCenterText(userGUI, Renderer.screenBottom - 2, ConsoleColor.Magenta, ConsoleColor.DarkMagenta);
            }
        }
        while(userInput.Key != ConsoleKey.Enter);
        Program.LastSelected = selectionIndex;
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
    
        if(!Menu.BinaryMenu(true,"MAIN MENU","New file"," Load file", Renderer.centerpointQ1.Item1, Renderer.midpointY, ConsoleColor.Green, ConsoleColor.DarkGreen))
        {
            Renderer.ClearLine(Console.GetCursorPosition().Item2);
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
}                                       
