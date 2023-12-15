using System;
using Avalonia;
using DAL.Entities.Good;
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
        var map = new GoodDbDataMapper();
        var good = new GoodDto();
        good.Name = "testovavava";
        good.Id = map.Save(good);
        good.Name = "testovichcsd";
        map.Update(good);
        var goods = map.GetAll();
        var goodgod = map.GetById(3);
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