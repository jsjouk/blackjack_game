
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
            Art.DrawLine(left,top+1,choices.Item2.Length,'-','-',c1,c2);
            keyStroke = Console.ReadKey(true);
            if (stacked)
            {
                Renderer.WriteText(choices.Item1, left, top + 2, c1, c2);
                Renderer.WriteText(choices.Item2, left, top + 3, c1, c2);
                if(keyStroke.Key == ConsoleKey.UpArrow && selectedIndex != 0)
                {
                    selectedIndex--;
                    Renderer.ClearSegment(left,top+2,choices.Item1.Length);
                    Renderer.ClearSegment(left,top+3,choices.Item1.Length);
                    choices = Renderer.GuiGetBinaryString(choice1,choice2,selectedIndex);
                    Renderer.WriteText(choices.Item1, left, top + 2, c1, c2);
                    Renderer.WriteText(choices.Item2, left, top + 3, c1, c2);
                }
                else if(keyStroke.Key == ConsoleKey.DownArrow && selectedIndex!= 1)
                {
                    selectedIndex++;
                    Renderer.ClearSegment(left,top+2,choices.Item1.Length);
                    Renderer.ClearSegment(left,top+3,choices.Item1.Length);
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
                    Renderer.ClearSegment(left, top+2,choiceString.Length);
                    choices = Renderer.GuiGetBinaryString(choice1,choice2,selectedIndex);
                    choiceString = choices.Item1 + "       " + choices.Item2;
                }
            }
        }
        while(keyStroke.Key != ConsoleKey.Enter);
        return selectedIndex == 0;
    }
    
    public static string TextInput(string text)
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
    public static int NumericInput(string text)
    {
        while (true)
        {
            Console.WriteLine(text);
            string answer = Console.ReadLine()??"";
            try
            {
                return int.Parse(answer);

            }
            catch (Exception ex)
            {
                Console.Write($"\n {ex.Message}");
                continue;
            };
        }
    }
    public static void AwaitKeystroke()
    {
        int midpointX = Console.WindowWidth / 2;
        string message = "[ PRESS ANY KEY ]";
        Console.SetCursorPosition(midpointX - (message.Length / 2), Console.WindowHeight - 3);
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
            Art.DrawLine(midpointX - (message.Length / 2), Console.WindowHeight - 2, 
            message.Length, '-', '-', ConsoleColor.Green, ConsoleColor.DarkGreen);
            ConsoleKeyInfo pressed;
            pressed = Console.ReadKey(true);
            if (pressed.Equals(pressed))
            {
                int currentTop = Console.GetCursorPosition().Item2;
                Console.SetCursorPosition(0, currentTop);
                string clear = new string(' ', Console.WindowWidth);
                Console.Write(clear);
                Console.SetCursorPosition(0, currentTop - 1);
                Console.Write(clear);
                break;
            }
        }
    }
    internal static LoadedUser LoadSaveFile()
    {
        while (true)
        {
            Console.Clear();
            int savesTextHeight = 1;
            int midpointX = Console.WindowWidth / 2; //setup for drawing save logo
            string[] savesIconLines = Art.OldSavesIcon.Split("\n"); 
            int savesIconLeft = midpointX - (savesIconLines[0].Length / 2);
            Renderer.DrawArt(savesIconLeft, savesTextHeight, 2, Art.OldSavesIcon, ConsoleColor.Magenta, ConsoleColor.DarkMagenta);
            int dividerHeight = savesTextHeight + savesIconLines.Length + 1;
            Console.SetCursorPosition(0, dividerHeight); //divider line
            for(int j = 0; j < Console.WindowWidth; j++)
            {
                if(j % 10 < 3)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("-");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write("-");
                }
            } 
            Console.Write("\n");
            string path = User.CWD + "/userdata";
            var filePaths = Directory.GetFiles(path);
            List<string> paths = filePaths.ToList(); paths.Remove(path + ".DS_Store");
            int i = 0;
            List<ConsoleColor> colors = [ConsoleColor.Yellow, ConsoleColor.DarkYellow];
            Random rng = new Random();
            foreach(string filePath in paths) //Drawing the save files
            {
                int ticket = rng.Next(0,colors.Count()-1);
                Console.ForegroundColor = colors[ticket];
                Console.WriteLine($"[{i}] {Path.GetFileName(filePath)[0..^4]}");
                i++;
            }
            Console.SetCursorPosition(0, savesTextHeight + dividerHeight + filePaths.Count()); //lower divider line
            for(int j = 0; j < Console.WindowWidth; j++)
            {
                if(j % 10 < 3)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("-");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write("-");
                }
            } 
            Console.SetCursorPosition(0, 13);
            while (true)
            {
                ConsoleKeyInfo input;
                input = Console.ReadKey(true);
                try
                {
                    if(input.KeyChar - '0' > filePaths.Count() - 1)
                    {
                        string text = $"Selected save file [{input.KeyChar - '0'}] does not exist...";
                        Renderer.WriteCenterText(text, Console.WindowHeight - 2, ConsoleColor.Red, ConsoleColor.DarkRed);
                        continue;
                    }
                        else
                    {
                        //Initialize LoadedUser object
                        i = 0;
                        foreach(string filePath in paths)
                        {
                            if(i == input.KeyChar - '0')
                            {
                                User selected = User.CreateUserObject(filePath);
                                Console.WriteLine($"Loaded user: {selected.Name}");
                                LoadedUser player = new LoadedUser(selected.Name, selected.LuckyNumber, selected.Chips);
                                return player;
                            }
                            else
                            {
                                i++;
                            }
                        }
                        continue;
                    }
                }
                catch
                {
                    Console.WriteLine("Save file does not exist or input was non-numeric");
                }
                
            }
        }
    }
}
