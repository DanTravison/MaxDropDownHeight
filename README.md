# MaxDropDownHeight
Ticket 520287: SfComboBox.MaxDropDownHeight issue.

## Context: 
I'm using SfComboBox to provide a dropdown
'attached' to a button. 
- The ComboBox is positioned at -1 ZIndex
- IsEditable/IsDropdownButtonVisible are set to false.
- All Color properties set to transparent.  
- The button's action toggles SfComboBox.IsDropdownOpen

## MaxDropdownHeight calculations
The goal is to ensure the height of the dropdown is set to
the height of the contents. 

To accomplish this I use the following logic:
- The items in the content use a set of well-known item templates
- SfComboBox.ItemTemplate is set to DataTemplateSelector that returns the appropriate DataTemplate
- The 'container' defines a hidden content view for each DataTemplate
  - These are referred to as 'placeholders' and are to measure the content height.

## MaxDropdownHeight Calculations
I use a custom type for the ItemsSource that pre-calculates the 
number of each 'item type' in the collection as well as the specific
item type that has the 'largest' size. The BindingContext of the 
placeholder items is updated.

## Setting MaxDropdownHeight
- Subscribe to SfComboBox.PropertyChanging and filter for IsDropdownOpen)
- Subscribe to SfComboBox.PropertyChanged and filter for ItemsSource
- On ItemsSource changed: 
   - Set a cached MaxDropDownHeight value to zero to force a recalculation.
- On IsDropDownOpen is changing:
  - Calculate MaxDropdownHeight if zero
  - Set SComboBox.MaxDropDownHeight to the cached value. 

## Results
At this point, everything works as expected.  The dropdown always reflects the calculated size.

# Repro Scenario
One usage of the ComboBox is to present an MRU of recently opened files.
In my application, this is where the repro occurs.

## Implement SelectedItem changed logic:
- Subscribe to SfComboBox.SelectedItem changed
- Within SelectedItem changed:
  - Create a new dropdown list with the selected item to the start of the list.
  - Update SfComboBox.ItemsSource to the new list.
  - At this point, MaxDropdownHeight continues to behave as expected.

Since I could not reproduce the issue in the sample application,
I updated my application's dropdown logic to mirror this implementation
and expected the to see the application behave correctly but the 
issue still occurs.

After comparing the two implementations in detail, I observed only
one difference. The sample application uses the default font size
in the various templates while the actual application
explicitly sets the font sizes.

I updated the sample application to use an explicit font size 
of 20 in the various templates and now the issue occurs, the 
MaxDropDownHeight is set correctly. Tracing before and after
the setting MaxDropDownHeight shows the expected value;
however, the size of the actual popup is too small.

# Conclusion:
It 'appears' the issue likely occurs when using a non-default or
perhaps 'larger' font size is used. There may be simpler scenarios
where this occurs but I focused on my use-case.

# Using the sample application
The sample application populates the dropdown with a set of directories
and files found at the root of c:\. If you are not running on 
Windows or do not have a 'C' drive, change the const string DefaultPath
in MainViewModel.cs.

* NOTE: The sample does not have a proper layout class so the initial
dropdown button size is incorrect.  Click the button twice to fix.

There are two actions available: 
* Select the 'Refresh Dropdown Contents' button to simply
refresh the dropdown contents.

* Select the button above the 'refresh' button to display the
dropdown and select one of the items.  This invokes the 'SelectedItem'
logic.

I first assumed that updating the ItemsSource within 
SelectedItem changed caused the problem; however, the 
problem occurs in both cases, simply refreshing and refreshing
within SelectedItem changed. 

If you remove the following line from MainPage.xaml, the 
repro no longer occurs:

```
<Setter Property="FontSize" Value="20"/>
```

# Images
The following shows the dropdown before refresh:

![Before Refresh](before.png)

And after refresh:

![After Refresh](after.png)




