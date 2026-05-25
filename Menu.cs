
public class Menu
{
    internal static bool BinaryMenu(bool stacked, string prompt, string choice1, 
    string choice2, int left, int top, ConsoleColor c1, ConsoleColor c2)
    {
        ConsoleKeyInfo keyStroke;
        int selectedIndex = 0;
        do
        {
            (string, string) choices = Renderer.GuiGetBinaryString(choice1, choice2, selectedIndex);
            Renderer.WriteText(prompt,left,top,c1,c2);
            Renderer.DrawLine(left,top+1,choices.Item2.Length,'-','-',c1,c2);
            keyStroke = Console.ReadKey(true);
            if (stacked)
            {
                Renderer.WriteText(choices.Item1, left, top + 2, c1, c2);
                Renderer.WriteText(choices.Item2, left, top + 3, c1, c2);
                if(keyStroke.Key == ConsoleKey.UpArrow && selectedIndex != 0)
                {
                    selectedIndex--;
                    Renderer.ClearSegment(left,top+2,choices.Item1.Length,false);
                    Renderer.ClearSegment(left,top+3,choices.Item1.Length,false);
                    choices = Renderer.GuiGetBinaryString(choice1,choice2,selectedIndex);
                    Renderer.WriteText(choices.Item1, left, top + 2, c1, c2);
                    Renderer.WriteText(choices.Item2, left, top + 3, c1, c2);
                }
                else if(keyStroke.Key == ConsoleKey.DownArrow && selectedIndex!= 1)
                {
                    selectedIndex++;
                    Renderer.ClearSegment(left,top+2,choices.Item1.Length,false);
                    Renderer.ClearSegment(left,top+3,choices.Item1.Length,false);
                    choices = Renderer.GuiGetBinaryString(choice1,choice2,selectedIndex);
                    Renderer.WriteText(choices.Item1, left, top + 2, c1, c2);
                    Renderer.WriteText(choices.Item2, left, top + 3, c1, c2);
                }
            }
            else
            {
                string choiceString = choices.Item1 + "       " + choices.Item2;
                Renderer.WriteText(choiceString, left, top + 2, c1, c2);
                if(keyStroke.Key == ConsoleKey.RightArrow && selectedIndex != 1)
                {
                    selectedIndex++;
                    Renderer.ClearSegment(left, top+2,choiceString.Length,false);
                    choices = Renderer.GuiGetBinaryString(choice1,choice2,selectedIndex);
                    choiceString = choices.Item1 + "       " + choices.Item2;
                }
            }
        }
        while(keyStroke.Key != ConsoleKey.Enter);
        return selectedIndex == 0;
    }
    public static string TextInput(int left, int top, string text, int min, int max)
    {
        while (true)
        {
            Console.Write(text + "      ");
            string answer = Console.ReadLine()??"";
            if(answer.Length < 2)
            {
                Console.Clear();
                Console.WriteLine("Not a valid input! Enter at least two characters...");
                continue;
            }
            else
            {
                return answer;
            }
        }
    }
    public static int NumericInput(string prompt, string error, int left, int top, int max, ConsoleColor c1, ConsoleColor c2)
    {
        bool extendLine = true;
        if(max == 0)
        {
            max = Console.WindowWidth - left;
            extendLine = false;
        }
        while (true)
        {
            Renderer.WriteCenterpointText(prompt, left, top, c1, c2);
            if (extendLine)
            {
                Renderer.DrawCenterpointLine(left, top + 1, prompt.Length + max + 2, '+','+',c1,c2);
            }
            else
            {
                Renderer.DrawCenterpointLine(left, top + 1, prompt.Length, '+','+',c1,c2);
            }
            Console.SetCursorPosition(left - (prompt.Length / 2) + prompt.Length + 1, top);
            string input = Console.ReadLine()??"";
            if(int.TryParse(input, out int number))
            {
                if(number.ToString().Count() > max)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Renderer.ClearSegment(left, top, prompt.Length,true);
                    if (extendLine)
                    {
                        Renderer.ClearSegment(left, top, prompt.Length + max + 1,true);
                    }
                    else
                    {
                        Renderer.ClearSegment(left, top, prompt.Length,true);
                    }
                    Renderer.WriteCenterpointText(error, left, top, c1, c2);
                    Renderer.DrawCenterpointLine(left, top + 1, error.Length, 'x','x', c1,c2);
                    AwaitKeystroke("Press any key to dismiss",left, top+3,true,c1,c2);
                    continue;
                }
                else
                {
                    return number;
                }
            }
            else
            {
                AwaitKeystroke($"Please input a numeric value [any key to dismiss]",left,top+3,true,c1,c2);
                continue;
            }
        }
    }
    public static void AwaitKeystroke(string message, int left, int top, bool centered,ConsoleColor c1, ConsoleColor c2)
    {
        int offsetX = 0;
        if (centered)
        {
            offsetX = message.Length / 2;
        }
        Console.SetCursorPosition(left - offsetX, top);
        while (true)
        {
            foreach(char c in message)
            {
                if(message.IndexOf(c) % 2 == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green; Console.Write(c);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen; Console.Write(c);
                }
            }
            if (centered)
            {
                Renderer.WriteCenterpointText(message, left,top,c1,c2);
                Renderer.DrawCenterpointLine(left,top+1,message.Length,'-','-',c1,c2);
            }
            else
            {
                Renderer.WriteText(message, left, top, c1,c2);
                Renderer.DrawLine(left, top+1, message.Length, '-', '-', ConsoleColor.Green, ConsoleColor.DarkGreen);
            }
            ConsoleKeyInfo pressed;
            pressed = Console.ReadKey(true);
            if (pressed.Equals(pressed))
            {
                Renderer.ClearSegment(left,top,message.Length,centered);
                Renderer.ClearSegment(left,top+1,message.Length,centered);
                break;
            }
        }
    }
    internal static LoadedUser LoadSaveFile()
    {
        Console.Clear();
        int savesTextHeight = 1;
        int midpointX = Console.WindowWidth / 2; //setup for drawing save logo
        string[] savesIconLines = Art.OldSavesIcon.Split("\n"); 
        int savesIconLeft = midpointX - (savesIconLines[0].Length / 2);
        Renderer.DrawArt(savesIconLeft, savesTextHeight, 2, Art.OldSavesIcon, ConsoleColor.Magenta, ConsoleColor.DarkMagenta);
        int dividerHeight = savesTextHeight + savesIconLines.Length + 1;
        Console.SetCursorPosition(0, dividerHeight); //divider line
        Renderer.DrawLine(0, dividerHeight,Console.WindowWidth,'-','-',ConsoleColor.Magenta, ConsoleColor.DarkMagenta);
        Console.Write("\n");
        string path = Path.Combine(User.CWD, "userdata");
        string[] filePaths = Directory.GetFiles(path);
        string pathToRemove = Path.Combine(path,".DS_Store");
        List<string> paths = filePaths.ToList(); paths.Remove(pathToRemove);
        List<string[]> arrangedFiles = new();
        string[] fullColumn = new string[5];
        bool subColumnFiles = paths.Count() % 5 != 0;
        int remainingFiles = paths.Count() % 5;
        int indexPerRow = 0;
        foreach(string filePath in paths)
        {
            string saveDisplay = filePath.Replace(".txt", ""); saveDisplay = saveDisplay.Replace(path + "/", "");
            fullColumn[indexPerRow] = saveDisplay;
            indexPerRow++;
            if(indexPerRow == 5)
            {
                arrangedFiles.Add(fullColumn.ToArray());
                indexPerRow = 0;
            }
        }
        if (subColumnFiles)
        {
            string[] leftOvers = new string[remainingFiles];
            leftOvers = fullColumn[0..remainingFiles];
            arrangedFiles.Add(leftOvers);
        }
        
        Console.SetCursorPosition(0, savesTextHeight + dividerHeight + filePaths.Count()); //lower divider line
        Renderer.DrawLine(0, savesTextHeight + dividerHeight + 5,midpointX * 2, '-','-',ConsoleColor.Magenta, ConsoleColor.DarkMagenta);
        int saveFilesLeft = Renderer.centerpointQ2.Item1;
        int saveFilesTop = 7;
        Renderer.RenderSaveFiles(saveFilesLeft, saveFilesTop, 0, 0, arrangedFiles);
        ConsoleKeyInfo keyStroke;
        int selectedX = 0;
        int selectedY = 0;
        while(true)
        {
            keyStroke = Console.ReadKey(true);
                if(keyStroke.Key == ConsoleKey.UpArrow && selectedY != 0)
                {
                        selectedY--;
                        Renderer.RenderSaveFiles(saveFilesLeft,saveFilesTop, selectedX,selectedY,arrangedFiles);
                }
                if(keyStroke.Key == ConsoleKey.DownArrow && selectedY != arrangedFiles[selectedX].Count() - 1)
                {
                        selectedY++;
                        Renderer.RenderSaveFiles(saveFilesLeft,saveFilesTop, selectedX,selectedY,arrangedFiles);
                }
                if(keyStroke.Key == ConsoleKey.RightArrow && selectedX != arrangedFiles.Count() - 1)
                {
                    if(selectedY > arrangedFiles[^1].Count() - 1 & selectedX + 1 == arrangedFiles.IndexOf(arrangedFiles[^1]))
                    {
                        
                    }
                    else
                    {
                        selectedX++;
                        Renderer.RenderSaveFiles(saveFilesLeft,saveFilesTop, selectedX,selectedY,arrangedFiles);
                    }
                }
                if(keyStroke.Key == ConsoleKey.LeftArrow && selectedX != 0)
                {
                        selectedX--;
                        Renderer.RenderSaveFiles(saveFilesLeft,saveFilesTop, selectedX,selectedY,arrangedFiles);
                }
                if(keyStroke.Key == ConsoleKey.Enter)
                {
                    break;
                }
                Renderer.WriteCenterText(selectedX.ToString() + selectedY.ToString(),13, ConsoleColor.Red, ConsoleColor.DarkRed);
        }
        User player = User.CreateUserObject(path + $"/{arrangedFiles[selectedX][selectedY]}.txt");
        return new LoadedUser(player.Name, player.LuckyNumber,player.Chips);
    }
    internal static LoadedUser? CreateUserMenu()
    {
        return null;
    }
}
