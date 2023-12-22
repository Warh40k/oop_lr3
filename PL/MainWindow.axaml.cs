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
    private IDataMapper<ShopDto> _mapper;
    public ObservableCollection<Shop> Shops { get; set; }
    
    public MainWindow()
    {
        InitializeComponent();
        _mapper = new ShopDbDataMapper();
        _shopRepo = new ShopRepository(_mapper);
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
        var shopWindow = new GoodsWindow(shop);
        shopWindow.ShowDialog(this);
    }
}