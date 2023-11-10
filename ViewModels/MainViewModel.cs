using MaxDropDownHeight.Model;
using System.ComponentModel;
using System.Windows.Input;

namespace MaxDropDownHeight.ViewModels;

internal class MainViewModel : INotifyPropertyChanged
{
    const string DefaultPath = @"c:\";
    const int ItemLimit = 5;
    string _path = DefaultPath;
    readonly List<FileItem> _files = new();
    readonly List<DirectoryItem> _directories = new();

    FileSystemItemCollection _items;
    bool _isDropdownOpen;

    public MainViewModel()
    {
        RefreshCommand = new Command(OnRefresh);
        DropdownCommand = new Command(OnDropdown);
        Populate();
        Refresh();
    }

    public string Path
    {
        get => _path;
        set
        {
            if (StringComparer.Ordinal.Compare(value, _path) != 0)
            {
                _path = value ?? DefaultPath;
                PropertyChanged?.Invoke(this, PathChangedEventArgs);
            }
        }
    }

    public DirectoryItem DirectoryPlaceHolder
    {
        get;
    }

    public FileItem FilePlaceHolder
    {
        get;
    }

    /// <summary>
    /// Gets the items to display.
    /// </summary>
    public FileSystemItemCollection Items
    {
        get => _items;
        private set
        {
            if (!ReferenceEquals(_items, value))
            {
                if (_items != null)
                {
                    _items.PropertyChanged -= OnItemsPropertyChanged;
                }
                _items = value;
                if (_items != null)
                {
                    _items.PropertyChanged += OnItemsPropertyChanged;
                }
                PropertyChanged?.Invoke(this, ItemsChangedEventArgs);
            }
        }
    }

    /// <summary>
    /// Gets or sets the value indicating if the dropdown is open.
    /// </summary>
    public bool IsDropdownOpen
    {
        get => _isDropdownOpen;
        set
        {
            if (value != _isDropdownOpen)
            {
                _isDropdownOpen = value;
                PropertyChanged?.Invoke(this, IsDropdownOpenChangedEventArgs);
            }
        }
    }

    /// <summary>
    /// Gets the <see cref="ICommand"/> to use to refresh <see cref="Items"/>.
    /// </summary>
    public ICommand RefreshCommand
    {
        get;
    }

    public ICommand DropdownCommand
    {
        get;
    }

    void OnRefresh(object command)
    {
        Refresh();
    }

    void OnDropdown(object command)
    {
        IsDropdownOpen = true;
    }

    void Populate()
    {
        foreach (string path in Directory.GetDirectories(Path))
        {
            DirectoryInfo info = new(path);
            if ((info.Attributes & FileAttributes.Hidden) == 0)
            {
                _directories.Add(new DirectoryItem(info));
                if (_directories.Count == ItemLimit)
                {
                    break;
                }
            }
        }

        foreach (string path in Directory.GetFiles(Path))
        {
            FileInfo info = new(path);
            if ((info.Attributes & FileAttributes.Hidden) == 0)
            {
                _files.Add(new FileItem(info));
            }
            if (_files.Count == ItemLimit * 2)
            {
                break;
            }
        }
    }

    public void Refresh()
    {
        List<FileSystemItem> items = new();
        
        items.AddRange(_directories);
        items.AddRange(_files);
        Items = new(items);
    }

    private void OnItemsPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (object.ReferenceEquals(e, FileSystemItemCollection.SelectedItemChangedEventArgs))
        {
            if (Items.SelectedItem != null)
            {
                FileSystemItem item = Items.SelectedItem;
                List<FileSystemItem> items = new(_items);
                // Simulate MRU
                items.Remove(item);
                items.Insert(0, item);
                Items = new(items);
            }
        }
    }

    #region INotifyPropertyChanged

    /// <summary>
    /// Occurs when a property changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    public static readonly PropertyChangedEventArgs PathChangedEventArgs = new(nameof(Path));
    public static readonly PropertyChangedEventArgs ItemsChangedEventArgs = new(nameof(Items));
    public static readonly PropertyChangedEventArgs IsDropdownOpenChangedEventArgs = new(nameof(IsDropdownOpen));

    #endregion INotifyPropertyChanged
}
