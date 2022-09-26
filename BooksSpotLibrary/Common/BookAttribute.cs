using AutoFixture;
using AutoFixture.Xunit2;

namespace Common
{
    public class BookAttribute : AutoDataAttribute
    {       
        public BookAttribute() : base(() =>
        {
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            fixture.Customizations.Add(new BookSpecimenBuilder());
            return fixture;
        }) 
        {            
        }       
    }
}
