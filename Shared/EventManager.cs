using System;
using System.Collections.Generic;
using System.Linq;

public static class EventManager
{
    private static Dictionary<string, List<Action<string[]>>> events = new Dictionary<string, List<Action<string[]>>>();

    public static void RegisterEvent(string name, Action<string[]> function)
    {
        if (!events.ContainsKey(name))
        {
            events[name] = new List<Action<string[]>>();
        }
        events[name].Add(function);
    }

    #region Event Handling
    public static void TriggerRaw(string message)
    {
        string[] content = Deserialize(message);
        if (content.Length == 0)
        {
            Console.WriteLine("Invalid event call");
            return;
        }

        string eventName = content[0];
        if (events.ContainsKey(eventName))
        {
            string[] args = content.Skip(1).ToArray();
            foreach (var function in events[eventName])
            {
                function.Invoke(args);
            }
        }
    }

    public static void Trigger(string name, string[] args)
    {
        TriggerRaw(Serialize(name, args));
    }
    #endregion

    #region Serializer
    public static string Serialize(string eventName, params object[] parameters)
    {
        if (parameters != null && parameters.Length > 0)
        {
            string paramStr = string.Join(",", parameters);
            return $"{eventName}|{paramStr}";
        }
        return $"{eventName}|";
    }

    public static string[] Deserialize(string message)
    {
        var parts = message.Split('|');
        if (parts.Length > 0)
        {
            var command = parts[0];
            var parameters = parts.Length > 1 ? parts[1].Split(',') : new string[] { };

            var result = new string[1 + parameters.Length];
            result[0] = command;
            for (int i = 0; i < parameters.Length; i++)
            {
                result[i + 1] = parameters[i];
            }
            return result;
        }

        return new string[] { };
    }
    #endregion
}
