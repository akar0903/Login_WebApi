using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace RepositoryLayer.Entity
{
    public class UserLabel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LabelId { get; set; }
        public string LabelName { get; set; }
        [ForeignKey("NoteUser")]
        public int UserId { get; set; }
        [JsonIgnore]
        public virtual User NotesUser { get; set; }
        [ForeignKey("LabelNotes")]
        public int NoteId { get; set; }
        [JsonIgnore]
        public virtual NotesEntity LabelNotes { get; set; }
    }
}
