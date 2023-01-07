using HotelDB;

namespace DBTests
{
    public class HashingTests
    {
        [Fact]
        public void SaltGenerationTest()
        {
            const int LENGTH = 16;
            Assert.Equal(LENGTH, SaltGenerator.GenerateSaltString().Length);
        }
    }
}