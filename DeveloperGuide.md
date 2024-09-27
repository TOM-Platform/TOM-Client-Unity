## Linting (code formatting)
**Visual Studio / (Code)**
1. Open the Command Palette (`Ctrl-Shift-P`) for Visual Studio Code, or Quick Launcher (`Ctrl-q`) for Visual Studio
2. Search for and select 'Format Document' to automatically format the `.cs` file


## Development

### Guides
- Please read the following guides to understand how the unity client works ([Private source](https://docs.google.com/document/d/13nuP668jawXzb_bnPgzxtRhcEUJzmzB0YHYm-yMr15I/edit#heading=h.d636ak5kflwe))
  - If you are not familiar with server, please be familiar with it first
  - [Unity Basics, 10 min](https://drive.google.com/file/d/1M4Vq_IMQfujuOemQ3fuzgfd5Zo94aOvx/view?usp=sharing)
  - [Client Basics, 11 min](https://drive.google.com/file/d/13NFbOevCWyhjBOa0BSbVWKU7OCkGnP2b/view?usp=sharing)
  - [Running assistance, 14 min](https://drive.google.com/file/d/1c5s3ike8seV_DbupD9M9-UU-oYuhr15o/view?usp=sharing)

### New applications
- All application logic should be inside [Assets/Scripts/Apps](Assets/Scripts/Apps)
- For new applications create a new namespace such as `TOM.Apps.<APP_NAME>`
- Create unit tests at [Assets/Tests](Assets/Tests) following [Testing](#testing)

### Protobuf
- Ensure you have `protoc` installed by typing `protoc --version` in your terminal. If it is not installed, you may follow the instructions [here](https://github.com/protocolbuffers/protobuf#protocol-compiler-installation).
- Create your proto file in `Assets/Scripts/Protobuf`. For more information on how to structure proto data, please refer [here](https://protobuf.dev/getting-started/csharptutorial/).
- `cd` to `Assets/Scripts` and run this command in your terminal `protoc -I=Protobuf --csharp_out=Protobuf Protobuf/proto_name.proto` to generate the builder class. Note that you have to run the command again if you edit the proto file.



## Testing

### Setup Unity TestRunner and Code Coverage
1. Activate Test Runner by going to (`Window -> General -> Testrunner`) [Ref](https://docs.unity3d.com/Packages/com.unity.test-framework@1.4/manual/getting-started.html)
2. Ensure these panels are open in your Unity Editor
3. Under the Code Coverage panel, check `Enable Code Coverage` under the **Settings** header

### Creating Test Scripts
- Unity has 2 kinds of test scripts.
    - **Play Mode Tests:** Run inside the editor's play mode, ideal for testing game logic and functionality that relies on running the game. Use the (`UnityTest`) attribute.
    - **Edit Mode Tests:** Run directly in the Unity Editor and not in play mode, useful for testing logic independent of the game running, like validation, data structures and editor extensions. Use the (`Test`) attribute.
- To create an Edit Mode test script, go to (`Assets/Scripts/Tests`), click on the EditMode folder, click the (`EditMode`) tab in the test runner and click (`Create Test Script in Current folder`)
- To create a Play Mode test script, go to (`Assets/Scripts/Tests`), click on the PlayMode folder, click the (`PlayMode`) tab in the test runner and click (`Create Test Script in Current folder`)

### Running Test Script & Obtaining Code Coverage
- Under Test Runner, click on either the EditMode or PlayMode tab, click run all or run selected
- When prompted to enter Debug Mode, click yes
- Code coverage report should be automatically generated after running tests. If not, go to the code coverage panel and click (`Generate from Last`) at the bottom right of the panel.
- Open the report in the form of an (`index.html`) file in the (`CodeCoverage`) folder in the project root folder.

### Creating Assembly Definition files for testing
1. Create an asmdef file in [Assets/Scripts/...](Assets/Scripts)
2. Look at the reference-related compilation errors (i.e., the not found errors)
3. One by one, drag the required references to the created asmdef file (under the section Assembly Definition References). Repeat till the errors are resolved.
4. Use those references in test files.

