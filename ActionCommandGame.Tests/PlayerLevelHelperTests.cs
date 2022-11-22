using ActionCommandGame.Helpers;
using Xunit;

namespace ActionCommandGame.Tests
{
    public class PlayerLevelHelperTests
    {
        [Fact]
        public void GetExperienceFromLevel_0_Returns_0()
        {
            //Arrange
            var level = 0;

            //Act
            var sut = PlayerLevelHelper.GetExperienceFromLevel(level);

            //Assert
            Assert.Equal(0, sut);
        }

        [Fact]
        public void GetExperienceFromLevel_1_Returns_0()
        {
            //Arrange
            var level = 1;

            //Act
            var sut = PlayerLevelHelper.GetExperienceFromLevel(level);

            //Assert
            Assert.Equal(0, sut);
        }

        [Fact]
        public void GetExperienceFromLevel_2_Returns_100()
        {
            //Arrange
            var level = 2;

            //Act
            var sut = PlayerLevelHelper.GetExperienceFromLevel(level);

            //Assert
            Assert.Equal(100, sut);
        }

        [Fact]
        public void GetExperienceFromLevel_3_Returns_300()
        {
            //Arrange
            var level = 3;

            //Act
            var sut = PlayerLevelHelper.GetExperienceFromLevel(level);

            //Assert
            Assert.Equal(300, sut);
        }

        [Fact]
        public void GetExperienceFromLevel_20_Returns_19000()
        {
            //Arrange
            var level = 20;

            //Act
            var sut = PlayerLevelHelper.GetExperienceFromLevel(level);

            //Assert
            Assert.Equal(19000, sut);
        }

        [Fact]
        public void GetExperienceFromLevel_30_Returns_43500()
        {
            //Arrange
            var level = 30;

            //Act
            var sut = PlayerLevelHelper.GetExperienceFromLevel(level);

            //Assert
            Assert.Equal(43500, sut);
        }
    }
}
