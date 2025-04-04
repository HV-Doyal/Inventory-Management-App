namespace UndergradProject.Pages;

public partial class Dashboard : ContentPage
{
	public Dashboard()
	{
		InitializeComponent();

        // Set the Label's Text property to "Welcome, [username]"
        string savedUsername = Preferences.Get("username", "Guest");
        welcomeLabel.Text = $"Welcome, {savedUsername}";
    }

    private async void inventoryButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new InventoryPage());
    }

    private async void analyticsButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new AnalyticsPage());
    }

    private async void checkoutButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new CheckoutPage());
    }

    private async void addItem_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new AddItemPage());
    }

    private async void editItem_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new EditItemPage());
    }

    private async void logoutButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}