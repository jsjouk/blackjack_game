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
        DrawLine(0, 0, Console.WindowWidth, '=', '=', ConsoleColor.Green, ConsoleColor.DarkGreen); //first top divider line
        WriteCenterText(persistentData, 1, ConsoleColor.Green, ConsoleColor.DarkGreen); //user info
        DrawLine(0, 2, Console.WindowWidth, '=', '=', ConsoleColor.Green, ConsoleColor.DarkGreen); //first bottom divider line
        int dealerArtXOffset = Art.DealerHeader.Split("\n")[0].Length; 
        int dealerArtYOffset = Art.DealerHeader.Split("\n").Count();
        DrawArt(midpointX - (dealerArtXOffset / 2), 4, 2, Art.DealerHeader, ConsoleColor.Green, ConsoleColor.DarkGreen); //dealer art
        DrawLine(0, 4 + dealerArtYOffset, Console.WindowWidth, '=', '=', ConsoleColor.Green, ConsoleColor.DarkGreen); //last top divider line
        int currentTop = Console.GetCursorPosition().Item2;
        WriteCenterText(dealerString,currentTop + 1, ConsoleColor.Green, ConsoleColor.DarkGreen); //dealer info
        DrawLine(0, currentTop + 2, Console.WindowWidth, '~', '~', ConsoleColor.Green, ConsoleColor.DarkGreen); //last bottom divider line
        // --- drawing the player side fof the screen
        int userTopGUI = screenBottom - 5;
        DrawLine(0, userTopGUI, Console.WindowWidth, '~', '~', ConsoleColor.Magenta, ConsoleColor.DarkMagenta); //first top divider
        WriteCenterText(playerString, userTopGUI + 1, ConsoleColor.Magenta, ConsoleColor.DarkMagenta);
        DrawLine(0, userTopGUI + 2, Console.WindowWidth, '=', '=', ConsoleColor.Magenta, ConsoleColor.DarkMagenta); //first bottom divider
        DrawLine(0, screenBottom - 1, Console.WindowWidth, '=', '=', ConsoleColor.Magenta, ConsoleColor.DarkMagenta); //last top divider
    }
    internal static void RenderSaveFiles(int startLeft, int startTop, int selectedX, int selectedY, List<string[]> files)
    {
        string selectedFileString;
        Console.SetCursorPosition(startLeft, startTop);
        if (files[selectedX][selectedY].Length < 10) //standardizing string lengths so they can be displayed beautifully
        {
            selectedFileString = "[x] " + files[selectedX][selectedY] + new string(' ', 10 - files[selectedX][selectedY].Length);
        }
        else if(files[selectedX][selectedY].Length > 10)
        {
            selectedFileString = "[x] " + files[selectedX][selectedY][0..9];
        }
        else
        {
            selectedFileString = "[x] " + files[selectedX][selectedY];
        }
        int gap = 0;
        foreach(string[] group in files)
        {
            string groupString = "";
            for(int indexY = 0; indexY < group.Length; indexY++)
            {
                if(files.IndexOf(group) == selectedX && indexY == selectedY)
                {
                    groupString += selectedFileString + "\n";
                }
                else
                {
                    if(group[indexY].Length < 10)
                    {
                        groupString += "[ ] " + group[indexY] + new string(' ', 10 - group[indexY].Length) + "\n";
                    }
                    else if(group[indexY].Length > 10)
                    {
                        groupString += "[ ] " + group[indexY][0..9] + "\n";
                    }
                    else
                    {
                        groupString += "[ ] " + group[indexY] + "\n";
                    }
                    
                }
            }
            Console.SetCursorPosition(startLeft + gap, startTop);
            DrawArt(startLeft + gap, startTop, 2, groupString, ConsoleColor.Yellow, ConsoleColor.DarkYellow);
            gap += 20;
        }
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
            }
            else
            {
                Console.ForegroundColor = c2;
            }
            Console.Write(c);
        }
        return top;
    }
    internal static int WriteCenterpointText(string text, int left, int top, ConsoleColor c1, ConsoleColor c2)
    {
        int offsetX = text.Length / 2;
        Console.SetCursorPosition(left - offsetX, top);
        foreach(char c in text)
        {
            if(text.IndexOf(c) % 2 == 0)
            {
                Console.ForegroundColor = c1;
            }
            else
            {
                Console.ForegroundColor = c2;
            }
            Console.Write(c);
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
    internal static void ClearSegment(int left, int top, int count, bool centered)
    {
        int offsetX = 0;
        if (centered)
        {
            offsetX = count / 2;
        }
        string blank = "";
        for(int i = 0; i < count; i++)
        {
            blank += ' ';
        }
        Console.SetCursorPosition(left - offsetX, top);
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
    public static int DrawLine(int left, int top, int length, 
    char symbol1, char symbol2, ConsoleColor c1, ConsoleColor c2)
    {
        string line = new string('_',length);
        Console.SetCursorPosition(left, top);
        for(int j = 0; j < length; j++)
        {
            if(j % 2 == 0)
            {
                Console.ForegroundColor = c1;
                Console.Write(symbol1);
            }
            else
            {
                Console.ForegroundColor = c2;
                Console.Write(symbol2);
            }
        } 
        return top;
    }
    public static int DrawCenterpointLine(int left, int top, int length, 
    char symbol1, char symbol2, ConsoleColor c1, ConsoleColor c2)
    {
        string line = new string('_',length);
        int offsetX = line.Length / 2;
        Console.SetCursorPosition(left - offsetX, top);
        for(int j = 0; j < length; j++)
        {
            if(j % 2 == 0)
            {
                Console.ForegroundColor = c1;
                Console.Write(symbol1);
            }
            else
            {
                Console.ForegroundColor = c2;
                Console.Write(symbol2);
            }
        } 
        return top;
    }
}