using Microsoft.Maui.Layouts;

namespace MaxDropDownHeight.Views;

internal class DropdownLayoutManager : LayoutManager
{
    DropdownLayout _layout;

    public DropdownLayoutManager(DropdownLayout layout)
        : base(layout)
    {
        _layout = layout;
    }

    public override Size Measure(double widthConstraint, double heightConstraint)
    {
    }

    public override Size ArrangeChildren(Rect bounds)
    {
    }
}
