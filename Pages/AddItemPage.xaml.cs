using Microsoft.Maui;
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

}