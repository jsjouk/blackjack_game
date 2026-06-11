***NOTE: This version only runs on MacOS. I am working on a fix for Windows.

An attempt at my first polished C# project. I enjoyed creating my previous card game so much that I wanted to take a stab
at something a little bit more complex. Working on this really solidified my understanding of OOP concepts, as I enforced a strict no AI policy on myself for this project. I did consult ChatGPT to fix a bug in the Menu.cs file, specifically regarding handling leftovers when splitting the userdata save files into (x) * 5 arrays. I have really enjoyed creating this so far and feel rewarded by the level of ownership over this project I can take. 

It has been more difficult than planned to fix the Windows-only bug. Once I make it compatible for Windows, the last big hurdle is redesigning the hand system to ensure I can handle splitting elegantly. I spent a lot of time so far trying to get the UI right, as one of my goals for the project was to create selection cursor controlled by the arrow keys for UI instead of commands or magical key presses. The basic game loop of Blackjack is implemented (hit, stand, double-down), but insurance is broken and splitting is still to-be implemented. The last thing I've done on this project was creating a Hand class to easily allow splitting, which was quicker than expected to substitute. I haven't had much time to work on this lately, but the last (foreseen) challenge in this project will be the method that renders multiple or split hands from the user. After that is done, as well as adding some polish / redoing some of the menus, I am interested in adding the following over time:

  1) Settings menu (Customize the color scheme and display elements)
  2) Cheat codes (Scripted hands that always make 21, X-Ray vision that can see the dealer's hidden card and upcoming ones in the shoe)
  3) Sounds
  4) Achievements and superlatives (max bet won/lost, became a millionaire, 5+ card 21, etc)
  5) Some sort of campaign mode where you progress through different stakes based on your chip balance
     
TO RUN
------
- Ensure you have .NET installed on your system
- After pulling the repository, navigate to ~/blackjack_game in your terminal
- Ensure you are selecting the directory (command: ls), you should see all of the .cs files
- Run command: dotnet run
