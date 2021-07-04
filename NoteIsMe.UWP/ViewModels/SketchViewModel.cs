using NoteIsMe.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteIsMe.UWP.ViewModels
{
    public class SketchViewModel
    {
        public ObservableCollection<Sketch> Sketches { get; set; }

        public Sketch Sketch { get; set; }

        public SketchViewModel()
        {
            Sketches = new ObservableCollection<Sketch>();
            Sketch = new Sketch();

        }

        public async Task LoadAllAsync()
        {
            List<Sketch> list = await App.UnitOfWork.SketchRepository.FindAllAsync();
            Sketches.Clear();
            foreach (Sketch e in list)
            {
                e.Owner = await App.UnitOfWork.UserRepository.FindByIdAsync(e.OwnerId);
                e.Notebook = await App.UnitOfWork.NotebookRepository.FindByIdAsync(e.NotebookId);
                e.SketchTags = await App.UnitOfWork.SketchTagsRepository.FindAllBySketchIdAsync(e.Id);

                Sketches.Add(e);
            }
        }

        public async Task<Sketch> FindbyIDAsync(int sketchID)
        {
            Sketch s = await App.UnitOfWork.SketchRepository.FindByIdAsync(sketchID);
            return s;
        }

        public async Task<bool> isEditPermitted(User user, Sketch sketch)
        {
            if (App.userViewModel.GetCurrentUserID() == sketch.OwnerId)
                return true;
            else
            {
                Task<Group> groupTask = App.UnitOfWork.GroupRepository.getUserNotebookGroupAsync(sketch.NotebookId, user.Id);
                Group group = await groupTask;
                return group.EditPermission;
            }
        }

        public static bool isEditPermittedStatic(User user, Sketch sketch)
        {
            if (App.userViewModel.GetCurrentUserID() == sketch.OwnerId)
                return true;

            else
            {
               Group group = App.UnitOfWork.GroupRepository.getUserNotebookGroup(sketch.NotebookId, user.Id);
                
                return group.EditPermission;
            }
        }

        internal async Task UpsertAsync(Sketch sketch)
        {
            await App.UnitOfWork.SketchRepository.UpsertAsync(sketch);
        }

        public async Task LoadAllofUserAsync(int userid)
        {
            await LoadSketchesByUserID(userid);
            List<Sketch> shared = new List<Sketch>();


            List<Notebook> sharedNotebooks = await App.UnitOfWork.NotebookRepository.FindAllSharedToMeNotebooksAsync(userid);
            foreach (Notebook nb in sharedNotebooks)
            {
                shared = await App.UnitOfWork.SketchRepository.FindByNotebookAsync(nb.Id);
                foreach (Sketch n in shared)
                {
                    n.Owner = await App.UnitOfWork.UserRepository.FindByIdAsync(n.OwnerId);

                    n.Notebook = await App.UnitOfWork.NotebookRepository.FindByIdAsync(n.NotebookId);

                    Sketches.Add(n);
                }
            }
        }

        public async Task LoadSketchesByUserID(int userid)
        {
            List<Sketch> list = await App.UnitOfWork.SketchRepository.FindSketchesByUserID(userid);
            Sketches.Clear();
            foreach (Sketch e in list)
            {
                e.Owner = await App.UnitOfWork.UserRepository.FindByIdAsync(e.OwnerId);

                e.Notebook = await App.UnitOfWork.NotebookRepository.FindByIdAsync(e.NotebookId);

            Sketches.Add(e);
            }
        }

        public async Task LoadNotebookAsync(int notebookid)
        {
            List<Sketch> list = await App.UnitOfWork.SketchRepository.FindByNotebookAsync(notebookid);
            Sketches.Clear();
            foreach (Sketch e in list)
            {
                e.Owner = await App.UnitOfWork.UserRepository.FindByIdAsync(e.OwnerId);

                Sketches.Add(e);
            }
        }



        internal async Task InsertAsync()
        {
            await App.UnitOfWork.SketchRepository.CreateAsync(Sketch);
        }

        internal async Task DeleteAsync(Sketch sketch)
        {
            await App.UnitOfWork.SketchRepository.DeleteAsync(sketch);
            Sketches.Remove(Sketch);
        }
    }
}
