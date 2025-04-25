using Microsoft.Maui;
using Newtonsoft.Json;
using UndergradProject.Business_Logic_Layer;
using UndergradProject.Data_Access_Layer.Models;

namespace UndergradProject.Pages;

public partial class AddItemPage : ContentPage
{
	public AddItemPage()
	{
		InitializeComponent();
        ItemMangement itemMangement = new ItemMangement();
        //itemMangement.getAllItemsFromDatabase();
    }

    private async void scanButton_Clicked(object sender, EventArgs e)
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

    private void OnBarcodeScanned(string barcode)
    {
        // Update the label with the scanned barcode
        barcodeStatusLabel.TextColor = Colors.Green;
        barcodeStatusLabel.Text = $"Scanned: {barcode}";
    }

    private async void addItemButton_Clicked(object sender, EventArgs e)
    {
        string itemName = itemNameEntry.Text?.Trim();
        if (itemName == null)
        {
            await DisplayAlert("Invalid Input", "Please enter a valid itemName.", "OK");
            itemNameEntry.BackgroundColor = Colors.Red;
            return;
        }

        // Try parsing quantity
        bool quantityParsed = int.TryParse(quantityEntry.Text?.Trim(), out int quantity);
        if (!quantityParsed)
        {
            await DisplayAlert("Invalid Input", "Please enter a valid quantity.", "OK");
            return;
        }

        // Try parsing price
        bool priceParsed = decimal.TryParse(priceEntry.Text?.Trim(), out decimal price);
        if (!priceParsed)
        {
            await DisplayAlert("Invalid Input", "Please enter a valid price.", "OK");
            return;
        }

        // Get category
        string category = categoryPicker.SelectedItem?.ToString();
        if (string.IsNullOrWhiteSpace(category))
        {
            await DisplayAlert("Missing Category", "Please select a category.", "OK");
            return;
        }

        // Extract barcode if it was scanned
        string barcode = null;
        if (barcodeStatusLabel.Text?.StartsWith("Scanned: ") == true)
        {
            barcode = barcodeStatusLabel.Text.Substring("Scanned: ".Length).Trim();
        }

        ItemMangement itemMangement = new ItemMangement();

        bool isItemCreated = await itemMangement.createItem(itemName, category, barcode, quantity, price);
        if (isItemCreated)
        {
            await DisplayAlert("Item Added", $"Name: {itemName}\nQty: {quantity}\nPrice: {price:C}\nCategory: {category}\nBarcode: {barcode ?? "None"}", "OK");
            await Navigation.PopModalAsync();
        }
        else
        {
            await DisplayAlert("Error", "Error adding Item",  "OK");
        }
    }

    private async void ImportFromCSV_Clicked(object sender, EventArgs e)
    {
        // Requesting permissions for Android (for file access)
        var status = await Permissions.RequestAsync<Permissions.StorageRead>();

        if (status != PermissionStatus.Granted)
        {
            await DisplayAlert("Permission Denied", "Storage permission is required to select a file.", "OK");
            return;
        }

        // Proceed with the file picker logic
        try
        {
            var fileResult = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select a JSON file",
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { ".json" } },
                { DevicePlatform.Android, new[] { "application/json" } },
                { DevicePlatform.iOS, new[] { "application/json" } },
                { DevicePlatform.MacCatalyst, new[] { "application/json" } },
            })
            });

            if (fileResult == null)
                return;

            using var stream = await fileResult.OpenReadAsync();
            using var reader = new StreamReader(stream);
            string json = await reader.ReadToEndAsync();

            string currentInventoryID = Preferences.Get("inventoryId", string.Empty);
            if (string.IsNullOrEmpty(currentInventoryID))
            {
                await DisplayAlert("Error", "No Inventory ID found for current session.", "OK");
                return;
            }

            List<Item> items = JsonConvert.DeserializeObject<List<Item>>(json);

            // Here we go through each item and add it to the database
            ItemMangement itemMangement = new ItemMangement();
            int successCount = 0;
            List<string> errors = new List<string>();

            foreach (var item in items)
            {
                // Add Inventory ID to each item
                item.InventoryId = currentInventoryID;

                bool created = await itemMangement.createItem(item.name, item.category, item.barcode, item.quantity, item.unitPrice);
                if (created)
                    successCount++;
                else
                    errors.Add($"Failed to add item: {item.name}");
            }

            await DisplayAlert("Import Complete", $"{successCount} items added.\n{errors.Count} errors.", "OK");

            if (errors.Count > 0)
                Console.WriteLine(string.Join(Environment.NewLine, errors));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }
}