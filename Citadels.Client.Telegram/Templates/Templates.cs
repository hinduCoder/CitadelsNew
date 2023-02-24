﻿using Citadels.Client.Telegram.Templates.Models;
using HandlebarsDotNet;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Citadels.Client.Telegram.Templates;

public static class Templates
{
    [AllowNull]
    public static HandlebarsTemplate<RegistrationContext, object> RegistrationTemplate { get; private set; }
    [AllowNull]
    public static HandlebarsTemplate<ChatMessage, object> InGameChatMessageTemplate { get; private set; }
    [AllowNull]
    public static HandlebarsTemplate<GameInvitation, object> GameInvitation { get; private set; }
    
    static Templates()
    {
        var props = typeof(Templates).GetProperties(BindingFlags.Public | BindingFlags.Static);
        foreach (var prop in props)
        {
            var name = prop.Name;
            var fileContent = File.ReadAllText($"Templates/{name}");
            var template = Handlebars.Compile(fileContent);
            prop.SetValue(null, template);
        }
    }
}
