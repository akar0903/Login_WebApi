using CommonLayer.RequestModel;
using RepositoryLayer.Entity;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface INoteRepository
    {
        public NotesEntity NoteCreation(NoteCreationModel model, int id);
        public List<NotesEntity> Notes(int id);
        public NotesEntity UpdateNotes(int userId, NoteCreationModel model, int NotesId);

        public NotesEntity Istrash(int userId, int notesId);

        public NotesEntity Delete(int userId, int NotesId);
        public NotesEntity Addcolor(string colour, int NotesId);
        public NotesEntity IsTrash(int userId, int notesId);
        public NotesEntity IsArchieve(int userId, int notesId);
        public NotesEntity isPin(int userId, int NotesId);
        public NotesEntity AddRemainder(int notesId, DateTime time);
        public string UploadImage(string fpath, int notesId,int userId);
    }
}
