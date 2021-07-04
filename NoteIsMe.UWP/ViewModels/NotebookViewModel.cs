using NoteIsMe.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteIsMe.UWP.ViewModels
{
    public class NotebookViewModel
    {
        public ObservableCollection<Notebook> Notebooks { get; set; }
        public ObservableCollection<Notebook> MyNotebooks { get; set; }
        public ObservableCollection<Notebook> SharedNotebooks { get; set; }


        public Notebook Notebook{ get; set; }

        public async Task LoadAllAsync()
        {

            List<Notebook> list = await App.UnitOfWork.NotebookRepository.FindAllAsync();

            Notebooks.Clear();
            foreach (Notebook e in list)
            {
                e.FolderNotebooks = await App.UnitOfWork.FolderNotebooksRepository.FindByNotebookIdAsync(e.Id);
                e.Owner = await App.UnitOfWork.UserRepository.FindByIdAsync(e.OwnerId);
                e.Notes = await App.UnitOfWork.NoteRepository.FindByNotebookAsync(e.Id);
                e.Sketches = await App.UnitOfWork.SketchRepository.FindByNotebookAsync(e.Id);
                e.Groups = await App.UnitOfWork.GroupRepository.FindByNotebookIdAsync(e.Id);

                Notebooks.Add(e);
            }
        }

        public NotebookViewModel()
        {
            Notebooks = new ObservableCollection<Notebook>();
            MyNotebooks = new ObservableCollection<Notebook>();
            SharedNotebooks = new ObservableCollection<Notebook>();
            Notebook = new Notebook();
        }

        public async Task<Notebook> FindbyIDAsync(int notebookID)
        {
            Notebook nb = await App.UnitOfWork.NotebookRepository.FindByIdAsync(notebookID);
            nb.Owner = await App.UnitOfWork.UserRepository.FindByIdAsync(nb.OwnerId);
            nb.Groups = (await App.UnitOfWork.GroupRepository.FindAllAsync()).Where(x => x.NotebookId == nb.Id).ToList();

            return nb;
        }

        public async Task LoadMyOwnedNotebooksAsync(int userid)
        {
            List<Notebook> list = await App.UnitOfWork.NotebookRepository.FindMyOwnedNotebooksAsync(userid);
            MyNotebooks.Clear();
            foreach (Notebook e in list)
            {
                
                e.Owner = await App.UnitOfWork.UserRepository.FindByIdAsync(e.OwnerId);
                e.Groups = (await App.UnitOfWork.GroupRepository.FindAllAsync()).Where(x => x.NotebookId == e.Id).ToList();

                foreach(Group g in e.Groups)
                {
                    g.User = await App.UnitOfWork.UserRepository.FindByIdAsync(g.UserId);
                }

                e.Notes = (await App.UnitOfWork.NoteRepository.FindAllAsync()).Where(x => x.NotebookId == e.Id).ToList();
                e.Sketches = (await App.UnitOfWork.SketchRepository.FindAllAsync()).Where(x => x.NotebookId == e.Id).ToList();
                e.FolderNotebooks = (await App.UnitOfWork.FolderNotebooksRepository.FindAllAsync()).Where(x => x.NoteBookId == e.Id).ToList();

                MyNotebooks.Add(e);
            }
        }
        public async Task LoadAllSharedToMeNotebooksAsync(int userid)
        {
            List<Notebook> list = await App.UnitOfWork.NotebookRepository.FindAllSharedToMeNotebooksAsync(userid);
            SharedNotebooks.Clear();
            foreach (Notebook e in list)
            {
                e.Owner = await App.UnitOfWork.UserRepository.FindByIdAsync(e.OwnerId);
                e.Groups = (await App.UnitOfWork.GroupRepository.FindAllAsync()).Where(x => x.NotebookId == e.Id).ToList();

                foreach (Group g in e.Groups)
                {
                    g.User = await App.UnitOfWork.UserRepository.FindByIdAsync(g.UserId);
                }

                e.Notes = (await App.UnitOfWork.NoteRepository.FindAllAsync()).Where(x => x.NotebookId == e.Id).ToList();
                e.Sketches = (await App.UnitOfWork.SketchRepository.FindAllAsync()).Where(x => x.NotebookId == e.Id).ToList();
                e.FolderNotebooks = (await App.UnitOfWork.FolderNotebooksRepository.FindAllAsync()).Where(x => x.NoteBookId == e.Id).ToList();

                SharedNotebooks.Add(e);
            }
        }

        internal async Task UpsertAsync()
        {
            await App.UnitOfWork.NotebookRepository.UpsertAsync(Notebook);
        }

        public async Task LoadAllFoldersAsync(int folderID)
        {
            List<Notebook> list = await App.UnitOfWork.NotebookRepository.FindAllInFolderAsync(folderID);
            Notebooks.Clear();
            foreach (Notebook e in list)
            {
                e.Owner = await App.UnitOfWork.UserRepository.FindByIdAsync(e.OwnerId);
                e.Groups = (await App.UnitOfWork.GroupRepository.FindAllAsync()).Where(x => x.NotebookId == e.Id).ToList();

                Notebooks.Add(e);
            }

        }

        public async Task LoadAllSharedAndOwnedNotebooksAsync(int userid)
        {
            await LoadAllSharedToMeNotebooksAsync(userid);
            await LoadMyOwnedNotebooksAsync(userid);

            Notebooks.Clear();

            foreach (Notebook e in MyNotebooks)
            {
                Notebooks.Add(e);
            }
            foreach (Notebook e in SharedNotebooks)
            {
                Notebooks.Add(e);
            }
        }

        internal async Task InsertAsync()
        {
            await App.UnitOfWork.NotebookRepository.CreateAsync(Notebook);


        }

        internal async Task DeleteAsync(Notebook Notebook)
        {
            await App.UnitOfWork.NotebookRepository.DeleteNotebookAsync(Notebook);
            Notebooks.Remove(Notebook);
        }

        public async Task<bool> isSharePermitted(User user, Notebook notebook)
        {
            if (App.userViewModel.GetCurrentUserID() == notebook.OwnerId)
                return true;
            else
            {
                Task<Group> groupTask = App.UnitOfWork.GroupRepository.getUserNotebookGroupAsync(notebook.Id, user.Id);
                Group group = await groupTask;
                return group.SharePermission;
            }
        }



        

    }
}
