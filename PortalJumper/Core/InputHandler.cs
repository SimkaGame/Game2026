namespace PortalJumper.Core;

using System;
using System.Collections.Generic;
using PortalJumper.Core.Interfaces;
using PortalJumper.Core.Commands;
using PortalJumper.Entities;

public class InputHandler
{
    private readonly Dictionary<ConsoleKey, ICommand> _keyBindings = new();

    public void Bind(ConsoleKey key, ICommand command)
    {
        _keyBindings[key] = command;
    }

    public void HandleInput(ConsoleKey key)
    {
        if (_keyBindings.ContainsKey(key))
        {
            _keyBindings[key].Execute();
        }
    }
}