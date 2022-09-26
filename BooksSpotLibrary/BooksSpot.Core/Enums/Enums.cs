using System.ComponentModel.DataAnnotations;

namespace BooksSpot.Core.Enums
{
    public enum BookStatus
    {
        [Display(Name = "Not Set")]
        NotSet,
        Available,
        Reserved,
        Borrowed
    }

    public enum Genre
    {
        [Display(Name ="Not Set")]
        NotSet,
        Classics,
        [Display(Name = "Folklore & Mythology")]
        FolkloreMythology,
        [Display(Name = "General Fiction")]
        GeneralFiction,
        Novels,
        Historical,
        Horror,
        [Display(Name = "Mystery Crime")]
        MysteryCrime,
        Poetry,
        Romance,
        Fantasy,
        Thrillers,
        [Display(Name = "Personal Growth")]
        PersonalGrowth,
        Finance,
        Health
    }

    public enum SearchType
    {
        All,
        Title,
        Author,
        Publisher,
        [Display(Name = "Published year")]
        PublishedYear,
        Isbn,       
    }
}


