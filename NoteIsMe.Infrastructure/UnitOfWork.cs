using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NoteIsMe.Domain;
using NoteIsMe.Domain.Repositories;
using NoteIsMe.Infrastructure.Repositories;

namespace NoteIsMe.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContextOptions Options { get; set; }

        public IFolderRepository FolderRepository => new FolderRepository(new NoteismeDbContext(Options));
        public IFolderNotebooksRepository FolderNotebooksRepository => new FolderNotebooksRepository(new NoteismeDbContext(Options));
        public IGroupRepository GroupRepository => new GroupRepository(new NoteismeDbContext(Options));
        public INotebookRepository NotebookRepository => new NotebookRepository(new NoteismeDbContext(Options));
        public INoteRepository NoteRepository => new NoteRepository(new NoteismeDbContext(Options));
        public INoteTagsRepository NoteTagsRepository => new NoteTagsRepository(new NoteismeDbContext(Options));
        public ISketchRepository SketchRepository => new SketchRepository(new NoteismeDbContext(Options));
        public ISketchTagsRepository SketchTagsRepository => new SketchTagsRepository(new NoteismeDbContext(Options));
        public ITagRepository TagRepository => new TagRepository(new NoteismeDbContext(Options));
        public IUserRepository UserRepository => new UserRepository(new NoteismeDbContext(Options));

        public UnitOfWork(DbContextOptions<NoteismeDbContext> options)
        {
            Options = options;

            NoteismeDbContext db = new NoteismeDbContext(options);
            db.Database.Migrate();
        }
    }
}
