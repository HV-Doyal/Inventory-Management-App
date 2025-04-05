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
            await Navigation.PushModalAsync(new BarcodeScannerPage());
        }
        else
        {
            await DisplayAlert("Permission Denied", "Camera permission is required to scan barcodes. Please enable it in the app settings.", "OK");
        }
    }

    private void addItemButton_Clicked(object sender, EventArgs e)
    {

    }
}