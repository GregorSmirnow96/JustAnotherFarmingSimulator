REPEATED WORKFLOWS
	Adding an item:
		Create the class
			- Create the ground item prefab
			- Create the sprite
			- Create the left-click indicator
			- If it's a seed:
				- Create the stage models
				- Add the growth script to S1
				- Add the behaviour scripts to the last stage
			- Set the ground item prefab script's item ID
		Instantiate the class in the list in Items.cs
		If it's a seed, add the mapping to PlantSpawner.cs

TODOs:
	Coding/modelling tasks:
		Fix the walkability of the forest
		Make fog work at boundary of scene during the day
		Implement forest curse
		Create the hut
		Create the utility plants
		Create the crops
		Create the animals that will eat crops
		Add an inventory / container system. Don't worry about the UI being pretty at first
		Create a crafting system
		Create a gathering system
		Create cryptids
			- Working on the behaviour of a centaurlike forest-spirit. Features:
				- It will stay in the trees until the player is a certain distance from the nearest light source
				- Once the player is in darkness, it will slowly close the distance from about 100% of {passive_distance} to 80% of {passive_distance}
				- Once they are close to the player, it will screech and charge the player
				- If they are creeping in towards the player and they re-enter a lit area, it will stand still and stare for a little while, then retreat to the woods
				- If they are mid charge and the player reaches light, it will continue charging if it's within a ceratin range that I'll call {follow_through_distance}
					Otherwise, it will slow the charge, then follow the logic of the previous step
				- Every so often, it will roll to see if it wants to extinguish the light sources within a certain range of the player using
					{extinguish_interval}, {extinguish_range}, {extinguish_chance}
	Design tasks:
		What resources should be gatherable? The array of collectable resources should:
			- Allow for progression within each gathering category (like mining has [tin/copper]-[amethyst], fishing has [nets]-[harpoons], farming better and better crops)
			- Provide materials crafting systems. Consider multiple crafting categories (potions, clothes, fertilizer?, fencing/containers, traps/bait)
			- Provide rewarding day-time activites for the day + provide incentive to venture out at night
		What's the progression of crops?
			- T1: basic vegetables
			- T2: potion ingedients (like berries or special leaves from bushes)
			- T3: hedges that can block line-of-sight? Perinials that can be repeatedly harvested for crafting ingredients? Rare animal bait (like a berry bush that bears will try to eat)? 
		What types of utility plants are there, and what tiers do they fall into? Tiering ties into progression.
			- T1 will be easy to acquire
			- T2 might require rare droppable seeds or higher tier tools to acquire them from nature
			- T3+ will either only drop from difficult to kill/find animals or require content locked methods like higher level trapping
		
Skipped VFX (come back and implement these when gameplay is done):
	- Leaves falling from tree. This should happen when a tree is hit, but could also happen intermitantly?
	- Tree falling over. When it hits the ground, it can split into logs? This could be rethought. Either way, figure out how to make the trees fall naturally.
	- The inventory UI elements are compressed / blurry at the edges. Recreate them with 2-4x the resolution. This should resolve the problem. If it doesn't, continue with the 9-slice tech.
	- The toolbar icons don't indicate the current equipped slot when the inventory is open. This can be alright as long as the scroll wheel doesn't work when the inventory is open. Make this happen.


TODO: Tomorrow, change the left-click subscribe logic to only sub once. The registered method will look at the currnetly equipped item. If it's not null and has a use script, it will call its method.
