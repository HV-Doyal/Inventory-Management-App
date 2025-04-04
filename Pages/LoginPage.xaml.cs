using UndergradProject.Pages;
namespace UndergradProject;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private void loginButton_Clicked(object sender, EventArgs e)
    {

    }

    private async void signupInsteadButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new SignupPage());
    }
}