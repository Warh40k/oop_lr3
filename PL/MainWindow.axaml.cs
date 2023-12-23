using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BLL.Entities;
using BLL.Repositories;
using DAL;
using DAL.Interfaces;
using DAL.Mappers.Csv;
using DAL.Mappers.Database;

namespace Client;

public partial class MainWindow : Window
{
    private ShopRepository _shopRepo;
    private GoodRepository _goodRepo;
    private IShopDataMapper _shopMapper;
    private IGoodDataMapper _goodMapper;
    public ObservableCollection<Shop> Shops { get; set; }
    
    public MainWindow()
    {
        InitializeComponent();
        _shopMapper = new ShopDbDataMapper();
        _goodMapper = new GoodJsonDataMapper();
        JsonData.Load();
        _shopRepo = new ShopRepository(_shopMapper);
        _goodRepo = new GoodRepository(_goodMapper);
        Shops = new ObservableCollection<Shop>(_shopRepo.GetAll());
        ShopsGrid.ItemsSource = Shops;
        GoodsComboBox.ItemsSource = _goodRepo.GetAll();
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

    private void CreateShop(object? sender, RoutedEventArgs e)
    {
        if (ShopNameBox.Text.Trim() != "" && ShopAddressBox.Text.Trim() != "")
        {
            var shop = new Shop()
            {
                Name = ShopNameBox.Text,
                Address = ShopAddressBox.Text
            };
            _shopRepo.Save(shop);
            Refresh();
        }
    }

    private void FindCheapestShop(object? sender, RoutedEventArgs e)
    {
        var good = GoodsComboBox.SelectedItem as Good;
        var shopId = _goodRepo.FindCheapestShop(good);
        foreach (var shop in Shops)
        {
            if (shop.Id == shopId)
            {
                var mb = new MessageBox("Самый дешевый товар", $"{shop.Name}");
                mb.ShowDialog(this);
            }
        }
    }
}