using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace belt1.Models
{
    public class Event{

        [Key]
        public int EventId {get;set;}

        [Required]
        [MinLength(2)]
        public string Title {get;set;}

        [Required]
        [DataType(DataType.Time)]
        public string Time {get;set;}

        [Required]
        [Range(0, Int16.MaxValue)]
        public int Duration {get;set;}

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date {get;set;}

        [Required]
        public int Creator {get;set;}
        [Required]
        public string CreatorName {get;set;}
        
        [Required]
        public string HM {get;set;}
       

        [Required]
        [MinLength(10)]
        public string Description {get;set;}

        public List<Rsvp> Coming {get;set;}
        
        public Event(){  
            Coming = new List<Rsvp>();         
        }


    }

}