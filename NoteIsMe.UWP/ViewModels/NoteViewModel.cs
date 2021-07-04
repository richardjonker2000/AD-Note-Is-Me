using NoteIsMe.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteIsMe.UWP.ViewModels
{
    public class NoteViewModel
    {
        public ObservableCollection<Note> Notes { get; set; }

        public Note Note { get; set; }

        public NoteViewModel()
        {
            Notes = new ObservableCollection<Note>();
            Note = new Note();

        }

        public async Task LoadAllAsync()
        {
            List<Note> list = await App.UnitOfWork.NoteRepository.FindAllAsync();
            Notes.Clear();
            foreach (Note e in list)
            {
                e.Owner = await App.UnitOfWork.UserRepository.FindByIdAsync(e.OwnerId);
                e.Notebook = await App.UnitOfWork.NotebookRepository.FindByIdAsync(e.NotebookId);
                e.NoteTags = await App.UnitOfWork.NoteTagsRepository.FindAllByNoteIdAsync(e.Id);

                Notes.Add(e);
            }
        }

        public async Task<bool> isEditPermitted(User user,Note note)
        {
            if (App.userViewModel.GetCurrentUserID() == note.OwnerId)
                return true;
            else
            {
                Task<Group> groupTask = App.UnitOfWork.GroupRepository.getUserNotebookGroupAsync(note.Notebook.Id, user.Id);
                Group group = await groupTask;
                return group.EditPermission;
            }
        }


        public async Task LoadAllofUser(int userid)
        {
            await LoadNotesByUserID(userid);
            List<Note> shared = new List<Note>();
            

            List<Notebook> sharedNotebooks = await App.UnitOfWork.NotebookRepository.FindAllSharedToMeNotebooksAsync(userid);
            foreach (Notebook nb in sharedNotebooks)
            {
                shared = await App.UnitOfWork.NoteRepository.FindByNotebookAsync(nb.Id);
                foreach(Note n in shared)
                {
                    Notes.Add(n);
                }
            }

            foreach(Note n in Notes)
            {
                n.Notebook = await App.UnitOfWork.NotebookRepository.FindByIdAsync(n.NotebookId);
            }
        }

        public async Task LoadNotesByUserID(int userid)
        {
            List<Note> list = await App.UnitOfWork.NoteRepository.FindNotesByUserID(userid);
            Notes.Clear();
            foreach (Note e in list)
            {
                Notes.Add(e);
            }
        }

        public async Task LoadNotebookAsync(int notebookid)
        {
            List<Note> list = await App.UnitOfWork.NoteRepository.FindByNotebookAsync(notebookid);
            Notes.Clear();
            foreach (Note e in list)
            {
                e.Owner = await App.UnitOfWork.UserRepository.FindByIdAsync(e.OwnerId);
                Notes.Add(e);
            }
        }
        internal async Task InsertAsync()
        {
            await App.UnitOfWork.NoteRepository.CreateAsync(Note);
        }

        internal async Task UpsertAsync(Note note)
        {
            await App.UnitOfWork.NoteRepository.UpsertAsync(note);
        }

        internal async Task DeleteAsync(Note Note)
        {
            await App.UnitOfWork.NoteRepository.DeleteAsync(Note);
            Notes.Remove(Note);
        }
    }
}
