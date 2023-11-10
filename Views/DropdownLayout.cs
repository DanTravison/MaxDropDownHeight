using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Inputs;

namespace MaxDropDownHeight.Views;

internal class DropdownLayout : Layout
{
    public DropdownLayout(SfComboBox comboBox, Button button)
    {
        Children.Add(comboBox);
        Children.Add(button);
        ComboBox = comboBox;
        Button = button;
    }


    internal SfComboBox ComboBox
    {
        get;
    }

    internal Button Button
    {
        get;
    }

    protected override ILayoutManager CreateLayoutManager()
    {
        return new DropdownLayoutManager(this);
    }
}
