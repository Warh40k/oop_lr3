using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BLL.Entities;
using BLL.Repositories;
using DAL.Interfaces;
using DAL.Mappers.Database;
using DTOs;

namespace Client;

public partial class MainWindow : Window
{
    private ShopRepository _shopRepo;
    private IShopDataMapper _shopMapper;
    private IGoodDataMapper _goodMapper;
    public ObservableCollection<Shop> Shops { get; set; }
    
    public MainWindow()
    {
        InitializeComponent();
        _shopMapper = new ShopDbDataMapper();
        _goodMapper = new GoodDbDataMapper();
        _shopRepo = new ShopRepository(_shopMapper);
        Shops = new ObservableCollection<Shop>(_shopRepo.GetAll());
        ShopsGrid.ItemsSource = Shops;
    }
    
    private void Refresh()
    {
        Shops = new ObservableCollection<Shop>(_shopRepo.GetAll());
        ShopsGrid.ItemsSource = Shops;
    }

    private void OpenShop(object? sender, RoutedEventArgs e)
    {
        var shop = ShopsGrid.SelectedItem as Shop; 
        var shopWindow = new ShopWindow(_goodMapper, shop);
        shopWindow.ShowDialog(this);
    }

    private void OpenGoods(object? sender, RoutedEventArgs e)
    {
        var goodWindow = new GoodsWindow(_goodMapper);
        goodWindow.ShowDialog(this);
    }
}