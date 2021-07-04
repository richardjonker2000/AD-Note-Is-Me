using NoteIsMe.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteIsMe.UWP.ViewModels
{
    public class FolderNotebookViewModel
    {
        public ObservableCollection<FolderNotebook> FolderNotebooks { get; set; }
        public FolderNotebook folderNotebook;

        public FolderNotebookViewModel()
        {
            FolderNotebooks = new ObservableCollection<FolderNotebook>();
            folderNotebook = new FolderNotebook();
        }

        public async Task LoadAllAsync()
        {

            List<FolderNotebook> list = await App.UnitOfWork.FolderNotebooksRepository.FindAllAsync();

            FolderNotebooks.Clear();
            foreach (FolderNotebook e in list)
            {
                FolderNotebooks.Add(e);
            }
        }

        internal async Task InsertAsync()
        {
            await App.UnitOfWork.FolderNotebooksRepository.CreateAsync(folderNotebook);
        
        }

        internal async Task DeleteAsync()
        {
            await App.UnitOfWork.FolderNotebooksRepository.DeleteAsync(folderNotebook);
        }
    }

}
