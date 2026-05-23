internal class Renderer
{
    internal static int screenBottom = Console.WindowHeight; internal static int screenRight = Console.WindowWidth;
    internal static int midpointX = screenRight / 2; internal static int midpointY = screenBottom / 2;
    internal static (int,int) centerpointQ2 = (midpointX / 2, midpointY / 2); 
    internal static (int,int) centerpointQ1 = ((int)(midpointX * 1.5), midpointY / 2);
    internal static (int,int) centerpointQ3 = (midpointX / 2, (int)(midpointY * 1.5));
    internal static (int,int) centerpointQ4 = ((int)(midpointX * 1.5), (int)(midpointY * 1.5));


    public static void RefreshDisplay(LoadedUser player, int userScore, int dealerScore)
    {
        Console.Clear();
        Console.CursorVisible = false;
        bool firstDeal = Program.DealerHand.Count() == 2;
        string gap = "              ";
        string persistentData = $"USER:  {player.Name.ToUpper()}"+ gap + $"BALANCE:   {player.Chips:C}";
        string dealerString = "DEALER HOLDS:    ";
        string playerString = "PLAYER HOLDS:    ";
        if (firstDeal)
        {
            dealerString += Program.DealerHand[0].ToString().Trim() + ", ??";
            dealerString += $"  [ {Program.DealerHand[0].Evaluate()} ]";
        }
        else
        {
            foreach(Card card in Program.DealerHand)
            {
                dealerString += card.ToString() + ", ";
            }
            dealerString += $"  [ {dealerScore} ]";
        }
            foreach(Card card in Program.UserHand)
            {
                playerString += card.ToString() + ", ";
            }
            playerString += $"  [ {userScore} ]";
            // --- drawing the dealer side of the screen
            Art.DrawLine(0, 0, Console.WindowWidth, '=', '=', ConsoleColor.Green, ConsoleColor.DarkGreen); //first top divider line
            Renderer.WriteCenterText(persistentData, 1, ConsoleColor.Green, ConsoleColor.DarkGreen); //user info
            Art.DrawLine(0, 2, Console.WindowWidth, '=', '=', ConsoleColor.Green, ConsoleColor.DarkGreen); //first bottom divider line
            int dealerArtXOffset = Art.DealerHeader.Split("\n")[0].Length; 
            int dealerArtYOffset = Art.DealerHeader.Split("\n").Count();
            DrawArt(Renderer.midpointX - (dealerArtXOffset / 2), 4, 2, Art.DealerHeader, ConsoleColor.Green, ConsoleColor.DarkGreen); //dealer art
            Art.DrawLine(0, 4 + dealerArtYOffset, Console.WindowWidth, '=', '=', ConsoleColor.Green, ConsoleColor.DarkGreen); //last top divider line
            int currentTop = Console.GetCursorPosition().Item2;
            Renderer.WriteCenterText(dealerString,currentTop + 1, ConsoleColor.Green, ConsoleColor.DarkGreen); //dealer info
            Art.DrawLine(0, currentTop + 2, Console.WindowWidth, '~', '~', ConsoleColor.Green, ConsoleColor.DarkGreen); //last bottom divider line
            // --- drawing the player side fof the screen
            int userTopGUI = Renderer.screenBottom - 5;
            Art.DrawLine(0, userTopGUI, Console.WindowWidth, '~', '~', ConsoleColor.Magenta, ConsoleColor.DarkMagenta); //first top divider
            Renderer.WriteCenterText(playerString, userTopGUI + 1, ConsoleColor.Magenta, ConsoleColor.DarkMagenta);
            Art.DrawLine(0, userTopGUI + 2, Console.WindowWidth, '=', '=', ConsoleColor.Magenta, ConsoleColor.DarkMagenta); //first bottom divider
            Art.DrawLine(0, Renderer.screenBottom - 1, Console.WindowWidth, '=', '=', ConsoleColor.Magenta, ConsoleColor.DarkMagenta); //last top divider
        }
    internal static int WriteCenterText(string text, int top, ConsoleColor c1, ConsoleColor c2)
    {
        int left = Console.WindowWidth / 2 - (text.Length / 2);
        Console.SetCursorPosition(left, top);
        foreach(char c in text)
        {
            if(text.IndexOf(c) % 2 == 0)
            {
                Console.ForegroundColor = c1;
                Console.Write(c);
            }
            else
            {
                Console.ForegroundColor = c2;
                Console.Write(c);
            }
        }
        return top;
    }
    internal static int WriteText(string text, int left, int top, ConsoleColor c1, ConsoleColor c2)
    {
        Console.SetCursorPosition(left,top);
        foreach(char c in text)
        {
            if(text.IndexOf(c) % 2 == 0)
            {
                Console.ForegroundColor = c1;
                Console.Write(c);
            }
            else
            {
                Console.ForegroundColor = c2;
                Console.Write(c);
            }
        }
        return top;
    }
    internal static (string, string) GuiGetBinaryString(string string1, string string2, int selectedIndex)
    {
        if(selectedIndex == 0)
        {
            return ($"[x] " + string1, "[ ]" + string2);
        }
        else if(selectedIndex == 1)
        {
            return ($"[ ] " + string1, "[x]" + string2);
        }
        return ("","");
    }
    internal static void ClearLine(int top)
    {
        Console.SetCursorPosition(0, top);
        string clear = new string(' ', Console.WindowWidth);
        Console.Write(clear);
    }
    internal static string GetInputString(bool spl, bool ins, int selectedAction, List<Program.Input> availableActions)
    {
        string space = "        ";
        string output = "";
        if(selectedAction == (int)Program.Input.Hit && availableActions.Contains(Program.Input.Hit))
        {
            output += "[x] Hit" + space;
        }
        else if(selectedAction != (int)Program.Input.Hit && availableActions.Contains(Program.Input.Hit))
        {
            output += "[ ] Hit" + space;
        }
        else
        {
                
        }
        if(selectedAction == (int)Program.Input.Stand && availableActions.Contains(Program.Input.Stand))
        {
            output += "[x] Stand" + space;
        }
        else if(selectedAction != (int)Program.Input.Stand && availableActions.Contains(Program.Input.Stand))
        {
            output += "[ ] Stand" + space;
        }
        else
        {
                
        }
        if(selectedAction == (int)Program.Input.DoubleDown && availableActions.Contains(Program.Input.DoubleDown))
        {
            output += "[x] Double Down" + space;
        }
        else if(selectedAction != (int)Program.Input.DoubleDown && availableActions.Contains(Program.Input.DoubleDown))
        {
            output += "[ ] Double Down" + space;
        }
        else
        {
                
        }
        if(selectedAction == (int)Program.Input.Split && availableActions.Contains(Program.Input.Split))
        {
            output += "[x] Split" + space;
        }
        else if(selectedAction != (int)Program.Input.Split && availableActions.Contains(Program.Input.Split))
        {
            output += "[ ] Split" + space;
        }
        else
        {
                
        }
        if(selectedAction == (int)Program.Input.Insurance && availableActions.Contains(Program.Input.Insurance))
        {
            output += "[x] Insurance" + space;
        }
        else if(selectedAction != (int)Program.Input.Insurance && availableActions.Contains(Program.Input.Insurance))
        {
            output += "[ ] Insurance" + space;
        }
        else
        {
                
        }
        return output;
    }
    internal static void ClearSegment(int left, int top, int count)
    {
        string blank = "";
        for(int i = 0; i < count; i++)
        {
            blank += ' ';
        }
        Console.SetCursorPosition(left, top);
        Console.Write(blank);
    }
    internal static void ClearInputGUI()
    {
        string clear = new(' ', Console.WindowWidth);
        Console.SetCursorPosition(0, Renderer.screenBottom - 2);
        Console.Write(clear);

    }
    internal static void DrawArt(int left, int top, int factor, 
    string art, ConsoleColor c1, ConsoleColor c2)
    {
        Console.SetCursorPosition(left, top);
        string[] lines = art.Split("\n");
        foreach(string _ in lines) 
        {
            Console.SetCursorPosition(left, top); 
            if(top % factor == 0)
            {
                Console.ForegroundColor = c1;
                Console.Write(_); top++;
            }
            else
            {
                Console.ForegroundColor = c2;
                Console.Write(_); top++;
            }
        }
    }
}