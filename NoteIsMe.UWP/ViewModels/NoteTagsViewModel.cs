using NoteIsMe.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteIsMe.UWP.ViewModels
{
    public class NoteTagsViewModel
    {
        public ObservableCollection<NoteTag> NoteTags { get; set; }
        public NoteTag NoteTag;

        public NoteTagsViewModel()
        {
            NoteTags = new ObservableCollection<NoteTag>();
            NoteTag = new NoteTag();
        }

        public async Task LoadAllAsync()
        {

            List<NoteTag> list = await App.UnitOfWork.NoteTagsRepository.FindAllAsync();

            NoteTags.Clear();
            foreach (NoteTag e in list)
            {
                NoteTags.Add(e);
            }
        }

        public async Task InsertAsync()
        {
            await App.UnitOfWork.NoteTagsRepository.CreateAsync(NoteTag);

        }

        public async Task DeleteAsync()
        {
            await App.UnitOfWork.NoteTagsRepository.DeleteAsync(NoteTag);
        }

        public async Task LoadAllTagAsync(int tagid)
        {
            List<NoteTag> list = await App.UnitOfWork.NoteTagsRepository.FindAllByTagAsyc(tagid);

            NoteTags.Clear();
            foreach (NoteTag e in list)
            {
                e.Note = await App.UnitOfWork.NoteRepository.FindByIdAsync(e.NoteId);
                NoteTags.Add(e);
            }
        }
    }
}
