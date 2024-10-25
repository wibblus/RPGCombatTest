# RPGCombatTest

## Members
**Willow Forte - 100828563**

## Interactive Project Description

The project I have been developing is a mechanics prototype for an RPG combat system. The player takes control of multiple characters, and is intended to defeat enemies presented to them. The combat follows a turn-based system, where character/enemy/etc. involved are represented by general 'combat units'. Each turn, a given combat unit is able to issue one type of 'combat action' that they have access to, such as an attack, and then it plays out, and the turn order continues forward. 

---

![](https://github.com/wibblus/RPGCombatTest/blob/main/READMEAssets/scene_labeled.jpg)

1. Action list. These are UI buttons that list the actions available to the current controllable combat unit.
2. Unit stats. Basic layout of the current unit's stats; name and current / maximumm health are listed here.
3. Ally unit. A controllable combat unit; they feature a Unit component that handles their base functionality like stats, and an ActionHandler component that allows it to act in combat.
4. Enemy unit. A combat unit that also features an ActionHandler. For development restriction reasons, are currently also controllable, but use a unique EnemyUnit class that would denote an AI-controlled unit in future prototypes.
5. Dummy unit. A combat unit that only has the base component. It can be targetted by actions, but does not itself act.
6. Undo action button. Testing element that undoes the effects of the last performed action in combat, and steps back the turn order.

---

# Design Pattern Implementations

## Singleton
![](https://github.com/wibblus/RPGCombatTest/blob/main/READMEAssets/singleton_UML.jpg)

The singleton pattern is implemented in three areas in this project:

- An Input Manager, that handles the functionality of the action and undo buttons, as well as the ability to select target units for certain actions. This component works well as a singleton since it allows action logic to use the unit selection functionality at any point, without needing to obtain a reference to the script, or duplicate the functionality, for any action. It also guarantees that the Unity Button events only need to link to a single component in the scene.
- A UI Manager, which handles all of the changes to the UI elements in the combat scene. By wrapping all the UI-related code into a singleton, other objects such as the CombatManager and Units only need to send basic updates to one static location, without having references to individual UI elements scattered across classes. For instance, when the CombatManager begins the next unit's turn, it tells the UIManager to update with a single function call, and the rest of the logic is handled outside.
- A Combat Manager, which is responsible for tracking the current state of combat, and advancing it by issuing actions. Due to all the key logic operated by this class, it works well as a singleton; other objects can easily grab important information from it, such as the ActionHandler of the unit who's turn it currently is. It also allows action logic to inform the CombatManager when the action has completed, so the turn order can continue.

In general, each of these classes are set up as singletons by inheriting from a base Singleton class. This base class defines a static reference to a variable object type (`\<T\>`), and assigns that reference to itself accordingly on `Awake()`. This keeps all the singleton logic outside of individual classes that inherit it.

## Command
![](https://github.com/wibblus/RPGCombatTest/blob/main/READMEAssets/command_UML.jpg)

The command pattern is implemented by the combat action logic. All actions that a unit can perform are represented by a CombatAction object. The base class establishes that every type of action has associated logic for when it is performed, completed, and undone. They also all keep reference to the ActionHandler (attached to a Unit) who issued the action.

Every ActionHandler contains a list of CombatAction objects to denote which ones they are able to use. When an action is issued by the CombatManager, a new CombatAction object is created with the specified child type (Attack, Defend, etc.). This object is pushed to the `actionStack` for later reference, and begins its `Perform()` coroutine. The rest of the specific logic is contatined within the action's class code, as the CombatManager awaits the action to complete. Because every performed action in a given combat scenario is stored in a Stack, they can be sequentially undone by popping an element and calling its `Undo()` function. This all gives the system incredible flexibility, able to wrap very unique action logic into a modular system, and the ability to step back and replay any number of actions natively.

## Factory
![](https://github.com/wibblus/RPGCombatTest/blob/main/READMEAssets/factory_UML.jpg)

The factory pattern is implemented for the dynamic creation of combat units during runtime. Various forms of combat units exist, but all share the same base class component, Unit. Thus, they fit nicely into the factory pattern, using an interface that allows unique types of Unit to implement their own initialization code. In this case, units can be created as either an ally, dummy, or enemy (added during development), which each have a Unity Prefab associated with them, and a unique composition of components created once initialized. For example, the AllyUnit prefab has a default 20 Health and creates an ActionHandler component, while the DummyUnit prefab has a default 5 Health and exists without an ActionHandler.

The UnitFactory objects are very simple, holding just the respective prefab reference, and function to instantiate and initialize the object.

## Observer
![](https://github.com/wibblus/RPGCombatTest/blob/main/READMEAssets/observer_UML.jpg)

The observer pattern is implemented in this project using Unity's Action event system. While also implemented inherently by the InputManager for button events, they are primarly used by the base Unit class. These events are invoked in different cases of the unit's health being changed, which is listened to by its ActionHandler and the UIManager. The ActionHandler is concerned with when the unit goes from death to life (between zero and non-zero health values), setting its ability to act in combat based on those changes. The UIManager listens for health changes of any kind, to update the displayed health counters on screen.

With the Unity Action implementation, this design pattern is incredibly useful. It can help to consolidate a large variety of logic that would otherwise require many scattered variable checks in Update functions. Here, the Unit objects need not be concerned with how other objects need to react to its own changes.

