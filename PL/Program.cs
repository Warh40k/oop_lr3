using System;
using Avalonia;
using BLL.Entities;
using BLL.Repositories;
using DAL.Mappers.Database;
using DTOs;

namespace Client;

internal class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        var repa = new GoodRepository(new GoodDbDataMapper());
        var good = new Good
        {
            Name = "abba"
        };
        repa.Save(good);
        var goods = repa.GetAll();
        //BuildAvaloniaApp()
        //.StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
    }
}