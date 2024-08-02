# TOM-Client-Unity

A Unity implementation of the client to support smart glasses and phones that receive data from the server
- This [Unity3D](https://unity.com/) client serves as the primary front-end interface for devices such as smart glasses and phones. 
- Users directly interact with this interface. 
- It's constructed using [MRTK 2.8](https://github.com/microsoft/MixedRealityToolkit-Unity/tree/releases/2.8.3) and employs web socket communication to connect with the [TOM-Server-Python](../TOM-Server-Python).


## Requirements
- For HoloLens2 development, you need to use a Windows 10+ PC. Add the required [prerequisites for HoloLens development](https://learn.microsoft.com/en-us/training/modules/learn-mrtk-tutorials/1-1-introduction#prerequisites)
- For voice dictation in HoloLens, make sure there is an active internet connection and online speech recognition service ( "Setting - > Privacy -> Speech "-> Turn on "Online speech recognition" and "Speech recognition").


## Installation
- Open the project in Unity
- Follow the platform-specific instructions below

### HoloLens2
- Configure *only* the Build Settings - see [Hololens with MRTK](https://learn.microsoft.com/en-us/training/paths/beginner-hololens-2-tutorials/) 
- Create a `tom_config.json` file inside `Videos/TOM` directory in HoloLens2
- Add the server address to the `tom_config.json` as follows
	- ```javascript
		{"host":"<IP_ADDRESS>","port":"8090"}
	  ```
	- NOTE: both the device (e.g., HoloLens2) and [server](https://github.com/NUS-SSI/TOM-Server-Python) computer should be connected via a PRIVATE network (e.g., phone hotspot)
- To auto stop the voice dictation after a timeout, modify the voice integration of MRTK for HL2 in `WindowsDictationInputProvider.cs`.
  - Search `WindowsDictationInputProvider` in the project using "All" (not "In Assets"), open the script, and modify the `DictationRecognizer_DictationComplete` method in *lines 392-408* as follows:
  - ```cs
    private void DictationRecognizer_DictationComplete(DictationCompletionCause cause){ 
        using (DictationCompletePerfMarker.Auto()){
            if(cause == DictationCompletionCause.TimeoutExceeded){
                Microphone.End(deviceName);
                dictationResult = textSoFar.ToString();
                StopRecording();
            }
            Service?.RaiseDictationComplete(inputSource, dictationResult, dictationAudioClip);
            textSoFar = null;
            dictationResult = string.Empty;
        }
    }
    ```

### Nreal
- TODO

### Simulator (Unity Editor)
- Make sure both the Unity Editor (Simulator) and [server](../TOM-Server-Python) are running on the same computer so that the server address is `127.0.0.1` (localhost)
- Create a `tom_config.json` file inside `Videos/TOM` in the computer's home directory
- Use the `127.0.0.1` as the server address in the `tom_config.json` as follows
    - ```javascript
    	{"host":"127.0.0.1","port":"8090"}
	  ```


## Application execution
- Start the [TOM-Server-Python](../TOM-Server-Python) to establish a socket connection
- Run the (Unity) client application in the respective platform (HoloLens2, Nreal, or Simulator)


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
- To create an Play Mode test script, go to (`Assets/Scripts/Tests`), click on the PlayMode folder, click the (`PlayMode`) tab in the test runner and click (`Create Test Script in Current folder`)

### Running Test Script & Obtaining Code Coverage
- Under Test Runner, click on either the EditMode or PlayMode tab, click run all or run selected
- When prompted to enter Debug Mode, click yes
- Code coverage report should be automatically generated after running tests. If not, go to the code coverage panel and click (`Generate from Last`) at the bottom right of the panel.
- Open the report in the form of an (`index.html`) file in the (`CodeCoverage`) folder in the project root folder.


## Linting (code formatting)
**Visual Studio / (Code)**
1. Open the Command Palette (`Ctrl-Shift-P`) for Visual Studio Code, or Quick Launcher (`Ctrl-q`) for Visual Studio
2. Search for and select 'Format Document' to automatically format the `.cs` file


## Development

### Guides
- Please read the [repository guide](https://docs.google.com/document/d/13nuP668jawXzb_bnPgzxtRhcEUJzmzB0YHYm-yMr15I/edit#heading=h.d636ak5kflwe) to understand how the server works
- Follow development guidelines in creating new service and components.

### Protobuf
- Ensure you have `protoc` installed by typing `protoc --version` in your terminal. If it is not installed, you may follow the instructions [here](https://github.com/protocolbuffers/protobuf#protocol-compiler-installation).
- Create your proto file in `Assets/Scripts/Protobuf`. For more information on how to structure proto data, please refer [here](https://protobuf.dev/getting-started/csharptutorial/).
- `cd` to `Assets/Scripts` and run this command in your terminal `protoc -I=Protobuf --csharp_out=Protobuf Protobuf/proto_name.proto` to generate the builder class. Note that you have to run the command again if you edit the proto file.


## References
- [Unity Learn](https://learn.unity.com/)
- [MixedRealityToolkit-Unity](https://github.com/microsoft/MixedRealityToolkit-Unity)
- [mixed-reality-toolkit-project-unity](https://learn.microsoft.com/en-us/training/modules/mixed-reality-toolkit-project-unity/)
- [HoloLens 2 fundamentals: develop mixed reality applications](https://learn.microsoft.com/en-us/training/paths/beginner-hololens-2-tutorials/)

### Eye tracking
- [Eye tracking on HoloLens 2](https://learn.microsoft.com/en-us/windows/mixed-reality/design/eye-tracking)
- [Enable eye-tracking ](https://learn.microsoft.com/en-us/training/modules/use-eye-tracking-voice-commands/)
- [Getting started with eye tracking in MRTK2](https://learn.microsoft.com/en-us/windows/mixed-reality/mrtk-unity/mrtk2/features/input/eye-tracking/eye-tracking-basic-setup?view=mrtkunity-2022-05)
- [Eyes and hands — MRTK2](https://learn.microsoft.com/en-us/windows/mixed-reality/mrtk-unity/mrtk2/features/input/eye-tracking/eye-tracking-eyes-and-hands?view=mrtkunity-2022-05)

