using RepositoryLayer.Entity;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Interfaces;
using CommonLayer.RequestModel;
using ManagerLayer.Interfaces;

namespace ManagerLayer.Services
{
    public class NotesManager : INoteManager
    {
        private readonly INoteRepository repository;

        public NotesManager(INoteRepository repository)
        {
            this.repository = repository;
        }
        public NotesEntity NoteCreation(NoteCreationModel model, int id)
        {
            return repository.NoteCreation(model,id);
        }
        public List<NotesEntity> Notes(int id) {
            return repository.Notes(id);
        }
        public NotesEntity UpdateNotes(int userId, NoteCreationModel model, int NotesId)
        {
            return repository.UpdateNotes(userId,model, NotesId);
        }
        public NotesEntity Istrash(int userId, int notesId)
        {
            return repository.Istrash(userId, notesId);
        }
        public NotesEntity Delete(int userId,int notesid) {
            return repository.Delete(userId, notesid);
        }
        public NotesEntity Addcolor(string colour, int NotesId)
        {
            return repository.Addcolor(colour, NotesId);
        }
        public NotesEntity IsArchieve(int userId, int notesId)
        {
            return repository.IsArchieve(userId, notesId);
        }
        public NotesEntity isPin(int userId, int notesId)
        {
            return repository.isPin(userId, notesId);
        }
        public NotesEntity AddRemainder(int notesId, DateTime time)
        {
            return repository.AddRemainder(notesId, time);
        }
        public string UploadImage(string fpath,int notesId,int userId)
        {
            return repository.UploadImage(fpath, notesId,userId);
        }
        public UserLabel AddLabel(int UserID, int NoteID, string name)
        {
            return repository.AddLabel(UserID, NoteID, name);
        }
        public UserLabel UpdateLabel(int NoteID,string name,string newname)
        {
            return repository.UpdateLabel(NoteID, name, newname);
        }
        public List<UserLabel> GetLabel(int id)
        {
            return repository.GetLabel(id);
        }
        public UserLabel LabelDelete(int labelId)
        {
            return repository.LabelDelete(labelId);
        }
        public NotesEntity GetNotesById(int notesId)
        {
            return repository.GetNotesById(notesId);
        }
    }
}