public class User
{
    public int Chips {get; set;}
    public string Name {get; set;}
    public int LuckyNumber {get; set;}
    public List<List<Card>> ?Hands {get; set;}
    public User(string n, int ln, int c){Name = n; LuckyNumber = ln; Chips = c;}
    public static string CWD = Directory.GetCurrentDirectory();

    public static User CreateUser()
    {
        int midpointX = Console.WindowWidth / 2;
        string usernameQuery = "What is your name..?";
        string luckyNumberQuery = "What is your lucky number?";
        string numericErrorString = "Input must be less than 3 digits";
        Console.SetCursorPosition(midpointX - (usernameQuery.Length - 2), Console.WindowHeight - 3);
        int promptX = Renderer.midpointX; int promptY = Renderer.midpointY + 7;
        string username = Menu.TextInput(promptX,promptY,usernameQuery,1,8);
        int clearTop = Console.GetCursorPosition().Item2;
        Renderer.ClearLine(clearTop);
        int luckynumber = Menu.NumericInput(luckyNumberQuery,numericErrorString,promptX,promptY,2,ConsoleColor.Green,ConsoleColor.DarkGreen);
        Renderer.ClearLine(clearTop);
        User newUser = new User(username, luckynumber, 0);

        string confirmationString = $"CREATED NEW USER: {newUser.Name}\nLUCKY NUMBER: {newUser.LuckyNumber}\nCHIP BALANCE: {newUser.Chips}";

        //Renderer.WriteCenterText(confirmationString, Renderer.screenBottom - 2,)

        newUser.WriteSaveFile();

        return newUser;
    }
    public string GenerateUserData()
    {
        return $"{Name}\n{LuckyNumber}\n{Chips}";
    }
    public void WriteSaveFile()
    {
        string data = GenerateUserData();
        File.WriteAllText(CWD + "/userdata" + $"/{Name}{LuckyNumber}.txt", data);
    }
    public static User CreateUserObject(string path)
    {
        string[] lines = File.ReadAllLines(path);
        string nameUser = lines[0]; int luckynumberUser = int.Parse(lines[1]); int chipcountUser = int.Parse(lines[2]);
        return new User(nameUser, luckynumberUser, chipcountUser);
    }
}
public class LoadedUser : User
{
    public LoadedUser(string name, int luckynumber, int chips) : base(name, luckynumber, chips){}
}