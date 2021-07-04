using NoteIsMe.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteIsMe.UWP.ViewModels
{
    public class SketchTagsViewModel
    {
        public ObservableCollection<SketchTag> SketchTags { get; set; }
        public SketchTag SketchTag;

        public SketchTagsViewModel()
        {
            SketchTags = new ObservableCollection<SketchTag>();
            SketchTag = new SketchTag();
        }

        public async Task LoadAllAsync()
        {

            List<SketchTag> list = await App.UnitOfWork.SketchTagsRepository.FindAllAsync();

            SketchTags.Clear();
            foreach (SketchTag e in list)
            {
                SketchTags.Add(e);
            }
        }

        public async Task InsertAsync()
        {
            await App.UnitOfWork.SketchTagsRepository.CreateAsync(SketchTag);

        }

        public async Task DeleteAsync()
        {
            await App.UnitOfWork.SketchTagsRepository.DeleteAsync(SketchTag);
        }

        public async Task LoadAllTagAsync(int tagid)
        {
            List<SketchTag> list = await App.UnitOfWork.SketchTagsRepository.FindAllByTagAsyc(tagid);

            SketchTags.Clear();
            foreach (SketchTag e in list)
            {
                e.Sketch = await App.UnitOfWork.SketchRepository.FindByIdAsync(e.SketchId);
                SketchTags.Add(e);
            }
        }
    }
}
