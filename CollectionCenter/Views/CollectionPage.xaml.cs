using CollectionCenter.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CollectionCenter.Views;

    [QueryProperty(nameof(CollectionName), nameof(CollectionName))]
    public partial class CollectionPage : ContentPage
    {
    private string collectionName;
        public string CollectionName
        {
            get { return collectionName; }
            set
            {
                if (value != null)
                {
                    collectionName = value;
                    LoadCollection(value);
                }
            }
        }

        public CollectionPage()
        {
            InitializeComponent();
            BindingContext = new SingleCollection();
        }

        private void LoadCollection(string collectionName)
        {
            LogFilePath(collectionName);
            var context = (SingleCollection) BindingContext;
            string collectablesFile = File.ReadAllText(Path.Combine(FileSystem.AppDataDirectory, $"{collectionName}_collection.txt"));
            string[] collectablesPerLine = collectablesFile.Split("\r\n");
            collectablesPerLine = collectablesPerLine.Skip(1).ToArray();

            foreach (string line in collectablesPerLine)
            {
                string[] data = line.Split(';');

                SingleCollectable collectable = new SingleCollectable()
                {
                    Name = data[0],
                    Price = float.Parse(data[1]),
                    Rating = int.Parse(data[2]),
                    Status = data[3],
                    Comment = data[4]
                };

                context.Collectables.Add(collectable);
            }
        }

        private void AddCollectable_Clicked(object sender, EventArgs e)
        {
            var context = (SingleCollection)BindingContext;
       
            if (!string.IsNullOrEmpty(context.NewCollectableName))
            {
            if (context.NewCollectableRating < 1 || context.NewCollectableRating > 10)
            {
                DisplayAlert("Error", "Rating must be between 1 and 10.", "OK");
                return;
            }

            string collectableLine = $"\r\n{context.NewCollectableName};{context.NewCollectablePrice};{context.NewCollectableRating};{context.NewCollectableStatus};{context.NewCollectableComment}";

                File.AppendAllText(Path.Combine(FileSystem.AppDataDirectory, $"{this.CollectionName}_collection.txt"), collectableLine);

                SingleCollectable collectable = new SingleCollectable()
                {
                    Name = context.NewCollectableName,
                    Price = context.NewCollectablePrice,
                    Rating = context.NewCollectableRating,
                    Status = context.NewCollectableStatus,
                    Comment = context.NewCollectableComment
                };

                context.Collectables.Add(collectable);
            }
       
     
        
        }


        private async void DeleteCollectable_Clicked(object sender, EventArgs e)
        {
            var context = (SingleCollection)BindingContext;
            var button = (Button)sender;
            var collectable = (SingleCollectable)button.BindingContext;

            string collectablesFromFile = File.ReadAllText(Path.Combine(FileSystem.AppDataDirectory, $"{this.CollectionName}_collection.txt"));
            string[] separatedCollectables = collectablesFromFile.Split("\r\n");
            separatedCollectables = separatedCollectables.Skip(1).ToArray();

            string newWholeCollection = $"{this.CollectionName}";
            foreach (string CollectableLine in separatedCollectables)
            {
                string[] things = CollectableLine.Split(';');

                if (things[0] != collectable.Name)
                {
                    newWholeCollection += $"\r\n{things[0]};{things[1]};{things[2]};{things[3]};{things[4]}";
                }
                else continue;
            }

            File.Delete(Path.Combine(FileSystem.AppDataDirectory, $"{this.CollectionName}_collection.txt"));
            File.WriteAllText(Path.Combine(FileSystem.AppDataDirectory, $"{this.CollectionName}_collection.txt"), newWholeCollection);
            context.Collectables.Remove(collectable);
        }

    private async void EditCollectable_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var collectableToEdit = (SingleCollectable)button.BindingContext;
        var context = (SingleCollection)BindingContext;


        var editPage = new EditCollectablePage(collectableToEdit, updatedCollectable =>
        {
            context.Collectables.Remove(collectableToEdit);

            DeleteCollectableFromFile(collectableToEdit);

            AppendUpdatedCollectableToFile(updatedCollectable);

            context.Collectables.Add(updatedCollectable);
        });

        await Navigation.PushModalAsync(editPage);
    }


    private void DeleteCollectableFromFile(SingleCollectable collectable)
    {
        var context = (SingleCollection)BindingContext;
        string collectablesFromFile = File.ReadAllText(Path.Combine(FileSystem.AppDataDirectory, $"{this.CollectionName}_collection.txt"));
        string[] separatedCollectables = collectablesFromFile.Split("\r\n");
        separatedCollectables = separatedCollectables.Skip(1).ToArray();

        string newWholeCollection = $"{this.CollectionName}";
        foreach (string CollectableLine in separatedCollectables)
        {
            string[] things = CollectableLine.Split(';');

            if (things[0] != collectable.Name)
            {
                newWholeCollection += $"\r\n{things[0]};{things[1]};{things[2]};{things[3]};{things[4]}";
            }
            else continue;
        }

        File.Delete(Path.Combine(FileSystem.AppDataDirectory, $"{this.CollectionName}_collection.txt"));
        File.WriteAllText(Path.Combine(FileSystem.AppDataDirectory, $"{this.CollectionName}_collection.txt"), newWholeCollection);
        context.Collectables.Remove(collectable);
    }

    private void AppendUpdatedCollectableToFile(SingleCollectable updated)
    {
        var context = (SingleCollection)BindingContext;
        var existingItem = context.Collectables.FirstOrDefault(c => c.Name == updated.Name);

        if (existingItem != null)
        {
            existingItem.Price = updated.Price;
            existingItem.Rating = updated.Rating;
            existingItem.Status = updated.Status;
            existingItem.Comment = updated.Comment;
        }
        else
        {
            context.Collectables.Add(updated);
        }

        string filePath = Path.Combine(FileSystem.AppDataDirectory, $"{this.CollectionName}_collection.txt");
        string newLine = $"\r\n{updated.Name};{updated.Price};{updated.Rating};{updated.Status};{updated.Comment}";
        File.AppendAllText(filePath, newLine);
    }



    private void LogFilePath(string collectionName)
    {
        string filePath = Path.Combine(FileSystem.AppDataDirectory, $"{collectionName}_collection.txt");
        Debug.WriteLine($"Collection file path for {collectionName}: {filePath}");
    }

}

