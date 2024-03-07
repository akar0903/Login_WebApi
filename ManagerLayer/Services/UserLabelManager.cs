using CommonLayer.RequestModel;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Services
{
    public class UserLabelManager:IUserLabelManager
    {
        private readonly IUserLabelRepository repository;
        public UserLabelManager(IUserLabelRepository repository)
        {
            this.repository = repository;
        }

        public UserLabel LabelCreation(UserLabelModel model, int id)
        {
            return repository.LabelCreation(model,id);
        }
    }
}
