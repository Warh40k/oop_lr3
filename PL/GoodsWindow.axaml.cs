using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BLL.Entities;
using BLL.Repositories;
using DAL.Interfaces;

namespace Client;

public partial class GoodsWindow : Window
{
    private GoodRepository _goodRepo;
    private IGoodDataMapper _mapper;
    public ObservableCollection<Good> Goods;
    public GoodsWindow(IGoodDataMapper mapper)
    {
        InitializeComponent();
        _mapper = mapper;
        _goodRepo = new GoodRepository(_mapper);
        Refresh();
    }
    
    private void Refresh()
    {
        Goods = new ObservableCollection<Good>(_goodRepo.GetAll());
        GoodsGrid.ItemsSource = Goods;
    }

    private void CreateGood(object? sender, RoutedEventArgs e)
    {
        var good = new Good()
        {
            Name = goodNameBox.Text
        };
        _goodRepo.Save(good);
        Refresh();
    }
}