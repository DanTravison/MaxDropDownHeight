namespace MaxDropDownHeight.Model;

internal class FileSystemTemplateSelector : DataTemplateSelector
{
    public DataTemplate File
    {
        get;
        set;
    }

    public DataTemplate Directory
    {
        get;
        set;
    }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        if (item is DirectoryItem)
        {
            return Directory;
        }
        if (item is FileItem)
        {
            return File;
        }
        return null;
    }
}
