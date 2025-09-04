# Asteroids

Simple asteroids clone made using Unity 6.1. 

## Architecture and components
- Game uses `VContainer` as its dependency injection framework. `GameLifetimeScope` class contains all the registrations related to the game.
- `GameContoller` functions as the main coordinating component of the game. It maintains the state of the game and handles reacting to state and game related events coming from the UI and game entities.
- UI handling is done through `GameUIController` component. Controller handles all the game UIs and switches between the UI views based on the changes to game state.
- `GameUIController` invokes game state related methods of `GameController` in response to user input such as clicking start game button.
- `LevelController` is responsible for state of the level. Component handles creation of level and spawning of required game entities based on the game events and time. Controller is also used to update the state of the level based on game events such as bullet hitting an asteroid.
- `ScoreController` handles awarding player points when they hit an asteroid.
- Game entities (`Player`, `Bullet` and `Asteroid`) are implemented as `MonoBehaviours` in order to make collision detection and moving of the objects easy. They handle their own movement either automatically or by reacting to player input.
- Game entities are instantiated through related factories.
- Game entities invoke methods of `GameController` when they detect game related events such as player firing a bullet or collision detection detecting collisions between game entities.
- Components requiring time information need to request it through the `ITimeProvider` implementation. This would make it easier to handle time in testing if test requiring time would be added.
- Components get configuration information related to the game through `GameSettings` scriptable object. This enables easy tweaking of configuration values even when the game is running.