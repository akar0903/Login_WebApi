using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;

namespace RepositoryLayer.Entity
{
    public class CollabEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CollabId {  get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CollabEmail { get; set; }
        public bool IsTrash { get; set; }
        [ForeignKey("NoteUser")]
        public int Id {  get; set; }
        [JsonIgnore]
        public virtual User NotesUser {  get; set; }
        [ForeignKey("NoteEntity")]
        public int NoteId {  get; set; }
        [JsonIgnore] 
        public virtual NotesEntity NotesEntity { get; set; }
    }
}
