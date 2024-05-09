# Running the Application

The application is distributed as an installer/launcher, which checks for newer versions of the app and downloads them automatically.

Here is how to run the application:

1. Download the `ForeverOnlineLauncer.exe` from [the latest release](https://github.com/fracampit/forever-online/releases/latest).
2. Move the executable to the desired location. This location will be used to install the application.
3. Run the executable. The application will be installed a directory called ForeverOnline, next to the executable.
4. If you want to edit the settings of the application, open the `appsettings.json` file in the `ForeverOnline` directory. You can edit the following settings:
    - `MoveMouse`: Whether to move the mouse or not.
    - `Delay`: Amount of time between each key press.

That's it! The launcher will automatically check for updates and download them when available. 
If you want to uninstall the application, simply delete the directory where you placed the launcher and the `ForeverOnline` directory.