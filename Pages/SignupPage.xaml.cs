using UndergradProject.Business_Logic_Layer;

namespace UndergradProject.Pages;

public partial class SignupPage : ContentPage
{
    Validation validation = new Validation();
	public SignupPage()
	{
		InitializeComponent();
    }

    private void signupButton_Clicked(object sender, EventArgs e)
    {
        string username = usernameEntry.Text?.Trim();
        string email = emailEntry.Text?.Trim();
        string password = passwordEntry.Text?.Trim();

        bool isValid = true;
        List<string> invalidFields = new List<string>();

        // Reset all entry borders to default
        usernameEntry.BackgroundColor = Constants.darkGrayColor;
        emailEntry.BackgroundColor = Constants.darkGrayColor;
        passwordEntry.BackgroundColor = Constants.darkGrayColor;

        // Username validation
        if (string.IsNullOrEmpty(username))
        {
            usernameEntry.BackgroundColor = Colors.Red;
            invalidFields.Add("Username");
            isValid = false;
        }

        // Email validation
        if (string.IsNullOrEmpty(email) || !validation.isEmailValid(email))
        {
            emailEntry.BackgroundColor = Colors.Red;
            invalidFields.Add("Email");
            isValid = false;
        }

        // Password validation
        if (string.IsNullOrEmpty(password) || password.Length < 6)
        {
            passwordEntry.BackgroundColor = Colors.Red;
            invalidFields.Add("Password");
            isValid = false;
        }

        if (isValid)
        {
            UserMangement userMangement = new UserMangement();
            userMangement.createUser(username, email, password);

            // Show success prompt
            DisplayAlert("Success", "User created successfully!", "OK");

            // Clear the entries
            usernameEntry.Text = string.Empty;
            emailEntry.Text = string.Empty;
            passwordEntry.Text = string.Empty;

            // TODO: Navigate to login page
            // e.g., Application.Current.MainPage = new LoginPage();

        }
        else
        {
            // Clear only the invalid fields
            if (usernameEntry.BackgroundColor == Colors.Red)
            {
                usernameEntry.Text = string.Empty;
            }

            if (emailEntry.BackgroundColor == Colors.Red)
            {
                emailEntry.Text = string.Empty;
            }

            if (passwordEntry.BackgroundColor == Colors.Red)
            {
                passwordEntry.Text = string.Empty;
            }

            string message = "Invalid entries: " + string.Join(", ", invalidFields);
            DisplayAlert("Validation Error", message, "OK");
        }
    }
}