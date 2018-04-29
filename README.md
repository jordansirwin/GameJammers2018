Avalache!
=========

A GameJammers 2018 game.


ToDo
----

* GameManager
  * Background music
  * Score keeping (based on avalache size)
  * Game state (main menu, gameplay, scoreboard)
    * STRETCH: pause game
* MapManager
  * stay relative to player so always spawns "under" them
  * spawn bonuses
  * spawn hazards
  * uses an array of prefabs to get objects to spawn
  * speeds up over time
* Bonuses (Prefabs)
  * Visuals
  * Sound effects for interaction
  * Increases avalache size modifier (therefore score!)
* Hazards (Prefabs)
  * Visuals
  * Sound effects for interaction
  * Has a speed modifier for how it impacts the player (pos or neg)
  * STRETCH: Speed adjustments for hazards (kids, yetis, trees, deer)
* Player
  * Visuals
  * Sounds
    * Got bonus
    * Hit hazard
  * Controller left/right
  * Tap or Hold button to go left/right
  * STRETCH: Top means slow down (but not to a complete stop)
* Avalache
  * Visuals
  * Sounds
    * More intense with size
  * Grow size with time
  * Screen shake?


* Intro Menu
  * Music
  * Start button
  * STRETCH: Cinematic start vai ski lift/helicoptor
* Exit scoreboard
  * Music
  * Retry button
  * Exit button

* Rewards/Prizes for high scores
  * During gameplay!
  * Think "who wants to be a millionare" ladder and eye-candy each time player scores next rank
  * "I'm double platinum" vs "I scored 123,432"



Q: What happens when player hits hazard
A: Player either slows down (tree) or speeds up (ice)

Q: What happens when player hits a bonus
A: Avalache gets bigger, therefore score increases

Q. How does the avalache look on screen?
A. Always on screen. Starts at the top. Consumes top 1/3 of screen. Moves up and down based on speed of player.

Q. How do we handle player speed increasing?
A. Speed increase moves player lower on screen. Slow down moves player higher on screen.

Q. What's the game exit criteria?
A. Player and avala==che collide