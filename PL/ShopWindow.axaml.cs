using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BLL.Entities;
using BLL.Repositories;
using DAL.Interfaces;

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
        if (good != null && !String.IsNullOrEmpty(goodPriceBox.Text) && !String.IsNullOrEmpty(goodPriceBox.Text))
        {
            good.Price = Decimal.Parse(goodPriceBox.Text);
            good.Quantity = Int32.Parse(goodCountBox.Text);
            _goodRepo.AddToShop(shopContext.Id, good);
            Refresh();
        }
    }

    private void FindForNRubles(object? sender, RoutedEventArgs e)
    {
        int budget = -1;
        Int32.TryParse(budgetBox.Text, out budget);
        if (budget != 0)
        {
            Goods = new ObservableCollection<Good>(_goodRepo.CountGoodCountForBudget(shopContext.Id, budget));
            GoodsGrid.ItemsSource = Goods;
        }
    }

    private void BuyGoods(object? sender, RoutedEventArgs e)
    {
        var good = GoodsGrid.SelectedItem as Good;
        int quantity;
        Int32.TryParse(quantityBox.Text, out quantity);
        if (good != null && quantity != 0)
        {
            bool result = _goodRepo.BuyGoods(shopContext.Id, good, quantity);
            if (result == false)
            {
                var mb = new MessageBox("Нет наличия", "Указанного количества нет на складе");
                mb.ShowDialog(this);
            } 
            else
                Refresh();
        }
    }
}