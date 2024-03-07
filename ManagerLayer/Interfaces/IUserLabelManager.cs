using CommonLayer.RequestModel;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Interfaces
{
    public interface IUserLabelManager
    {
        public UserLabel LabelCreation(UserLabelModel model, int id);
    }
}
