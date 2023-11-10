using System.Collections;
using System.ComponentModel;

namespace MaxDropDownHeight.Model;

internal class FileSystemItemCollection : IReadOnlyList<FileSystemItem>, INotifyPropertyChanged
{
    readonly List<FileSystemItem> _items;
    readonly List<DirectoryItem> _directories = new();
    readonly List<FileItem> _files = new();
    FileSystemItem _selectedItem;

    public FileSystemItemCollection(IEnumerable<FileSystemItem> items)
    {
        _items = new(items);
        DirectoryItem maxDir = null;
        FileItem maxFile = null;

        // Determine the FileItem with the longest path and the DirectoryItem with 
        // the longest name.
        foreach (FileSystemItem item in _items)
        {
            if (item is DirectoryItem dir)
            {
                _directories.Add(dir);
                if (maxDir == null || dir.Info.Name.Length > maxDir.Info.Name.Length)
                {
                    maxDir = dir;
                }
            }
            else if (item is FileItem file)
            {
                _files.Add(file);
                if (maxFile == null || file.Info.FullName.Length > maxFile.Info.FullName.Length)
                {
                    maxFile = file;
                }
            }
        }
        MaxDirectory = maxDir;
        MaxFile = maxFile;
        _items = new(_items);
        _selectedItem = _items[0];
    }

    public FileSystemItem this[int index]
    {
        get => _items[index];
    }

    /// <summary>
    /// Gets the directories in the collection.
    /// </summary>
    /// <value>
    /// A <see cref="IReadOnlyCollection{DirectoryItem}"/> returning zero or more elements.
    /// </value>
    public IReadOnlyCollection<DirectoryItem> Directories
    {
        get => _directories;
    }

    /// <summary>
    /// Gets the files in the collection.
    /// </summary>
    /// <value>
    /// A <see cref="IReadOnlyCollection{FileItem}"/> returning zero or more elements.
    /// </value>
    public IReadOnlyCollection<FileItem> Files
    {
        get => _files;
    }

    /// <summary>
    /// Gets the <see cref="FileItem"/> with the longest <see cref="FileSystemInfo.FullName"/>.
    /// </summary>
    /// <value>
    /// A <see cref="FileItem"/> instance; otherwise a null reference if no files are in the collection.
    /// </value>
    public FileItem MaxFile
    {
        get;
    }

    /// <summary>
    /// Gets the <see cref="DirectoryItem"/> with the longest <see cref="FileSystemInfo.FullName"/>.
    /// </summary>
    /// <value>
    /// A <see cref="DirectoryItem"/> instance; otherwise a null reference if no files are in the collection.
    /// </value>
    public DirectoryItem MaxDirectory
    {
        get;
    }

    /// <summary>
    /// Gets the number of elements in the collection.
    /// </summary>
    /// <value>
    /// A zero-based integer.
    /// </value>
    public int Count
    {
        get => _items.Count;
    }

    /// <summary>
    /// Gets or set the selected item.
    /// </summary>
    /// <value>
    /// The selected <see cref="FileSystemItem"/>; otherwise, a null reference if no items is selected.
    /// </value>
    public FileSystemItem SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (!ReferenceEquals(_selectedItem, value))
            {
                _selectedItem = value;
                PropertyChanged?.Invoke(this, SelectedItemChangedEventArgs);
            }
        }
    }

    #region IEnumerable<FileSystemItem>

    public IEnumerator<FileSystemItem> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    #endregion IEnumerable<FileSystemItem>

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    public static readonly PropertyChangedEventArgs SelectedItemChangedEventArgs = new(nameof(SelectedItem));

    #endregion INotifyPropertyChanged
}
