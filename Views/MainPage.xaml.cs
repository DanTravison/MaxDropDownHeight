using MaxDropDownHeight.ViewModels;
using Syncfusion.Maui.Inputs;
using System.ComponentModel;

namespace MaxDropDownHeight.Views;


public partial class MainPage : ContentPage
{
    readonly MainViewModel _model;
    double _dropdownHeight;

    public MainPage()
    {
        BindingContext = _model = new();
        InitializeComponent();
        MeasureItems();
        ComboBox.PropertyChanging += OnDropdownOpening;
        ComboBox.PropertyChanged += OnComboBoxPropertyChanged;
    }

    private void OnComboBoxPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (StringComparer.Ordinal.Compare(e.PropertyName, SfComboBox.ItemsSourceProperty.PropertyName) == 0)
        {
            MeasureItems();
        }
    }

    private void OnDropdownOpening(object sender, Microsoft.Maui.Controls.PropertyChangingEventArgs e)
    {
        if (StringComparer.Ordinal.Compare(e.PropertyName, SfComboBox.IsDropDownOpenProperty.PropertyName) == 0)
        {
            if (_dropdownHeight == 0)
            {
                MeasureItems();
            }
            ComboBox.MaxDropDownHeight = _dropdownHeight;
        }
    }

    void MeasureItems()
    {
        double height = 0;
        double width = 0;
        if (_model.Items.Files.Count > 0)
        {
            FilePlaceHolder.BindingContext = _model.Items.MaxFile;
            Size size = FilePlaceHolder.Measure(double.PositiveInfinity, double.PositiveInfinity);
            width = Math.Max(width, size.Width);
            height = size.Height * _model.Items.Files.Count;
        }
        if (_model.Items.Directories.Count > 0)
        {
            DirectoryPlaceHolder.BindingContext = _model.Items.MaxDirectory;
            Size size = DirectoryPlaceHolder.Measure(double.PositiveInfinity, double.PositiveInfinity);
            width = Math.Max(width, size.Width); 
            height += _model.Items.Directories.Count * size.Width;
        }
        _dropdownHeight = height;
        // NOTE: Add 50 to account for the dropdown icon.
        ComboBox.WidthRequest = width + 50;
    }
}
