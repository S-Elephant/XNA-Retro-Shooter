  ______     _               _____ _                 _              _____ 
  | ___ \   | |             /  ___| |               | |            |_   _|
  | |_/ /___| |_ _ __ ___   \ `--.| |__   ___   ___ | |_ ___ _ __    | |  
  |    // _ \ __| '__/ _ \   `--. \ '_ \ / _ \ / _ \| __/ _ \ '__|   | |  
  | |\ \  __/ |_| | | (_) | /\__/ / | | | (_) | (_) | ||  __/ |     _| |_ 
  \_| \_\___|\__|_|  \___/  \____/|_| |_|\___/ \___/ \__\___|_|     \___/ 
                                                                        

==============================================================================
General
==============================================================================

- A top-down shooter created and designed in 2 days by Napoleon. Well actually
  it's total timespan is 3 days but I had to take a break that 2nd day.
- The portable version still requires the XNA 4.0 redistributable Framework
  and the .NET Framework 4.0.
- All artwork, FX and music were taken from www.OpenGameArt.org or self made.
- Created with C# & XNA.
- Implemented a custom broadphase collision detection algorithm. The collision
  detection may use multiple hitboxes per sprite.
- Ow yeah I was bored.
- The only good thing about writing a readme is that I can chose the ASCII
  art. But I'll regret it later if I don't.

==============================================================================
Installation Issues
==============================================================================

Error:
You receive an error like "Already Installed".
Solution:
Uninstall the previous installation of RetroShooter. Backup your Settings and
highscores xml files if you'd like to keep them.

==============================================================================
Gameplay & Game Mechanics
==============================================================================

Pickup types:
1. Rocket       : Rockets
2. Red Cross    : +50 HP
3. Round Orange : Auto Aim Cannon. Aim's for a random enemy.
4. Boomerang    : Boomer Cannon 
5. Yellow       : Upgrades Ship Speed (stackable but is capped at some point)
6. Shield(blue) : Upgrades the ships max shields. (stackable but is capped at
                  some point)
7. Shield(green): Upgrades the shield regeneration rate. (stackable but is
                  capped at some point).
8. Dual rockets : Dual 45 degree missiles.



Level:
A: Crashing into enemies will kill them but will cause a lot of damage to
   your ship. This will not reward you the kill bounty.
B: Killing enemies rewards $.
C: Killing enemies gives a loot chance.
D: When you already have the weapon displayed on a pickup or when you are
   capped on that item then you will be healed for 15 HP instead.
E: When you are hit for a certain amount of damage you have 50% chance to
   have a random weapon other than your machineguns destroyed. Shield damage
   does not count.
F: When you are healed half of the amount healed is reducted from the total
   damage taken that is used to randomly destroy a weapon.
G: Enemies are spawned in waves. Higher waves contain new enemy types and will
   spawn more units.
H: When you start on wave 10+ you receive a single speed & shield regen for
   every 10th level (rounded down). So when you start on wave 20 you receive 2
   speed and 2 speed regeneration upgrades.
I: MG bullets are colored dark red on the icy maps. It's too hard to spot them
   otherwise.
J: You receive a bonus equal to waveNr * 200 * score modifier when you clear
   all enemies before the next wave starts. 
K: The big floating scrapstation that can not be hit by your bullets is a
   shop. Fly over it to open the shop. It may randomly appear starting from
   wave 12.
L: Tier II equipment can be unlocked in the shop. They are not cheap.
M: The score (cash) modifier is affected by your game settings like the spawn
   delay, droprate, etc.
N: Deposit exess $ in the Shops bank to receive it back with a random 15%-25%
   interest after 8 waves. You can't spend the money while it is on the bank
   and when you die it is not addded to your final score. You will hear a coin
   drop sound when you receive your money+interest. This is not affected by
   the score modifier.
O: HP regeneration does not reduce the amount of damage taken for losing a
   weapon.
P: Nukes can not be destroyed. They are yours until you die or use them.



Controls:
- Use the arrows keys or mouse (change in options) to fly.
- Press [ESC] to exit to the main menu when playing a level or to return to
  the level when shopping.
- Press [P] to pause/unpause the game.
- Press [SPACE]/[RIGHT MOUSE BUTTON] to launch a nuclear bomb (if you have
  any).
- Press [X] to synchronize all weapons.



Ship Types:
Normal:
	- Everything standard, no bonuses nor penalties.
	- Starts with extra cash.
Destroyer:
	- More damage (all weapons)
	- Flies slower.
	- Is bigger and thus is easier to get hit.
Regeneration:
	- More shield regeneration.
	- Slightly more shields.
	- Regenerates some HP over time.
Cruiser:
	- Flies a lot faster
	- Deals slightly less damage.
	- Has somewhat less hitpoints.
	- Slightly more shields.
	- Starts with extra cash.
Tanker:
	- Has almost twice the hitpoints.
	- Deals slightly less damage.
	- Flies slower (max-speed is even lower than the Destroyer type)
	- Has somewhat more shields.
	- Is bigger and thus is easier to get hit.
	- Regenerates a little HP over time.
	- Loses weapons after taking a certain amount of damage 3x slower.
==============================================================================
Known Issues & Bugs
==============================================================================

- None.

==============================================================================
Hints / Spoilers
==============================================================================

- Clear the waves early game before the next wave starts to receive a huge
  bonus. You might need this bonus to survive the higher levels. If you do
  well and are lucky enough to visit the shop at level 12 then you can buy
  Tier 2 weapons at level 12 which is really overpowered for the next 10
  levels.
- Use your shield to clear the lower waves fast.
- Save your nukes for waves 45-60.

==============================================================================
Changelog
==============================================================================

Version 0.9.7 (November 08 2011)
	- Added different player ships
	- Added the option to show a round GUI around the player displaying the
	  players hp and shields.
	- Some enemies no longer reappear on the top after getting to the bottom
	  of the screen. This is to reduce the amount of ships on the screen on
	  the higher levels.
	- Settings from a previous version will be automatically overwritten.
	  The higshcores will stay intact.
	- The options menu now responds to the escape key.
	- Added the option to synchronize all weapons by pressing [X].
	- Added a HP Regen item to the shop and every ship can upgrade the HP
	  regeneration at least once.
	- The Game Options Menu now shows the score modifier.
	- Set default wave delay from 15 to 18 seconds and changed the values.
	- The player receives a bonus score for each killed ship after the player
	  died.
	- Extended the highscores. Old highscores will be reset when viewing the
	  highscores the first time.
	- Added a new level music (Poss).

Version 0.9.6a (Oktober 30 2011)
	- Added crash logs.
	- Fixed a crash from version 0.9.6 that was caused by the new enemy. They
	  sometimes spawned outside the collision grid.

Version 0.9.6 (Oktober 29 2011)
	- Extended the readme
	- Prevented the background music from restarting itself when browsing
	  through certain menus.
	- Added a shopkeeper with a short tutorial dialog.
	- Added different music to the shop.
	- Fixed a bug that caused the HP of all enemies to be set to 40 after the
	  enemy object pool started at the first item again.
	- Added a new enemy that comes from the side and goes to the other side.
	  It also moves slightly down with the same speed as the background. These
	  spawn on wave 10+.

Version 0.9.5 (October 06 2011)
	- Added starting resources when starting out on a higher level.
	- Forced the Scrap Station to spawn when starting on level >= 20.
	- Added the option to show the spawn delay on screen.

Version 0.9.4 (October 03 2011)
	- Added mouse support.
	- The shop is now forced to spawn if it hasn't spawned for 10 waves in
	  a row.
	- Added tier 2 weapons and tier 2 offensive & defensive upgrades in the
	  shop.
	- Droprates have been slightly lowered.
	- Created a new projectile texture for the AutoAim gun.
	- Increased the bounty on enemies and some other minor balances.
	- Added a new feature: Interest.
	- Picking up shield items also restores some shield points.
	- Fixed a bug where the music continued to play for several seconds after
	  the game was shutdown (properly).
	- I made lots of changes in a short time with this version but it got a
	  lot easier now. I managed to beat level 60 on my third attempt including
	  all achievements. I'm just afraid for the bugs due to the short testing
	  time.

Version 0.9.3 (October 02 2011)
	- Fixed spawn location of 2 enemy types (one was too close to the level
	  and one was too far away).
	- Fixed a mistake in the credits.
	- Added an after burner texture to the player.
	- Limited the suicider enemies to 7 per wave and made their speed random.
	  Their speed is on average between 7 to 9. It used to be always 9.
	- Increased bombard collision damage from 15 to 30.
	- Added a big floating scrapstation. It is a shop.
	- Added the N-Bomb.
	- Added a shield sprite to the player.

Version 0.9.2 (October 01 2011)
	- Optimized the audio playback to limit garbage. Removed XACT entirely.
	- Fixed some small bugs in the menu's.
	- Added a new menu that allows the player to enter his/her name for the
	  highscores.
	- Changed the timestamp on the highscores and added the killcount.
	  Highscores from previous versions (0.9.1 & 0.9.0) must be deleted
	  because they are no longer compatible.
	- Added more music and effects.
	- Fixed the scrolling background again because it was still bugging.
	- Added a new font for the score. Also fixed a minor layout issue here.
	- Added a score modifier. The more difficult you set your game options the
	  higher the modifier and vice versa.
	- AutoAim now aims for the bottom of an enemy instead of the center part.
	- Fixed the droprate None to zero. Before it was still 1%

Version 0.9.1 (September 30 2011)
	- Added a shield and a shield pickup.
	- Improved some graphics.
	- Fixed a bug where the shadows are drawn on top of other ships.
	- Added a wave cleared bonus.
	- More garbage optimizations.
	- Optimized the broadphase collision check.
	- Limited the amount of simultaneous MachineGun enemies as they are pretty
	  cpu heavy in large numbers.
	- Changed some stats.
	- Added a new enemy with a 100% item dropchance.
	- Added more enemies.
	- Added a new gun: Dual missile 45 for both the cpu and the players with
	  pickup.
	- There was a small chance for 0 enemies to spawn in a wave. I fixed that.
	  A minimum of 5 enemies are now enforced on the lower waves.
	- Fixed a bug that didn't update the hp-bar after receiving a heal.
	- Increased the amount healed from the hp pickup from 50 to 65.
	- Added a shield regeneration upgrade pickup. Also added this one as a
	  bonus for starting on higher waves.
	- Increased the HP gained from an item that the player already has or
	  has capped from 10 to 15.
	- When a gun from the player is destroyed an explosion is shown on the
	  player's ship.
	- Added the aquired/missing/lost guns to the GUI.
	- Pooled all guns, proectiles and enemies.
	- Pooled the visuals correctly. They seemed to allocate new visuals in
	  the old pool... Visuals now also appear after killing a lot of enemies
	  as that also seemed to bug because if that in version 0.9.0.
	- Spawn delays are slightly increased every 5th level. This effect is
	  already applied when starting out on a higher level.
	- Velocities have been altered. The FPS is now locked at 30.
	- Added a kill counter.
	- Fixed the scrolling background. 0.9.0 had a bug that showed a small
	  gap between them.

Version 0.9.0 (September 29 2011)
	- First release.