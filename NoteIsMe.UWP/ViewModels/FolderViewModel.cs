using NoteIsMe.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteIsMe.UWP.ViewModels
{
    public class FolderViewModel
    {
        public ObservableCollection<Folder> Folders { get; set; }
        public Folder Folder { get; set; }

        public FolderViewModel()
        {
            Folders = new ObservableCollection<Folder>();
            Folder = new Folder();

        }

        public async Task LoadAllAsync()
        {

            List<Folder> list = await App.UnitOfWork.FolderRepository.FindAllAsync();

            Folders.Clear();
            foreach (Folder e in list)
            {
                e.FolderNotebooks = await App.UnitOfWork.FolderNotebooksRepository.FindByFolderIdAsync(e.Id);
                e.Owner = await App.UnitOfWork.UserRepository.FindByIdAsync(e.OwnerId);

                Folders.Add(e);
            }
        }



        public async Task LoadAllMineAsync()
        {
            List<Folder> list = await App.UnitOfWork.FolderRepository.FindAllMyFoldersAsync(App.userViewModel.GetCurrentUserID());
            Folders.Clear();
            foreach (Folder e in list)
            {
                e.FolderNotebooks = await App.UnitOfWork.FolderNotebooksRepository.FindByFolderIdAsync(e.Id);
                e.Owner = await App.UnitOfWork.UserRepository.FindByIdAsync(e.OwnerId);
                Folders.Add(e);
            }
        }

        internal async Task InsertAsync()
        {
            await App.UnitOfWork.FolderRepository.CreateAsync(Folder);
        }

        internal async Task UpsertAsync()
        {
            await App.UnitOfWork.FolderRepository.UpdateAsync(Folder);
        }

        internal async Task DeleteAsync(Folder folder)
        {
            
            await App.UnitOfWork.FolderRepository.DeleteWithForeignKeyAsync(folder);
            Folders.Remove(folder);
        }
    }
}
