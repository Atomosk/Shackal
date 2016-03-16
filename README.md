Shackal

## What is it?
It's a game. You pick a folder with images, press the button. It shows extremly pixelated image and slowly make it less pixelated. First right guess about what's on picture wins.

## What it realy is?
Actualy it is 'lets try shaders in WPF'.
Lessons learned:
1. There is no nice way to compile shader in csproj. Here is the ugly choices:
  - Install some runtime compiler like SharpDX from nuget. Didn't work in my case, was complaining about missing dx dll, forgot the name.
  - Add pre build action. Surprisingly if something works in 'Developer Command Prompt for VS2015' it doesn't mean same thing will work in pre build action. Pre build action knows nothing about fxc.exe location or %WindowsSdkDir% value.
  - Create C++ project and make WPF project depend on it. C++ project supports hlsl compilation.
2. Wpf doesn't support pixel effects > 3.0.
3. There is a lot of obsolete stuff in the internet. Effect shaders, *color* semantics, *.fx files all deprecated. Funny fxc.exe doesn't compile *.fx files.
