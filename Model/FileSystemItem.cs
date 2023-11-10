using MaxDropDownHeight.Resources;

namespace MaxDropDownHeight.Model;

internal class FileSystemItem
{
    readonly FileSystemInfo _info;

    protected FileSystemItem(FileSystemInfo info)
    {
        _info = info;
        IsDirectory = info is DirectoryInfo;
        IconGlyph = IsDirectory ? FluentUI.Folder : FluentUI.Document;
    }

    public bool IsDirectory
    {
        get;
    }

    public FileSystemInfo Info
    {
        get => _info;
    }

    /// <summary>
    /// Gets the <see cref="FluentUI"/> icon glyph.
    /// </summary>
    public string IconGlyph
    {
        get;
    }

    public override string ToString()
    {
        return _info.FullName;
    }
}
