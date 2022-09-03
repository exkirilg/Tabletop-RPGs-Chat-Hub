﻿using Server.Hubs;

namespace Server;

public class State
{
    private readonly Dictionary<string, ConnectionSettings> _connectionsSettings = new();

    public void AddConnectionSettings(string connectionId)
    {
        if (_connectionsSettings.ContainsKey(connectionId)) return;
        
        _connectionsSettings.Add(connectionId, new());
    }
    public ConnectionSettings GetConnectionSettings(string connectionId)
    {
        if (_connectionsSettings.ContainsKey(connectionId) == false)
        {
            _connectionsSettings.Add(connectionId, new());
        }

        return _connectionsSettings[connectionId];
    }
    public IEnumerable<string> GetConnections()
    {
        return _connectionsSettings.Keys;
    }
    public void RemoveConnectionSettings(string connectionId)
    {
        _connectionsSettings.Remove(connectionId);
    }
}
