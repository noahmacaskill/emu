// <copyright file="FixCheckCommandTests.cs" company="QutEcoacoustics">
// All code in this file and all associated files are the copyright and property of the QUT Ecoacoustics Research Group.
// </copyright>

namespace MetadataUtility.Tests.Commands.Fix
{
    using System.CommandLine.Parsing;
    using MetadataUtility.Commands;
    using Xunit;

    public class FixCheckCommandTests
    {
        [Fact]
        public void FixOptionDoesNotBundleArgument()
        {
            var optionFirst = "fix check B:\\Marina\\**\\*.flac -f FL010";
            var optionSecond = "fix check -f FL010 B:\\Marina\\**\\*.flac";

            var parser = EmuEntry.BuildCommandLine();
            var result1 = parser.Parse(optionFirst);
            var result2 = parser.Parse(optionSecond);

            Assert.True(result1.Errors.Count == 0);
            Assert.True(result2.Errors.Count == 0);
            Assert.Equal(
                result1.CommandResult.FindResultFor(CommonArguments.Fixes).GetValueOrDefault<string[]>(),
                result2.CommandResult.FindResultFor(CommonArguments.Fixes).GetValueOrDefault<string[]>());

            Assert.All(
                new string[][]
                {
                    result1.CommandResult.FindResultFor(CommonArguments.Targets).GetValueOrDefault<string[]>(),
                    result2.CommandResult.FindResultFor(CommonArguments.Targets).GetValueOrDefault<string[]>(),
                },
                (value) => Assert.Equal(new string[] { "B:\\Marina\\**\\*.flac" }, value));
        }

        [Theory]
        [InlineData("fix check B:\\Marina\\**\\*.flac -f FL010,FL020")]
        [InlineData("fix check B:\\Marina\\**\\*.flac -f=FL010,FL020")]
        [InlineData("fix check B:\\Marina\\**\\*.flac --fix FL010,FL020")]
        [InlineData("fix check B:\\Marina\\**\\*.flac --fix=FL010,FL020")]
        [InlineData("fix check  -f FL010,FL020 B:\\Marina\\**\\*.flac")]
        [InlineData("fix check  -f=FL010,FL020 B:\\Marina\\**\\*.flac")]
        [InlineData("fix check  --fix FL010,FL020 B:\\Marina\\**\\*.flac")]
        [InlineData("fix check  --fix=FL010,FL020 B:\\Marina\\**\\*.flac")]
        public void FixOptionSupportsCommaDelimitter(string command)
        {
            var parser = EmuEntry.BuildCommandLine();
            var result = parser.Parse(command);

            Assert.True(result.Errors.Count == 0);

            Assert.Equal(
                new string[] { "FL010", "FL020" },
                result.CommandResult.FindResultFor(CommonArguments.Fixes).GetValueOrDefault<string[]>());
        }
    }
}
