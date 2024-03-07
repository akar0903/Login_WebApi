using CommonLayer.RequestModel;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Services
{
    public class UserLabelRepository:IUserLabelRepository
    {
        private readonly FundoContext context;
        public UserLabelRepository(FundoContext context)
        {
            this.context = context;
        }
        public UserLabel LabelCreation(UserLabelModel model, int id)
        {
            UserLabel entity = new UserLabel();
            entity.LabelId = model.LabelId;
            entity.LabelName = model.LabelName;
            entity.Id = model.Id;
            entity.NotesId = model.NotesId;
            context.UserLabel.Add(entity);
            context.SaveChanges();
            return entity;
        }
    }
}
