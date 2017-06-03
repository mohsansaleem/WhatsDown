using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WhatsDown.Core.Domain
{
    public class Message
    {
        public Message()
        {
            
        }
        
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Level { get; set; }

        public float FullPrice { get; set; }

        //public virtual User Author { get; set; }

        public int AuthorId { get; set; }
        
        public bool IsBeginnerCourse
        {
            get { return Level == 1; }
        }
    }
}
