using Syncfusion.Maui.Inputs;

namespace MaxDropDownHeight.Views;

public sealed class Dropdown : ContentView
{
    readonly DropdownLayout _layout;
    readonly SfComboBox _comboBox;
    readonly Button _button;
    double _dropdownHeight;
    readonly DataTemplate _itemTemplate;

    public Dropdown()
    {
        _comboBox = new SfComboBox()
        {
            Margin = new(),
            TextColor = Colors.Transparent,
            Stroke = Colors.Transparent,
            BackgroundColor = Colors.Transparent,
            DropDownIconColor = Colors.Transparent,
            IsDropdownButtonVisible = false,
            IsEditable = false,
            CursorPosition = 0,
            ZIndex = -1
        };
        _button = new Button();

        _layout = new(_comboBox, _button);
        _comboBox.PropertyChanging += OnComboBoxPropertyChanging;
        _comboBox.PropertyChanged += OnComboBoxPropertyChanged;
    }

    private void OnComboBoxPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (StringComparer.Ordinal.Compare(e.PropertyName, SfComboBox.ItemsSourceProperty.PropertyName) == 0)
        {
            MeasureItems();
        }
    }

    private void OnComboBoxPropertyChanging(object sender, PropertyChangingEventArgs e)
    {
        if (StringComparer.Ordinal.Compare(e.PropertyName, SfComboBox.IsDropDownOpenProperty.PropertyName) == 0)
        {
            if (_dropdownHeight == 0)
            {
                MeasureItems();
            }
            _comboBox.MaxDropDownHeight = _dropdownHeight;
        }
    }

    #region Bindable Properties

    #region ItemTemplate

    /// <summary>
    /// Gets or sets the <see cref="DataTemplateSelector"/> to use to create views for each item
    /// in the <see cref="ItemsSource"/> property.
    /// </summary>
    public DataTemplate ItemTemplate
    {
        get => GetValue(ItemTemplateProperty) as DataTemplate;
        set => SetValue(ItemTemplateProperty, value);
    }

    /// <summary>
    /// Provides the <see cref="BindableProperty"/> backing store for <see cref="ItemTemplate"/>.
    /// </summary>
    public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create
    (
        nameof(ItemTemplate),
        typeof(DataTemplate),
        typeof(Dropdown),
        null,

        propertyChanged: (bindableObject, oldValue, newValue) =>
        {
            if (bindableObject is Dropdown dropdown)
            {
                dropdown.OnItemTemplateChanged();
            }
        }
    );

    void OnItemTemplateChanged()
    {
        _itemTemplate = ItemTemplate;
        _comboBox.ItemTemplate = ItemTemplate;
        InvalidateMeasure();
    }

    #endregion ItemTemplate

    #region ItemsSource

    /// <summary>
    /// Gets or sets the <see cref="IEnumerable{IToolbarItem}"/> to use to populate the control.
    /// </summary>
    public object ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    /// <summary>
    /// Provides the <see cref="BindableProperty"/> backing store for <see cref="ToolbarItemTemplateSelector"/>.
    /// </summary>
    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create
    (
        nameof(ItemsSource),
        typeof(object),
        typeof(Dropdown),
        null,
        propertyChanged: (bindableObject, oldValue, newValue) =>
        {
            if (bindableObject is Dropdown dropdown)
            {
                dropdown.OnItemSourceChanged();
            }
        }
    );

    void OnItemSourceChanged()
    {
        _comboBox.ItemsSource = ItemsSource;
        InvalidateMeasure();
    }

    #endregion ItemsSource

    #endregion Bindable Properties
}
