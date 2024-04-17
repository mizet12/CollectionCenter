using CollectionCenter.Models;

namespace CollectionCenter.Views;

public partial class EditCollectablePage : ContentPage
{
    private SingleCollectable originalCollectable;
    private Action<SingleCollectable> onSave;

    public EditCollectablePage(SingleCollectable collectable, Action<SingleCollectable> onSaveCallback)
    {
        InitializeComponent();
        originalCollectable = collectable;
        onSave = onSaveCallback;

        NameEntry.Text = collectable.Name;
        PriceEntry.Text = collectable.Price.ToString();
        RatingEntry.Text = collectable.Rating.ToString();
        StatusEntry.Text = collectable.Status;
        CommentEntry.Text = collectable.Comment;
    }

    private void SaveChanges_Clicked(object sender, EventArgs e)
    {
        originalCollectable.Name = NameEntry.Text;
        originalCollectable.Price = float.Parse(PriceEntry.Text);
        originalCollectable.Rating = int.Parse(RatingEntry.Text);
        originalCollectable.Status = StatusEntry.Text;
        originalCollectable.Comment = CommentEntry.Text;

        onSave?.Invoke(originalCollectable);
        this.Navigation.PopModalAsync();
    }
}
