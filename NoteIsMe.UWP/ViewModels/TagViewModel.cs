using NoteIsMe.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteIsMe.UWP.ViewModels
{
    public class TagViewModel
    {

        public ObservableCollection<Tag> Tags { get; set; }
        public Tag Tag { get; set; }

        public TagViewModel()
        {
            Tags = new ObservableCollection<Tag>();
            Tag = new Tag();
        }

        public async Task LoadAllAsync()
        {

            List<Tag> list = await App.UnitOfWork.TagRepository.FindAllAsync();
            
            Tags.Clear();
            foreach (Tag e in list)
            {
                e.NoteTags = await App.UnitOfWork.NoteTagsRepository.FindAllByTagAsyc(e.Id);
                e.SketchTags = await App.UnitOfWork.SketchTagsRepository.FindAllByTagAsyc(e.Id);
                e.User = await App.UnitOfWork.UserRepository.FindByIdAsync(e.UserId);

                Tags.Add(e);
            }
        }


        public async Task LoadAllMineAsync()
        {


            List<Tag> list = await App.UnitOfWork.TagRepository.FindAllMyTagsAsync(App.userViewModel.GetCurrentUserID());
            Tags.Clear();
            foreach (Tag e in list)
            {
                e.NoteTags = await App.UnitOfWork.NoteTagsRepository.FindAllByTagAsyc(e.Id);
                foreach (NoteTag f in e.NoteTags)
                {
                    f.Note = await App.UnitOfWork.NoteRepository.FindByIdAsync(f.NoteId);
                }
                    e.SketchTags = await App.UnitOfWork.SketchTagsRepository.FindAllByTagAsyc(e.Id);
                foreach (SketchTag f in e.SketchTags)
                {
                    f.Sketch = await App.UnitOfWork.SketchRepository.FindByIdAsync(f.SketchId);
                }
                Tags.Add(e);
            }
        }

        internal async Task InsertAsync()
        {
            await App.UnitOfWork.TagRepository.CreateAsync(Tag);
        }

        internal async Task UpsertAsync()
        {
            await App.UnitOfWork.TagRepository.UpdateAsync(Tag);
        }

        internal async Task DeleteAsync(Tag Tag)
        {            
            await App.UnitOfWork.TagRepository.DeleteWithForeignKeyAsync(Tag);
            Tags.Remove(Tag);
        }

        

    }


}
