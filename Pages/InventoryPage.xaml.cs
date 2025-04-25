using UndergradProject.Business_Logic_Layer;
using UndergradProject.Data_Access_Layer;
using UndergradProject.Data_Access_Layer.Models;

namespace UndergradProject.Pages;

public partial class InventoryPage : ContentPage
{
    public InventoryPage()
    {
        InitializeComponent();

        LoadItems();
    }

    private async void LoadItems()
    {
        ItemMangement itemMangement = new ItemMangement();
        try
        {
            var items = await itemMangement.getAllItemsFromDatabase();

            // Clear out any existing rows/children
            InventoryGrid.Children.Clear();
            InventoryGrid.RowDefinitions.Clear();
            InventoryGrid.ColumnDefinitions.Clear();

            // Define the columns for the Item grid
            InventoryGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) }); // Name
            InventoryGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) }); // Category
            InventoryGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Quantity
            InventoryGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Unit Price

            int row = 0;

            // Add row for headers
            InventoryGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Add headers for each column

            var headerName = CreateLabel("Name", true);
            InventoryGrid.Children.Add(headerName);
            Grid.SetRow(headerName, row);
            Grid.SetColumn(headerName, 0);

            var headerCategory = CreateLabel("Category", true);
            InventoryGrid.Children.Add(headerCategory);
            Grid.SetRow(headerCategory, row);
            Grid.SetColumn(headerCategory, 1);

            var headerQuantity = CreateLabel("Quantity", true);
            InventoryGrid.Children.Add(headerQuantity);
            Grid.SetRow(headerQuantity, row);
            Grid.SetColumn(headerQuantity, 2);

            var headerUnitPrice = CreateLabel("Unit Price", true);
            InventoryGrid.Children.Add(headerUnitPrice);
            Grid.SetRow(headerUnitPrice, row);
            Grid.SetColumn(headerUnitPrice, 3);

            string currentInventoryID = Preferences.Get("inventoryId", string.Empty);

            // Filter items by current inventory ID
            var filteredItems = items.Where(item => item.InventoryId == currentInventoryID);

            // Add rows for each item
            foreach (var item in filteredItems)
            {
                row++;
                InventoryGrid.RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(60),
                });

                // Name column
                var nameLabel = CreateLabel(item.name);
                InventoryGrid.Children.Add(nameLabel);
                Grid.SetRow(nameLabel, row);
                Grid.SetColumn(nameLabel, 0);

                // Category column
                var categoryLabel = CreateLabel(item.category);
                InventoryGrid.Children.Add(categoryLabel);
                Grid.SetRow(categoryLabel, row);
                Grid.SetColumn(categoryLabel, 1);

                // Quantity column
                var quantityLabel = CreateLabel(item.quantity.ToString());
                InventoryGrid.Children.Add(quantityLabel);
                Grid.SetRow(quantityLabel, row);
                Grid.SetColumn(quantityLabel, 2);

                // Unit Price column
                var unitPriceLabel = CreateLabel(item.unitPrice.ToString("C"));
                InventoryGrid.Children.Add(unitPriceLabel);
                Grid.SetRow(unitPriceLabel, row);
                Grid.SetColumn(unitPriceLabel, 3);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load items: {ex.Message}", "OK");
        }
    }

    private Label CreateLabel(string text, bool isHeader = false)
    {
        return new Label
        {
            Text = text,
            TextColor = Color.FromArgb("#FFFFFF"),
            FontAttributes = isHeader ? FontAttributes.Bold : FontAttributes.None,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Center
        };
    }
}
