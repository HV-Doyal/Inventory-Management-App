using Microsoft.Maui;
using UndergradProject.Business_Logic_Layer;
using UndergradProject.Data_Access_Layer.Models;
using ZXing.QrCode.Internal;

namespace UndergradProject.Pages;

public partial class EditItemPage : ContentPage
{
    DatabaseService databaseItemService = new DatabaseService(Constants.itemsDatabasePath);
    Item itemToEdit;
    public EditItemPage()
	{
		InitializeComponent();
	}

    private async void applyChangesButton_Clicked(object sender, EventArgs e)
    {
        string itemName = itemNameEntry.Text?.Trim();
        if (itemName == null)
        {
            await DisplayAlert("Invalid Input", "Please enter a valid itemName.", "OK");
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

        itemToEdit.name = itemName;
        itemToEdit.category = category;
        itemToEdit.quantity = quantity;
        itemToEdit.unitPrice = price;

        await databaseItemService.UpdateDataAsync(itemToEdit);
        await Navigation.PopModalAsync();
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
    private async void OnBarcodeScanned(string barcode)
    {
        // Update the label with the scanned barcode
        barcodeStatusLabel.TextColor = Colors.Green;
        barcodeStatusLabel.Text = $"Scanned: {barcode}";

        ItemMangement itemMangement = new ItemMangement();
        await itemMangement.initialiseItemTable();
        string query = $"SELECT * FROM Item WHERE Barcode = ? AND InventoryId = {LoginPage.currentInventoryID}";
        var result = await databaseItemService.QueryAsync<Item>(query, barcode);

        if (result == null)
        {
            await DisplayAlert("Error", "No item found with the scanned barcode.", "OK");
            return;
        }

        itemToEdit = result.FirstOrDefault();

        itemNameEntry.Text = itemToEdit.name;
        quantityEntry.Text = itemToEdit.quantity.ToString();
        priceEntry.Text = itemToEdit.unitPrice.ToString();
        categoryPicker.SelectedItem = itemToEdit.category;
    }
}