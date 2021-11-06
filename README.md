Rage Component .NET
============================
[![Build Status](https://ci.appveyor.com/api/projects/status/github/ranstar74/ragecomponent?branch=master&svg=true)](https://ci.appveyor.com/project/ranstar74/ragecomponent)

This is an implementation of the Component pattern for GTA V.

## Requirements

* [C++ ScriptHook by Alexander Blade](http://www.dev-c.com/gtav/scripthookv/)
* [.NET Framework ≥ 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48)
* [ScriptHookVDotNet ≥ 3.3.2](https://github.com/crosire/scripthookvdotnet)

## Usage Example
```cs
public class Bar
{
  public string Name => "Im Foobar!";
  
  public Vector3 Position { get; set; }
  
  public Vehicle Vehicle { get; set; }

  // Components of the Bar
  public BarComponents Components { get; set; }
}
```
```cs
public class FooComponent : Component<Bar>
{
  public override void Start()
  {
    // Will be called after calling ComponentHandler.Lock();
    
    GTA.UI.Screen.ShowSubtitle($"{Parent.Name} Started!");
  }

  public override void OnTick()
  {
    // Will be called every tick
  
    Parent.Position += Vector3.WorldUp() * Game.LastFrameTime;
  }
  
  public override void Destroy()
  {
    // Will be called after calling ComponentHandler.Dispose();
    
    Parent.Vehicle.Delete();
  }
}
```
```cs
public class BarComponents : ComponentsHandler<Bar>
{
  public FooComponent FooComponent;
  
  // Initialize all components and lock handler
  public BarComponents()
  {
    Create(ref FooComponent);
    
    // Don't forget to lock!
    // Without it Start() and OnTick() won't be called
    Lock();
  }
}
```
