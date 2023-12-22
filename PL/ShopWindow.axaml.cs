using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml.Templates;
using BLL.Entities;
using BLL.Repositories;
using DAL.Interfaces;
using DAL.Mappers.Database;

namespace Client;

public partial class ShopWindow : Window
{
    private IGoodDataMapper _mapper;
    private GoodRepository _goodRepo;
    private ObservableCollection<Good> Goods { get; set; }
    private Shop? shopContext;
    
    public ShopWindow(IGoodDataMapper mapper, Shop? shop)
    {
        InitializeComponent();
        shopContext = shop;
        _mapper = mapper;
        _goodRepo = new GoodRepository(_mapper);
        goodNameComboBox.ItemsSource = _goodRepo.GetAll();
        Refresh();
    }

    private void Refresh()
    {
        Goods = new ObservableCollection<Good>(_goodRepo.GetGoodsFromShop(shopContext.Id));

        GoodsGrid.ItemsSource = Goods;
    }

    private void CreateGood(object? sender, RoutedEventArgs e)
    {
        var good = goodNameComboBox.SelectedItem as Good;
        _goodRepo.AddToShop(shopContext.Id, good);
        Refresh();
    }

    private void AddGoods(object? sender, RoutedEventArgs e)
    {
        var good = GoodsGrid.SelectedItem as Good;
        //Refresh();
    }

    private void FindForNRubles(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}