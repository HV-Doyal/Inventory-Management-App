namespace UndergradProject.Pages;

public partial class AddItemPage : ContentPage
{
	public AddItemPage()
	{
		InitializeComponent();
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

    private void addItemButton_Clicked(object sender, EventArgs e)
    {

    }
}