using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Pharmacy.ViewModels;

public class ListMedicine : INotifyPropertyChanged
{
    public ObservableCollection<string> Items { get; set; }

    public ListMedicine()
    {
        Items = new ObservableCollection<string>();
        LoadData();
    }

    private async Task LoadData()
    {
        using(PharmacyDbContext db = new PharmacyDbContext())
        {
            var med = await db.Medicines.ToListAsync();
            if (med != null)
            {
                foreach (var item in med)
                {
                    Items.Add(item.Name);
                }
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
