using BooksSpot.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksSpot.Web.DTOs
{
    public class RezervationInfoDto
    {     
        public DateTime? RezerverdDate { get; set; } = null;
        public DateTime? RezervationExpirationDate { get; set; } = null;
        public int ExpirationDays { get; set; }        

        public RezervationInfoDto()
        {
        }

        public RezervationInfoDto(BookRezervation info)
        {
            RezerverdDate = info.RezerverdDate;
            RezervationExpirationDate = info.RezervationExpirationDate;
        }
    }
}
