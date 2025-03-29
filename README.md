![](https://github.com/GlitchyPSIX/NGIO.NET/blob/main/.github/banner.png?raw=true)

# Newgrounds.io .NET

A C# library intended to allow usage of the [Newgrounds.io API](https://newgrounds.io) in .NET environments. The library itself is built against .NET Standard 2.0, which allows it to be used in a lot of places given the target platform has a Communicator implementation that can handle its asynchronous HTTP calls (e.g. Task in Task Communicator, Coroutine in Unity) or even its own JSON (de)serialization (by default, Newtonsoft.Json is the JSON library for (de)serialization, required dependency.)


## Support status
| Runtime | Status |
|-----|-----|
| .NET Desktop (MAUI, WinForms, MonoGame...) | ✅ (Task Communicator, included) |
| Headless (serverside) | ✅ (Task Communicator, included, no referral links shall open) |
| Blazor / [KNI](https://github.com/kniEngine/kni) WebGL | ⚠ Untested, Task Communicator likely working |
| .NET Mobile (MAUI Android, iOS, MonoGame(?) | ⚠ Untested, Task Communicator likely working |
| Unity Desktop | ✅[^1] 2021.3 minimum (Task Communicator, included)<br>[✅ 2021.3 minimum (Unity Coroutine Communicator)](https://github.com/GlitchyPSIX/NGIO.NET-Unity) |
| Unity Mobile | ⚠[^1] Task Communicator likely working, but untested.<br>[✅ 2021.3 minimum (Unity Coroutine Communicator)](https://github.com/GlitchyPSIX/NGIO.NET-Unity)|
| Unity WebGL | [✅ 2021.3 minimum (Unity Coroutine Communicator)](https://github.com/GlitchyPSIX/NGIO.NET-Unity) |
| Godot 3.x | ❌ Requires own Communicator, planned |
| Godot 4.x | ❌ Likely unviable; [no .NET WebGL export](https://github.com/godotengine/godot/issues/70796) |

[^1]: The core does not and will not have have an UPM package. Manual installation is required.

## Usage
I haven't written the wiki yet please do not hurt me I will get to it eventually...

However, basic usage is relatively simple:
1. Instantiate your Communicator and pass to it your Newgrounds App ID and Encryption Key, optionally other settings as outlined in the constructor.
2. Use ``Init()`` to initialize the Communicator.
   - After that you should use ``StartHeartbeat()`` to begin a forever-task to keep pinging Newgrounds so your session doesn't expire and some session checks are done automatically.
3. Use ``LogIn()`` to ask Newgrounds for a new session and open the Newgrounds Passport URL in the browser.
4. When the Communicator signals Ready, you may use any of the functions in the Communicator to interface with Newgrounds, such as GetMedal, UnlockMedal, SetSaveSlot, and so on.
   - If you're feeling fancy, you can also skip the friendly methods and [execute components yourself](https://www.newgrounds.io/help/components/) by instantiating them and sending them using ``SendRequest``.
     - The built in Task Communicator has its own Task-based Async version of SendRequest you can also use. 

## Contributing
Contributions are welcome! I would personally wait until the Unity Communicator is done before doing anything, though. It should be relatively easy to create a Communicator for your preferred platform, following the guideline of the Task Communicator.

Every other Communicator must be a separate project (and by extension, separate repository) to avoid cluttering the core, and have users only install the core and their preferred Communicator for their project. Separate projects with their Communicators will be added to the Support table above.

You can also contribute to this core project, but keep in mind to not haphazardly change things such as the friendly methods in the abstract Communicator so as to not break any other implementations that rely on them.

The project enforces the C# 7.3 version, as per .NET Standard 2.0's C# version.

## License
MIT. You're free to fork and roll your own versions of this, but just credit me.

## Helpful links

- [Newgrounds.io documentation](https://www.newgrounds.io/help/)
