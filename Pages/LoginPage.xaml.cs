using UndergradProject.Pages;
using UndergradProject.Business_Logic_Layer;
using UndergradProject.Data_Access_Layer;
using System.Threading.Tasks;
namespace UndergradProject;
using Microsoft.Maui.Storage;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void loginButton_Clicked(object sender, EventArgs e)
    {
        string username = usernameEntry.Text?.Trim();
        string password = passwordEntry.Text?.Trim();
        UserMangement userMangement = new UserMangement();
        await userMangement.initialiseUsersTable();

        // Check if username or password is null or whitespace
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Username or password cannot be empty!", "OK");
            return; 
        }

        List<User> users = await userMangement.getAllUsersFromDatabase();

        if (await userMangement.isAccountValid(username, password))
        {
            Preferences.Set("username", username);
            await DisplayAlert("Success", "Login successful!", "OK");
            await Navigation.PushModalAsync(new Dashboard());
        }
        else
        {
            await DisplayAlert("Error", "Invalid username or password!", "OK");
            usernameEntry.Text = string.Empty;
            passwordEntry.Text = string.Empty;
        }
    }


    private async void signupInsteadButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new SignupPage());
    }
}