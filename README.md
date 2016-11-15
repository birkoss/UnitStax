TODO
====

* Combat Resolution
 * Keep a list of every units (in the order they were placed)
 * Resolve the combat from the last unit placed to the first (if still alive)

* DragHandler
 * Use a boolean to hold if it's draggable or not
  * Not draggable by default (But the card manager will change that)
 * Remove all Pointer Down (move to another class)

* Create a clickHandler
 * Raise an event when clicked (and not dragged)

* Refactor the Turn Controller
 * Begin()
 * StartAction()
 * Waiting for Input
  * Cards.Show() (And waiting for player input)
    * OnTurnEnded(unit)
  * #AI()
   * OnTurnEnded(unit)
 * OnTurnEnded(unit)
 * Cards.Hide()
 * #ActivateMap
 * Fight
 * #ResolveMap
 * End()
 
* Uniformized all animations
 * Cards

* Change spritesheets
 * One spritesheet per Unit (Idle animation)
 
* Show a health bar over the unit
 * Change the grid size to 5x5 and make the tile bigger than the unit (allowing a health bar)
 
* Leveling Unit
 * Need X unit killed to levelup (each enemy level give 1 xp)
 * Each level will affect the stats at the same rate
  * Odd = HP, Even = Atk
 * Stats of Unit should be static (since it's the same)
  * MaxHP, Atk, level, toLevel
 * Need to save:
  * Unit Name
  * Current Level
  * To Level
  
* Generate decks (for player and enemy)

* Better AI
 * Using a Gambit System similar to FF12, syntax: Target: Action ? Treshold, ex:
  * Ally: heal < 50%
  * Foe: Attack < 10%

Levelup formula (from Disgaea)
==============================

round( 0.04 * (level ^ 3) + 0.8 * (level ^ 2) + 2 * level)

IDEAS / NTH
===========

* When an unit with a drop abilities (damage every neighboors) is dragged, show the tiles it will affect
* When an unit is dragged, it will show with the enemy health bar which will be the result of the fight (if the enemy will be killed or hurt...)
