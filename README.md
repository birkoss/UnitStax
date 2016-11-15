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

IDEAS
=====

* When a unit with a drop abilities (damage every neighboors) is dragged, show the tiles it will affect
