using System.Threading.Tasks;
using ZXing.Net.Maui;

namespace UndergradProject.Pages;

public partial class BarcodeScannerPage : ContentPage
{
    private readonly Action<string> _onBarcodeScanned;

    public BarcodeScannerPage(Action<string> onBarcodeScanned)
    {
        InitializeComponent();
        _onBarcodeScanned = onBarcodeScanned;
        CheckCameraPermissions();

        barcodeReader.Options = new ZXing.Net.Maui.BarcodeReaderOptions
        {
            Formats = ZXing.Net.Maui.BarcodeFormat.Ean13,
            AutoRotate = true,
            TryHarder = true,
            Multiple = true
        };
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
        barcodeReader.IsDetecting = false;
        var first = e.Results.FirstOrDefault();
        if (first is null)
            return;

        Dispatcher.DispatchAsync(async () =>
        {
            _onBarcodeScanned?.Invoke(first.Value); // Send barcode back
            await Navigation.PopModalAsync(); // Close scanner
        });
    }
}
