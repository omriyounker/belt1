using System.ComponentModel.DataAnnotations;
using System;

using System.Collections.Generic;
namespace belt1.Models
{
    public class Rsvp{

        [Key]
        public int RSVPId {get;set;}

        [Required]
        public int UserId {get;set;}
        public User User {get;set;}

        [Required]
        public int EventId {get;set;}
        public Event Event {get;set;}
    }
}