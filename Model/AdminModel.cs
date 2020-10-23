using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pempo_backend.Model
{

    public class PempoBase
    {        
        public PempoBase ()
        {
            DateCreated = DateTime.Now;
            DateLastUpdated = DateTime.Now;
        }

        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }                
    }
    public class Admin : PempoBase
    {        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] Salt { get; set; }
        public string Password { get; set; }
        #nullable enable
        public byte[]? ProfileImage { get; set; }
        #nullable disable
    }

    public class Post : PempoBase
    {
        public string Title { get; set; }
        public string Content { get; set; }    
        public int AdminId { get; set; }

        [ForeignKey("AdminId")]
        public Admin Admin { get; set; }
    }

    public class Media : PempoBase
    {
        public byte[] MediaData { get; set; }       
        public int PostId { get; set; }

        [ForeignKey("PostId")]
        public Post Post { get; set; }
    }

    public class Comments : PempoBase
    {
        public int PostId { get; set; }
        public string Comment { get; set; }

        [ForeignKey("PostId")]
        public Post Post { get; set; }
    }

    public class Likes : PempoBase
    {
        public int Count { get; set; }
        public int PostId { get; set; }

        [ForeignKey("PostId")]
        public Post Post { get; set; }
    }


}
