using UndergradProject.Business_Logic_Layer;
using UndergradProject.Data_Access_Layer.Models;

namespace UndergradProject.Pages;

public partial class CheckoutPage : ContentPage
{
    int row = 0;
    List<Sale> currentSales = new List<Sale>(); // Track sales
    decimal totalPrice = 0;

    public CheckoutPage()
    {
        InitializeComponent();
        initialiseGrid();
    }

    private void updateTotalPrice()
    {
        foreach(Sale sale in currentSales)
        {
            totalPrice += sale.totalPrice;
        }

        totalPriceLabel.Text = $"Total Price: {totalPrice}";
    }

    private async void addButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new CheckoutAddItem(onAddButtonClicked));
        updateTotalPrice();
    }

    private void onAddButtonClicked(Sale sale)
    {
        sale.date = DateTime.Now.ToString();
        addOrUpdateSaleInGrid(sale);
    }

    private async void scanBarcode_Clicked(object sender, EventArgs e)
    {
        var status = await Permissions.RequestAsync<Permissions.Camera>();
        if (status == PermissionStatus.Granted)
        {
            await Navigation.PushModalAsync(new BarcodeScannerPage(OnBarcodeScanned));
        }
        else
        {
            await DisplayAlert("Permission Denied", "Camera permission is required to scan barcodes. Please enable it in the app settings.", "OK");
        }
    }

    private async void OnBarcodeScanned(string barcode)
    {
        ItemMangement itemMangement = new ItemMangement();
        await itemMangement.initialiseItemTable();

        DatabaseService databaseItemService = new DatabaseService(Constants.itemsDatabasePath);
        string query = $"SELECT * FROM Item WHERE Barcode = ?";
        var result = await databaseItemService.QueryAsync<Item>(query, barcode);

        if (result == null || !result.Any())
        {
            await DisplayAlert("Error", "No item found with the scanned barcode.", "OK");
            return;
        }

        Item item = result.First();
        Sale sale = new Sale(item.name, 1, item.category, item.unitPrice, DateTime.Now.ToString());

        addOrUpdateSaleInGrid(sale);
    }

    private async void checkoutButton_Clicked(object sender, EventArgs e)
    {
        SaleManagement saleManagement = new SaleManagement();
        ItemMangement itemMangement = new ItemMangement();
        List<Item> items = await itemMangement.getAllItemsFromDatabase();

        List<Item> updatedItems = new List<Item>(); // To store the updated items

        foreach (Sale sale in currentSales)
        {
            // Add the sale to the database
            await saleManagement.addSaleToDatabase(sale);

            // Find the item that matches the sale's name and unitPrice
            Item matchingItem = items.FirstOrDefault(item => item.name == sale.name && item.unitPrice == sale.unitPrice);

            if (matchingItem != null)
            {
                // Decrease the quantity of the matching item by the sale's quantity
                matchingItem.quantity -= sale.quantity;

                // Add the updated item to the list
                updatedItems.Add(matchingItem);
            }
        }
        // After the loop, update the database with the new quantities
        foreach (Item updatedItem in updatedItems)
        {
            await itemMangement.updataItemDB(updatedItem);
        }
        await DisplayAlert("Checkout Complete", $"The items have been successfully checked out and updated. {totalPriceLabel.Text}", "OK");
        await Navigation.PopModalAsync();
    }


    private void initialiseGrid()
    {
        checkoutGrid.Children.Clear();
        checkoutGrid.RowDefinitions.Clear();
        checkoutGrid.ColumnDefinitions.Clear();

        checkoutGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
        checkoutGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        checkoutGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        checkoutGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        checkoutGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        var headerName = CreateLabel("Name", true);
        checkoutGrid.Children.Add(headerName);
        Grid.SetRow(headerName, row);
        Grid.SetColumn(headerName, 0);

        var headerQuantity = CreateLabel("Quantity", true);
        checkoutGrid.Children.Add(headerQuantity);
        Grid.SetRow(headerQuantity, row);
        Grid.SetColumn(headerQuantity, 1);

        var headerUnitPrice = CreateLabel("Unit Price", true);
        checkoutGrid.Children.Add(headerUnitPrice);
        Grid.SetRow(headerUnitPrice, row);
        Grid.SetColumn(headerUnitPrice, 2);

        var headerPrice = CreateLabel("Price", true);
        checkoutGrid.Children.Add(headerPrice);
        Grid.SetRow(headerPrice, row);
        Grid.SetColumn(headerPrice, 3);
    }

    private void addOrUpdateSaleInGrid(Sale newSale)
    {
        var existingSale = currentSales.FirstOrDefault(s => s.name == newSale.name && s.unitPrice == newSale.unitPrice);

        if (existingSale != null)
        {
            existingSale.quantity++;

            int existingRowIndex = currentSales.IndexOf(existingSale) + 1; // +1 due to header row

            foreach (var child in checkoutGrid.Children)
            {
                if (checkoutGrid.GetRow(child) == existingRowIndex)
                    {
                    int col = checkoutGrid.GetColumn(child);

                    if (col == 1 && child is Label quantityLabel)
                        quantityLabel.Text = existingSale.quantity.ToString();

                    if (col == 3 && child is Label totalPriceLabel)
                        totalPriceLabel.Text = existingSale.totalPrice.ToString("C");
                }
            }
        }
        else
        {
            currentSales.Add(newSale);
            row++;
            checkoutGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60) });

            var nameLabel = CreateLabel(newSale.name);
            checkoutGrid.Children.Add(nameLabel);
            Grid.SetRow(nameLabel, row);
            Grid.SetColumn(nameLabel, 0);

            var quantityLabel = CreateLabel(newSale.quantity.ToString());
            checkoutGrid.Children.Add(quantityLabel);
            Grid.SetRow(quantityLabel, row);
            Grid.SetColumn(quantityLabel, 1);

            var unitPriceLabel = CreateLabel(newSale.unitPrice.ToString("C"));
            checkoutGrid.Children.Add(unitPriceLabel);
            Grid.SetRow(unitPriceLabel, row);
            Grid.SetColumn(unitPriceLabel, 2);

            var totalPriceLabel = CreateLabel(newSale.totalPrice.ToString("C"));
            checkoutGrid.Children.Add(totalPriceLabel);
            Grid.SetRow(totalPriceLabel, row);
            Grid.SetColumn(totalPriceLabel, 3);
        }
        updateTotalPrice();
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
