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

    private void inventoryButton_Clicked(object sender, EventArgs e)
    {

    }

    private void analyticsButton_Clicked(object sender, EventArgs e)
    {

    }

    private void checkoutButton_Clicked(object sender, EventArgs e)
    {

    }

    private void addItem_Clicked(object sender, EventArgs e)
    {

    }

    private void editItem_Clicked(object sender, EventArgs e)
    {

    }

    private async void logoutButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}