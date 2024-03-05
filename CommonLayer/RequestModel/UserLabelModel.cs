using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.RequestModel
{
    public class UserLabelModel
    {
        public int LabelId { get; set; }
        public string LabelName { get; set; }
        public int NoteId {  get; set; }
        public string NoteName { get; set; }
    }
}
