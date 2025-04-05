namespace UndergradProject.Pages;

public partial class CheckoutPage : ContentPage
{
	public CheckoutPage()
	{
		InitializeComponent();
	}

    private async void addButton_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushModalAsync(new CheckoutAddItem());
    }
}