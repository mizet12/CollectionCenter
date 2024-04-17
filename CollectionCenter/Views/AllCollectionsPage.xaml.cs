using CollectionCenter.Models;
namespace CollectionCenter.Views;

public partial class AllCollectionsPage : ContentPage
{
	public AllCollectionsPage()
	{
		InitializeComponent();
        BindingContext = new AllCollections();
	}

    protected override void OnAppearing()
    {
        ((AllCollections)BindingContext).LoadCollections();
    }

    private async void AddCollectionClicked(object sender, EventArgs e)
    {
       if(BindingContext is AllCollections context && !string.IsNullOrEmpty(context.NewCollectionName)){
            string fileName = $"{context.NewCollectionName}_collection.txt";
            File.WriteAllText(Path.Combine(FileSystem.AppDataDirectory, fileName), context.NewCollectionName);
            await Shell.Current.GoToAsync($"{nameof(CollectionPage)}?{nameof(CollectionPage.CollectionName)}={context.NewCollectionName}");
        }
    }

    private async void ClassButtonClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var context = (SingleCollection)button.BindingContext;
        await Shell.Current.GoToAsync($"{nameof(CollectionPage)}?{nameof(CollectionPage.CollectionName)}={context.CollectionName}");
    }
}