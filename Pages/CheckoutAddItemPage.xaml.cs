using System.Threading.Tasks;
using UndergradProject.Data_Access_Layer.Models;

namespace UndergradProject.Pages;

public partial class CheckoutAddItem : ContentPage
{
    private readonly Action<Sale> _onAddButtonClicked;
    public CheckoutAddItem(Action<Sale> onAddButtonClicked)
	{
		InitializeComponent();
        _onAddButtonClicked = onAddButtonClicked;
	}

    private async void addItemButton_Clicked(object sender, EventArgs e)
    {
        Sale sale = new Sale
        {
            name = itemNameEntry.Text,
            quantity = int.Parse(manualQuantityEntry.Text),
            unitPrice = decimal.Parse(manualPriceEntry.Text),
        };
        _onAddButtonClicked?.Invoke(sale);
        await Navigation.PopModalAsync();
    }
}