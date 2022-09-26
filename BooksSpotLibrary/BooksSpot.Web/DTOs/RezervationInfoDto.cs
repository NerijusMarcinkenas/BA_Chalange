using BooksSpot.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksSpot.Web.DTOs
{
    public class RezervationInfoDto
    {     
        public DateTime RezerverdDate { get; set; } 
        public DateTime RezervationExpirationDate { get; set; } 
        public int ExpirationDays { get; set; }        

        public RezervationInfoDto()
        {
        }

        public RezervationInfoDto(BookRezervation info)
        {
            RezerverdDate = info.RezerverdDate.Date;
            RezervationExpirationDate = info.RezervationExpirationDate.Date;
        }
    }
}
