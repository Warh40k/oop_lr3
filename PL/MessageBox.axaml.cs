using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client;

public partial class MessageBox : Window
{
    public MessageBox(string title, string text)
    {
        InitializeComponent();
        this.Title = title;
        this.messageBox.Content = text;
    }
}