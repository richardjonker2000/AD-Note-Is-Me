using System;
using System.Collections.Generic;
using System.Text;
using NoteIsMe.Domain.Repositories;

namespace NoteIsMe.Domain
{
    public interface IUnitOfWork
    {
        IFolderRepository FolderRepository { get; }
        IFolderNotebooksRepository FolderNotebooksRepository { get; }
        IGroupRepository GroupRepository { get; }
        INotebookRepository NotebookRepository { get; }
        INoteRepository NoteRepository { get; }
        INoteTagsRepository NoteTagsRepository { get; }
        ISketchRepository SketchRepository { get; }
        ISketchTagsRepository SketchTagsRepository { get; }
        ITagRepository TagRepository { get; }
        IUserRepository UserRepository { get; }


    }
}
