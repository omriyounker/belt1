using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace belt1.Models
{
    public class User{

        [Key]
        public int UserId {get;set;}

        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string FirstName {get;set;}


        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string LastName {get;set;}

        [Required]
        [EmailAddress]
        public string Email {get;set;}

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password {get;set;}

        public List<Rsvp> Going {get;set;}
       

        public User(){
           
            Going = new List<Rsvp>();
        }


    }

}