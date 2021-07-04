using NoteIsMe.Domain.Models;
using NoteIsMe.UWP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteIsMe.UWP.ViewModels
{
    public class GroupViewModel
    {
        public ObservableCollection<Group> Groups { get; set; }



        public Group Group { get; set; }

        public GroupViewModel()
        {
            Groups = new ObservableCollection<Group>();
            Group = new Group();

        }

        public async Task LoadAllAsync()
        {
            List<Group> list = await App.UnitOfWork.GroupRepository.FindAllAsync();
            Groups.Clear();
            foreach (Group e in list)
            {
                e.User = await App.UnitOfWork.UserRepository.FindByIdAsync(e.UserId);
                e.Notebook = await App.UnitOfWork.NotebookRepository.FindByIdAsync(e.NotebookId);

                e.Notebook.Owner = await App.UnitOfWork.UserRepository.FindByIdAsync(e.Notebook.OwnerId);

                Groups.Add(e);
            }
        }

        internal async Task InsertAsync()
        {
            await App.UnitOfWork.GroupRepository.CreateAsync(Group);
        }

        internal async Task DeleteAsync(Group Group)
        {
            await App.UnitOfWork.GroupRepository.DeleteAsync(Group);
            Groups.Remove(Group);
        }

        internal async Task<bool> IsNoteSharedToUserAsync(int notebookID, int userID)
        {
            Group group = await App.UnitOfWork.GroupRepository.getUserNotebookGroupAsync(notebookID, userID);
            if (group == null)
                return false;
            return true;

        }
    }
}
