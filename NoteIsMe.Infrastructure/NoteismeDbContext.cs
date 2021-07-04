using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NoteIsMe.Domain.Models;

namespace NoteIsMe.Infrastructure
{
    public class NoteismeDbContext : DbContext
    {

        public DbSet<Note> Notes { get; set; }
        public DbSet<Sketch> Sketches { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Notebook> Notebooks { get; set; }
        public DbSet<Folder> Folders { get; set; }


        public DbSet<NoteTag> NoteTags { get; set; }
        public DbSet<SketchTag> SketchTags { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<FolderNotebook> FolderNotebooks { get; set; }


        public NoteismeDbContext(DbContextOptions options) : base(options)
        {
        }

        public NoteismeDbContext()
        {

        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost; Initial Catalog=note-is-me; Integrated Security = True; Connect Timeout = 30;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // for composite primary keys in many to many relation created tables
            modelBuilder.Entity<NoteTag>()
                        .HasKey(nt => new { nt.NoteId, nt.TagId });

            modelBuilder.Entity<SketchTag>()
                        .HasKey(st => new { st.SketchId, st.TagId });

            modelBuilder.Entity<Group>()
                        .HasKey(gr => new { gr.UserId, gr.NotebookId });

            modelBuilder.Entity<FolderNotebook>()
                        .HasKey(fn => new { fn.FolderId, fn.NoteBookId });

            // Foreign keys for one to many relations
            modelBuilder.Entity<Tag>()
                        .HasOne<User>(u => u.User)
                        .WithMany(t => t.Tags)
                        .HasForeignKey(fk => fk.UserId)
                        .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Notebook>()
                        .HasOne<User>(u => u.Owner)
                        .WithMany(nb => nb.Notebooks)
                        .HasForeignKey(fk => fk.OwnerId)
                        .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Sketch>()
                        .HasOne<Notebook>(nb => nb.Notebook)
                        .WithMany(s => s.Sketches)
                        .HasForeignKey(fk => fk.NotebookId)
                        .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Note>()
                        .HasOne<Notebook>(nb => nb.Notebook)
                        .WithMany(s => s.Notes)
                        .HasForeignKey(fk => fk.NotebookId)
                        .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Sketch>()
                        .HasOne<User>(u => u.Owner)
                        .WithMany(s => s.Sketches)
                        .HasForeignKey(fk => fk.OwnerId)
                        .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Note>()
                        .HasOne<User>(u => u.Owner)
                        .WithMany(s => s.Notes)
                        .HasForeignKey(fk => fk.OwnerId)
                        .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Folder>()
                        .HasOne<User>(u => u.Owner)
                        .WithMany(f => f.Folders)
                        .HasForeignKey(fk => fk.OwnerId)
                        .OnDelete(DeleteBehavior.NoAction);



            modelBuilder.Entity<User>().Property(i => i.Password).IsRequired().HasMaxLength(1000);
            modelBuilder.Entity<User>().Property(i => i.Salt).IsRequired().HasMaxLength(1000);

            modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();





            //Adding users
            var u1 = new User { Id = 1, Name = "admin", Email = "admin@admin.com", Role = 3, Salt = "/zzRmyEbyhaZfi+SCExAeweJFih4D8aZ/uL8XYy03dE=", Password = "1zbbwGguKx3wT7UHPOTHjS1mCEaj5xriQAEqKQQQlVw=" };
            var u2 = new User { Id = 2, Name = "User One", Email = "user1@example.com", Role = 1, Salt = "F9wZzYqCFH/RiIPMqT4x/SDXkb7plRkwT6LM74xJ3FE=", Password = "NeP5cLGOuXtXI4YL0zbY27Nrawu8/SRS4RDs4LRshZ4=" };
            var u3 = new User { Id = 3, Name = "User Two", Email = "user2@example.com", Role = 1, Salt = "RoFZqkf6zcV2WFs/3sfXze58KmDY1vumxRYOht5w/nQ=", Password = "yttEU+VeADGQ3zv4911feTo1EyA2B9dfl8WH8ppbv1I=" };


            modelBuilder.Entity<User>().HasData(u1);
            modelBuilder.Entity<User>().HasData(u2);
            modelBuilder.Entity<User>().HasData(u3);


            //Adding folders
            var f1 = new Folder { Id = 1, Name = "Folder", OwnerId = 1, IconURL = "/Assets/folderIcons/home.png" };
            var f2 = new Folder { Id = 2, Name = "School Item", OwnerId = 2, IconURL = "/Assets/folderIcons/school.png" };
            var f3 = new Folder { Id = 3, Name = "My Misc", OwnerId = 2, IconURL = "/Assets/folderIcons/misc.png" };
            var f4 = new Folder { Id = 4, Name = "Only One", OwnerId = 3, IconURL = "/Assets/folderIcons/work.png" };

            modelBuilder.Entity<Folder>().HasData(f1);
            modelBuilder.Entity<Folder>().HasData(f2);
            modelBuilder.Entity<Folder>().HasData(f3);
            modelBuilder.Entity<Folder>().HasData(f4);



            //Adding tags
            var t1 = new Tag { Id = 1, UserId = 1, Name = "App Dev", Color = "#8c34eb" };

            modelBuilder.Entity<Tag>().HasData(t1);




            //Adding notebooks
            var nb1 = new Notebook { Id = 1, OwnerId = 1, Title = "Only Mine" };
            var nb2 = new Notebook { Id = 2, OwnerId = 2, Title = "Shared NB" };
            var nb3 = new Notebook { Id = 3, OwnerId = 2, Title = "Stuff" };
            var nb4 = new Notebook { Id = 4, OwnerId = 3, Title = "Everyone's NB" };

            modelBuilder.Entity<Notebook>().HasData(nb1);
            modelBuilder.Entity<Notebook>().HasData(nb2);
            modelBuilder.Entity<Notebook>().HasData(nb3);
            modelBuilder.Entity<Notebook>().HasData(nb4);



            //Adding foldernotebooks
            var fn1 = new FolderNotebook { NoteBookId = 1, FolderId = 1 };
            var fn2 = new FolderNotebook { NoteBookId = 2, FolderId = 2 };
            var fn3 = new FolderNotebook { NoteBookId = 4, FolderId = 3 };

            modelBuilder.Entity<FolderNotebook>().HasData(fn1);
            modelBuilder.Entity<FolderNotebook>().HasData(fn2);
            modelBuilder.Entity<FolderNotebook>().HasData(fn3);


            //Adding groups
            var g1 = new Group { UserId = 3, NotebookId = 2, ViewPermission = true, EditPermission = true, SharePermission = true };
            var g2 = new Group { UserId = 1, NotebookId = 4, ViewPermission = true, EditPermission = false, SharePermission = true };
            var g3 = new Group { UserId = 2, NotebookId = 4, ViewPermission = true, EditPermission = true, SharePermission = false };

            modelBuilder.Entity<Group>().HasData(g1);
            modelBuilder.Entity<Group>().HasData(g2);
            modelBuilder.Entity<Group>().HasData(g3);
















        }


    }
}
