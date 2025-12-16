"# GAME351_Final_Project" 

Our game features a humble farmer fending off a field of interdimensional vermin to
collect enought crypto coins to enter the portal in order to beat the level. A 
procedurally generated wheat maze is created at the beginning of the game of a specified
width, and a predetermined number of shotgun shells, health, and crypto is spawned
in the maze for the player to collect, with vermin walking around the maze that can
attack the player by shooting plasma. 

The maze is generated using an algorithm that populates bitmap, which is then sampled
for each element of a grid to determine wall or open tiles. Each tile is represented by
a structure "MazeGridTile" contained within WheatMaze.cs that holds the grids grid position,
global position, a boolean flag to determine if it's filled, an ID, and a reference to the
gameobject of it's wall object if filled. After generating the maze walls, open grid
spaces are picked at random iteratively for each object type (ammo, health, crypto) to
be placed on the map. After all objects are placed, the vermin are spawned on random
open grid tiles, followed by the player. 

The player has a PlayerControll script, Health script, and shooting script for the shotgun.
The PlayerControll script handles mouse looking and movement, along with containing the trigger
function that is called when the player interacts with a crypto coin. The Health script increments
health when interacting with a health object via a collision trigger, and is linked with a
UI element displaying the health. The Shooting script handles firing the shotgun, and 
contains a collision trigger that increments the amount of ammo when interacting with
an ammo object in the maze. When shooting, a bright light at the front of the shotgun is
enabled for a fraction of a second, a particle system containing only a few particles is
emmitted with a muzzel flash texture for a fraction of a second, and satisfying shotgun
blast sound is played to give the feel and power of a gun being fired at night. Three 
raycasts are emmited from the front of the shotgun barrel with random angular deviations.
Each ray can independently interact with a vermin, and when hitting one, reduces health of
the vermin and emmits a blood spatter particle system at the location it hit. For the 
player to receive damage, the particles of plasma from the vermin have collision triggers
enabled with the player's collider, and each one that collides with the player takes a 
small amout of health (currently 0.5 out of 100) from the players health.

Each vermin has a single script (Vermin.cs) that controls movement, health, and attacks. for
movement, the vermin has assigned a current maze tile and a next maze tile, and the position
is lerped between the two by adding a movement speed multiplied with deltaTime added to a 'dt'
value that ranges from 0 to 1. Rotation works in a similar way, and is lerped over time to look
at the next tile. The vermin can only move towards the next tile when facing it. Once the vermin's
movement dt value is greater that or equal to 1, the current tile is set to the "next" tile, and 
new "next" tile is selected. To determin the next tile, the open tiles in front, to the left, and to the
right of the vermin are placed in a list and selected at random.  If the front, left, and right tiles 
are all filled, then the tile behind the vermin is selected. A raycast is fired in the forward direction
from the vermin, and if it hits the player, the vermin stops completely and starts a timer with 
random variation that triggers the shooting of plasma particles from the mouth.

A timeline is used to play the game music and fade betwee the three bluegrass music tracks, and repeats
when all three tracks are played. Once the player has collected a certain amount of crypto, the portal
is spawned at the pre-determined exit tile, which is randomly selected from one of the walls of the maze.
The exit portal has a collision trigger than opens a "win scene". If the player loses all health, a 
"lose scene" is opened.

CONTRIBUTIONS:

Dante Bernel:
	* Found three decent blue-grass songs for the sound track.

Emanuel Linton:
	* Created the Game UI and win scene UI.
	* Create the initial health script. 
	* Created the initial shooting script (that shot out a flat plane from the gun).
	* Created the initial exit portal script with a simple portal object.

Jonathan Rurka:
	* Created the maze generator that placed wheat walls, ammo, health, crypto objects, and vermin.
	* Custom made the wheat stock walls from another plant model (added wheat texture to it).
	* Created the player controll script.
	* Improved the shooting script to use raycasts and to interact with the vermin to reduce health,
		the gun fire effects, and pick up ammo.
	* Created logic for maze objects to rotate and play sound effects when picked up.
	* Added Logic to the health script to pick up health objects.
	* Create the vermin movement, attack logic, animations, and particle systems.
	* Improved the exit portal script to be placed properly, and found the final model.
	* Added all sound effects, and timeline for music.
	* found/Added skybox and terrain. Placed wheat outside maze walls using terrain tree placer.
	* Created overall game controll logic to trigger the creation of the map and transition to 
		win/loose scenes.
