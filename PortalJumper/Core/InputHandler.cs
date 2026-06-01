using System;
using System.Collections.Generic;
using PortalJumper.Core.Interfaces;

namespace PortalJumper.Core;

public class InputHandler
{
    private readonly Dictionary<ConsoleKey, ICommand> _keyBindings = new();

    public void Bind(ConsoleKey key, ICommand command)
    {
        _keyBindings[key] = command;
    }

    public void HandleInput(ConsoleKey key)
    {
        if (_keyBindings.TryGetValue(key, out var command))
        {
            command.Execute();
        }
    }
}