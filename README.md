***NOTE: This version only runs on MacOS, as I just tested in on Windows after sending it here and pulling it back from GitHub on my PC. 
It's simply a matter of changing a few lines in the file I/O code, so the next version will certainly work on Windows. 

An attempt at my first polished C# project. I enjoyed creating my previous Ride the Bus card game so much, and I wanted to take a stab
at something a little bit more complex. Working on this SERIOUSLY solidified my understanding of OOP concepts. Not a single line was
written by AI, but I did consult ChatGPT to fix a bug in the Menu.cs file, specifically regarding splitting the userdata save files
into (x) * 5 arrays. Nonetheless, all of the code, faulty or not, is my own, and I can explain every block and why it exists. 

Initially, the project was only 5 files: Program, User, Card, Deck, and Shoe. As I pushed onward, I realized it would be too laborious
to scale it to what I had envisioned, so I refactored multiple times. I ended up with a lot of code that could be used in a card game 
engine for the console. I will certainly make this happen at some point, but I need a break from card games after this is done. 
As of writing this (5/24/26), I estimate that I have spent roughly 20 hours on this across ~ 3 weeks. 

Once I make it compatible for Windows, the last big hurdle is redesigning the hand system to ensure I retain the will to live when
introducing splitting to the game loop. I spent a lot of time so far trying to get the UI right, as entering text or numbers to perform
most actions was beneath what I thought this project deserved. I can't believe it's taken so long to even get to the game loop. I'll keep
my nose on the grindstone, and I think I will have the base game in a week or two. Other things I have planned for this project (besides
being able to play Blackjack) are:

  1) Settings menu (Customize the color scheme and display elements)
  2) Cheat codes (Scripted hands that always make 21, X-Ray vision that can see the dealer's hidden card and upcoming ones in the shoe)
  3) Sounds
  4) Achievements and superlatives (max bet won/lost, became a millionaire, 5-10 card 21, etc)
  5) Possibly some sort of campaign mode where you progress through different stakes based on your chip balance
     
These are bonus features that I will only look into once you can play a full version of Blackjack. Wish me luck!
