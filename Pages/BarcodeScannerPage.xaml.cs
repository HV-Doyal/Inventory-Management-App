using System.Threading.Tasks;
using ZXing.Net.Maui;

namespace UndergradProject.Pages;

public partial class BarcodeScannerPage : ContentPage
{
    public BarcodeScannerPage()
    {
        InitializeComponent();
        CheckCameraPermissions();

        barcodeReader.Options = new ZXing.Net.Maui.BarcodeReaderOptions
        {
            Formats = ZXing.Net.Maui.BarcodeFormat.Ean13,
            AutoRotate = true,
            TryHarder = true,
            Multiple = true
        };

    }

    // Handle Barcode Detected
    private async void barcodeReader_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    // Handle Camera Permissions
    private async void CheckCameraPermissions()
    {
        var cameraPermissionStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();

        if (cameraPermissionStatus != PermissionStatus.Granted)
        {
            var permissionRequest = await Permissions.RequestAsync<Permissions.Camera>();
            if (permissionRequest != PermissionStatus.Granted)
            {
                await DisplayAlert("Permission Denied", "Cannot access camera to scan barcodes.", "OK");
            }
        }
    }

    private void barcodeReader_BarcodesDetected_1(object sender, BarcodeDetectionEventArgs e)
    {
        var first = e.Results.FirstOrDefault();
        if (first is null)
            return;

        Dispatcher.DispatchAsync(async () =>
        {
            await DisplayAlert("Barcode Detected", first.Value, "OK");
            // Stop scanning after the first barcode is detected
            barcodeReader.IsDetecting = false;
            await Navigation.PopModalAsync();
        });
        
    }
}
