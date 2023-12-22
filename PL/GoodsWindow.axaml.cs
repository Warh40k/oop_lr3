using System.Collections;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BLL.Entities;
using BLL.Repositories;
using DAL.Interfaces;
using DAL.Mappers.Database;
using DTOs;

namespace Client;

public partial class GoodsWindow : Window
{
    private IGoodDataMapper _mapper;
    private IShopGoodDataMapper _shopGoodMap;
    private GoodRepository _goodRepo;
    private ObservableCollection<Good> Goods { get; set; }
    private Shop? shopContext;
    
    public GoodsWindow(Shop? shop)
    {
        InitializeComponent();
        shopContext = shop;
        _mapper = new GoodDbDataMapper();
        _shopGoodMap = new ShopGoodDbDataMapper();
        _goodRepo = new GoodRepository(_mapper, _shopGoodMap);
        Refresh();
    }

    private void Refresh()
    {
        if (shopContext != null)
        {
            Goods = new ObservableCollection<Good>(_goodRepo.GetGoodsFromShop(shopContext.Id));
        }
        else
        {
            Goods = new ObservableCollection<Good>(_goodRepo.GetAll());
        }

        GoodsGrid.ItemsSource = Goods;
    }

    private void CreateGood(object? sender, RoutedEventArgs e)
    {
        var good = new Good
        {
            Name = goodNameBox.Text
        };
        _goodRepo.Save(good);
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