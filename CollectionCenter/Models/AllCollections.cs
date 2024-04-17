using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CollectionCenter.Models
{
    internal class AllCollections
    {
        public ObservableCollection<SingleCollection> AllCollectionz { get; set; } = new ObservableCollection<SingleCollection>();

        public string NewCollectionName { get; set; }

        public AllCollections() => LoadCollections();
    
        public void LoadCollections()
        {
            AllCollectionz.Clear();
            IEnumerable<SingleCollection> collection = Directory.EnumerateFiles(FileSystem.AppDataDirectory, $"*_collection.txt").Select(collName => new SingleCollection() { CollectionName = File.ReadAllLines(collName).First(), });
            foreach(SingleCollection singleColl in collection)
            {
                AllCollectionz.Add(singleColl);
            }
        }        
    }
}
