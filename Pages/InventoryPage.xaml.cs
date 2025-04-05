using UndergradProject.Data_Access_Layer;

namespace UndergradProject.Pages;

public partial class InventoryPage : ContentPage
{
    DatabaseService _databaseService = new DatabaseService(Constants.databasePath);
    public InventoryPage()
    {
        InitializeComponent();

        LoadUsers();
    }

    private async void LoadUsers()
    {
        try
        {
            await _databaseService.CreateTableAsync<User>();
            var users = await _databaseService.GetDataAsync<User>();

            // Clear out any existing rows/children
            InventoryGrid.Children.Clear();
            InventoryGrid.RowDefinitions.Clear();

            InventoryGrid.ColumnDefinitions.Clear();
            InventoryGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // ID
            InventoryGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Username
            InventoryGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Email
            InventoryGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Role


            int row = 0;

            // Add row for headers
            InventoryGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Add headers
            var headerId = CreateLabel("ID", true);
            InventoryGrid.Children.Add(headerId);
            Grid.SetRow(headerId, row);
            Grid.SetColumn(headerId, 0);

            var headerUsername = CreateLabel("Username", true);
            InventoryGrid.Children.Add(headerUsername);
            Grid.SetRow(headerUsername, row);
            Grid.SetColumn(headerUsername, 1);

            var headerEmail = CreateLabel("Email", true);
            InventoryGrid.Children.Add(headerEmail);
            Grid.SetRow(headerEmail, row);
            Grid.SetColumn(headerEmail, 2);

            var headerRole = CreateLabel("Role", true);
            InventoryGrid.Children.Add(headerRole);
            Grid.SetRow(headerRole, row);
            Grid.SetColumn(headerRole, 3);


            // Add user rows
            foreach (var user in users)
            {
                row++;
                InventoryGrid.RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(60),
                });

                // ID column
                var idLabel = CreateLabel(user.userId.ToString());
                InventoryGrid.Children.Add(idLabel);
                Grid.SetRow(idLabel, row);
                Grid.SetColumn(idLabel, 0);

                // Username column
                var usernameLabel = CreateLabel(user.Username);
                InventoryGrid.Children.Add(usernameLabel);
                Grid.SetRow(usernameLabel, row);
                Grid.SetColumn(usernameLabel, 1);

                // Email column
                var emailLabel = CreateLabel(user.Email);
                InventoryGrid.Children.Add(emailLabel);
                Grid.SetRow(emailLabel, row);
                Grid.SetColumn(emailLabel, 2);

                // Role column (assuming you have a Role property — if not, replace with something else)
                var roleLabel = CreateLabel(user.Password); // or user.Password if you're using that for now
                InventoryGrid.Children.Add(roleLabel);
                Grid.SetRow(roleLabel, row);
                Grid.SetColumn(roleLabel, 3);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load users: {ex.Message}", "OK");
        }
    }

    private Label CreateLabel(string text, bool isHeader = false)
    {
        return new Label
        {
            Text = text,
            TextColor = Color.FromArgb("#FFFFFF"),
            FontAttributes = isHeader ? FontAttributes.Bold : FontAttributes.None,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Center
        };
    }
}
