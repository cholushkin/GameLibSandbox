![logo](Images/repository-open-graph-gamelibsandbox.png)

# GameLibSandbox
Sandbox for testing and experimenting with my different libraries:
  * GameLib (https://github.com/cholushkin/gamelib)
  * TowerGenerator (https://github.com/cholushkin/TowerGenerator)
  * VersionHistory (https://github.com/cholushkin/VersionHistory)

Also contains experiments and examples of third parties libraries such as:
  * DoTween
  * Dear ImGUI Unity (https://github.com/cholushkin/dear-imgui-unity)
  * ProBuilder
  
## Dependencies and examples

| Name  | dependency type | URL | Descritpion |
| ----- | --------------- | --- | ----------- |
| GameLib | git submodule | https://github.com/cholushkin/gamelib.git | GameLib generic utilities/libraries/helpers 
| UConsole | git submodule | https://github.com/cholushkin/uconsole.git | Ingame console + lua |
| NaughtyAttributes | UPM | com.dbrizov.naughtyattributes | Some useful inspector attributes |
| VersionHistory | git submodule | https://github.com/cholushkin/VersionHistory.git | Version history library |
| TowerGenerator | git submodule | https://github.com/cholushkin/towergenerator.git | Tower generator library |
| UMoonSharp | git submodule | https://github.com/cholushkin/umoonsharp.git | Unity lua support |


Also contains custom unit tests for GameLib and some other libraries.
Integration of all libraries to GameLibSandbox project is also an example of how to integrate them to your game and check out how all of them can live together. 
GameLibsSandbox always contains most recent version of modules and libraries. Unity version is also most actual.


Full lists of libs used: [Libs README.md](GameLibSandboxUnity/Assets/Libs/README.md).
